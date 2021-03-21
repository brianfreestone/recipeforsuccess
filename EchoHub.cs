using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RecipeForSuccess.ServiceLayer;
using System.Diagnostics;


namespace RecipeForSuccess_mvc
{

    [HubName("echo")]
    public class EchoHub : Hub
    {

        
        public void Hello(string message)
        {
            Trace.WriteLine(message);

            // set clients
            var allClients = Clients.All;

            // call js function
            allClients.test("this is a test, signalR connected");
        }

        public void Notify(string friend)
        {
            UsersService usersService = new UsersService();
            FriendsService friendsService = new FriendsService();

            // get friend's id

            int friendID = usersService.GetUserIDByUserName(friend);

            // get friend count
            int friendRequestCount = friendsService.GetFriendRequestCount(friendID);

            // set clients
            var clients = Clients.Others;

            // call js function
            clients.friendRequestNotify(friend, friendRequestCount);
        }

        public void GetFriendRequestCount(int userId) 
        {
            FriendsService friendsService = new FriendsService();
            UsersService usersService = new UsersService();

            string userName = Context.User.Identity.Name;

            // get friend count
            int friendRequestCount = friendsService.GetFriendRequestCount(userId);

            // set clients
            var clients = Clients.All;

            // call js function
            clients.friendRequestCount(userName, friendRequestCount);
        }
    }
}