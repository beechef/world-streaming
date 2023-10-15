using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	[Serializable]
	public class ChunkInfo
	{
		[Header("Chunk Info")] [ReadOnly] public int id;
		public string name;
		public ChunkArea area;

		[Header("Chunk Data")] public Vector2Int size;
		public Vector3 position;
		public Rect rect;

		[ReadOnly] public List<int> adjacentChunkIds;

		public override bool Equals(object obj)
		{
			if (obj is ChunkInfo chunkInfo)
			{
				return id == chunkInfo.id && area == chunkInfo.area;
			}

			return false;
		}
	}
}