using UnityEngine;

namespace WorldStreaming
{
	public class Circle
	{
		public Vector2 Center { get; set; }
		public float Radius { get; set; }

		public Circle(Vector2 center, float radius)
		{
			Center = center;
			Radius = radius;
		}
	}
}