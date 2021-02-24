using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ViewModels;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess_mvc.CustomFilters;

namespace RecipeForSuccess_mvc.Controllers
{
    public class RecipesController : Controller
    {
        IRecipesService recipesService;

        public RecipesController(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        // GET: Recipes
        [UserAuthorizationFilterAttribute]
        public ActionResult Index()
        {
            int user_id = Convert.ToInt32(Session["CurrentUserID"]);

            List<RecipeVM> recipeVMs = recipesService.GetRecipesByUserID(user_id);

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
        public ActionResult AddRecipe(RecipeVM recipeVM)
        {
            //int recipeID = 0;
            if (ModelState.IsValid)
            {
                int user_id = Convert.ToInt32(Session["CurrentUserID"]);
                recipeVM.User_id = user_id;
                recipesService.InsertRecipe(recipeVM);

                TempData["Success"] = "Recipe Added";
            }
            else
            {
                ViewBag.RecipeInserted = false;
                ModelState.AddModelError("key", "All Fields must be filled out");
            }

            return RedirectToAction("Index","Recipes");

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
    }
}