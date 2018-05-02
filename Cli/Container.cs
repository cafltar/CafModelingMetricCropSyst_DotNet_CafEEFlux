using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Infrastructure;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    
    public class Container
    {
        /// <summary>
        /// Wires up all dependencies, called by composition root
        /// </summary>
        /// <returns>Engine that runs the program</returns>
        public Engine ResolveEEFlux()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri("https://eeflux-level1.appspot.com");
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            IEEFluxClient f = new EEFluxClientWebApi(c);
            IGetParameters p = null;
            CommandLineApplication a = getConfiguredCli();

            return new Engine(f, p, a);
        }

        private CommandLineApplication getConfiguredCli()
        {
            // Learned how to use CommandLineApplication from here: https://gist.github.com/iamarcel/8047384bfbe9941e52817cf14a79dc34
            // Using updated version, not Microsofts: https://github.com/natemcmaster/CommandLineUtils
            CommandLineApplication a = new CommandLineApplication();
            a.Name = "eeflux";
            a.HelpOption("-?|-h|--help");

            a.OnExecute(() =>
            {
                Console.WriteLine("Please specify a command to run");
                return 0;
            });

            return a;
        }
    }
}
