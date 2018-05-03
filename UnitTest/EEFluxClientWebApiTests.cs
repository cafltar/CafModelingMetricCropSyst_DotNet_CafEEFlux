using Caf.CafModelingMetricCropSyst.Cli;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Caf.CafModelingMetricCropSyst.Infrastructure;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Caf.CafModelingMetricCropSyst.UnitTest
{
    public class EEFluxClientWebApiTests
    {
        [Fact]
        public async Task GetImageMetadata_ValidParams_ReturnsDictionaryOfEEFluxImageMetadata()
        {
            // ARRANGE
            var parameters = getCafEEFluxParametersValid();
            var expected = getEEFluxImageMetadataValid();
            var actual = new Dictionary<int, EEFluxImageMetadata>();

            EEFluxClientWebApi sut = arrangeEEFluxClientWebApi(
                "landsat",
                getEEFluxResponseLandsatValid());

            // ACT
            actual = await sut.GetImageMetadataAsync(parameters);

            // ASSERT
            Assert.Equal(actual, expected);

        }

        [Fact]
        public async Task GetImageUri_Etof_ReturnsDictionaryOfEEFluxImages()
        {
            // ARRANGE
            string imageId = "LE70420282015170EDC00";
            var parameters = getCafEEFluxParametersValid();
            var expected = getEEFluxResponseDownloadEtofValid();
            EEFluxClientWebApi sut = arrangeEEFluxClientWebApi(
                "download_etof",
                "{\"etof\": {\"url\": \"https://earthengine.googleapis.com/api/download?docid=6277fdbbcb5e4e0bc2f2d5562f4d1c4a&token=da96164b598072a0163f34ceaac8f5b6\"}}");
            
            // ACT
            var actual = await sut.GetImageUriAsync(
                parameters, 
                imageId, 
                EEFluxImageTypes.Etof);

            // ASSERT
            Assert.Equal(actual, expected);
        }

        private string getEEFluxResponseLandsatValid()
        {
            string content = File.ReadAllText("Assets/EEFluxResponseLandsatValid.json");
            return content;
        }
        private CafEEFluxParameters getCafEEFluxParametersValid()
        {
            CafEEFluxParameters parameters = new CafEEFluxParameters(
                47.4395843855568,
                -118.35367508232594,
                new DateTime(2015, 06, 01),
                new DateTime(2015, 06, 05),
                30,
                1,
                "/Output/test.zip");

            return parameters;
        }
        private Dictionary<int, EEFluxImageMetadata> getEEFluxImageMetadataValid()
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
        private EEFluxClientWebApi arrangeEEFluxClientWebApi(
            string path,
            string mockResponse)
        {
            // From: https://github.com/richardszalay/mockhttp
            // And: https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler
                .When($"https://eeflux-level1.appspot.com/{path}")
                .Respond("application/json", mockResponse);
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var baseAddress = Container.GetBaseAddress();
            var map = Container.ResolveImageTypeToUriMap();

            EEFluxClientWebApi r = new EEFluxClientWebApi(
                httpClient, baseAddress, map);

            return r;
        }
        private Dictionary<EEFluxImageTypes, EEFluxImage> 
            getEEFluxResponseDownloadEtofValid()
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
    }
}
