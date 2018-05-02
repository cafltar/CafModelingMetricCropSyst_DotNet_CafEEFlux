using System;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Models
{
    public class EEFluxResponseImageMetadata
    {
        public int id { get; set; }
        public List<EEFluxImageMetadata> ImageMetadatas { get; set; }
    }
}
