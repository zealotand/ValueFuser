using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameUtility
{
    using ModOps = ModifyOperations;
    public class Modifier : GameValue
    {
        public override float Value
        {
            get
            {
                if (need_re_cal)
                    Calculate();
                return value_cache;
            }
            set
            {
                throw new NotImplementedException("Setting Modifier's value is not allowed!");
            }
        }

        public float BaseValue
        {
            get { return base_value.Value; }
        }
        /// <summary>
        /// 不要频繁调用，开销较大
        /// </summary>
        /// <param name="_base_value"></param>
        public void SetBaseValue(IGameValue _base_value)
        {
            if (_base_value.Equals(this))
                throw new StackOverflowException("Do not set Modifier itself as base value!");
            base_value.NoLongerReferredBy(this);
            base_value = _base_value;
            base_value.BeReferredBy(this);
            NeedReCalculate();
        }

        public float PartnerValue
        {
            get { return partner_value.Value; }
        }

        public void SetPartnerValue(IGameValue _partner_value)
        {
            if (_partner_value.Equals(this))
                throw new StackOverflowException("Do not set Modifier itself as partner value!");
            partner_value.NoLongerReferredBy(this);
            partner_value = _partner_value;
            partner_value.BeReferredBy(this);
            NeedReCalculate(); ;
        }

        public void Swap()
        {
            IGameValue temp_value=base_value;
            base_value = partner_value;
            partner_value = temp_value;
            NeedReCalculate(); ;
        }

        public ModOps Operation
        {
            get { return mod_op; }
            set { mod_op = value; NeedReCalculate(); }
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
                stb.Remove(stb.Length - 1, 1).Append(']');
                return stb.ToString();
            }
        }

        public override float BeReferredBy(IGameValue _game_value)
        {
            if (be_referred.Add(new WeakReference<IGameValue>(_game_value)))
                return value_cache;
            else
                throw new ArgumentException("The object pointed by weakRef you added has already been in hashset!");
        }

        public override void NoLongerReferredBy(IGameValue _game_value)
        {
            be_referred.RemoveWhere(new RemoveWeakRefOfObj(_game_value).PredicateFunc);
        }

        public override void NeedReCalculate()
        {
            need_re_cal = true;
            MayBeChanged();
        }

        public void Calculate()
        {
            float temp_value = 0.0f;
            switch(mod_op)
            {
                case ModOps.ADD:
                    temp_value = base_value.Value+partner_value.Value;
                    break;
                case ModOps.MULTIPLY:
                    temp_value = base_value.Value * partner_value.Value;
                    break;
                case ModOps.MINUS:
                    temp_value = base_value.Value - partner_value.Value;
                    break;
                case ModOps.DIVIDE_BY:
                    temp_value = base_value.Value / partner_value.Value;
                    break;
                default:
                    throw new NotImplementedException();
            }
            value_cache = temp_value;
            need_re_cal = false;
            //MayBeChanged();
            Debug.Log("修改器计算:"+ base_value.Value+","+ partner_value.Value+"->"+value_cache);
        }

        private void MayBeChanged()
        {
            be_referred.RemoveWhere(RemoveNullWeakRef.PredicateFunc);

            IGameValue value_to_recalculate;
            foreach (WeakReference<IGameValue> ptr in be_referred)
            {
                if (ptr.TryGetTarget(out value_to_recalculate))
                {
                    value_to_recalculate.NeedReCalculate();
                }
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public Modifier(IGameValue _base_value, IGameValue _partner_value, ModOps _mod_ops)
        {
            be_referred = new HashSet<WeakReference<IGameValue>>(new WeakRefComparer<IGameValue>());
            need_re_cal = false;

            base_value = _base_value;
            base_value.BeReferredBy(this);
            partner_value = _partner_value;
            partner_value.BeReferredBy(this);
            mod_op = _mod_ops;
            Calculate();
        }

        private float value_cache;
        private bool need_re_cal;
        private IGameValue base_value;
        private IGameValue partner_value;
        private ModOps mod_op;
        
        internal HashSet<WeakReference<IGameValue>> be_referred;
    }
}
