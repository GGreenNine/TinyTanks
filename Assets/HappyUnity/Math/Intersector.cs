using UnityEngine;

namespace HappyUnity.Math
{
    public static class Intersector
    {
        public static Rect Intersection(this Rect rectangle1, Rect rectangle2)
        {
            float xMin = Mathf.Max(rectangle1.xMin, rectangle2.xMin);
            float yMin = Mathf.Max(rectangle1.yMin, rectangle2.yMin);
            float xMax = Mathf.Min(rectangle1.xMax, rectangle2.xMax);
            float yMax = Mathf.Min(rectangle1.yMax, rectangle2.yMax);
            return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }
    }
}