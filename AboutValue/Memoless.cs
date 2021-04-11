using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtility
{
    public struct Memoless : IGameValue
    {
        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public float BeReferredBy(IGameValue _game_value)
        {
            //�޼��䲻��Ҫ��¼�Լ������õ����
            return value;
        }

        public void NoLongerReferredBy(IGameValue _game_value)
        {
            ;
        }

        public void NeedReCalculate()
        {
            //�޼��䲻��Ҫ����,��Ϊ��������������Ϸֵ�������Լ�������ֵ
            ;
        }

        //public Memoless() { value = 0; }
        public Memoless(float num) { value = num; }
        public object Clone()
        {
            return new Memoless(this.value);
        }

        public string BeReferredObjectsValues
        {
            get { return "[Memoless won\'t be referred by other IGameValues]"; }
        }

        public static explicit operator Memoless(float num){return new Memoless(num);}

        public static Modifier operator +(Memoless x, Memoless y) { return new Modifier(x, y, ModifyOperations.ADD); }
        public static Modifier operator +(Memoless x, IGameValue y) { return new Modifier(x, y, ModifyOperations.ADD); }
        public static Modifier operator +(IGameValue x, Memoless y) { return new Modifier(x, y, ModifyOperations.ADD); }

        public static Modifier operator *(Memoless x, Memoless y){return new Modifier(x, y, ModifyOperations.MULTIPLY); }
        public static Modifier operator *(Memoless x, IGameValue y){return new Modifier(x, y, ModifyOperations.MULTIPLY);}
        public static Modifier operator *(IGameValue x, Memoless y){return new Modifier(x, y, ModifyOperations.MULTIPLY);}

        public static Modifier operator /(Memoless x, Memoless y) { return new Modifier(x, y, ModifyOperations.DIVIDE_BY); }
        public static Modifier operator /(Memoless x, IGameValue y) { return new Modifier(x, y, ModifyOperations.DIVIDE_BY); }
        public static Modifier operator /(IGameValue x, Memoless y) { return new Modifier(x, y, ModifyOperations.DIVIDE_BY); }

        public static Modifier operator -(Memoless x, Memoless y) { return new Modifier(x, y, ModifyOperations.MINUS); }
        public static Modifier operator -(Memoless x, IGameValue y) { return new Modifier(x, y, ModifyOperations.MINUS); }
        public static Modifier operator -(IGameValue x, Memoless y) { return new Modifier(x, y, ModifyOperations.MINUS); }

        private float value;
    }
}
