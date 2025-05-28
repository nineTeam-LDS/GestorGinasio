using System;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class JsonService : IJsonService
    {
        public T Load<T>(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json)!;
            }
            catch (JsonException ex)
            {
                throw new JsonFileFormatException(path, ex);
            }
            catch (IOException ex)
            {
                // Pode registar em log
                throw;
            }
        }

        public void Save<T>(string path, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (IOException ex)
            {
                // Pode registar em log
                throw;
            }
        }
    }
}