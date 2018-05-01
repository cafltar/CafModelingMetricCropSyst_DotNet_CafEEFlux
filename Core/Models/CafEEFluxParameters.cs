using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    public class CafEEFluxParameters
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public double CloudinessThreshold { get; private set; }
        public int TierThreshold { get; private set; }

        public CafEEFluxParameters(
            double latitude,
            double longitude,
            DateTime startDate,
            DateTime endDate,
            double cloudinessThreshold,
            int tierThreshold)
        {
            Latitude = latitude;
            Longitude = longitude;
            StartDate = startDate;
            EndDate = endDate;
            CloudinessThreshold = cloudinessThreshold;
            TierThreshold = tierThreshold;
        }
    }
}
