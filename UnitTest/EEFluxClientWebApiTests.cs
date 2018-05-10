using Caf.CafModelingMetricCropSyst.Cli;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Caf.CafModelingMetricCropSyst.Infrastructure;
using Caf.CafModelingMetricCropSyst.TestUtils;
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
        public EEFluxClientWebApiTests()
        {
            Directory.CreateDirectory("Output");
        }

        [Fact]
        public async Task GetImageMetadata_ValidParams_ReturnsDictionaryOfEEFluxImageMetadata()
        {
            // ARRANGE
            var parameters = Arranger.GetCafEEFluxParametersValid();
            var expected = Arranger.GetEEFluxImageMetadataValid();
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
            var parameters = Arranger.GetCafEEFluxParametersValid();
            var expected = Arranger.GetEEFluxResponseDownloadEtofValid();
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
        
    }
}
