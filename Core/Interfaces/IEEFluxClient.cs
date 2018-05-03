using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caf.CafModelingMetricCropSyst.Core.Interfaces
{
    public interface IEEFluxClient<TResult> where TResult : class
    {
        Task<Dictionary<int, EEFluxImageMetadata>> 
            GetImageMetadataAsync(CafEEFluxParameters parameters);
        Task<Dictionary<EEFluxImageTypes, EEFluxImage>> 
            GetImageUriAsync(
                CafEEFluxParameters parameters, 
                string imageId,
                EEFluxImageTypes imageType);
        Task<TResult> DownloadImageAsync(
            Uri imageUri);
    }
}
