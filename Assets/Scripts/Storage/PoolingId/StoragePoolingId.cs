namespace WorldStreaming.Storage
{
	public class StoragePoolingId : Storage<StoragePoolingIdData>
	{
		public override StorageKey Key => StorageKey.PoolingId;

		protected override StoragePoolingIdData GetDefaultData()
		{
			return new StoragePoolingIdData();
		}
	}
}