using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WorldStreaming
{
	public static class WorldRules
	{
		private const string ChunkRootPath = "Assets/Prefabs/WorldStreaming/Chunks/";
		private const string ChunkAreaPath = ChunkRootPath + "{0}/";
		private const string ChunkPrefabPath = ChunkAreaPath + "{1}.prefab";

		private const string AreaGroup = "World_Streaming-{0}";
		private const string ChunkKey = "Chunk-{0}-{1}-{2}";

		private const int AreaOffset = 1000;
		public const float MinDetectedDistance = 1f;

		public static string GetAreaPath(ChunkInfo chunkInfo)
		{
			return string.Format(ChunkAreaPath, chunkInfo.area);
		}

		public static string GetPrefabPath(ChunkInfo chunkInfo)
		{
			return string.Format(ChunkPrefabPath, chunkInfo.area, chunkInfo.name);
		}

		public static bool IsExistPrefab(ChunkInfo chunkInfo)
		{
			var path = GetPrefabPath(chunkInfo);
			return AssetDatabase.LoadAssetAtPath<GameObject>(path);
		}

		public static void SaveChunkInfo(Chunk chunk)
		{
			chunk = PrefabUtility.SaveAsPrefabAssetAndConnect(chunk.gameObject, GetPrefabPath(chunk.info), InteractionMode.AutomatedAction)
				.GetComponent<Chunk>();
			CreateAddressableEntry(chunk);
		}

		public static void CreateChunkPrefab(Chunk chunk)
		{
			var areaPath = GetAreaPath(chunk.info);
			Directory.CreateDirectory(areaPath);

			SaveChunkInfo(chunk);
		}

		public static string GetAreaGroup(ChunkArea area)
		{
			return string.Format(AreaGroup, area);
		}

		public static string GetChunkKey(ChunkInfo chunkInfo)
		{
			return string.Format(ChunkKey, chunkInfo.area, chunkInfo.id, chunkInfo.name);
		}

		public static void CreateAddressableEntry(Chunk chunk)
		{
			var areaGroup = GetAreaGroup(chunk.info.area);
			var chunkKey = GetChunkKey(chunk.info);
			var settings = AddressableAssetSettingsDefaultObject.Settings;

			var group = settings.FindGroup(areaGroup);
			group ??= settings.CreateGroup(areaGroup, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
			var guid = AssetDatabase.AssetPathToGUID(GetPrefabPath(chunk.info));

			var entry = settings.CreateOrMoveEntry(guid, group);
			entry.SetAddress(chunkKey);
			entry.SetLabel(areaGroup, true, true);
		}

		public static int CreateChunkId(Dictionary<ChunkArea, int> ids, ChunkInfo chunkInfo)
		{
			var area = chunkInfo.area;
			ids.TryAdd(area, (int)area * AreaOffset);

			ids[area]++;
			return ids[area];
		}

		public static List<Vector2> GetAdjacentChunkPositions(ChunkInfo targetChunk)
		{
			var adjacentChunkPositions = new List<Vector2>();

			var leftCenter = new Vector2(targetChunk.rect.xMin, targetChunk.rect.center.y);
			var rightCenter = new Vector2(targetChunk.rect.xMax, targetChunk.rect.center.y);
			var topCenter = new Vector2(targetChunk.rect.center.x, targetChunk.rect.yMax);
			var bottomCenter = new Vector2(targetChunk.rect.center.x, targetChunk.rect.yMin);

			var leftTop = new Vector2(targetChunk.rect.xMin, targetChunk.rect.yMax);
			var rightTop = new Vector2(targetChunk.rect.xMax, targetChunk.rect.yMax);
			var leftBottom = new Vector2(targetChunk.rect.xMin, targetChunk.rect.yMin);
			var rightBottom = new Vector2(targetChunk.rect.xMax, targetChunk.rect.yMin);

			adjacentChunkPositions.Add(leftCenter);
			adjacentChunkPositions.Add(rightCenter);
			adjacentChunkPositions.Add(topCenter);
			adjacentChunkPositions.Add(bottomCenter);

			adjacentChunkPositions.Add(leftTop);
			adjacentChunkPositions.Add(rightTop);
			adjacentChunkPositions.Add(leftBottom);
			adjacentChunkPositions.Add(rightBottom);

			return adjacentChunkPositions;
		}

		public static List<Rect> GetAdjacentChunkRect(ChunkInfo targetChunk, Vector2 detectedZoneSize)
		{
			var adjacentChunkRects = new List<Rect>();
			var adjacentChunkPositions = GetAdjacentChunkPositions(targetChunk);

			detectedZoneSize += Vector2.one;

			foreach (var adjacentChunkPosition in adjacentChunkPositions)
			{
				var adjacentChunkRect = new Rect(adjacentChunkPosition.x - detectedZoneSize.x / 2f, adjacentChunkPosition.y - detectedZoneSize.y / 2f,
					detectedZoneSize.x, detectedZoneSize.y);

				adjacentChunkRects.Add(adjacentChunkRect);
			}

			return adjacentChunkRects;
		}

		public static List<int> GetAdjacentChunkIds(Chunk targetChunk, List<Chunk> chunks, Vector2 detectedZoneSize)
		{
			var adjacentChunkIds = new List<int>();
			var adjacentChunkRects = GetAdjacentChunkRect(targetChunk.info, detectedZoneSize);

			foreach (var chunk in chunks)
			{
				if (targetChunk.info.id == chunk.info.id) continue;

				foreach (var adjacentChunkRect in adjacentChunkRects)
				{
					if (!adjacentChunkRect.IsIntersecting(chunk.info.rect)) continue;

					adjacentChunkIds.Add(chunk.info.id);
					break;
				}
			}

			return adjacentChunkIds;
		}
	}
}