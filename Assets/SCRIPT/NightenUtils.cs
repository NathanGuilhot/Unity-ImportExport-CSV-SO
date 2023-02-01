using System.Collections.Generic;
using UnityEngine;

namespace NightenUtils
{
    public delegate int _ProcessInt(int pAmount);
    public delegate bool _CheckCondition();
    public static class ChainDelegate
    {
        public static int ChainIntDelegate(int pAmount, List<_ProcessInt> pDelegateList)
        {
            int result = pAmount;
            for (int i = 0; i < pDelegateList.Count; i++)
            {
                result = pDelegateList[i](result);
            }
            return result;

        }
    }
}
