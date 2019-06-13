using System;
using UnityEngine;

public static class MathsUtils
{
 
    public static bool IsApproximately(this Vector3 vec, Vector3 other, float theta = 0.01f)
    {
        return (Mathf.Abs(vec.x - other.x) < theta) && (Mathf.Abs(vec.y - other.y) < theta) && (Mathf.Abs(vec.z - other.z) < theta);
    }

    public static bool IsApproximately(this float a, float b, float theta = 0.01f)
    {
        return Math.Abs(a - b) < theta;
    }

    public static bool IsApproximately(this double a, double b, double theta = 0.01)
    {
        return Math.Abs(a - b) < theta;
    }

    public static bool IsApproximately(this Color c1, Color c2, float epsilon = 0.001f) =>
        Mathf.Abs(c1.r - c2.r) < epsilon
        && Mathf.Abs(c1.g - c2.g) < epsilon
        && Mathf.Abs(c1.b - c2.b) < epsilon
        && Mathf.Abs(c1.a - c2.a) < epsilon;

    public static bool IsApproximately(this Quaternion quat1, Quaternion quat2, float epsilon = 0.01f) =>
        Mathf.Abs(quat1.x - quat2.x) < epsilon
        && Mathf.Abs(quat1.y - quat2.y) < epsilon
        && Mathf.Abs(quat1.z - quat2.z) < epsilon
        && Mathf.Abs(quat1.w - quat2.w) < epsilon;


    /// <returns></returns>
    public static Vector3 RoundedTo(this Vector3 vec, int dp = 0)
    {
        float f = 1f;
        for (int i = dp; i-- > 0;)
            f *= 10f;

        Vector3 v = new Vector3(
            Mathf.Round(vec.x * f) / f,
            Mathf.Round(vec.y * f) / f,
            Mathf.Round(vec.z * f) / f
        );

        return v;
    }


    public static Vector3 RoundedTo(this Vector3 vec, float f)
    {
        Vector3 v = new Vector3(
            Mathf.Round(vec.x * f) / f,
            Mathf.Round(vec.y * f) / f,
            Mathf.Round(vec.z * f) / f
        );

        return v;
    }

    public static double RoundDownToNearest(this double value, double interval) =>
        interval * Math.Floor(value / interval);

    public static double RoundUpToNearest(this double value, double interval) =>
        interval * Math.Ceiling(value / interval);

  
    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

   
}