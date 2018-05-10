using System;
using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    /// <summary>
    /// Represents the application doer -- it does it
    /// </summary>
    public class Engine
    {
        private readonly IEEFluxClient<HttpResponseMessage> client;
        private readonly CommandLineApplication app;
        private readonly IParseParameters<CommandOption> parameterParser;

        public Engine(
            IEEFluxClient<HttpResponseMessage> fluxClient,
            CommandLineApplication commandLineApplication,
            IParseParameters<CommandOption> parameterParser)
        {
            this.client = fluxClient;
            this.app = commandLineApplication;
            this.parameterParser = parameterParser;   
        }

        public void Execute(string[] args)
        {
            setupCommandLineInterface();
            app.Execute(args);
        }

        private void setupCommandLineInterface()
        {
            var latitudeOption = app.Option<Double>(
                "--lat",
                "Latitude (decimal degrees) for image location",
                CommandOptionType.SingleValue)
                .IsRequired();

            var longitudeOption = app.Option<Double>(
                "--lon",
                "Longitude (decimal degrees) for image location",
                CommandOptionType.SingleValue).IsRequired();

            var startDateOption = app.Option(
                "--startdate",
                "Starting date to get images; in form of yyyyMMdd",
                CommandOptionType.SingleValue)
                .IsRequired();

            var endDateOption = app.Option(
                "--enddate",
                "Ending date to get images; in form of yyyyMMdd",
                CommandOptionType.SingleValue).IsRequired();

            var cloudinessThresholdOption = app.Option<Double>(
                "--cloudiness",
                "Percent cloudiness value (0-100), images with value above specified value will be excluded from download",
                CommandOptionType.SingleValue);

            var tierThresholdOption = app.Option(
                "--tier",
                "Tier value threshold (1,2), images with values above specified value will be excluded from downloaded",
                CommandOptionType.SingleValue)
                .Accepts(v => v.Values("1", "2"));

            var writeFilePathOption = app.Option(
                "--writepath",
                "Absolute or relative path to write the files to.  If not specified, images will be downloaded to current working directory",
                CommandOptionType.SingleValue);

            var imageTypesOption = app.Option(
                "--imagetypes",
                "Comma separated list of image types to download. Quotes are required. [true_color, false_color_4, false_color_7, albedo, ndvi, dem, land_use, lst, etr24, eto24, etrf, etof, eta]",
                CommandOptionType.SingleValue);

            var isQuietMode = app.Option(
                "-q|--quiet",
                "Sets quiet mode, meaning no user interaction.  Default is true. [true, false']",
                CommandOptionType.SingleValue)
                .Accepts(v => v.Values("T", "F"));

            app.OnExecute(() =>
            {
                var parameters = parameterParser.Parse(app.GetOptions());
                GetImages(parameters).Wait();

                return 0;
            });
        }

        public async Task<int> GetImages(CafEEFluxParameters parameters)
        {
            Console.WriteLine("Getting images...");

            Dictionary<int, EEFluxImageMetadata> imageMetas = 
                await client.GetImageMetadataAsync(parameters);

            Console.WriteLine($"Found {imageMetas.Count} landsat images.");

            // TODO: Filter by cloudiness, tier, landsat8
            var imageMetasFiltered =
                filterImages(
                    imageMetas,
                    parameters.CloudinessThreshold,
                    parameters.TierThreshold,
                    true);

            //Console.WriteLine($"After filtering, {imageMetasFiltered.Count} images remain");
            string writeDirPath;
            if (Path.IsPathRooted(parameters.OutputDirectoryPath))
            {
                writeDirPath = parameters.OutputDirectoryPath;
            }
            else
            {
                writeDirPath = 
                    Path.GetFullPath(parameters.OutputDirectoryPath);
            }

            Console.WriteLine($"Images will be saved to: {writeDirPath}");

            if (!parameters.IsQuietMode)
            {
                if (!Prompt.GetYesNo("Would you like to proceed?", true))
                {
                    return 0;
                }
            }

            Directory.CreateDirectory(writeDirPath);

            foreach(EEFluxImageTypes type in parameters.ImageTypes)
            {

                Console.WriteLine($"Working on {type.ToString()}...");

                // TODO: Move this to a new function to clean up code
                foreach(var image in imageMetasFiltered)
                {

                    string imageId = image.Value.ImageId;

                    // Check if image already downloaded
                    if (doesImageFileExist(
                        imageId,
                        type.ToString(),
                        writeDirPath))
                    {
                        Console.WriteLine($"  File {imageId}_{type.ToString()} found, skipping download.");
                        continue;
                    }

                    Dictionary<EEFluxImageTypes, EEFluxImage> eeFluxImageUrl =
                        await client.GetImageUriAsync(
                            parameters,
                            imageId,
                            type);
                    Uri imageUrl =
                        new Uri(eeFluxImageUrl[type].Url);

                    Console.WriteLine($"  Downloading: {imageUrl}... ");

                    Task<HttpResponseMessage> download = 
                        client.DownloadImageAsync(imageUrl);

                    while (!download.IsCompleted)
                    {
                        Thread.Sleep(5000);
                        Console.Write(".");
                    }
                    var response = await download;

                    if(response == null)
                    {
                        Console.WriteLine("    Error downloading and/or saving file");
                        continue;
                    }
                    string fileName = 
                        response.Content.Headers.ContentDisposition.FileName;
                    string filePath =
                        $"{writeDirPath}\\" +
                        $"{fileName}";

                    Console.WriteLine($"    Saving to: {filePath}... ");

                    using (Stream readStream = 
                        await response.Content.ReadAsStreamAsync())
                    {
                        using (Stream writeStream = File.Open(
                            filePath, 
                            FileMode.Create))
                        {
                            await readStream.CopyToAsync(writeStream);
                        }
                    }
                }
            }

            return 0;
        }

        private bool doesImageFileExist(
            string imageName,
            string fileType,
            string outputDir)
        {
            var files = Directory.GetFiles(outputDir);
            var dirs = Directory.GetDirectories(outputDir);

            string name = $"{imageName}_{fileType}".ToUpper();

            if(files.Length > 0)
            {
                if(files.Where(
                    f => f.Contains($"{name}.zip")).ToList().Count > 0)
                {
                    return true;
                }
            }
            if(dirs.Length > 0)
            {
                if(dirs.Where(
                    d => d.Contains(name)).ToList().Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private Dictionary<int, EEFluxImageMetadata> filterImages(
            Dictionary<int, EEFluxImageMetadata> imageMetas,
            double minAcceptedCloudPercent,
            int minAcceptedTierLevel,
            bool shouldOnlyAcceptLandsat8)
        {
            var response = new Dictionary<int, EEFluxImageMetadata>();

            foreach (var item in imageMetas)
            {
                var v = item.Value;
                bool isAcceptable =
                    isAcceptableCloudiness(
                        v.PercentCloudCover,
                        minAcceptedCloudPercent)
                    && isAcceptableTier(
                        v.Tier,
                        minAcceptedTierLevel)
                    && isAcceptableSatellite(v.ImageId);

                if(isAcceptable)
                {
                    response.Add(item.Key, item.Value);
                }
            }

            return response;
        }

        private bool isAcceptableCloudiness(
            double value,
            double threshold)
        {
            if (value > threshold) return false;

            return true;
        }

        private bool isAcceptableTier(
            string value,
            int threshold)
        {
            // Filters out RT; will cause issues if I implement R2 downloading
            if (value != "T1" && value != "T2")
                return false;

            if (Convert.ToInt32(value[1].ToString()) > threshold)
                return false;

            return true;
        }

        private bool isAcceptableSatellite(
            string imageId)
        {
            if (imageId[2].ToString() != "8")
                return false;

            return true;
        }
    }
}
