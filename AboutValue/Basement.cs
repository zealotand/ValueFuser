using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameUtility
{
    public class Basement:GameValue
    {
        public override float Value
        {
            get { return value; }
            set { this.value = value;MayBeChanged(); }
        }

        public override string BeReferredObjectsValues
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.Append('[');
                foreach (WeakReference<IGameValue> ptr in be_referred)
                {
                    IGameValue ig;
                    if (ptr.TryGetTarget(out ig))
                    {
                        stb.Append(ig.GetType().Name).Append(':');
                        stb.Append(ig.Value.ToString());
                    }
                    else
                        stb.Append(ptr.ToString());
                    stb.Append(',');
                }
                if (stb.Length > 1)
                    stb.Remove(stb.Length - 1, 1);
                stb.Append(']');
                return stb.ToString();
            }
        }

        public override float BeReferredBy(IGameValue _game_value)
        {
            if (be_referred.Add(new WeakReference<IGameValue>(_game_value)))
                return value;
            else
                throw new ArgumentException("The object pointed by weakRef you added has already been in hashset!");
        }

        public override void NoLongerReferredBy(IGameValue _game_value)
        {
            be_referred.RemoveWhere(new RemoveWeakRefOfObj(_game_value).PredicateFunc);
        }

        public override void NeedReCalculate()
        {
            //地基不需要重算,因为它不依赖其它游戏值来计算自己的最终值
            ;
        }

        private void MayBeChanged()
        {
            be_referred.RemoveWhere(RemoveNullWeakRef.PredicateFunc);

            IGameValue value_to_recalculate;
            foreach(WeakReference<IGameValue> ptr in be_referred)
            {
                if(ptr.TryGetTarget(out value_to_recalculate))
                {
                    value_to_recalculate.NeedReCalculate();
                }
            }
        }

        public Basement(float _value)
        {
            be_referred = new HashSet<WeakReference<IGameValue>>(new WeakRefComparer<IGameValue>());

            value = _value;
        }

        public Basement() : this(0) { }

        public override object Clone()
        {
            return new Memoless(this.value);
        }

        //public static explicit operator Basement(float num) { return new Basement(num); }

        private float value;
        internal HashSet<WeakReference<IGameValue>> be_referred;
    }
}
