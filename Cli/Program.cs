using System;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Container container = new Container();

            Container.ResolveEEFlux().Execute(args);
        }
    }
}
