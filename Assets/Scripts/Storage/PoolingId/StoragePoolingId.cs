namespace WorldStreaming.Storage
{
	public class StoragePoolingId : Storage<StoragePoolingIdData>
	{
		protected override StoragePoolingIdData GetDefaultData()
		{
			return new StoragePoolingIdData();
		}
	}
}