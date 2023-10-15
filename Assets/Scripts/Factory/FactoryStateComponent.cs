namespace WorldStreaming.StateData
{
	public static class FactoryStateComponent
	{
		public static IStateComponent Create(IStateData data)
		{
			if (data is ISceneObject iSceneObject)
			{
				var sceneObject = SceneObjectController.SceneObjects[iSceneObject.IdSceneObject];

				var stateComponent = sceneObject.GetComponent<IStateComponent>();
				if (stateComponent == null) return null;

				stateComponent.SetData(data);
				return stateComponent;
			}

			return null;
		}
	}
}