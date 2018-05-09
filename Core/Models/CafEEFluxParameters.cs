using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    /// <summary>
    /// Represents all parameters needed for the application to run.
    /// </summary>
    public class CafEEFluxParameters
    {
        /// <summary>
        /// Decimal degrees
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Decimal degrees
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// Date to begin searching for available images for download
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Date to end searching for available images for download
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Percent of cloud cover for the given image (0-100)
        /// </summary>
        public double CloudinessThreshold { get; private set; }

        /// <summary>
        /// Tier quality (1, 2)
        /// <note>https://landsat.usgs.gov/what-are-landsat-collection-1-tiers</note>
        /// TODO: Change this to either an enum or a string to handle Real-Time or "RT" values
        /// </summary>
        public int TierThreshold { get; private set; }

        /// <summary>
        /// Path to save downloaded images to
        /// </summary>
        public string OutputDirectoryPath { get; private set; }
        
        public List<EEFluxImageTypes> ImageTypes { get; private set; }

        public CafEEFluxParameters(
            double latitude,
            double longitude,
            DateTime startDate,
            DateTime endDate,
            double cloudinessThreshold,
            int tierThreshold,
            string outputDirectoryPath,
            List<EEFluxImageTypes> imageTypes)
        {
            Latitude = latitude;
            Longitude = longitude;
            StartDate = startDate;
            EndDate = endDate;
            CloudinessThreshold = cloudinessThreshold;
            TierThreshold = tierThreshold;
            OutputDirectoryPath = outputDirectoryPath;
            ImageTypes = imageTypes;
        }
    }
}
