using Common.Domain;
using Common.Services;
using HomeBrain.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.Services
{
    internal sealed class MessageService
    {
        private static readonly MessageService InstanceObj = new MessageService();

        private MessageService()
        {
        }

        public static MessageService Instance
        {
            get
            {
                return InstanceObj;
            }
        }

        public bool ManageMessage(byte[] rawMessage)
        {
            Message message = SerializeHelper.Deserialize<Message>(rawMessage);

            if (message is TeleInfoData)
            {
                this.ManageMessage(message as TeleInfoData);
            }

            return true;
        }

        private bool ManageMessage(TeleInfoData message)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                if (message != null)
                {
                    this.AddTimeSerie(message.Date, message.PeekHourCpt, "POWER_PEEK_CPT", context);
                    this.AddTimeSerie(message.Date, message.LowHourCpt, "POWER_LOW_CPT", context);
                    this.AddTimeSerie(message.Date, message.ActualIntensity, "POWER_INTENSITY", context);

                    this.UpdateDayTimeSerie(message.Date, message.PeekHourCpt, "POWER_PEEK_CPT", context);
                    this.UpdateDayTimeSerie(message.Date, message.PeekHourCpt, "POWER_LOW_CPT", context);
                }

                context.SaveChanges();
                context.Dispose();
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }

        private void UpdateDayTimeSerie(DateTime date, decimal value, string code, ApplicationDbContext context)
        {
            // Selects the current value stored for the day
            TimeSerie serie = context.DayTimeSeries.Where(e => e.Code.Equals(code) && e.Date.Equals(date.Date)).FirstOrDefault();

            // Create or update he value so that we have only one value per day.
            if (serie == null)
            {
                serie = new TimeSerie
                {
                    Date = date.Date,
                    Value = value,
                    Code = code
                };

                context.TimeSeries.Add(serie);
            }
            else
            {
                serie.Value = value;
            }
        }

        private void AddTimeSerie(DateTime date, decimal value, string code, ApplicationDbContext context)
        {
            TimeSerie timeSerie = new TimeSerie
            {
                Date = date,
                Value = value,
                Code = code
            };

            context.TimeSeries.Add(timeSerie);
        }
    }
}
