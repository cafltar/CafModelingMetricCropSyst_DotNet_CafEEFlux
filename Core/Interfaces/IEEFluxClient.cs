using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caf.CafModelingMetricCropSyst.Core.Interfaces
{
    /// <summary>
    /// Client to interact with the EEFlux online application: https://eeflux-level1.appspot.com/
    /// </summary>
    /// <typeparam name="TResult">Can be of any type that is a class.  This is used by DownloadImagesAsync.</typeparam>
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
