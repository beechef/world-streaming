namespace WorldStreaming
{
	public interface ISimulatedCycle
	{
		public void OnStart();
		public void OnUpdate(float deltaTime);
		public void OnStop();
	}
}