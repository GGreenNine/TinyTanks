using UnityEngine;

namespace HappyUnity.Math
{
    public static class VectorExtensions
    {
        /** Returns the angle in degrees of this vector relative to the x-axis. Angles are towards the positive y-axis (typically
	     *  counter-clockwise) and between 0 and 360. 
         */
        public static float AngleX(this Vector2 vector)
        {
            float angle = AngleXRad(vector) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }


        /** Returns the angle in degrees of this vector relative to the x-axis. Angles are towards the positive y-axis (typically
       *  counter-clockwise) and between 0 and 360. 
       */
        public static float AngleX(this Vector3 vector)
        {
            float angle = AngleXRad(vector) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        /** Returns the angle in radians of this vector (point) relative to the x-axis. Angles are towards the positive y-axis.
	      * (typically counter-clockwise) 
          */
        public static float AngleXRad(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        /** Returns the angle in radians of this vector (point) relative to the x-axis. Angles are towards the positive y-axis.
	      * (typically counter-clockwise) 
          */
        public static float AngleXRad(this Vector3 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        /** Rotates the Vector2 by the given angle, counter-clockwise assuming the y-axis points up.
         * Parameter degrees - the angle in degrees */
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            return RotateRad(vector, degrees * Mathf.Deg2Rad);
        }

        /** Rotates the Vector2 by the given angle, counter-clockwise assuming the y-axis points up.
         * Parameter radians - the angle in radians */
        public static Vector2 RotateRad(this Vector2 vector, float radians)
        {
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            vector.x = newX;
            vector.y = newY;

            return vector;
        }

        public static bool PrecisionEquals(this Vector2 lhs, Vector2 rhs, float precision)
        {
            return Vector2.SqrMagnitude(lhs - rhs) < precision;
        }

        public static bool PrecisionEquals(this Vector3 lhs, Vector3 rhs, float precision)
        {
            return Vector3.SqrMagnitude(lhs - rhs) < precision;
        }

        public static bool PrecisionEquals(this Vector4 lhs, Vector4 rhs, float precision)
        {
            return Vector4.SqrMagnitude(lhs - rhs) < precision;
        }

        public static Vector2 SetX(this Vector2 vector, float value)
        {
            vector.x = value;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }

        public static Vector4 SetX(this Vector4 vector, float value)
        {
            vector.x = value;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float value)
        {
            vector.y = value;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }

        public static Vector4 SetY(this Vector4 vector, float value)
        {
            vector.y = value;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }

        public static Vector4 SetZ(this Vector4 vector, float value)
        {
            vector.z = value;
            return vector;
        }

        public static Vector4 SetW(this Vector4 vector, float value)
        {
            vector.w = value;
            return vector;
        }

        public static Vector2 Set(this Vector2 vector, float x, float y)
        {
            vector.x = x;
            vector.y = y;
            return vector;
        }

        public static Vector3 Set(this Vector3 vector, float x, float y, float z)
        {
            vector.x = x;
            vector.y = y;
            vector.z = z;
            return vector;
        }

        public static Vector4 Set(this Vector4 vector, float x, float y, float z, float w)
        {
            vector.x = x;
            vector.y = y;
            vector.z = z;
            vector.w = w;
            return vector;
        }
    }
}