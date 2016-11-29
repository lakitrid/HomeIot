using Fr.Lakitrid.CentralBrain.Models;
using Fr.Lakitrid.Common.Models;
using System;

namespace Fr.Lakitrid.CentralBrain.Services
{
    internal class CentralService
    {
        BusReader<PowerMessage> _powerMessageReader;

        public CentralService()
        {
            BusParameters parameters = new BusParameters();

            this._powerMessageReader = new BusReader<PowerMessage>(parameters);
        }

        internal void Start()
        {
            this._powerMessageReader.Start();
        }

        internal void Stop()
        {
            this._powerMessageReader.Stop();
        }
    }
}