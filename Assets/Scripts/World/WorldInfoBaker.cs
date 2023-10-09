using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace WorldStreaming
{
	public class WorldInfoBaker : MonoBehaviour
	{
		[SerializeField] private WorldInfo worldInfo;
		[SerializeField] private List<Chunk> chunks;

		private void Refresh()
		{
			chunks = gameObject.GetAllComponentInChildren<Chunk>();

			var chunkIds = new Dictionary<ChunkArea, int>();

			foreach (var chunk in chunks)
			{
				chunk.Bake();

				chunk.info.id = WorldRules.CreateChunkId(chunkIds, chunk.info);
				chunk.info.adjacentChunkIds = WorldRules.GetAdjacentChunkIds(chunk, chunks, worldInfo.detectedZoneSize);
			}
		}

		[Button]
		public void Bake()
		{
			Refresh();

			worldInfo.chunkInfos.Clear();
			foreach (var chunk in chunks)
			{
				var isExist = WorldRules.IsExistPrefab(chunk.info);

				if (isExist)
				{
					WorldRules.SaveChunkInfo(chunk);
				}
				else
				{
					WorldRules.CreateChunkPrefab(chunk);
				}

				var chunkPrefab = AssetDatabase.LoadAssetAtPath<Chunk>(WorldRules.GetPrefabPath(chunk.info));
				worldInfo.chunkInfos.Add(chunkPrefab.info);
			}
		}
	}
}