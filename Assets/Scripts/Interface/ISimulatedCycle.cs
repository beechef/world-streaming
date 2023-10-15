namespace WorldStreaming
{
	public interface ISimulatedCycle
	{
		public ISimulatedCycleData Data { get; }
		public void OnStart();
		public void OnUpdate(float deltaTime);
		public void OnStop();
	}
}