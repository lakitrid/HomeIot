using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain
{
    /// <summary>
    /// Defines a task to execute :
    /// The task mapping is :
    /// - the start timespan describes the time between the start of the day and the start of the task
    /// - the duration is from the start of task, and it can't be longer thant the end of the day.
    /// So the task can't be on two days.
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Gets o sets the Id of the task
        /// </summary>
        public decimal Id { get; set; }

        /// <summary>
        /// Gets or sets the code of the task
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the tasks is a specific task that will overrides non
        /// specific task of the same time on the same planning
        /// </summary>
        public bool IsSpecificTask { get; set; }

        /// <summary>
        /// Gets or sets the type of day
        /// </summary>
        public TypeOfDay TypeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the start date (used only for specific tasks)
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the start date (used only for specific tasks)
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets whether the task run on all the day
        /// </summary>
        public bool isFullDay { get; set; }

        /// <summary>
        /// Gets or sets the start time of the task from the beginning of the day
        /// </summary>
        public TimeSpan Start { get; set; }

        /// <summary>
        /// Gets or sets the duration of the task
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets whether the task is active
        /// </summary>
        public bool Active { get; set; }
    }
}
