using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Interfaces
{
    public interface IEEFluxClient
    {
        void GetImageMetadata(CafEEFluxParameters parameters);
        void GetImageUri();
        void GetImage();
    }
}
