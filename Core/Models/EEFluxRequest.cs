using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    public class EEFluxRequest
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lon { get; set; }

        [JsonProperty("date_info")]
        public string Date { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }
    }
}
