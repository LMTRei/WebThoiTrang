using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebThoiTrang.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string user, string message)
        {
            Clients.All.receiveMessage(user, message);
        }
    }
}