using Microsoft.Extensions.Configuration;

namespace Battleship.Handlers
{
    public class ConfigHandler
    {
        /// <summary>
        /// Call as little as possible!
        /// </summary>
        private IConfiguration Config {
            get {
                try
                {
                    return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException(@"File could not be found at ConfigHandler.ConfigFile.get(): \appsettings.json could not be read.");
                }
            }
        }

        /// <summary>
        /// Gets the value with the specified key and converts it to type T.
        /// </summary>
        /// <param name="key">The json key to look for.</param>
        /// <returns>integer value</returns>
        /// <exception cref="NullReferenceException"></exception>
        public T GetValue<T>(string key)
        {
            try 
            {
                return Config.GetSection("Battleship").GetValue<T>(key)!;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException($"Unexpected null reference at ConfigHandler.GetInt(): \\appsettings.json does not contain key: \"{key}\"");
            }
        }
    }
}
