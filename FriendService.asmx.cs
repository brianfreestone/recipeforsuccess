using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess.ViewModels;

namespace RecipeForSuccess_mvc
{
    /// <summary>
    /// Summary description for FriendService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FriendService : System.Web.Services.WebService
    {

        [WebMethod]
        public void SearchRecipes(int pageNumber, int pageSize, string searchVal)
        {
            RecipesService recipesService = new RecipesService();
            List<RecipeVM> listRecipes = recipesService.SearchRecipesByTerm(pageNumber, pageSize, searchVal);
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(listRecipes));
        }

        [WebMethod]
        public void GetRecipes(int pageNumber, int pageSize, int userID)
        {
            List<UserRecipeVM> listUserRecipes = new List<UserRecipeVM>();
            FriendsService friendsService = new FriendsService();
            UsersService usersService = new UsersService();

            List<RecipeVM> friendsRecipes = friendsService.GetFriendsRecipesByUserID(pageNumber, pageSize, userID);

            int numRates = 0; // RecipesService.GetRates();
            float rating = 3; // RecipesService.GetRating();

            foreach (RecipeVM item in friendsRecipes)            
            {
                UserVM user = usersService.GetUserByUserID((int)item.User_id);
                UserRecipeVM userRecipeVM = new UserRecipeVM()
                {
                    Description = item.Description,
                    RecipeID = (int)item.recipe_id,
                    UserID = (int)item.User_id,
                    Created = item.Created.ToString("dddd, dd MMMM yyyy hh:mm tt"),
                    RecipeName = item.Name,
                    Username = user.Username,
                    UserFullName = user.First_name + ' ' + user.Last_name,
                    NumRates = numRates,
                    Rating = rating
                };
                listUserRecipes.Add(userRecipeVM);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(listUserRecipes));

        }

        [WebMethod]
        public void GetFriends(int pageNumber, int pageSize, int viewingID, int userID)
        {
            FriendsService friendsService = new FriendsService();

            //string connString = @"Data Source=INFO4430-RS-DEV\SQLEXPRESS;Initial Catalog=recipesuccess;User ID=website_user;Password=XIO4$ujdC$b9";
            List<UserVM> listFriends;

            // get the logged in user's friends
            Dictionary<int, UserVM> dictUserFriends = friendsService.GetFriends(pageNumber, pageSize, userID);// new Dictionary<int, UserVM>();

            // remove the user with id of userID, it is the user
            dictUserFriends.Remove(userID);

            if (userID == viewingID)
            {
                // the user is looking at their home page, show their friends with the no add attribute
                foreach (UserVM userVM in dictUserFriends.Values)
                {

                    userVM.ShowAddFriend = false;
                }

                listFriends = dictUserFriends.Values.ToList();
            }
            else
            {

                // get the logged in user's friends
                Dictionary<int, UserVM> dictViewingFriends = friendsService.GetFriends(pageNumber, pageSize, viewingID);// new Dictionary<int, UserVM>();

                // remove the viewing friend

                dictViewingFriends.Remove(viewingID);

                foreach (KeyValuePair<int, UserVM> kvp in dictViewingFriends)
                {
                    if (dictUserFriends.ContainsKey(kvp.Key))
                    {
                        if (dictUserFriends[kvp.Key].Pending)
                        {
                            kvp.Value.Pending = true;
                        }
                        else
                        {
                            kvp.Value.ShowAddFriend = false;
                        }

                    }
                    else if (kvp.Key.Equals(userID))
                    {
                        kvp.Value.ShowAddFriend = false;
                    }
                    else
                    {
                        kvp.Value.ShowAddFriend = true;
                    }



                }


                listFriends = dictViewingFriends.Values.ToList();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(listFriends));
        }
    }
}
