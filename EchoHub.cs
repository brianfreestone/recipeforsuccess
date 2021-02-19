using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace RecipeForSuccess_mvc
{
    [HubName("echo")]
    public class EchoHub : Hub
    {
        public void Hello(string message)
        {
            // set clients
            var allClients = Clients.All;

            // call js function
            allClients.test("this is a test");
        }
    }
}