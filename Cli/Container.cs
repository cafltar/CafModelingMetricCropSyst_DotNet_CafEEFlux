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
    /// <summary>
    /// Represents a poor man's dependency injector of sorts
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// Wires up all dependencies, called by composition root
        /// </summary>
        /// <returns>Engine that runs the program</returns>
        public static Engine ResolveCafEEFluxCli()
        {
            string b = GetBaseAddress();
            HttpClient c = ResolveHttpClient();
            var m = ResolveImageTypeToUriMap();
            IEEFluxClient<HttpResponseMessage> f = new EEFluxClientWebApi(c, b, m);
            CommandLineApplication a = getConfiguredCli();
            IParseParameters<CommandOption> p = new CommandLineUtilParameterParser();

            return new Engine(f, a, p);
        }

        public static HttpClient ResolveHttpClient()
        {
            HttpClient c = new HttpClient();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            c.Timeout = TimeSpan.FromMinutes(30);

            return c;
        }
        public static string GetBaseAddress()
        {
            return "https://eeflux-level1.appspot.com";
        }
        public static Dictionary<EEFluxImageTypes, string> ResolveImageTypeToUriMap()
        {
            MapImageTypeToUrlPath imageTypeToUriMap = 
                new MapImageTypeToUrlPath();
            return imageTypeToUriMap.map;
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
