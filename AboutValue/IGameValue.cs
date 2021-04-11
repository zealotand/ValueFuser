using System;
using System.Collections.Generic;

namespace GameUtility
{
    public interface IGameValue : ICloneable
    {
        float Value { get; }
        string BeReferredObjectsValues { get; }

        float BeReferredBy(IGameValue _game_value);
        void NoLongerReferredBy(IGameValue _game_value);
        void NeedReCalculate();

    }

    public abstract class GameValue : IGameValue
    {
        public virtual float Value { get; set; }
        public abstract string BeReferredObjectsValues { get; }

        public abstract float BeReferredBy(IGameValue _game_value);
        public abstract object Clone();
        public abstract void NeedReCalculate();
        public abstract void NoLongerReferredBy(IGameValue _game_value);
        public static Modifier operator+(GameValue x,GameValue y)
        {
            return new Modifier(x, y, ModifyOperations.ADD);
        }
        public static Modifier operator *(GameValue x, GameValue y)
        {
            return new Modifier(x, y, ModifyOperations.MULTIPLY);
        }
        public static Modifier operator -(GameValue x, GameValue y)
        {
            return new Modifier(x, y, ModifyOperations.MINUS);
        }
        public static Modifier operator /(GameValue x, GameValue y)
        {
            return new Modifier(x, y, ModifyOperations.DIVIDE_BY);
        }
    }
}
