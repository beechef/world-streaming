using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace WorldStreaming
{
	public class Chunk : MonoBehaviour, ISimulatedCycle
	{
		public ChunkInfo info;

		[ShowInInspector] private List<ISimulatedCycle> _simulatedCycles;
		private Simulator _simulator = new();

		private void Awake()
		{
			_simulatedCycles = gameObject.GetAllComponentInChildren<ISimulatedCycle>();
			_simulator.SimulatedCycles.AddRange(_simulatedCycles);
		}

		private void OnEnable()
		{
			gameObject.name = name;
			transform.position = info.position;
		}

		public void OnStart()
		{
			_simulator.Start();
		}

		public void OnUpdate(float deltaTime)
		{
			_simulator.Update(deltaTime);
		}

		public void OnStop()
		{
			_simulator.Stop();
		}

		public void Simulate(float time)
		{
			_simulator.Simulate(time);
		}

		public void Bake()
		{
			if (string.IsNullOrEmpty(name)) name = gameObject.name;

			info.position = transform.position;
			info.rect = new Rect(info.position.x - info.size.x / 2f, info.position.z - info.size.y / 2f, info.size.x, info.size.y);
		}
	}
}