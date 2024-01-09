using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication3
{
    public class QueueTicketHub : Hub
    {
        public static void SendUpdate(string department, string currentQueueTicket)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<QueueTicketHub>();
            context.Clients.All.UpdateQueueTicket(department, currentQueueTicket);
        }
    }
}