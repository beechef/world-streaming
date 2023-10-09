using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WorldStreaming.Pooling;

namespace WorldStreaming.Storage
{
	public class StorageController : MonoBehaviour
	{
		public static StorageController Instance { get; private set; }

		private readonly Dictionary<Type, IStorage> _storages = new();

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			_storages.Add(typeof(StoragePoolingId), new StoragePoolingId());

			Load();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus) return;

			Save();
		}

		private void OnApplicationQuit()
		{
			Save();
		}

		public T GetStorage<T>() where T : IStorage
		{
			return (T)_storages[typeof(T)];
		}

		private void Load()
		{
			foreach (var storage in _storages.Values)
			{
				storage.Load();
			}
		}

		private void Save()
		{
			foreach (var storage in _storages.Values)
			{
				storage.Save();
			}
		}

		[Button]
		public void GetId()
		{
			Debug.Log(PoolingId.GetId());
		}
	}
}