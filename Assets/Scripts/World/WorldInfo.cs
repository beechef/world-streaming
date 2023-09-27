using System.Collections.Generic;
using UnityEngine;

namespace WorldStreaming
{
	[CreateAssetMenu]
	public class WorldInfo : ScriptableObject
	{
		public Vector2Int detectZoneSize;

		public List<ChunkInfo> chunks;
	}
}