using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    /// <summary>
    /// Represents types of images that can be downloaded from EEFlux
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EEFluxImageTypes
    {
        [EnumMember(Value = "true_color")]
        TrueColor,
        [EnumMember(Value = "false_color_4")]
        FalseColor4,
        [EnumMember(Value = "false_color_7")]
        FalseColor7,
        [EnumMember(Value = "albedo")]
        Albedo,
        [EnumMember(Value = "ndvi")]
        Ndvi,
        [EnumMember(Value = "dem")]
        Dem,
        [EnumMember(Value = "land_use")]
        LandCover,
        [EnumMember(Value = "lst")]
        LandSurfaceTemperature,
        [EnumMember(Value = "etr24")]
        AlfalfaReferenceET,
        [EnumMember(Value = "eto24")]
        GrassReferenceET,
        [EnumMember(Value = "etrf")]
        ETrF,
        [EnumMember(Value = "etof")]
        Etof,
        [EnumMember(Value = "eta")]
        Eta
    }
}
