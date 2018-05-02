using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caf.CafModelingMetricCropSyst.Core.Interfaces
{
    public interface IEEFluxClient
    {
        Task<Dictionary<int, EEFluxImageMetadata>> GetImageMetadata(CafEEFluxParameters parameters);
        void GetImageUri();
        void GetImage();
    }
}
