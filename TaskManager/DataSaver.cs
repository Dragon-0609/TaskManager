using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Win32;
using Newtonsoft.Json;
namespace TaskManager
{
	public static class DataSaver
	{
		private static RegistryKey _subKey;

		static DataSaver()
		{
			_subKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Dragon", true).CreateSubKey("TaskManager", true);
		}
#region Load

		public static int Load(string key, int defaultValue)
		{
			return (int)_subKey.GetValue(key, defaultValue);
		}

		public static bool Load(string key, bool defaultValue)
		{
			int value = defaultValue ? 1 : 0;
			return (int)_subKey.GetValue(key, value) == 1;
		}

		public static float Load(string key, float defaultValue)
		{
			return (float)_subKey.GetValue(key, defaultValue);
		}


		public static string Load(string key, string defaultValue)
		{
			return (string)_subKey.GetValue(key, defaultValue);
		}

		public static T Load<T>(string key, T defaultValue)
		{
			return JsonConvert.DeserializeObject<T>(Load(key, JsonConvert.SerializeObject(defaultValue)));
		}

#endregion

#region Save

		public static void Save(string key, int value)
		{
			_subKey.SetValue(key, value);
		}
		public static void Save(string key, bool value)
		{
			_subKey.SetValue(key, value ? 1 : 0);
		}

		public static void Save(string key, float value)
		{
			_subKey.SetValue(key, value);
		}

		public static void Save(string key, string value)
		{
			_subKey.SetValue(key, value);
		}

		public static void Save<T>(string key, T value)
		{
			string json = JsonConvert.SerializeObject(value);
			Save(key, json);
		}

#endregion

		public static bool Exists(string key)
		{
			return _subKey.GetValueNames().Any(c => c == key);
		}

	}
}