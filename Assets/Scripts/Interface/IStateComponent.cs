using WorldStreaming.StateData;

namespace WorldStreaming
{
	public interface IStateComponent
	{
		public StateDataType StateDataType { get; }
		public void InitDefaultData();
		public IStateData StateData { get; }
		public bool IsSave { get; }

		public void SetData(IStateData data);
	}
}