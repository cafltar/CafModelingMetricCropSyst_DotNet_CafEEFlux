using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    /// <summary>
    /// Represents an EEFlux image URL
    /// </summary>
    public class EEFluxImage : IEquatable<EEFluxImage>
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EEFluxImage);
        }

        public bool Equals(EEFluxImage other)
        {
            return other != null &&
                   Url == other.Url;
        }

        public override int GetHashCode()
        {
            return -1915121810 + EqualityComparer<string>.Default.GetHashCode(Url);
        }

        public static bool operator ==(EEFluxImage image1, EEFluxImage image2)
        {
            return EqualityComparer<EEFluxImage>.Default.Equals(image1, image2);
        }

        public static bool operator !=(EEFluxImage image1, EEFluxImage image2)
        {
            return !(image1 == image2);
        }
    }
}
