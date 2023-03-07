
using UnityEngine;

namespace Utilities
{
    public static class VectorExtensions
    {
        public static bool IsInRangeXToY(this Vector2 vector2, float number)
        {
            return vector2.x < number && number <= vector2.y;
        }

        public static Vector3 SetZ(this Vector3 @this,float zValue)
        {
            return new Vector3(@this.x, @this.y, zValue);
        }
        public static Vector3 SetY(this Vector3 @this,float yValue)
        {
            return new Vector3(@this.x, yValue, @this.z);
        }
        public static Vector3 SetX(this Vector3 @this,float xValue)
        {
            return new Vector3(xValue, @this.y, @this.z);
        }
        
        public static Vector3 AddZ(this Vector3 @this,float zValue)
        {
            return new Vector3(@this.x, @this.y, @this.z + zValue);
        }
        public static Vector3 AddY(this Vector3 @this,float yValue)
        {
            return new Vector3(@this.x, @this.y + yValue, @this.z);
        }

        public static Vector3 AddWithEffect(this Vector3 @this, Vector3 additionVector,Vector3 effectVector)
        {
            return new Vector3(@this.x + (additionVector.x * effectVector.x),
                @this.y + (additionVector.y * effectVector.y), @this.z + (additionVector.z * effectVector.z));
        }
        
        public static Vector3 GetPositionOnCircle(this Vector3 centerPos,float angle,float radius)
        {
            return centerPos + 
                   new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) 
                   * radius;
        }

        public static Vector3 LerpZ(this Vector3 @this, Vector3 secondVec, float value)
        {
            value = Mathf.Clamp(value, 0f,1f);
            return new Vector3(@this.x, @this.y, Mathf.Lerp(@this.z, secondVec.z, value));
        }
        public static Vector3 LerpY(this Vector3 @this, Vector3 secondVec, float value)
        {
            value = Mathf.Clamp(value, 0f,1f);
            return new Vector3(@this.x, Mathf.Lerp(@this.y, secondVec.y, value), @this.z);
        }
        public static Vector3 LerpX(this Vector3 @this, Vector3 secondVec, float value)
        {
            value = Mathf.Clamp(value, 0f,1f);
            return new Vector3(Mathf.Lerp(@this.x, secondVec.x, value), @this.y, @this.z);
        }

        public static Vector3 LerpXZ(this Vector3 @this, Vector3 secondVec, float value)
        {
            var xLerp = @this.LerpX(secondVec, value);
            return xLerp.LerpZ(secondVec, value);
        }
        
    }
}
