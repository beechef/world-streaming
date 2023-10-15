using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WorldStreaming.Pooling;
using WorldStreaming.Storage;

namespace WorldStreaming
{
	[CreateAssetMenu]
	public class WorldInfo : ScriptableObject
	{
		public Vector2Int detectedZoneSize;

		public List<ChunkInfo> chunkInfos;

		public readonly Dictionary<int, ChunkInfo> IDSortedChunkInfos = new();
		public readonly Dictionary<ChunkArea, ChunkInfo> AreaSortedChunkInfos = new();

		[ShowInInspector] private readonly SerializableDictionary<ChunkArea, StoragePoolingIdData> _storagePoolingIdDataGroup = new();

		private void OnEnable()
		{
			IDSortedChunkInfos.Clear();
			AreaSortedChunkInfos.Clear();

			foreach (var chunkInfo in chunkInfos)
			{
				IDSortedChunkInfos.TryAdd(chunkInfo.id, chunkInfo);
				AreaSortedChunkInfos.TryAdd(chunkInfo.area, chunkInfo);
			}
		}

		public int GetChunkId(ChunkInfo chunkInfo)
		{
			var existChunk = chunkInfos.Find(x => x.Equals(chunkInfo));
			if (existChunk != null) return existChunk.id;

			_storagePoolingIdDataGroup.TryAdd(chunkInfo.area, new StoragePoolingIdData());

			var poolingId = new PoolingId(_storagePoolingIdDataGroup[chunkInfo.area]);
			var id = (int)poolingId.GetId();
			id += (int)chunkInfo.area * WorldRules.AreaOffset;

			return id;
		}
	}
}