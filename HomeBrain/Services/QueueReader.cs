using log4net;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace HomeBrain.Services
{
    internal class QueueReader
    {
        private string _queue;
        private string _server;
        private ILog _logger;

        private Task _reader;
        private CancellationTokenSource _token;
        private bool _isOpen;
        private int _waitOnFailed;
        private ConnectionFactory _factory;
        private IModel _channel;
        private QueueingBasicConsumer _consumer;
        private int _waitQueueTimeout;

        internal QueueReader(string queue)
        {
            this._queue = queue;

            this._waitOnFailed = int.Parse(ConfigurationManager.AppSettings["WaitOnConnectionFailed"]);
            this._waitQueueTimeout = int.Parse(ConfigurationManager.AppSettings["WaitQueueTimeout"]);
            this._server = ConfigurationManager.AppSettings["queueServer"];

            this._token = new CancellationTokenSource();
            this._reader = new Task(this.Reader, this._token.Token);

            this._factory = new ConnectionFactory();
            this._factory.HostName = this._server;
        }

        internal void Start()
        {
            this._reader.Start();
        }

        internal void Stop()
        {
            this._token.Cancel();
            this._reader.Wait();
            this._channel.Close();
        }

        private void Reader()
        {
            while (!this._token.IsCancellationRequested)
            {
                if (!this._isOpen)
                {
                    this.Open();
                }

                if (this._isOpen)
                {
                    BasicDeliverEventArgs message;
                    this._consumer.Queue.Dequeue(this._waitQueueTimeout, out message);

                    if (MessageService.Instance.ManageMessage(message.Body))
                    {
                        this._channel.BasicAck(message.DeliveryTag, false);
                    }
                    else
                    {
                        this._channel.BasicNack(message.DeliveryTag, false, true);
                    }
                }
                else
                {
                    Thread.Sleep(this._waitOnFailed);
                }
            }
        }

        private void Open()
        {
            try
            {
                IConnection connection = this._factory.CreateConnection();
                this._channel = connection.CreateModel();
                this._channel.BasicQos(100, 100, true);
                this._consumer = new QueueingBasicConsumer(this._channel);
                this._channel.BasicConsume(this._queue, false, this._consumer);

                this._isOpen = true;
            }
            catch (Exception exc)
            {
                this._isOpen = false;
            }
        }
    }
}
