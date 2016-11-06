using Common.Domain;
using Common.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.Services
{
    internal class QueueWriter
    {
        private IModel _channel;
        private ConnectionFactory _factory;
        private bool _isOpen;
        private string _server;

        internal QueueWriter()
        {
            this._server = ConfigurationManager.AppSettings["queueServer"];

            this._factory = new ConnectionFactory();
            this._factory.HostName = this._server;
        }

        /// <summary>
        /// Sends commands
        /// </summary>
        /// <param name="commands">commands to sends</param>
        /// <returns>whether all the have been sent</returns>
        internal bool Publish(List<Command> commands)
        {
            bool isOk = true;
            foreach (Command command in commands)
            {
                try
                {
                    if (this._channel == null || this._channel.IsClosed)
                    {
                        this.Open();
                    }

                    if (this._channel != null && this._channel.IsOpen)
                    {
                        string bodyText = SerializeHelper.Serialize(command);
                        byte[] body = Encoding.UTF8.GetBytes(bodyText);
                        this._channel.BasicPublish("", "", null, body);
                    }
                }
                catch (Exception exc)
                {
                    isOk = false;
                }
            }

            return isOk;
        }

        private void Open()
        {
            try
            {
                IConnection connection = this._factory.CreateConnection();
                this._channel = connection.CreateModel();
            }
            catch (Exception exc)
            {
            }
        }
    }
}
