using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Service.Icon.Api.Examples.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Icon.Api.Examples
{
    public class BaseTest
    {
        private readonly string _settingsUrl;
        protected readonly KeyWallet _secretWallet;
        protected readonly KeyWallet _toWallet;
        protected readonly IContainer _container;

        //This is needed to run tests locally!
        public BaseTest()
        {
            #region LoadEnvVariables

            try
            {
                using (var file = File.OpenText("Properties\\launchSettings.json"))
                {
                    var reader = new JsonTextReader(file);
                    var jObject = JObject.Load(reader);

                    var variables = jObject
                        .GetValue("profiles")
                        //select a proper profile here
                        .SelectMany(profiles => profiles.Children())
                        .SelectMany(profile => profile.Children<JProperty>())
                        .Where(prop => prop.Name == "environmentVariables")
                        .SelectMany(prop => prop.Value.Children<JProperty>())
                        .ToList();

                    foreach (var variable in variables)
                    {
                        Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            #endregion LoadEnvVariables

            var secretKey1 = Environment.GetEnvironmentVariable("PrivateKey");
            var secretKey2 = Environment.GetEnvironmentVariable("PrivateKey2");

            _settingsUrl = Environment.GetEnvironmentVariable("SettingsUrl");
            _secretWallet = KeyWallet.Load(new Bytes(secretKey1));
            _toWallet = KeyWallet.Load(new Bytes(secretKey2));
            _container = TestModulesRegistrator.GetContainer(_settingsUrl).Result;
        }
    }
}
