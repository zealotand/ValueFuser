using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtility
{
    internal class WeakRefComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
    {
        public bool Equals(WeakReference<T> x, WeakReference<T> y)
        {
            //Debug.Log("Equals");
            return x.Equals(y);
        }

        public int GetHashCode(WeakReference<T> obj)
        {
            //Debug.Log("GetHashCode");
            return obj.GetHashCode();
        }
    }

    internal class RemoveWeakRefOfObj
    {
        public IGameValue game_value;
        public bool PredicateFunc(WeakReference<IGameValue> obj)
        {
            IGameValue temp;
            if (obj.TryGetTarget(out temp))
            {
                if (temp.Equals(game_value))
                {
                    Debug.Log("发现指向相同对象的弱引用");
                    return true;
                }
                else
                    return false;
            }
            else
            {
                throw new NullReferenceException("A weakReference you want to compare pointed to null!");
            }
        }
        public RemoveWeakRefOfObj(IGameValue _game_value)
        {
            game_value = _game_value;
        }
    }

    internal class RemoveNullWeakRef
    {
        public static bool PredicateFunc(WeakReference<IGameValue> obj)
        {
            IGameValue temp;
            if (obj.TryGetTarget(out temp))
            {
                return false;
            }
            else
            {
                Debug.Log("发现空指针");
                return true;
            }
        }
    }
}
