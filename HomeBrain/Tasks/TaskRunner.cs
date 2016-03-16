using Common.Domain;
using HomeBrain.DataAccess;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.Tasks
{
    [DisallowConcurrentExecution]
    internal class TaskRunner : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DateTime currentDate = this.GetCurrentDate();
            TypeOfDay currentTypeOfDay = this.GetTypeOfDay();

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Get all active tasks (specific or not)
            IQueryable<TaskInfo> tasks = dbContext.Tasks
                .Where(e => e.Active &&
                    (!e.IsSpecificTask ||
                        (e.IsSpecificTask && currentDate.Date >= e.StartDate && currentDate <= e.EndDate)));

            tasks = tasks.Where(e => e.TypeOfDay.Equals(currentTypeOfDay) || e.TypeOfDay.Equals(TypeOfDay.Any));

        }

        private TypeOfDay GetTypeOfDay()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Extracts the date with hour and minutes
        /// </summary>
        /// <returns>Date to the minute</returns>
        private DateTime GetCurrentDate()
        {
            DateTime date = DateTime.Now;

            DateTime current = date.Date.AddHours(date.Hour).AddMinutes(date.Minute);

            return current;
        }
    }
}
