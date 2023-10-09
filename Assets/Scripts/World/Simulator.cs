using System.Collections.Generic;
using UnityEngine;

namespace WorldStreaming
{
	public class Simulator
	{
		private const float SimulationDeltaTime = 0.1f;

		public readonly LinkedList<ISimulatedCycle> SimulatedCycles = new();

		public void Start()
		{
			foreach (var cycle in SimulatedCycles)
			{
				cycle.OnStart();
			}
		}

		public void Update(float deltaTime)
		{
			var cycle = SimulatedCycles.First;

			while (cycle != null)
			{
				var next = cycle.Next;
				cycle.Value.OnUpdate(deltaTime);
				cycle = next;
			}
		}

		public void Stop()
		{
			foreach (var cycle in SimulatedCycles)
			{
				cycle.OnStop();
			}
		}

		public void Simulate(float time)
		{
			var cycles = Mathf.RoundToInt(time / SimulationDeltaTime);

			for (var i = 0; i < cycles; i++)
			{
				Update(SimulationDeltaTime);
			}
		}
	}
}