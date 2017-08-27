using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JokesAPI.Models;

namespace ContactsList.API
{
    public class GenericStorage
    {
        private string _filePath;

        public GenericStorage()
        {
            var webAppsHome = Environment.GetEnvironmentVariable("HOME");

           // _filePath = "C:\\PracticeProject\\JokesAPI\\";

            if (String.IsNullOrEmpty(webAppsHome))
            {
                _filePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "\\";
            }
            else
            {
                _filePath = webAppsHome + "\\site\\wwwroot\\";
            }
        }

        public async Task<IEnumerable<Joke>> Save(IEnumerable<Joke> target, string filename)
        {
            var json = JsonConvert.SerializeObject(target);
            File.WriteAllText(_filePath + filename, json);
            return target;
        }

        public async Task<IEnumerable<Joke>> Get(string filename)
        {
            var contactsText = String.Empty;
            if (File.Exists(_filePath + filename))
            {
                contactsText = File.ReadAllText(_filePath + filename);
            }

            var contacts = JsonConvert.DeserializeObject<Joke[]>(contactsText);
            return contacts;
        }
    }
}
