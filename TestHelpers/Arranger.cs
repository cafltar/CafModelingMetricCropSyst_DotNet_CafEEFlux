using Caf.CafModelingMetricCropSyst.Core.Models;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.TestUtils
{
    public static class Arranger
    {
        public static List<CommandOption> GetCommandOptionsListValid()
        {
            List<CommandOption> options = new List<CommandOption>();

            var lat = new CommandOption("--lat", CommandOptionType.SingleValue);
            lat.TryParse("46.84176076135668");

            var lon = new CommandOption("--lon", CommandOptionType.SingleValue);
            lon.TryParse("-118.26578445732594");

            var start = new CommandOption("--startdate", CommandOptionType.SingleValue);
            start.TryParse("20150601");

            var end = new CommandOption("--enddate", CommandOptionType.SingleValue);
            end.TryParse("20150605");

            var cloud = new CommandOption("--cloudiness", CommandOptionType.SingleValue);
            cloud.TryParse("30");

            var tier = new CommandOption("--tier", CommandOptionType.SingleValue);
            tier.TryParse("1");

            var writepath = new CommandOption(
                "--writepath", 
                CommandOptionType.SingleValue);
            writepath.TryParse(@"Output");

            var images = new CommandOption(
                "--imagetypes",
                CommandOptionType.SingleValue);
            images.TryParse("ndvi, etof, eta");

            var quiet = new CommandOption(
                "--quiet",
                CommandOptionType.SingleOrNoValue);

            options.Add(lat);
            options.Add(lon);
            options.Add(start);
            options.Add(end);
            options.Add(cloud);
            options.Add(tier);
            options.Add(writepath);
            options.Add(images);
            options.Add(quiet);

            return options;
        }

        public static List<CommandOption> ResetCommandOption(
            List<CommandOption> originalList,
            string longName,
            string newValue = null)
        {
            List<CommandOption> newList = new List<CommandOption>(originalList);

            newList.Remove(newList.Single(o => o.LongName == longName));

            CommandOption c = new CommandOption($"--{longName}", CommandOptionType.SingleValue);

            if (!String.IsNullOrEmpty(newValue)) c.TryParse(newValue);

            newList.Add(c);

            return newList;
        }

        public static CafEEFluxParameters GetCafEEFluxParametersValid()
        {
            CafEEFluxParameters parameters = new CafEEFluxParameters(
                47.4395843855568,
                -118.35367508232594,
                new DateTime(2015, 06, 01),
                new DateTime(2015, 06, 05),
                30,
                1,
                "/Output/test.zip",
                new List<EEFluxImageTypes>() { EEFluxImageTypes.Eta });

            return parameters;
        }

        public static Dictionary<int, EEFluxImageMetadata> GetEEFluxImageMetadataValid()
        {
            Dictionary<int, EEFluxImageMetadata> imageMetas = new Dictionary<int, EEFluxImageMetadata>();

            imageMetas.Add(0, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 01),
                ImageId = "LE70440272015152EDC00",
                Tier = "T1",
                PercentCloudCover = 88.0
            });
            imageMetas.Add(1, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430272015153LGN01",
                Tier = "T1",
                PercentCloudCover = 90.540000000000006
            });
            imageMetas.Add(2, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430282015153LGN01",
                Tier = "T1",
                PercentCloudCover = 55.759999999999998
            });
            return imageMetas;
        }
        public static Dictionary<int, EEFluxImageMetadata> GetEEFluxImageMetadataOneNoClouds()
        {
            Dictionary<int, EEFluxImageMetadata> imageMetas = new Dictionary<int, EEFluxImageMetadata>();

            imageMetas.Add(0, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 01),
                ImageId = "LE70440272015152EDC00",
                Tier = "T1",
                PercentCloudCover = 88.0
            });
            imageMetas.Add(1, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430272015153LGN01",
                Tier = "T1",
                PercentCloudCover = 90.540000000000006
            });
            imageMetas.Add(2, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430282015153LGN01",
                Tier = "T1",
                PercentCloudCover = 0.0
            });
            return imageMetas;
        }
        public static Dictionary<int, EEFluxImageMetadata> GetEEFluxImageMetadataOneTier2()
        {
            Dictionary<int, EEFluxImageMetadata> imageMetas = new Dictionary<int, EEFluxImageMetadata>();

            imageMetas.Add(0, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 01),
                ImageId = "LE70440272015152EDC00",
                Tier = "T1",
                PercentCloudCover = 88.0
            });
            imageMetas.Add(1, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430272015153LGN01",
                Tier = "T1",
                PercentCloudCover = 90.540000000000006
            });
            imageMetas.Add(2, new EEFluxImageMetadata()
            {
                Date = new DateTime(2015, 06, 02),
                ImageId = "LC80430282015153LGN01",
                Tier = "T2",
                PercentCloudCover = 55.759999999999998
            });
            return imageMetas;
        }

        public static Dictionary<EEFluxImageTypes, EEFluxImage> 
            GetEEFluxResponseDownloadEtofValid()
        {
            Dictionary<EEFluxImageTypes, EEFluxImage> images = 
                new Dictionary<EEFluxImageTypes, EEFluxImage>();
            images.Add(
                EEFluxImageTypes.Etof,
                new EEFluxImage()
                {
                    Url = "https://earthengine.googleapis.com/api/download?docid=6277fdbbcb5e4e0bc2f2d5562f4d1c4a&token=da96164b598072a0163f34ceaac8f5b6"
                });
            return images;
        }
        public static Dictionary<EEFluxImageTypes, EEFluxImage> 
            GetEEFluxResponseDownloadEtaValid()
        {
            Dictionary<EEFluxImageTypes, EEFluxImage> images = 
                new Dictionary<EEFluxImageTypes, EEFluxImage>();
            images.Add(
                EEFluxImageTypes.Eta,
                new EEFluxImage()
                {
                    Url = "https://earthengine.googleapis.com/api/download?docid=6277fdbbcb5e4e0bc2f2d5562f4d1c4a&token=da96164b598072a0163f34ceaac8f5b6"
                });
            return images;
        }
    }
}
