using System.Collections.Generic;
using UnityEngine;

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
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;
			var chunk = _chunks.First;

			while (chunk != null)
			{
				var next = chunk.Next;
				chunk.Value.OnUpdate(deltaTime);
				chunk = next;
			}
		}

		private void OnDisable()
		{
			_streamingController.OnChunkLoaded -= OnChunkLoaded;

			foreach (var chunk in _chunks)
			{
				chunk.OnStop();
			}
			
			_chunks.Clear();
		}

		private void OnChunkLoaded(Chunk chunk)
		{
			chunk.OnStart();
			_chunks.AddLast(chunk);
		}

		private void OnChunkUnloaded(Chunk chunk)
		{
			chunk.OnStop();
			_chunks.Remove(chunk);
		}
	}
}