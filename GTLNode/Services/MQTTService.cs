using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace GTLNode.Services
{
    internal sealed class MQTTService
    {
        private static readonly MQTTService InstanceObj = new MQTTService();
        private readonly string ServerIp = "192.168.1.110";
        private readonly string NodeName = "GTLNode-Rpi-1";
        internal static readonly string PowerErrorRoutingKey = "GTLNode-Power-Error";
        internal static readonly string PowerRoutingKey = "GTLNode-Power";
        internal static readonly string InfoRoutingKey = "GTLNode-Info";

        private MqttClient _client;

        private MQTTService()
        {
            this._client = new MqttClient(this.ServerIp);
            Debug.WriteLine($"Openning MQTT connection");

            this._client.Connect(this.NodeName, "devices", "devices");
        }

        public static MQTTService Instance
        {
            get
            {
                return InstanceObj;
            }
        }

        public void PublishMessage(string routingKey, string data)
        {
            try
            {
                if(!this._client.IsConnected)
                {
                    this._client.Connect(this.NodeName, "devices", "devices");
                }

                Debug.WriteLine($"Publishing message");

                this._client.Publish(routingKey, Encoding.UTF8.GetBytes(data), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            }
            catch (Exception exc)
            {
                Debug.WriteLine($"Error while accessing MQTT server : {exc.Message}");
            }
        }
    }
}
