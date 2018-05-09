using Caf.CafModelingMetricCropSyst.Core.Models;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Caf.CafModelingMetricCropSyst.Cli;
using System.Linq;

namespace Caf.CafModelingMetricCropSyst.Test
{
    public class CommandLineUtilParameterParserTest
    {
        [Fact]
        public void Parse_ValidParams_ReturnsExpected()
        {
            // ARRANGE
            var parameters = getCommandOptionsListValid();
            var sut = new CommandLineUtilParameterParser();

            // Act
            var actual = sut.Parse(parameters);

            // Assert
            Assert.Equal(46.84176076135668, actual.Latitude);
            Assert.Equal(-118.26578445732594, actual.Longitude);
            Assert.Equal(new DateTime(2015, 06, 01), actual.StartDate);
            Assert.Equal(new DateTime(2015, 06, 05), actual.EndDate);
            Assert.Equal(30, actual.CloudinessThreshold);
            Assert.Equal(1, actual.TierThreshold);
            Assert.Equal(@"C:\Dev\Projects\CafModelingMetricCropSyst\DotNet\CafEEFlux", actual.OutputDirectoryPath);
            Assert.Equal(3, actual.ImageTypes.Count);
        }

        [Fact]
        public void Parse_SingleImageType_SetsOneImageType()
        {
            // ARRANGE
            var parameters = getCommandOptionsListValid();
            parameters = resetCommandOption(parameters, "imagetypes", "etof");
            var sut = new CommandLineUtilParameterParser();
            var expected = EEFluxImageTypes.Etof;

            // ACT
            var actual = sut.Parse(parameters);

            // ASSERT
            Assert.Equal(expected, actual.ImageTypes[0]);
        }

        private List<CommandOption> getCommandOptionsListValid()
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
            writepath.TryParse(@"C:\Dev\Projects\CafModelingMetricCropSyst\DotNet\CafEEFlux");

            var images = new CommandOption(
                "--imagetypes",
                CommandOptionType.SingleValue);
            images.TryParse("ndvi, etof, eta");

            options.Add(lat);
            options.Add(lon);
            options.Add(start);
            options.Add(end);
            options.Add(cloud);
            options.Add(tier);
            options.Add(writepath);
            options.Add(images);

            return options;
        }

        private List<CommandOption> resetCommandOption(
            List<CommandOption> originalList,
            string longName,
            string newValue)
        {
            List<CommandOption> newList = new List<CommandOption>(originalList);

            newList.Remove(newList.Single(o => o.LongName == longName));

            CommandOption c = new CommandOption($"--{longName}", CommandOptionType.SingleValue);
            c.TryParse(newValue);

            newList.Add(c);

            return newList;
        }
    }
}
