using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EEFluxImageTypes
    {
        [EnumMember(Value = "ndvi")]
        Ndvi,
        [EnumMember(Value = "etof")]
        Etof,
        [EnumMember(Value = "eta")]
        Eta
    }
}
