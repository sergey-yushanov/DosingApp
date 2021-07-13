using DosingApp.Models.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Services
{
    public class DeserializeService
    {
        void commonJSON()
        {

        }

        void dispenserCollectorJSON()
        {

        }

        void messageContainsKeys(IncomingMessage incomingMessage)
        {
            if (incomingMessage.Common != null)
            {
                Console.WriteLine("Common not null");
            }
            else
            {
                Console.WriteLine("Common is null");
            }
            //if (incomingMessage )
        }
    }
}
