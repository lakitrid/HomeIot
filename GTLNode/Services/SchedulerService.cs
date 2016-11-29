using GTLNode.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTLNode.Services
{
    /// <summary>
    /// Scheduler service for lanching sync task on time interval
    /// </summary>
    internal sealed class SchedulerService
    {
        private static readonly SchedulerService instance = new SchedulerService();
        private Task _scheduler;
        private List<ScheduleRunner> _schedules;
        private bool toStop = false;
        private List<Task> _currentTasks;
        private readonly int _period = 10000;
        private int _periodSeconds;
        private readonly int _periodTaskCheck = 10;
        private int _taskCheckIndex;

        private SchedulerService()
        {
            this._schedules = new List<ScheduleRunner>();
            this._scheduler = new Task(() => { ExecuteSchedule(); });

            this._currentTasks = new List<Task>();

            // Initialize the duration of a period in seconds to use against the period sets dor a tasks.
            // Ideally the period of a task must be a multiple of this value.
            this._periodSeconds = this._period / 1000;
        }

        public static SchedulerService Instance { get { return instance; } }

        /// <summary>
        /// Registers an action with a periodic schedule
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="schedule">schedule</param>
        public void RegisterAction(Action action, Schedule schedule)
        {
            if (!this._schedules.Any(e => e.Schedule.Name.Equals(schedule.Name)))
            {
                ScheduleRunner runner = new ScheduleRunner
                {
                    Action = action,
                    Schedule = schedule,
                    // Initialize the delay period counts
                    PeriodsBeforeNextRun = schedule.StartDelay / this._periodSeconds
                };

                this._schedules.Add(runner);
            }
        }

        /// <summary>
        /// Starts the Scheduler
        /// </summary>
        public void Start()
        {
            this._scheduler.Start();
        }

        /// <summary>
        /// Stops the scheduler
        /// </summary>
        public void Stop()
        {
            this.toStop = true;

            Task.WaitAll(this._currentTasks.ToArray());
        }

        /// <summary>
        /// Main execution loop that will check periodically if a task must be run
        /// </summary>
        private void ExecuteSchedule()
        {
            Stopwatch sw = new Stopwatch();

            while (!toStop)
            {
                sw.Restart();

                foreach (ScheduleRunner runner in this._schedules)
                {
                    if (runner.PeriodsBeforeNextRun == 0)
                    {
                        // Checks if the action is already running
                        if (!runner.IsRunning)
                        {
                            runner.IsRunning = true;

                            Task task = new Task(() =>
                            {
                                try
                                {
                                    runner.Action.Invoke();
                                }
                                catch(Exception exc)
                                {
                                    Debug.WriteLine($"The action end badly", exc);
                                }

                                runner.IsRunning = false;
                            });                            

                            this._currentTasks.Add(task);
                            task.Start();

                            // reset the interval for the next run
                            runner.PeriodsBeforeNextRun = (runner.Schedule.Interval / this._periodSeconds);
                        }
                    }
                    else
                    {
                        runner.PeriodsBeforeNextRun--;
                    }
                }

                // Checks whether to do the task check or not
                if(this._taskCheckIndex%this._periodTaskCheck == 0)
                {
                    this.CheckTasks();
                }

                this._taskCheckIndex++;

                // Calculates the scheduler duration to remove thie value to the 1 second wait
                sw.Stop();
                int nextWait = this._period - (int)sw.ElapsedMilliseconds;

                if (nextWait > 0)
                {
                    // Wait for period duration to do the next schedule turn
                    Task wait = Task.Delay(nextWait);
                    Debug.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] period duration : [{nextWait}] ms");
                    wait.Wait();
                }
                else
                {
                    Debug.WriteLine($"Execution time too long ! [{sw.ElapsedMilliseconds} ms]");
                }
            }
        }

        /// <summary>
        /// Performs a periodic check to remove ended task from the list
        /// </summary>
        private void CheckTasks()
        {
            Task[] tasks = this._currentTasks.ToArray();

            foreach(Task task in tasks)
            {
                if(task.Status == TaskStatus.RanToCompletion || task.Status == TaskStatus.Faulted)
                {
                    this._currentTasks.Remove(task);
                }
            }
        }
    }
}
