using System;
using System.Collections.Generic;

namespace WorldStreaming
{
	public class Simulator
	{
		private const float SimulationDeltaTime = 0.1f;

		private LinkedList<ISimulatedCycle> SimulatedCycles { get; } = new();
		private LinkedList<ISimulatedCycle> UnSyncedCycles { get; } = new();

		private bool _isSimulating;

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
			var currentTicks = TimeManager.Instance.CurrentTicks;
			var deltaTimeTicks = TimeSpan.FromSeconds(deltaTime).Ticks;

			while (cycle != null)
			{
				cycle.Value.OnUpdate(deltaTime);

				if (cycle.Value.Data != null)
				{
					cycle.Value.Data.LastTick += deltaTimeTicks;

					if (currentTicks - cycle.Value.Data.LastTick >= 0)
					{
						UnSyncedCycles.AddLast(cycle.Value);
					}
				}

				cycle = cycle.Next;
			}

			Simulate();
		}

		public void Stop()
		{
			foreach (var cycle in SimulatedCycles)
			{
				cycle.OnStop();
			}
		}

		public void Simulate()
		{
			if (_isSimulating) return;
			if (UnSyncedCycles.Count == 0) return;

			_isSimulating = true;

			var currentTicks = TimeManager.Instance.CurrentTicks;
			var simulationDeltaTimeTicks = TimeSpan.FromSeconds(SimulationDeltaTime).Ticks;

			while (UnSyncedCycles.Count > 0)
			{
				var cycle = UnSyncedCycles.First;

				while (cycle != null)
				{
					var deltaTimeTicks = currentTicks - cycle.Value.Data.LastTick;
					deltaTimeTicks = deltaTimeTicks > simulationDeltaTimeTicks ? simulationDeltaTimeTicks : deltaTimeTicks;

					var deltaTime = (float)TimeSpan.FromTicks(deltaTimeTicks).TotalSeconds;
					cycle.Value.OnSimulate(deltaTime);
					cycle.Value.Data.LastTick += simulationDeltaTimeTicks;

					var nextCycle = cycle.Next;
					if (cycle.Value.Data.LastTick + simulationDeltaTimeTicks >= currentTicks)
					{
						UnSyncedCycles.Remove(cycle.Value);
					}

					cycle = nextCycle;
				}
			}

			_isSimulating = false;
		}

		public void Add(ISimulatedCycle cycle, bool isTriggerStart = true, bool isSimulate = true)
		{
			SimulatedCycles.AddLast(cycle);

			var currentTick = TimeManager.Instance.CurrentTicks;
			if (cycle.Data != null && cycle.Data.LastTick < currentTick)
			{
				UnSyncedCycles.AddLast(cycle);
				if (isSimulate && !_isSimulating) Simulate();
			}

			if (isTriggerStart) cycle.OnStart();
		}

		public void Add(IEnumerable<ISimulatedCycle> cycles, bool isTriggerStart = true)
		{
			foreach (var cycle in cycles)
			{
				Add(cycle, isTriggerStart, false);
			}

			if (!_isSimulating) Simulate();
		}

		public void Remove(ISimulatedCycle cycle, bool isTriggerStop = true)
		{
			SimulatedCycles.Remove(cycle);
			UnSyncedCycles.Remove(cycle);

			if (isTriggerStop) cycle.OnStop();
		}

		public void Remove(IEnumerable<ISimulatedCycle> cycles, bool isTriggerStop = true)
		{
			foreach (var cycle in cycles)
			{
				Remove(cycle, isTriggerStop);
			}
		}

		public void Clear()
		{
			SimulatedCycles.Clear();
			UnSyncedCycles.Clear();
		}
	}
}