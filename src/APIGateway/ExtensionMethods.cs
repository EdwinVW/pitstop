using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ocelot.Configuration.File;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pitstop.APIGateway
{

    /*********************************************************************************
     * Temporary overload !!
     *********************************************************************************
     * This overload is added to the Ocelot code-base through a PR 
     * I have created. But is is not yet available in a new version 
     * of the Ocelot NuGet package. Will be available in next release.
     *********************************************************************************/

    public static class ExtensionMethods
    {
        /// <summary>
        /// Overload of the AddOcelot configuration extension method that accepts a folder to load from.
        /// </summary>
        /// <param name="builder">The configuration-builder to extend.</param>
        /// <param name="folder">The folder to read the config files from.</param>
        /// <returns>Initialized configuration-builder instance.</returns>
        public static IConfigurationBuilder AddOcelot(this IConfigurationBuilder builder, string folder)
        {
            const string pattern = "(?i)ocelot\\.([a-zA-Z0-9]*)(\\.json)$";

            var reg = new Regex(pattern);

            var files = Directory.GetFiles(folder)
                .Where(path => reg.IsMatch(path))
                .ToList();

            var fileConfiguration = new FileConfiguration();

            foreach (var file in files)
            {
                // windows and unix sigh...
                if (files.Count > 1 && (file == "./ocelot.json" || file == ".\\ocelot.json"))
                {
                    continue;
                }

                var lines = File.ReadAllText(file);

                var config = JsonConvert.DeserializeObject<FileConfiguration>(lines);

                // windows and unix sigh...
                if (file == "./ocelot.global.json" || file == ".\\ocelot.global.json")
                {
                    fileConfiguration.GlobalConfiguration = config.GlobalConfiguration;
                }

                fileConfiguration.Aggregates.AddRange(config.Aggregates);
                fileConfiguration.ReRoutes.AddRange(config.ReRoutes);
            }

            var json = JsonConvert.SerializeObject(fileConfiguration);

            File.WriteAllText("ocelot.json", json);

            builder.AddJsonFile("ocelot.json");

            return builder;
        }
    }
}
