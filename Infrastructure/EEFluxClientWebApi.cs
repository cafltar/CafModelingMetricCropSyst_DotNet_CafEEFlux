using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Newtonsoft.Json;

namespace Caf.CafModelingMetricCropSyst.Infrastructure
{
    /// <summary>
    /// Web API implementation of IEEFluxClient
    /// </summary>
    public class EEFluxClientWebApi : IEEFluxClient<HttpResponseMessage>
    {
        private readonly HttpClient client;
        private readonly Uri baseAddress;
        private readonly Dictionary<EEFluxImageTypes, string> imageTypeToUriMap;
        
        /// <summary>
        /// Constructor, no overloads
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="baseAddress">Base URL for HttpClient</param>
        /// <param name="imageTypeToUriMap">Dictionary to map <see cref="EEFluxImageTypes" /> to URL path string</param>
        public EEFluxClientWebApi(
            HttpClient httpClient, 
            string baseAddress,
            Dictionary<EEFluxImageTypes, string> imageTypeToUriMap)
        {
            client = httpClient;
            this.baseAddress = new Uri(baseAddress);
            this.imageTypeToUriMap = imageTypeToUriMap;
        }
        
        /// <summary>
        /// Sends an HTTP POST to {baseAddress}/landsat to obtain a list of available images 
        /// and their metadata between the specified dates in the parameter file
        /// </summary>
        /// <param name="parameters">Required parameters to be set: StartDate, EndDate, ImageId, Latitude, Longitude</param>
        /// <returns>Task representing a dictionary of keys (0-#) and values (image metadata).</returns>
        public async Task<Dictionary<int, EEFluxImageMetadata>> 
            GetImageMetadataAsync(CafEEFluxParameters parameters)
        {
            // TODO: Error checking

            EEFluxRequest requestContent = getEEFluxRequest(parameters);

            StringContent content = 
                new StringContent(JsonConvert.SerializeObject(requestContent));
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

        /// <summary>
        /// Gets an image URL by sending an HTTP POST to {baseAddress}/{imageType}
        /// </summary>
        /// <param name="parameters">Required parameters to be set: StartDate, EndDate, ImageId, Latitude, Longitude</param>
        /// <param name="imageId">ID of image</param>
        /// <param name="imageType">Type of image to be requested (NDVI, Actual ET, etc)</param>
        /// <returns></returns>
        public async Task<Dictionary<EEFluxImageTypes, EEFluxImage>> GetImageUriAsync(
            CafEEFluxParameters parameters,
            string imageId,
            EEFluxImageTypes imageType)
        {
            EEFluxRequest requestContent = getEEFluxRequest(
                parameters,
                imageId);
            StringContent content = 
                new StringContent(JsonConvert.SerializeObject(requestContent));
            Uri uri = 
                new Uri(baseAddress, this.imageTypeToUriMap[imageType]);

            HttpResponseMessage response = await client.PostAsync(
                uri.ToString(),
                content);

            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                Dictionary<EEFluxImageTypes, EEFluxImage> imageUris =
                    JsonConvert.DeserializeObject<
                        Dictionary<EEFluxImageTypes, EEFluxImage>>(result);

                return imageUris;
            }
            else { return new Dictionary<EEFluxImageTypes, EEFluxImage>(); }
        }

        /// <summary>
        /// Sends an HTTP GET request to supplied image URL and download the image
        /// </summary>
        /// <param name="imageUri">Complete URL of the file to be downloaded</param>
        /// <returns>Task representing HTTP response message.
        /// <remarks>Header content contains filename</remarks>
        /// </returns>
        // TODO: Right now this loads the entire image to memory before sending to caller.  Figure out how to stream the data without depending on FileIO.
        public async Task<HttpResponseMessage> DownloadImageAsync(
            //CafEEFluxParameters parameters,
            Uri imageUri)
        {
            HttpResponseMessage response = await client.GetAsync(
                imageUri);

            if(response.IsSuccessStatusCode)
            {
                return response;
            }
            else { return response; }

        }

        /// <summary>
        /// Converts CafEEFluxParameters to EEFluxRequest
        /// </summary>
        /// <param name="parameters">CafEEFluxParameters, expects StartDate, EndDate, ImageId, Latitude, Longitude</param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        private EEFluxRequest getEEFluxRequest(
            CafEEFluxParameters parameters,
            string imageId = "")
        {
            EEFluxRequest eefluxRequest = new EEFluxRequest()
            {
                Date = 
                    $"{parameters.StartDate.ToString("yyyy-MM-dd")} to " +
                    $"{parameters.EndDate.ToString("yyyy-MM-dd")}",
                ImageId = imageId,
                Lat = parameters.Latitude,
                Lon = parameters.Longitude
            };

            return eefluxRequest;
        }
    }
}
