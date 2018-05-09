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

namespace Caf.CafModelingMetricCropSyst.Cli
{
    /// <summary>
    /// Represents the application doer -- it does it
    /// </summary>
    public class Engine
    {
        private readonly IEEFluxClient<HttpResponseMessage> client;
        private readonly CommandLineApplication app;

        public Engine(
            IEEFluxClient<HttpResponseMessage> fluxClient,
            CommandLineApplication commandLineApplication)
        {
            this.client = fluxClient;
            this.app = commandLineApplication;
        }

        public void Execute(string[] args)
        {
            setupCommandLineInterface();
            app.Execute(args);
        }

        private void setupCommandLineInterface()
        {
            app.Command("get", (command) =>
            {
                command.Description = "Get EEFlux images";
                command.HelpOption("-?|-h|--help");

                var latitudeOption = command.Option(
                    "--lat",
                    "Latitude (decimal degrees) for image location",
                    CommandOptionType.SingleValue)
                    .IsRequired();

                var longitudeOption = command.Option(
                    "--lon",
                    "Longitude (decimal degrees) for image location",
                    CommandOptionType.SingleValue).IsRequired();

                var startDateOption = command.Option(
                   "--startdate",
                   "Starting date to get images; in form of yyyyMMdd",
                   CommandOptionType.SingleValue)
                   .IsRequired();

                var endDateOption = command.Option(
                   "--enddate",
                   "Ending date to get images; in form of yyyyMMdd",
                   CommandOptionType.SingleValue).IsRequired();

                var cloudinessThresholdOption = command.Option(
                   "--cloudiness",
                   "Percent cloudiness value (0-100), images with value above specified value will be excluded from download",
                   CommandOptionType.SingleValue);

                var tierThresholdOption = command.Option(
                   "--tier",
                   "Tier value threshold (1,2), images with values above specified value will be excluded from downloaded",
                   CommandOptionType.SingleValue)
                   .Accepts(v => v.Values("1", "2"));

                var writeFilePath = command.Option(
                    "--writepath",
                    "Absolute or relative path to write the files to",
                    CommandOptionType.SingleValue)
                    .Accepts(v => v.ExistingDirectory());

                command.OnExecute(() =>
                {
                    // TODO: Create a parameter builder...
                    CafEEFluxParameters parameters = new CafEEFluxParameters(
                        Convert.ToDouble(latitudeOption.Value()),
                        Convert.ToDouble(longitudeOption.Value()),
                        DateTime.ParseExact(
                            startDateOption.Value(), 
                            "yyyyMMdd", 
                            CultureInfo.InvariantCulture),
                        DateTime.ParseExact(
                            endDateOption.Value(), 
                            "yyyyMMdd", 
                            CultureInfo.InvariantCulture),
                        Convert.ToDouble(cloudinessThresholdOption.Value()),
                        Convert.ToInt16(tierThresholdOption.Value()),
                        writeFilePath.Value().ToString());

                    Get(parameters).Wait();

                    return 0;
                });
            });
        }

        private async Task<int> Get(CafEEFluxParameters parameters)
        {
            Console.WriteLine("Getting images...");

            Dictionary<int, EEFluxImageMetadata> imageMetas = 
                await client.GetImageMetadataAsync(parameters);

            Console.WriteLine($"Found {imageMetas.Count} landsat images.");

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

            if(!Prompt.GetYesNo("Would you like to proceed?",true))
            {
                return 0;
            }

            Directory.CreateDirectory(writeDirPath);

            // TODO: This should loop over a list generated from params
            foreach(var type in Enum.GetValues(typeof(EEFluxImageTypes)))
            {
                Console.WriteLine($"Working on {type.ToString()}...");

                // TODO: Move this to a new function to clean up code
                foreach(var image in imageMetas)
                {

                    // TODO: Filter by cloudiness and tiers (or create a new list and work on that) (or a function in EEFluxClientWebApi?  Maybe this isn't it's responsibility?)
                    string imageId = image.Value.ImageId;

                    Dictionary<EEFluxImageTypes, EEFluxImage> eeFluxImageUrl =
                        await client.GetImageUriAsync(
                            parameters,
                            imageId,
                            (EEFluxImageTypes)type);
                    Uri imageUrl =
                        new Uri(eeFluxImageUrl[(EEFluxImageTypes)type].Url);

                    Console.WriteLine($"  Downloading: {imageUrl}... ");

                    Task<HttpResponseMessage> download = 
                        client.DownloadImageAsync(imageUrl);
                    while(!download.IsCompleted)
                    {
                        Thread.Sleep(5000);
                        Console.Write(".");
                    }
                    var response = await download;

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
    }
}
