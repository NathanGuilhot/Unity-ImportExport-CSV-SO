using System;
using System.Collections.Generic;
using UnityEngine;

namespace NightenUtils
{
    public delegate bool _CheckCondition();
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
}
