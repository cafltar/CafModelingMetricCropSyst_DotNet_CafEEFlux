using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;

namespace Caf.CafModelingMetricCropSyst.Infrastructure
{
    public class EEFluxClientWebApi : IEEFluxClient
    {
        // No interface available
        // Do not wrap in using statement: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private readonly HttpClient client;

        public EEFluxClientWebApi(HttpClient httpClient)
        {
            client = httpClient;
        }
        public void GetImage()
        {
            throw new NotImplementedException();
        }

        public void GetImageMetadata(EEFluxParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void GetImageUri()
        {
            throw new NotImplementedException();
        }
    }
}
