using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.Common.Models
{
    public class PowerMessage : Message
    {
        /// <summary>
        /// Meter Id
        /// </summary>
        public string MeterId { get; set; }

        /// <summary>
        /// Account intensity in A
        /// </summary>
        public int AccountIntensity { get; set; }

        /// <summary>
        /// Peek hour compteur in Wh
        /// </summary>
        public decimal PeekHourCpt { get; set; }

        /// <summary>
        /// Low hour compteur in Wh
        /// </summary>
        public decimal LowHourCpt { get; set; }

        /// <summary>
        /// Actual intensity used in A
        /// </summary>
        public int ActualIntensity { get; set; }

        /// <summary>
        /// Max intensity used from the start of the Meter use in A
        /// </summary>
        public int MaxIntensity { get; set; }

        /// <summary>
        /// Aparrent power in VA
        /// </summary>
        public int ApparentPower { get; set; }

        public bool HasExceed { get; set; }
    }
}
