using UnityEngine;

namespace WorldStreaming
{
	public static class CollisionUtility
	{
		public static bool IsIntersecting(Circle circle, Rect rect)
		{
			var deltaX = circle.Center.x - Mathf.Max(rect.xMin, Mathf.Min(circle.Center.x, rect.xMax));
			var deltaY = circle.Center.y - Mathf.Max(rect.yMin, Mathf.Min(circle.Center.y, rect.yMax));

			return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
		}

		public static bool IsIntersecting(Rect rect1, Rect rect2)
		{
			return rect1.xMin < rect2.xMax && rect1.xMax > rect2.xMin && rect1.yMin < rect2.yMax && rect1.yMax > rect2.yMin;
		}
	}
}