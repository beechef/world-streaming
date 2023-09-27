using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldStreaming
{
	public class WorldStreamingController : MonoBehaviour
	{
		[SerializeField] private WorldInfo worldInfo;
		[ShowInInspector] private IObservableMovement target;

		private List<ChunkInfo> _loadedChunks = new();

		private void OnEnable()
		{
			target = GetComponentInChildren<IObservableMovement>();
			target.OnMoved += OnTargetMoved;
		}

		private void OnDisable()
		{
			target.OnMoved -= OnTargetMoved;
		}

		private void OnTargetMoved(Vector3 position)
		{
			var chunks = worldInfo.chunks;
			var detectZone = new Rect()
			{
				center = new Vector2(position.x, position.z),
				size = worldInfo.detectZoneSize
			};

			foreach (var chunk in chunks)
			{
				var existingChunk = _loadedChunks.Find(c => c.id == chunk.id);
				if (existingChunk)
				{
					if (CollisionUtility.IsIntersecting(detectZone, chunk.rect)) continue;
					Destroy(existingChunk.gameObject);
					_loadedChunks.Remove(existingChunk);
				}
				else
				{
					if (!CollisionUtility.IsIntersecting(detectZone, chunk.rect)) continue;

					var newChunk = Instantiate(chunk, transform);
					newChunk.transform.position = chunk.position;
					newChunk.gameObject.SetActive(true);
					newChunk.gameObject.GetOrAddComponent<ChunkSimulator>();

					_loadedChunks.Add(newChunk);
				}
			}
		}
	}
}