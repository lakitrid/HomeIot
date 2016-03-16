using HomeBrain.Tasks;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.Services
{
    internal sealed class ScheduleService
    {
        private static readonly ScheduleService InstanceObj = new ScheduleService();

        private IScheduler _scheduler;

        private ScheduleService()
        {
            this._scheduler = StdSchedulerFactory.GetDefaultScheduler();
            this.InitTasks();
        }

        private void InitTasks()
        {
            IJobDetail job = JobBuilder.Create<TaskRunner>()
                .WithIdentity("taskRunner", "main")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggerRunner", "main")
                .StartNow()
                .WithDailyTimeIntervalSchedule((dailySchedule) =>
                {
                    dailySchedule.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0));
                    dailySchedule.WithIntervalInMinutes(1);
                })
                .Build();

            this._scheduler.ScheduleJob(job, trigger);
        }

        public static ScheduleService Instance
        {
            get
            {
                return InstanceObj;
            }
        }

        public void Start()
        {
            if (!this._scheduler.IsStarted)
            {
                this._scheduler.Start();
            }
        }

        public void Stop()
        {
            if (this._scheduler.IsStarted)
            {
                this._scheduler.Shutdown();
            }
        }
    }
}
