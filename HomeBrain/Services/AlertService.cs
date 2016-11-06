using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.Services
{
    /// <summary>
    /// Alert service to send alert by mail ? and log them in database ?
    /// </summary>
    internal sealed class AlertService
    {
        private static readonly AlertService InstanceObj = new AlertService();

        private AlertService()
        {
        }

        public static AlertService Instance
        {
            get
            {
                return InstanceObj;
            }
        }

        /// <summary>
        /// Sends an alert by mail
        /// </summary>
        /// <param name="message">message to send</param>
        internal void SendAlert(string message)
        {
            
        }
    }
}
