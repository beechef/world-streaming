using UnityEngine;

namespace WorldStreaming
{
	[RequireComponent(typeof(WorldStreamingController), typeof(WorldDataController))]
	public class World : MonoBehaviour
	{
		[SerializeField] private WorldInfo worldInfo;

		private WorldStreamingController _streamingController;
		private WorldDataController _dataController;

		private void Awake()
		{
			_streamingController = GetComponent<WorldStreamingController>();
			_streamingController.worldInfo = worldInfo;

			_dataController = GetComponent<WorldDataController>();
		}

		private void OnEnable()
		{
			_dataController.Clear();

			_streamingController.OnChunkLoaded += OnChunkLoaded;
			_streamingController.OnChunkUnloaded += OnChunkUnloaded;
		}

		private void OnDisable()
		{
			_streamingController.OnChunkLoaded -= OnChunkLoaded;
			_streamingController.OnChunkUnloaded -= OnChunkUnloaded;
		}

		private void OnChunkLoaded(Chunk chunk)
		{
			_dataController.Add(chunk);
		}

		private void OnChunkUnloaded(Chunk chunk)
		{
			_dataController.Remove(chunk);
		}
	}
}