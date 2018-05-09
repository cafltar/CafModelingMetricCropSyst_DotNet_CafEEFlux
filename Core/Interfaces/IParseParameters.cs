using Caf.CafModelingMetricCropSyst.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Core.Interfaces
{
    public interface IParseParameters<T> where T : class
    {
        CafEEFluxParameters Parse(IEnumerable<T> parameters);
    }
}
