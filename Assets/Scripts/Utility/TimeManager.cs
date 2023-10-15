using System;
using UnityEngine;

namespace WorldStreaming
{
	[DefaultExecutionOrder(-100)]
	public class TimeManager : Singleton<TimeManager>
	{
		public long CurrentTicks { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			CurrentTicks = DateTime.UtcNow.Ticks;
		}

		private void Update()
		{
			CurrentTicks += TimeSpan.FromSeconds(Time.deltaTime).Ticks;
		}
	}
}