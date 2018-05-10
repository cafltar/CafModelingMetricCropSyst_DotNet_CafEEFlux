using Caf.CafModelingMetricCropSyst.Cli;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Caf.CafModelingMetricCropSyst.TestUtils;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Caf.CafModelingMetricCropSyst.UnitTest
{
    public class EngineTest
    {
        public EngineTest()
        {
            Directory.CreateDirectory("Output");
        }

        [Fact]
        public async Task GetImages_FolderExists_DoesNotDownload()
        {
            // ARRANGE
            var p = Arranger.GetCafEEFluxParametersValid();
            var parameters = new CafEEFluxParameters(
                p.Latitude, p.Latitude,
                p.StartDate, p.EndDate,
                p.CloudinessThreshold, p.TierThreshold,
                $"{Directory.GetCurrentDirectory()}\\Assets\\TestFolderOnly",
                new List<EEFluxImageTypes>() { EEFluxImageTypes.Ndvi });

            var imageMeta =
                new Dictionary<int, EEFluxImageMetadata>();
            imageMeta.Add(
                0,
                Arranger.GetEEFluxImageMetadataValid()[0]);
                
            Mock<IEEFluxClient<HttpResponseMessage>> e = 
                new Mock<IEEFluxClient<HttpResponseMessage>>();
            e.Setup(x => x.GetImageMetadataAsync(parameters))
                .Returns(Task.FromResult(imageMeta));
            //e.Setup(x => x.GetImageUriAsync(parameters, It.IsAny<string>(), It.IsAny<EEFluxImageTypes>()))

            var c = new CommandLineApplication();

            var o = new Mock<IParseParameters<CommandOption>>();
            o.Setup(x =>  x.Parse(
                It.IsAny<IEnumerable<CommandOption>>()))
                .Returns(parameters);

            var sut = new Engine(e.Object, c, o.Object);

            // ACT
            int result = await sut.GetImages(parameters);

            // ASSERT
            e.Verify(x => x.GetImageUriAsync(parameters, It.IsAny<string>(), It.IsAny<EEFluxImageTypes>()),Times.Never);
        }
    }
}
