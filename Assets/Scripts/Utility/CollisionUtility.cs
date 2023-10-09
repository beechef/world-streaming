using UnityEngine;

namespace WorldStreaming
{
	public static class CollisionUtility
	{
		public static bool IsIntersecting(this Circle circle, Rect rect)
		{
			var deltaX = circle.Center.x - Mathf.Max(rect.xMin, Mathf.Min(circle.Center.x, rect.xMax));
			var deltaY = circle.Center.y - Mathf.Max(rect.yMin, Mathf.Min(circle.Center.y, rect.yMax));

			return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
		}
		
		public static bool IsIntersecting(this Rect rect, Circle circle)
		{
			return IsIntersecting(circle, rect);
		}

		public static bool IsIntersecting(this Rect rect1, Rect rect2)
		{
			return rect1.Overlaps(rect2, true);
		}

		public static bool IsContains(this Rect rect, Vector2 point)
		{
			return rect.Contains(point);
		}
	}
}