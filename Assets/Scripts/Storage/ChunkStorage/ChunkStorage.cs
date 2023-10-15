using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using WorldStreaming.JsonConverter;

namespace WorldStreaming.Storage
{
	public static class ChunkStorage
	{
		private static readonly JsonSerializerSettings JsonSerializerSettings = new()
		{
			Formatting = Formatting.None,
			TypeNameHandling = TypeNameHandling.None,
			Converters = new Newtonsoft.Json.JsonConverter[]
			{
				new StringEnumConverter(), new ConverterStateData()
			}
		};

		private static ChunkData LoadChunkData(string key)
		{
			var json = PlayerPrefs.GetString(key);
			if (string.IsNullOrEmpty(json)) return null;

			var chunkData = JsonConvert.DeserializeObject<ChunkData>(json, JsonSerializerSettings);
			return chunkData;
		}

		public static ChunkData LoadChunkData(ChunkInfo chunkInfo)
		{
			var storageKey = ChunkStorageRules.GetChunkDataStorageKey(chunkInfo);
			return LoadChunkData(storageKey);
		}

		public static void SaveChunkData(ChunkInfo chunkInfo, ChunkData chunkData)
		{
			var json = JsonConvert.SerializeObject(chunkData, JsonSerializerSettings);
			var storageKey = ChunkStorageRules.GetChunkDataStorageKey(chunkInfo);

			PlayerPrefs.SetString(storageKey, json);
		}
	}
}