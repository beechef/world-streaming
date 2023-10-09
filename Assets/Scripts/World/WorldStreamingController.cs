using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	public class WorldStreamingController : MonoBehaviour
	{
		[SerializeField] private WorldInfo worldInfo;

		[ShowInInspector] private IObservableMovement _target;
		private Vector3 _lastPosition;

		private WorldSpawner _spawner;

		private Chunk _currentChunk;
		private readonly List<Chunk> _loadedChunks = new();
		private readonly List<int> _loadedChunkIds = new();

		private readonly object _lockQueue = new();
		private readonly Queue<Chunk> _queuedChunks = new();

		private void Awake()
		{
			_spawner = new WorldSpawner();
		}

		private void OnEnable()
		{
			_currentChunk = null;
			_loadedChunks.Clear();
			_loadedChunkIds.Clear();
			ClearQueueChunk();

			_spawner.Init(worldInfo, transform);

			//Cheat
			SetTarget(gameObject.GetAllComponentInChildren<Entity>().First());

			if (_target == null) return;

			_target.OnMoved += OnTargetMoved;
			_lastPosition = _target.Position;

			StreamingProcess(_lastPosition);
		}

		private void OnDisable()
		{
			_spawner.Dispose();

			if (_target == null) return;

			_target.OnMoved -= OnTargetMoved;
		}

		public void SetTarget(IObservableMovement target)
		{
			if (_target != null) _target.OnMoved -= OnTargetMoved;

			_target = target;
			_target.OnMoved -= OnTargetMoved;
			_target.OnMoved += OnTargetMoved;

			_lastPosition = _target.Position;
			StreamingProcess(_lastPosition);
		}

		private Chunk GetCurrentChunk(Vector3 position)
		{
			foreach (var chunk in _loadedChunks)
			{
				if (chunk.info.rect.IsContains(new Vector2(position.x, position.z))) return chunk;
			}

			return null;
		}

		private async void InstantiateChunk(ChunkInfo chunkInfo)
		{
			_loadedChunkIds.Add(chunkInfo.id);

			var chunk = await _spawner.InstantiateChunk(chunkInfo);
			AddQueueChunk(chunk);
		}

		private void AddQueueChunk(Chunk chunk)
		{
			lock (_lockQueue)
			{
				_queuedChunks.Enqueue(chunk);
			}
		}

		private void MergeQueueChunk()
		{
			lock (_lockQueue)
			{
				while (_queuedChunks.Count > 0)
				{
					var chunk = _queuedChunks.Dequeue();
					_loadedChunks.Add(chunk);
				}
			}
		}

		private void ClearQueueChunk()
		{
			lock (_lockQueue)
			{
				_queuedChunks.Clear();
			}
		}

		private void StreamingProcess(Vector3 position)
		{
			var detectZone = new Rect(position.x - worldInfo.detectedZoneSize.x / 2f, position.z - worldInfo.detectedZoneSize.y / 2f,
				worldInfo.detectedZoneSize.x, worldInfo.detectedZoneSize.y);

			MergeQueueChunk();
			_currentChunk = GetCurrentChunk(position);

			for (var index = 0; index < _loadedChunks.Count; index++)
			{
				var loadedChunk = _loadedChunks[index];
				if (detectZone.IsIntersecting(loadedChunk.info.rect)) continue;

				_spawner.DestroyChunk(loadedChunk);
				_loadedChunks.RemoveAt(index--);
				_loadedChunkIds.Remove(loadedChunk.info.id);
			}

			if (!_currentChunk)
			{
				foreach (var chunkInfo in worldInfo.chunkInfos)
				{
					var isExist = _loadedChunkIds.Contains(chunkInfo.id);
					var isIntersecting = detectZone.IsIntersecting(chunkInfo.rect);
					if (isExist || !isIntersecting) continue;

					InstantiateChunk(chunkInfo);
				}
			}
			else
			{
				foreach (var adjacentChunkId in _currentChunk.info.adjacentChunkIds)
				{
					var adjacentChunkInfo = worldInfo.idSortedChunkInfos[adjacentChunkId];
					var isExist = _loadedChunkIds.Contains(adjacentChunkInfo.id);
					var isIntersecting = detectZone.IsIntersecting(adjacentChunkInfo.rect);
					if (isExist || !isIntersecting) continue;

					InstantiateChunk(adjacentChunkInfo);
				}
			}
		}

		private void OnTargetMoved(Vector3 position)
		{
			if (Vector3.Distance(_lastPosition, position) < WorldRules.MinDetectedDistance) return;
			_lastPosition = position;

			StreamingProcess(position);
		}
	}
}