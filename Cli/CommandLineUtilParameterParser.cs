using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using System.Linq;
using System.Globalization;
using System.IO;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    public class CommandLineUtilParameterParser : IParseParameters<CommandOption>
    {
        public CafEEFluxParameters Parse(
            IEnumerable<CommandOption> parameters)
        {
            var lat = parameters.SingleOrDefault(
                o => o.LongName == "lat");
            var lon = parameters.SingleOrDefault(
                o => o.LongName == "lon");
            var start = parameters.SingleOrDefault(
                o => o.LongName == "startdate");
            var end = parameters.SingleOrDefault(
                o => o.LongName == "enddate");
            var cloud = parameters.SingleOrDefault(
                o => o.LongName == "cloudiness");
            var tier = parameters.SingleOrDefault(
                o => o.LongName == "tier");
            var path = parameters.SingleOrDefault(
                o => o.LongName == "writepath");
            var images = parameters.SingleOrDefault(
                o => o.LongName == "imagetypes");
            var quiet = parameters.SingleOrDefault(
                o => o.LongName == "quiet");

            double latv = lat.HasValue() 
                ? Convert.ToDouble(lat.Value()) 
                : throw new ArgumentException();
            double lonv = lon.HasValue() 
                ? Convert.ToDouble(lon.Value()) 
                : throw new ArgumentException();
            DateTime startv = end.HasValue() 
                ? DateTime.ParseExact(start.Value(), "yyyyMMdd", CultureInfo.InvariantCulture) 
                : throw new ArgumentException();
            DateTime endv = end.HasValue() 
                ? DateTime.ParseExact(end.Value(), "yyyyMMdd", CultureInfo.InvariantCulture) 
                : throw new ArgumentException();
            double cloudv = cloud.HasValue()
                ? Convert.ToDouble(cloud.Value())
                : 100;
            int tierv = tier.HasValue()
                ? Convert.ToInt32(tier.Value())
                : 1;
            string pathv = path.HasValue()
                ? path.Value()
                : Directory.GetCurrentDirectory();
            List<EEFluxImageTypes> imagev = images.HasValue()
                ? parseImageTypes(images.Value())
                : new List<EEFluxImageTypes>();
            bool quietv = quiet.HasValue()
                ? Boolean.Parse(quiet.Value())
                : true;

            var p = new CafEEFluxParameters(
                latv, 
                lonv, 
                startv, 
                endv, 
                cloudv, 
                tierv, 
                pathv,
                imagev,
                quietv);

            return p;
        }

        private List<EEFluxImageTypes> parseImageTypes(
            string commandOptionValue)
        {
            var result = new List<EEFluxImageTypes>();

            string[] imageStrs = commandOptionValue
                .Trim()
                .Replace(" ", "")
                .Split(",");

            foreach(var imageStr in imageStrs)
            {
                EEFluxImageTypes image =
                    (EEFluxImageTypes)System.Enum.Parse(
                        typeof(EEFluxImageTypes), imageStr, true);
                result.Add(image);
            }
            return result;
        }
    }
}
