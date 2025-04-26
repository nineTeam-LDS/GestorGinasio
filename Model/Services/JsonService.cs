using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
	public class JsonService
	{
		public static void Guardar<T>(string filePath, T obj)
		{
			var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		public static T Carregar<T>(string filePath)
		{
			if (!File.Exists(filePath)) return default;
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}