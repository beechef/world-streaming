using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace WorldStreaming
{
	public class WorldSpawner
	{
		private WorldInfo _worldInfo;
		private Transform _worldContainer;

		private readonly Dictionary<ChunkArea, Transform> _areaTransforms = new();

		private readonly Dictionary<object, AsyncOperationHandle> _chunkHandles = new();
		private CancellationTokenSource _cancellationToken;

		public void Init(WorldInfo worldInfo, Transform worldContainer)
		{
			ReleaseAllChunks();

			_worldInfo = worldInfo;
			_worldContainer = worldContainer;

			_cancellationToken = new CancellationTokenSource();
		}

		public void Dispose()
		{
			_cancellationToken?.Cancel();
			_cancellationToken?.Dispose();

			ReleaseAllChunks();
		}

		private Transform GetOrAddAreaTransform(ChunkArea area)
		{
			if (_areaTransforms.TryGetValue(area, out var areaTransform)) return areaTransform;

			var areaGameObject = new GameObject(area.ToString());
			areaTransform = areaGameObject.transform;
			areaTransform.SetParent(_worldContainer);

			_areaTransforms.Add(area, areaTransform);
			return areaTransform;
		}

		public async UniTask<Chunk> InstantiateChunk(ChunkInfo chunkInfo)
		{
			var chunkKey = WorldRules.GetChunkKey(chunkInfo);
			var handle = Addressables.InstantiateAsync(chunkKey);
			var go = await handle.WithCancellation(_cancellationToken.Token);

			var chunk = go.GetComponent<Chunk>();
			chunk.transform.SetParent(GetOrAddAreaTransform(chunkInfo.area));

			_chunkHandles.Add(chunk, handle);
			return chunk;
		}

		public void DestroyChunk(Chunk chunk)
		{
			if (!_chunkHandles.TryGetValue(chunk, out var handle)) return;

			Addressables.ReleaseInstance(handle);
			_chunkHandles.Remove(chunk);
		}

		public void ReleaseAllChunks()
		{
			foreach (var handle in _chunkHandles.Values)
			{
				Addressables.ReleaseInstance(handle);
			}

			_chunkHandles.Clear();
		}
	}
}