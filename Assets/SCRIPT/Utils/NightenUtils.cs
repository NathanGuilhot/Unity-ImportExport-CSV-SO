using System;
using System.Collections.Generic;
using UnityEngine;

namespace NightenUtils
{
    public static class ChainDel
    {
        public static T ChainDelegate<T>(T pAmount, List<Func<T, T>> pDelegateList)
        {
            T result = pAmount;
            for (int i = 0; i < pDelegateList.Count; i++)
            {
                result = pDelegateList[i](result);
            }
            return result;

        }
    }

    [System.Serializable]
    public class KeyValuePair<TKey, TValue>
    {
        public KeyValuePair()
        {
        }

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        [field: SerializeField] public TKey Key { get; set; }
        [field: SerializeField] public TValue Value { get; set; }
    }

    //Extension for the Random
    static class _Random
    {
        public static T[] Shuffle<T>(T[] pArray)
        {
            T[] shuffledArray = pArray.Clone() as T[];
            for (int i = 0; i < shuffledArray.Length; i++)
            {
                int r = UnityEngine.Random.Range(i, shuffledArray.Length);
                (shuffledArray[i], shuffledArray[r]) = (shuffledArray[r], shuffledArray[i]);
            }
            return pArray;
        }
    }
}
