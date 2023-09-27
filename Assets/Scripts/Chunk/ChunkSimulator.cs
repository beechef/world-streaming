using UnityEngine;

namespace WorldStreaming
{
	[RequireComponent(typeof(ChunkInfo))]
	public class ChunkSimulator : MonoBehaviour, ISimulatedCycle
	{
		private ChunkInfo _chunkInfo;

		private void Awake()
		{
			_chunkInfo = GetComponent<ChunkInfo>();
		}

		public void OnUpdate(float deltaTime)
		{
		}
	}
}