using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Battleship.Handlers
{
    public class ConfigHandler
    {
        /// <summary>
        /// Gets a value from App.config and parses it as an integer.
        /// </summary>
        /// <param name="key">The config key to look for.</param>
        /// <returns>int value</returns>
        /// <exception cref="Exception"></exception>
        public int GetInt(string key)
        {
            bool isParsed = int.TryParse(ConfigurationManager.AppSettings[key], out int parsedValue);
            if (isParsed)
            {
                return parsedValue;
            } 
            else
            {
                throw new Exception($"int.TryParse() failed at ConfigHandler.GetInt() on key: {key}");
            }
        }

        /// <summary>
        /// Gets a string value from App.config.
        /// </summary>
        /// <param name="key">The config key to look for.</param>
        /// <returns>string value</returns>
        /// <exception cref="Exception"></exception>
        public string GetString(string key)
        {
            string? configValue = ConfigurationManager.AppSettings[key];
            if (configValue != null)
            {
                return configValue;
            }
            else
            {
                throw new NullReferenceException($"Unexpected null reference at ConfigHandler.GetString() on key: {key}, expected a string value...");
            }
        }
    }
}
