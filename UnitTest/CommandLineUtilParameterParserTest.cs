using Caf.CafModelingMetricCropSyst.Core.Models;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Caf.CafModelingMetricCropSyst.Cli;
using System.Linq;
using System.IO;
using Caf.CafModelingMetricCropSyst.TestUtils;

namespace Caf.CafModelingMetricCropSyst.UnitTest
{
    public class CommandLineUtilParameterParserTest
    {
        public CommandLineUtilParameterParserTest()
        {
            Directory.CreateDirectory("Output");
        }

        [Fact]
        public void Parse_ValidParams_ReturnsExpected()
        {
            // ARRANGE
            var parameters = Arranger.GetCommandOptionsListValid();
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
            Assert.Equal(@"Output", actual.OutputDirectoryPath);
            Assert.Equal(3, actual.ImageTypes.Count);
        }

        [Fact]
        public void Parse_OutputDirNotSpecified_SetsCurrentWorkingDir()
        {
            // ARRANGE
            string cwd = Directory.GetCurrentDirectory();

            var parameters = 
                Arranger.ResetCommandOption(
                    Arranger.GetCommandOptionsListValid(), "writepath");
            var sut = new CommandLineUtilParameterParser();
            var expected = @"C:\";
            Directory.SetCurrentDirectory(expected);

            // ACT
            var actual = sut.Parse(parameters);
            Directory.SetCurrentDirectory(cwd);

            // ASSERT
            Assert.Equal(expected, actual.OutputDirectoryPath);
        }

        [Fact]
        public void Parse_SingleImageType_SetsOneImageType()
        {
            // ARRANGE
            var parameters = Arranger.GetCommandOptionsListValid();
            parameters = Arranger.ResetCommandOption(parameters, "imagetypes", "etof");
            var sut = new CommandLineUtilParameterParser();
            var expected = EEFluxImageTypes.Etof;

            // ACT
            var actual = sut.Parse(parameters);

            // ASSERT
            Assert.Equal(expected, actual.ImageTypes[0]);
        }


        
    }
}
