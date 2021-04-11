using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUtility
{
    [Obsolete]
    public class Fuser:GameValue
    {
        //实现接口，返回一个值
        public override float Value
        {
            get { return value_cache; }
        }
        //与基础值交互
        public float BaseValue
        {
            get { return base_value; }
            set { base_value = value; }
        }

        public override string BeReferredObjectsValues => throw new NotImplementedException();

        public override object Clone()
        {
            return new Memoless(this.value_cache);
        }

        public Fuser(float _base_value)
        {
            base_value = _base_value;
            value_cache = base_value;
            be_referred = new List<WeakReference<IGameValue>>();
            name_seq = new List<string>();
            //name_to_log = new Dictionary<string, FuserLog>();
        }

        public Fuser() : this(0) { }

        public override float BeReferredBy(IGameValue _game_value)
        {
            be_referred.Add(new WeakReference<IGameValue>(_game_value));
            return value_cache;
        }

        public override void NeedReCalculate()
        {
            throw new NotImplementedException();
        }

        public void BeChanged()
        {
            IGameValue value_to_recalculate;
            foreach (WeakReference<IGameValue> ptr in be_referred)
            {
                if (ptr.TryGetTarget(out value_to_recalculate))
                {
                    value_to_recalculate.NeedReCalculate();
                }
                else
                {
                    be_referred.Remove(ptr);
                }
            }
        }

        public override void NoLongerReferredBy(IGameValue _game_value)
        {
            throw new NotImplementedException();
        }


        //值缓存，用于减少值未更新时对其频繁的读带来的开销
        private float value_cache;
        //基础值，最基础的值，其它层的值基于其计算而来
        private float base_value;

        internal List<WeakReference<IGameValue>> be_referred;

        private List<string> name_seq;
        //private Dictionary<string, FuserLog> name_to_log;
    }
}
