using UnityEngine;
using WorldStreaming.StateData;

namespace WorldStreaming.Entity
{
	public class Monkey : Entity
	{
		public override StateDataType StateDataType { get; }

		public override IStateData StateData => _data;
		public override bool IsSave { get; }

		public override ISimulatedCycleData Data { get; }

		private StateDataMonkey _data;

		public override void InitDefaultData()
		{
			_data = new StateDataMonkey();
		}

		public override void SetData(IStateData data)
		{
			_data = data as StateDataMonkey;
		}

		public override void OnStart()
		{
		}

		public override void OnUpdate(float deltaTime)
		{
			transform.position += Vector3.forward * deltaTime;
		}

		public override void OnStop()
		{
		}
	}
}