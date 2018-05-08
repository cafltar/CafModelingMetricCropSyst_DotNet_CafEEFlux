using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Infrastructure
{
    public class MapImageTypeToUrlPath
    {
        public Dictionary<EEFluxImageTypes, string> map { get; private set; }

        /// <summary>
        /// Constructor creates a default map
        /// </summary>
        public MapImageTypeToUrlPath()
        {
            map = new Dictionary<EEFluxImageTypes, string>();

            map.Add(EEFluxImageTypes.TrueColor, "true_color_download");
            map.Add(EEFluxImageTypes.FalseColor4, "false_color_4_download");
            map.Add(EEFluxImageTypes.FalseColor7, "false_color_7_download");
            map.Add(EEFluxImageTypes.Albedo, "download_albedo");
            map.Add(EEFluxImageTypes.Ndvi, "download_ndvi");
            map.Add(EEFluxImageTypes.Dem, "download_dem");
            map.Add(EEFluxImageTypes.LandCover, "download_LU");
            map.Add(EEFluxImageTypes.LandSurfaceTemperature, "download_LST");
            map.Add(EEFluxImageTypes.AlfalfaReferenceET, "download_etr");
            map.Add(EEFluxImageTypes.GrassReferenceET, "download_eto");
            map.Add(EEFluxImageTypes.ETrF, "download_etrf");
            map.Add(EEFluxImageTypes.Etof, "download_etof");
            map.Add(EEFluxImageTypes.Eta, "download_eta");
        }
    }
}
