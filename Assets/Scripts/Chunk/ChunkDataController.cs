using UnityEngine;
using WorldStreaming.StateData;

namespace WorldStreaming
{
	public class ChunkDataController : MonoBehaviour
	{
		public ChunkData Data { get; private set; }

		public void InitDefaultData()
		{
			Data = new ChunkData();

			var stateComponents = gameObject.GetAllComponentInChildren<IStateComponent>();

			foreach (var stateComponent in stateComponents)
			{
				InitDefaultData(stateComponent);
			}
		}

		private void InitDefaultData(IStateComponent stateComponent)
		{
			var currentTick = TimeManager.Instance.CurrentTicks;

			stateComponent.InitDefaultData();

			if (stateComponent.StateData is ISimulatedCycleData cycleData)
			{
				cycleData.LastTick = currentTick;
			}

			Data.StateComponents.Add(stateComponent);
			Data.StateData.Add(stateComponent.StateData);
		}

		public void Init(ChunkData data)
		{
			Data = data;

			foreach (var stateData in data.StateData)
			{
				var stateComponent = FactoryStateComponent.Create(stateData);
				Data.StateComponents.Add(stateComponent);
			}

			var stateComponents = gameObject.GetAllComponentInChildren<IStateComponent>();
			foreach (var stateComponent in stateComponents)
			{
				if (stateComponent.StateData != null) continue;
				
				InitDefaultData(stateComponent);
			}
		}
	}
}