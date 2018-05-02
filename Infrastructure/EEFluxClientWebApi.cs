using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Newtonsoft.Json;

namespace Caf.CafModelingMetricCropSyst.Infrastructure
{
    public class EEFluxClientWebApi : IEEFluxClient
    {
        // No interface available
        // Do not wrap in using statement: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private readonly HttpClient client;
        private readonly Uri baseAddress;
        

        public EEFluxClientWebApi(HttpClient httpClient, string baseAddress)
        {
            client = httpClient;
            this.baseAddress = new Uri(baseAddress);
        }
        public void GetImage()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<int, EEFluxImageMetadata>> GetImageMetadata(CafEEFluxParameters parameters)
        {
            EEFluxRequest requestContent = new EEFluxRequest()
            {
                Date = $"{parameters.StartDate.ToString("yyyy-MM-dd")} to {parameters.EndDate.ToString("yyyy-MM-dd")}",
                ImageId = "",
                Lat = parameters.Latitude,
                Lon = parameters.Longitude
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(requestContent));
            Uri uri = new Uri(baseAddress, "landsat");

            HttpResponseMessage response = await client.PostAsync(
                uri.ToString(), 
                content);

            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                Dictionary<int, EEFluxImageMetadata> imageMetadatas =
                    JsonConvert.DeserializeObject<Dictionary<int, EEFluxImageMetadata>>(result);

                return imageMetadatas;
            }
            else
            {
                return new Dictionary<int, EEFluxImageMetadata>();
            }
        }

        public void GetImageUri()
        {
            throw new NotImplementedException();
        }
    }
}
