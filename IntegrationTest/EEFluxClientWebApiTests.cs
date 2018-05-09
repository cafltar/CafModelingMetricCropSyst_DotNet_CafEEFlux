using Caf.CafModelingMetricCropSyst.Cli;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Caf.CafModelingMetricCropSyst.Infrastructure;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Caf.CafModelingMetricCropSyst.IntegrationTest
{
    public class EEFluxClientWebApiTests
    {
        [Fact]
        public async Task Integration_Valid_ReturnsExpected()
        {
            // ARRANGE
            string b = Container.GetBaseAddress();
            HttpClient c = Container.ResolveHttpClient();
            var m = Container.ResolveImageTypeToUriMap();
            IEEFluxClient<HttpResponseMessage> sut = new EEFluxClientWebApi(c, b, m);

            CafEEFluxParameters p = getCafEEFluxParametersValid();

            // ACT
            Dictionary<int, EEFluxImageMetadata> imageMetas = 
                await sut.GetImageMetadataAsync(p);

            Dictionary<EEFluxImageTypes, EEFluxImage> image =
                await sut.GetImageUriAsync(
                    p,
                    imageMetas[0].ImageId,
                    EEFluxImageTypes.Ndvi);
            Uri imageUrl = new Uri(image[EEFluxImageTypes.Ndvi].Url);
            string fullPath = Path.GetFullPath(p.OutputDirectoryPath);
            if(!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // await sut.DownloadImageAsync(fullPath, imageUrl);
            Task<HttpResponseMessage> download = sut.DownloadImageAsync(imageUrl);

            var response = await download;

            string filePath =
                $"{p.OutputDirectoryPath}\\{response.Content.Headers.ContentDisposition.FileName}";

            using (Stream readStream = await response.Content.ReadAsStreamAsync())
            {
                using (Stream writeStream = File.Open(
                    filePath, 
                    FileMode.Create))
                {
                    await readStream.CopyToAsync(writeStream);
                }
            }

            // ASSERT
            Assert.True(
                File.Exists("Output/LE70440272015152EDC00_NDVI.zip"));
            double expectedLength = new FileInfo("Assets/LE70440272015152EDC00_NDVI.zip").Length;
            double actualLength = new FileInfo("Output/LE70440272015152EDC00_NDVI.zip").Length;
            Assert.Equal(
                expectedLength,
                actualLength);
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
                "Output",
                new List<EEFluxImageTypes>() { EEFluxImageTypes.Eta });

            return parameters;
        }

        private string calculateMd5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter
                        .ToString(hash)
                        .Replace("-", "")
                        .ToLowerInvariant();
                }
            }
        }
    }
}
