using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    public class EEFluxImageMetadata
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("tier")]
        public string Tier { get; set; }

        [JsonProperty("id")]
        public string ImageId { get; set; }

        [JsonProperty("cloud")]
        public double PercentCloudCover { get; set; }
    }
}
