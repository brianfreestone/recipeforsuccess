using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ViewModels;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess_mvc.CustomFilters;
using System.IO;

namespace RecipeForSuccess_mvc.Controllers
{
    public class RecipesController : Controller
    {
        IRecipesService recipesService;
        IFriendsService friendsService;
        IUsersService usersService;

        public RecipesController(IRecipesService recipesService, IFriendsService friendsService, IUsersService usersService)
        {
            this.recipesService = recipesService;
            this.friendsService = friendsService;
            this.usersService = usersService;
        }

        // GET: Recipes
        [UserAuthorizationFilterAttribute]
        public ActionResult Index(int userID = 0, bool fav = false)
        {

            int userID2 = 0;
            string userName = User.Identity.Name;
            ViewBag.Username = userName;
            int loggedInUserID = usersService.GetUserIDByUserName(userName);
            if (userID == 0 || userID == loggedInUserID)
            {
                ViewBag.UserType = "owner";
                userID = loggedInUserID;
                userID2 = loggedInUserID;
                ViewBag.FavUserID = userID;
            }
            else
            {
                ViewBag.UserType = "guest";
                userID2 = loggedInUserID;
                ViewBag.FavUserID = userID2;
            }

            ViewBag.UserID = userID;

            // get friend request count
            var friendRequestCount = friendsService.GetFriendRequestCount(userID);
            if (friendRequestCount > 0)
            {
                ViewBag.FriendRequestCount = friendRequestCount;
            }

            List<RecipeVM> recipeVMs;

            if (fav == true)
            {
                recipeVMs = recipesService.GetFavoriteRecipesByUserID(userID);
            }
            else
            {
                recipeVMs = recipesService.GetRecipesByUserID(userID, userID2);
            }
            

            return View(recipeVMs);
        }

        [HttpGet]
        [UserAuthorizationFilterAttribute]
        public ActionResult AddRecipe()
        {

            List<MealTypeVM> mealTypeVMs = recipesService.GetMealTypes();

            ViewBag.MealTypes = mealTypeVMs;

            //RecipeVM addRecipeVM = new AddRecipeVM();
            return View();

        }

        [HttpPost]
        public ActionResult AddRecipe(RecipeVM recipeVM, HttpPostedFileBase file)
        {
            //int recipeID = 0;
            if (ModelState.IsValid)
            {
                int user_id = Convert.ToInt32(Session["CurrentUserID"]);
                recipeVM.User_id = user_id;

                // check if file was uploaded
                if (file != null && file.ContentLength > 0)
                {
                    // get extension
                    string ext = file.ContentType.ToLower();

                    // verify extension
                    if (ext != "image/jpg" &&
                        ext != "image/jpeg" &&
                        ext != "image/pjpeg" &&
                        ext != "image/gif" &&
                        ext != "image/x-png" &&
                        ext != "image/png")
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                        return View("AddRecipe", recipeVM);
                    }

                }

                int recipeID = recipesService.InsertRecipe(recipeVM, file);

                // set upload directory
                var uploadsDirectory = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));

                // set image name
                string imageName = recipeID + ".jpg";

                // set path
                var path = string.Format("{0}\\{1}", uploadsDirectory, imageName);

                // save image
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(path);
                }

                TempData["Success"] = "Recipe Added";
            }
            else
            {
                ViewBag.RecipeInserted = false;
                ModelState.AddModelError("key", "All Fields must be filled out");

                List<MealTypeVM> mealTypeVMs = recipesService.GetMealTypes();

                ViewBag.MealTypes = mealTypeVMs;

                return View(recipeVM);
            }

            return RedirectToAction("Index", "Recipes");

        }

        [HttpGet]
        [UserAuthorizationFilterAttribute]
        public ActionResult ViewRecipe(int recipeID)
        {
            string userName = User.Identity.Name;
            int userID = usersService.GetUserIDByUserName(userName);

            // todo check if recipe exists
            if (recipesService.RecipeExistsByRecipeID(recipeID))
            {
                //// check if user is allowed to look at recipe
                //if (recipesService.IsFriendsRecipeIDs(recipeID, userID))
                //{

                RecipeViewVM recipe = recipesService.GetRecipeByRecipeID(recipeID);
                recipe.recipe_id = recipeID;
                recipe.Username = usersService.GetUsernameByUserID((int)recipe.User_id);
                recipe.UserFullName = usersService.GetUserFullNameByUserName(recipe.Username);

                List<CommentVM> comments = recipesService.GetCommentsByRecipeID(recipeID);
                ViewBag.Comments = comments;
                return View(recipe);
                //}
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ViewRecipeById(int recipeID)
        {
            RecipeViewVM recipe = recipesService.GetRecipeByRecipeID(recipeID);
            recipe.recipe_id = recipeID;
            recipe.Username = usersService.GetUserFullNameByUserID((int)recipe.User_id);
            return View(recipe);
        }

        [HttpGet]
        public ActionResult AddIngredient()
        {
            int recipe_id;
            recipe_id = Convert.ToInt32(Session["RecipeID"]);
            List<IngredientVM> ingredients = recipesService.GetIngredientsByRecipeID(recipe_id);
            ViewBag.RecipeName = Session["RecipeName"].ToString();
            return View(ingredients);
        }

        [HttpPost]
        public JsonResult AddIngredient(string measure, string ingredient, string instruction)
        {
            // add ingredient to db here
            int recipe_id = Convert.ToInt32(Session["RecipeID"]);
            AddIngredientVM addIngredientVM = new AddIngredientVM()
            {
                RecipeID = recipe_id,
                IngredientName = ingredient,
                Instruction = instruction,
                MeasureValue = measure
            };

            // check if recipe id was saved
            recipesService.InsertIngredient(addIngredientVM);

            List<IngredientVM> ingredients = recipesService.GetIngredientsByRecipeID(recipe_id);

            return Json(ingredients);
        }

        [HttpPost]
        public void UpdateFavorite(int recipeID, int userID, string favoriteType)
        {
            recipesService.UpdateFavorite(recipeID, userID, favoriteType);
        }



        //Kate Stuff
        
     
        [HttpPost]
        public void AddRating(int recipeID, int userID, int rating)
        {
            recipesService.AddRating(recipeID, userID, rating);
        }

        [HttpPost]
        public void AddComment(int recipeID, string comment)
        {
            if (comment != "")
            {

                string username = User.Identity.Name;
                int userID = usersService.GetUserIDByUserName(username);

                recipesService.AddComment(recipeID, userID, comment);

            }
            else
            {
                TempData["Error"] = "Please enter a comment";
            }

        }


    }
}