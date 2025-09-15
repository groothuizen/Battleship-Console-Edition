using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Handlers
{
    public class ConfigHandler
    {
        public ConfigHandler() 
        { 

        }

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
