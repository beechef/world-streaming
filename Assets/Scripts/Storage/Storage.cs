using Newtonsoft.Json;
using UnityEngine;

namespace WorldStreaming.Storage
{
	public abstract class Storage<T> : IStorage
	{
		public abstract StorageKey Key { get; }
		public T Data { get; private set; }

		public void Load()
		{
			Data = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(Key.ToString())) ?? GetDefaultData();
			OnLoad();
		}

		protected void OnLoad()
		{
		}

		protected abstract T GetDefaultData();

		public void Save()
		{
			PlayerPrefs.SetString(Key.ToString(), JsonConvert.SerializeObject(Data));
			OnSave();
		}

		public void OnSave()
		{
		}
	}
}