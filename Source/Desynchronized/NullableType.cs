﻿using System;
using Verse;

namespace Desynchronized
{
    public struct NullableType<T> : IExposable where T : struct
    {
        private bool hasValue;

        private T insideValue;

        public bool HasValue => hasValue;

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("NullableType<T> : IExposable has no value.");
                }

                return insideValue;
            }
        }

        public NullableType(T initialValue)
        {
            hasValue = true;
            insideValue = initialValue;
        }
        
        public void ExposeData()
        {
            Scribe_Values.Look(ref hasValue, "hasValue");
            Scribe_Values.Look(ref insideValue, "insideValue");
        }

        public T GetValueOrDefault(T defaultValue = default(T))
        {
            if (HasValue)
            {
                return Value;
            }
            else
            {
                return defaultValue;
            }
        }

        public override bool Equals(object obj)
        {
            if (!hasValue)
            {
                return obj == null;
            }
            else
            {
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    return base.Equals(obj);
                }
            }
        }

        public override int GetHashCode()
        {
            if (hasValue)
            {
                return Value.GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            if (hasValue)
            {
                return Value.ToString();
            }
            else
            {
                return "";
            }
        }

        public static implicit operator NullableType<T>(T from)
        {
            return new NullableType<T>(from);
        }

        public static implicit operator NullableType<T>(T? from)
        {
            if (from.HasValue)
            {
                return new NullableType<T>(from.Value);
            }
            else
            {
                return new NullableType<T>();
            }
        }

        public static implicit operator T?(NullableType<T> from)
        {
            if (from.hasValue)
            {
                return new T?(from.Value);
            }
            else
            {
                return new T?();
            }
        }

        public static explicit operator T(NullableType<T> from)
        {
            return from.Value;
        }
    }
}
