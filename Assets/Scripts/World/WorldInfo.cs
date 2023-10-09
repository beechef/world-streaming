using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace WorldStreaming
{
	[CreateAssetMenu]
	public class WorldInfo : ScriptableObject
	{
		public Vector2Int detectedZoneSize;

		public List<ChunkInfo> chunkInfos;

		public readonly Dictionary<int, ChunkInfo> idSortedChunkInfos = new();
		public readonly Dictionary<ChunkArea, ChunkInfo> areaSortedChunkInfos = new();

		private void OnEnable()
		{
			idSortedChunkInfos.Clear();
			areaSortedChunkInfos.Clear();

			foreach (var chunkInfo in chunkInfos)
			{
				idSortedChunkInfos.TryAdd(chunkInfo.id, chunkInfo);
				areaSortedChunkInfos.TryAdd(chunkInfo.area, chunkInfo);
			}
		}
	}
}