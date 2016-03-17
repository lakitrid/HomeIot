using Common.Domain;
using HomeBrain.DataAccess;
using HomeBrain.Services;
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
        /// <summary>
        /// Job that process the task to apply on the current hour and minute.
        /// The job launch the command whether the command has already been issued or not.
        /// It's the responsibility of the receveir to check if the command must be applied at each
        /// call.
        /// </summary>
        /// <param name="context">Job context</param>
        public void Execute(IJobExecutionContext context)
        {
            DateTime currentDate = this.GetCurrentDate();
            TypeOfDay currentTypeOfDay = this.GetTypeOfDay(currentDate);

            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Get all active tasks (specific or not)
            IQueryable<TaskInfo> tasks = dbContext.Tasks
                .Where(e => e.Active &&
                    (!e.IsSpecificTask ||
                        (e.IsSpecificTask && currentDate.Date >= e.StartDate && currentDate <= e.EndDate)));

            // Filter the tasks that must applies on this day
            tasks = tasks.Where(e => e.TypeOfDay.Equals(currentTypeOfDay) || e.TypeOfDay.Equals(TypeOfDay.Any));

            // Filter the tasks that must applies on this hour and minute
            tasks = tasks.Where(e => (currentDate.Date.Add(e.Start) > currentDate
                    && currentDate.Date.Add(e.Start + e.Duration) < currentDate)
                || e.isFullDay
                );

            List<Command> commands = dbContext.Commands.Where(e => tasks.Any(f => f.Id == e.Task.Id)).ToList();

            if(!new QueueWriter().Publish(commands))
            {
                AlertService.Instance.SendAlert($"Error while sending commands");
            }
        }

        /// <summary>
        /// Determines the type of day (week day or week end)
        /// </summary>
        /// <param name="currentDate">current date</param>
        /// <returns>type of day</returns>
        private TypeOfDay GetTypeOfDay(DateTime currentDate)
        {
            if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return TypeOfDay.WeekEnd;
            }

            return TypeOfDay.WeekDay;
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
