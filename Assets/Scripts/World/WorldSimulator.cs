using System.Collections.Generic;
using UnityEngine;

namespace WorldStreaming
{
	[RequireComponent(typeof(WorldStreamingController))]
	public class WorldSimulator : MonoBehaviour
	{
		private WorldStreamingController _streamingController;

		private Simulator _simulator;
		private readonly LinkedList<Chunk> _chunks = new();

		private void Awake()
		{
			_simulator = new Simulator();
			_streamingController = GetComponent<WorldStreamingController>();
		}

		private void OnEnable()
		{
			_chunks.Clear();

			_streamingController.OnChunkLoaded += OnChunkLoaded;
			_streamingController.OnChunkUnloaded += OnChunkUnloaded;
		}

		private void Update()
		{
			_simulator.Update(Time.deltaTime);
		}

		private void OnDisable()
		{
			_streamingController.OnChunkLoaded -= OnChunkLoaded;
			_streamingController.OnChunkUnloaded -= OnChunkUnloaded;

			_simulator.Stop();

			_chunks.Clear();
			_simulator.SimulatedCycles.Clear();
		}

		private void OnChunkLoaded(Chunk chunk)
		{
			chunk.OnStart();

			_chunks.AddLast(chunk);
			_simulator.SimulatedCycles.AddLast(chunk);
		}

		private void OnChunkUnloaded(Chunk chunk)
		{
			chunk.OnStop();

			_chunks.Remove(chunk);
			_simulator.SimulatedCycles.Remove(chunk);
		}
	}
}