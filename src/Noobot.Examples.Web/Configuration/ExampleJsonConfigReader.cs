﻿using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Noobot.Core.Configuration;
using Noobot.Examples.Web.Encryption;

namespace Noobot.Examples.Web.Configuration
{
    /// <summary>
    /// An example of how you could create a config reader. You might want to read out of AppConfig instead?
    /// </summary>
    public class ExampleJsonConfigReader : IConfigReader
    {
        public string SlackApiKey()
        {
            string apiToken = GetConfigEntry<string>("slack:apiToken");
            return EncryptionHelper.Decrypt(apiToken);
        }

        public bool HelpEnabled()
        {
            return true;
        }

        public T GetConfigEntry<T>(string entryName)
        {
            JObject jObject = GetJObject();
            return jObject.Value<T>(entryName);
        }

        private JObject _currentJObject;
        private JObject GetJObject()
        {
            if (_currentJObject == null)
            {
                string fileName = Path.Combine(Environment.CurrentDirectory, @"configuration\config.json");
                string json = File.ReadAllText(fileName);
                _currentJObject = JObject.Parse(json);
            }

            return _currentJObject;
        }
    }
}