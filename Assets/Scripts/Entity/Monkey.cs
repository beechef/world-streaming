using WorldStreaming.StateData;

namespace WorldStreaming.Entity
{
	public class Monkey : Entity
	{
		public override StateDataType StateDataType { get; }

		public override IStateData StateData => _data;
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
			OnMoved?.Invoke(transform.position);
		}

		public override void OnSimulate(float deltaTime)
		{
			OnUpdate(deltaTime);
		}

		private void Update()
		{
			OnMoved?.Invoke(transform.position);

		}

		public override void OnStop()
		{
		}
	}
}