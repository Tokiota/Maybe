namespace MaybeMonad
{
    using System;
    using System.Collections.Generic;

    public class Maybe<T> : IEquatable<Maybe<T>>
    {
        #region Static Fields

        public static Maybe<T> Nothing = new Maybe<T>();

        #endregion

        #region Constructors and Destructors

        public Maybe(T valor, bool hasValue)
        {
            HasValue = hasValue;
            Value = valor;
        }

        private Maybe()
        {
            HasValue = false;
        }

        #endregion

        #region Public Properties

        public bool HasValue { get; private set; }

        public T Value { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(Maybe<T> left,
                                       Maybe<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Maybe<T> left,
                                       Maybe<T> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Maybe<T> other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return HasValue.Equals(other.HasValue)
                   && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj))
            {
                return false;
            }
            if(ReferenceEquals(this, obj))
            {
                return true;
            }
            if(obj.GetType()
               != typeof(Maybe<T>))
            {
                return false;
            }
            return Equals((Maybe<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (HasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }

        #endregion
    }
}