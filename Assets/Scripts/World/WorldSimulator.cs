using System.Collections.Generic;
using UnityEngine;
using WorldStreaming.Storage;

namespace WorldStreaming
{
	[RequireComponent(typeof(WorldStreamingController))]
	public class WorldSimulator : MonoBehaviour
	{
		private WorldStreamingController _streamingController;

		private readonly LinkedList<Chunk> _chunks = new();

		private void Awake()
		{
			_streamingController = GetComponent<WorldStreamingController>();
		}

		private void OnEnable()
		{
			_chunks.Clear();

			_streamingController.OnChunkLoaded += OnChunkLoaded;
			_streamingController.OnChunkUnloaded += OnChunkUnloaded;
		}
		
		private void OnDisable()
		{
			_streamingController.OnChunkLoaded -= OnChunkLoaded;
			_streamingController.OnChunkUnloaded -= OnChunkUnloaded;


			_chunks.Clear();
		}

		private void OnChunkLoaded(Chunk chunk)
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

			_chunks.AddLast(chunk);
		}

		private void OnChunkUnloaded(Chunk chunk)
		{
			var chunkData = chunk.GetData();

			ChunkStorage.SaveChunkData(chunk.info, chunkData);
			chunk.OnSave();
			chunk.OnStop();
			chunk.OnUnload();

			_chunks.Remove(chunk);
		}

		private void SaveAll()
		{
			foreach (var chunk in _chunks)
			{
				var chunkData = chunk.GetData();

				ChunkStorage.SaveChunkData(chunk.info, chunkData);
				chunk.OnSave();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus) return;

			SaveAll();
		}

		private void OnApplicationQuit()
		{
			SaveAll();
		}
	}
}