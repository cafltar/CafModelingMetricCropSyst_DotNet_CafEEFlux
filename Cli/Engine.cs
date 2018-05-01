using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using System.Globalization;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    public class Engine
    {
        private readonly IEEFluxClient fluxClient;
        private readonly IGetParameters parameterEngine;
        private readonly CommandLineApplication app;

        public Engine(
            IEEFluxClient fluxClient,
            IGetParameters parameterEngine,
            CommandLineApplication commandLineApplication)
        {
            this.fluxClient = fluxClient;
            this.parameterEngine = parameterEngine;
            this.app = commandLineApplication;
        }

        public void Execute(string[] args)
        {
            setupCli();
            app.Execute(args);
        }

        private void setupCli()
        {
            app.Command("get", (command) =>
            {
                command.Description = "Get EEFlux images";
                command.HelpOption("-?|-h|--help");

                var latitudeOption = command.Option(
                    "--lat",
                    "Latitude (decimal degrees) for image location",
                    CommandOptionType.SingleValue);

                var longitudeOption = command.Option(
                    "--lon",
                    "Longitude (decimal degrees) for image location",
                    CommandOptionType.SingleValue);

                var startDateOption = command.Option(
                   "--startdate",
                   "Starting date to get images; in form of yyyyMMdd",
                   CommandOptionType.SingleValue);

                var endDateOption = command.Option(
                   "--enddate",
                   "Ending date to get images; in form of yyyyMMdd",
                   CommandOptionType.SingleValue);

                var cloudinessThresholdOption = command.Option(
                   "--cloudiness",
                   "Percent cloudiness value (0-100), images with value above specified value will be excluded from download",
                   CommandOptionType.SingleValue);

                var tierThresholdOption = command.Option(
                   "--tier",
                   "Tier value threshold (1,2), images with values above specified value will be excluded from downloaded",
                   CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
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
                        Convert.ToInt16(tierThresholdOption.Value()));

                    fluxClient.GetImageMetadata(parameters);

                    return 0;
                });
            });
        }
    }
}
