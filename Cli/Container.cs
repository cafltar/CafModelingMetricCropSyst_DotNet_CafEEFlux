using Caf.CafModelingMetricCropSyst.Core.Interfaces;
using Caf.CafModelingMetricCropSyst.Core.Models;
using Caf.CafModelingMetricCropSyst.Infrastructure;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Caf.CafModelingMetricCropSyst.Cli
{
    
    public static class Container
    {
        /// <summary>
        /// Wires up all dependencies, called by composition root
        /// </summary>
        /// <returns>Engine that runs the program</returns>
        public static Engine ResolveEEFlux()
        {
            string b = GetBaseAddress();
            HttpClient c = ResolveHttpClient();
            var m = ResolveImageTypeToUriMap();
            IEEFluxClient f = new EEFluxClientWebApi(c, b, m);
            IGetParameters p = null;
            CommandLineApplication a = getConfiguredCli();

            return new Engine(f, p, a);
        }

        public static HttpClient ResolveHttpClient()
        {
            HttpClient c = new HttpClient();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return c;
        }
        public static string GetBaseAddress()
        {
            return "https://eeflux-level1.appspot.com";
        }
        public static Dictionary<EEFluxImageTypes, string> ResolveImageTypeToUriMap()
        {
            Dictionary<EEFluxImageTypes, string> imageTypeToUriMap =
                new Dictionary<EEFluxImageTypes, string>();
            imageTypeToUriMap.Add(EEFluxImageTypes.Ndvi, "download_ndvi");
            imageTypeToUriMap.Add(EEFluxImageTypes.Etof, "download_etof");
            imageTypeToUriMap.Add(EEFluxImageTypes.Eta, "download_eta");

            return imageTypeToUriMap;
        }
        private static CommandLineApplication getConfiguredCli()
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
