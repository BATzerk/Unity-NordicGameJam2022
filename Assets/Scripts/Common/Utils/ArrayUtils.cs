using UnityEngine;

namespace Assets.Scripts.Common.Utils
{
    public static class ArrayUtils
    {
        public static T RandomItem<T>(this T[] arr)
        {
            return arr[Random.Range(0, arr.Length)];
        }
    }
}