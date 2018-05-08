using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    /// <summary>
    /// Represents image metadata for a landsat 8 image via EEFlux
    /// </summary>
    public class EEFluxImageMetadata : IEquatable<EEFluxImageMetadata>
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("tier")]
        public string Tier { get; set; }

        [JsonProperty("id")]
        public string ImageId { get; set; }

        [JsonProperty("cloud")]
        public double PercentCloudCover { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EEFluxImageMetadata);
        }

        public bool Equals(EEFluxImageMetadata other)
        {
            return other != null &&
                   Date == other.Date &&
                   Tier == other.Tier &&
                   ImageId == other.ImageId &&
                   PercentCloudCover == other.PercentCloudCover;
        }

        public override int GetHashCode()
        {
            var hashCode = 289791039;
            hashCode = hashCode * -1521134295 + Date.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tier);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ImageId);
            hashCode = hashCode * -1521134295 + PercentCloudCover.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(EEFluxImageMetadata metadata1, EEFluxImageMetadata metadata2)
        {
            return EqualityComparer<EEFluxImageMetadata>.Default.Equals(metadata1, metadata2);
        }

        public static bool operator !=(EEFluxImageMetadata metadata1, EEFluxImageMetadata metadata2)
        {
            return !(metadata1 == metadata2);
        }
    }
}
