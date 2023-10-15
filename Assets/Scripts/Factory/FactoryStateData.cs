namespace WorldStreaming.StateData
{
	public static class FactoryStateData
	{
		public static IStateData Create(StateDataType type)
		{
			switch (type)
			{
				case StateDataType.BananaTree:
					return new StateDataBananaTree();

				case StateDataType.Monkey:
					return new StateDataMonkey();

				default:
					return null;
			}
		}
	}
}