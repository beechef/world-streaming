using UnityEngine;
using WorldStreaming.Storage;

namespace WorldStreaming
{
	public class WorldDataController : MonoBehaviour
	{
		public readonly WorldData Data = new();

		public void Add(Chunk chunk)
		{
			var chunkData = ChunkStorage.LoadChunkData(chunk.info);

			if (chunkData == null)
			{
				chunk.InitDefaultData();
			}
			else
			{
				chunk.Init(chunkData);
			}

			chunk.OnLoad();
			chunk.OnStart();

			Data.Chunks.Add(chunk);
		}

		public void Remove(Chunk chunk)
		{
			var chunkData = chunk.GetData();

			ChunkStorage.SaveChunkData(chunk.info, chunkData);
			chunk.OnSave();
			chunk.OnStop();
			chunk.OnUnload();

			Data.Chunks.Remove(chunk);
		}

		public void Clear()
		{
			Data.Chunks.Clear();
		}

		private void SaveAll(bool isQuit = false)
		{
			foreach (var chunk in Data.Chunks)
			{
				var chunkData = chunk.GetData();

				ChunkStorage.SaveChunkData(chunk.info, chunkData);
				chunk.OnSave();

				if (!isQuit) continue;

				chunk.OnStop();
				chunk.OnUnload();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus) return;

			SaveAll();
		}

		private void OnApplicationQuit()
		{
			SaveAll(true);
		}
	}
}