using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HappyUnity.Math
{
    [Serializable]
    public class Cuboid
    {
        public float Left
        {
            get
            {
                return Center.x - Width / 2f;
            }
        }

        public float Bottom
        {
            get
            {
                return Center.y - Height / 2f;
            }
        }

        public float Back
        {
            get
            {
                return Center.z - Depth / 2f;
            }
        }

        public float Right
        {
            get
            {
                return Center.x + Width / 2f;
            }
        }

        public float Top
        {
            get
            {
                return Center.y + Height / 2f;
            }
        }

        public float Front
        {
            get
            {
                return Center.z + Depth / 2f;
            }
        }

        public float Width
        {
            get
            {
                return Size.x;
            }
        }

        public float Height
        {
            get
            {
                return Size.y;
            }
        }

        public float Depth
        {
            get
            {
                return Size.z;
            }
        }


        public Vector3 Center;
        public Vector3 Size;

        public Vector3 LeftBottomBackVertex
        {
            get
            {
                return new Vector3(Left, Bottom, Back);
            }
        }

        public Vector3 LeftBottomFrontVertex
        {
            get
            {
                return new Vector3(Left, Bottom, Front);
            }
        }

        public Vector3 LeftTopBackVertex
        {
            get
            {
                return new Vector3(Left, Top, Back);
            }
        }

        public Vector3 LeftTopFrontVertex
        {
            get
            {
                return new Vector3(Left, Top, Front);
            }
        }

        public Vector3 RightBottomBackVertex
        {
            get
            {
                return new Vector3(Right, Bottom, Back);
            }
        }

        public Vector3 RightBottomFrontVertex
        {
            get
            {
                return new Vector3(Right, Bottom, Front);
            }
        }

        public Vector3 RightTopBackVertex
        {
            get
            {
                return new Vector3(Right, Top, Back);
            }
        }

        public Vector3 RightTopFrontVertex
        {
            get
            {
                return new Vector3(Right, Top, Front);
            }
        }

        public float RandomX()
        {
            return Random.Range(Left, Right);
        }

        public float RandomY()
        {
            return Random.Range(Bottom, Top);
        }

        public float RandomZ()
        {
            return Random.Range(Back, Front);
        }

        public void DrawWireGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(Center, Size);
        }
    }
}