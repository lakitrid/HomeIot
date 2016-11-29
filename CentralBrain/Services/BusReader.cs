using Fr.Lakitrid.CentralBrain.Models;
using Fr.Lakitrid.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fr.Lakitrid.CentralBrain.Services
{
    public class BusReader<T> where T : Message
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private bool _stopping;

        public BusReader(BusParameters parameters)
        {
            this._factory = new ConnectionFactory()
            {
                HostName = "192.168.1.110"
            };
        }

        internal void Start()
        {
            this.CreateConnection();

        }

        internal void Stop()
        {
            this._stopping = true;
            this.CloseConnection();
        }

        private void CreateConnection()
        {
            this._connection = this._factory.CreateConnection();
            this._channel = this._connection.CreateModel();

            this._channel.BasicQos(0, 100, true);

            var consumer = new EventingBasicConsumer(this._channel);
            consumer.Received += Consumer_Received;
            consumer.Shutdown += Consumer_Shutdown;

            this._channel.BasicConsume("GTLNode-Power", false, consumer);
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            if (!this._stopping)
            {
                Thread.Sleep(2000);

                this.CloseConnection();
                this.CreateConnection();
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Body);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            settings.Converters.Add(new StringEnumConverter());

            JsonSerializer serializer = JsonSerializer.Create(settings);

            try
            {
                T value = serializer.Deserialize<T>(new JsonTextReader(new StringReader(data)));

                if (MessageService.Instance.Manage(value))
                {
                    this._channel.BasicAck(e.DeliveryTag, false);
                }
                else
                {
                    this._channel.BasicNack(e.DeliveryTag, false, true);
                }
            }
            catch(Exception exc)
            {
                this._channel.BasicAck(e.DeliveryTag, false);
            }
        }

        private void CloseConnection()
        {
            if (this._channel != null && this._channel.IsOpen)
            {
                this._channel.Close();
                this._connection.Close();
            }
        }
    }
}
