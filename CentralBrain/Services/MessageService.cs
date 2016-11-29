using Fr.Lakitrid.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.CentralBrain.Services
{
    internal sealed class MessageService
    {
        private static readonly MessageService InstanceObj = new MessageService();

        private MessageService()
        {

        }

        public static MessageService Instance { get { return InstanceObj; } }

        internal bool Manage(Message data)
        {
            if(data is PowerMessage)
            {
                return this.Manage((PowerMessage)data);
            }

            return true;
        }

        internal bool Manage(PowerMessage message)
        {
            PowerDay data = new PowerDay()
            {
                Date = message.Date,
                PeekHourIndex = message.PeekHourCpt,
                LowHourIndex = message.LowHourCpt
            };

            BrainDbContext context = new BrainDbContext();

            context.PowerDay.Add(data);

            context.SaveChanges();

            context.Dispose();

            return true;
        }
    }
}
