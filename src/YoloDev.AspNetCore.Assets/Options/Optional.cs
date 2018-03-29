using System;

namespace YoloDev.AspNetCore.Assets.Options
{
  public struct Optional<T>
  {
    private readonly bool _hasValue;
    private readonly T _value;

    private Optional(T value)
    {
      _hasValue = true;
      _value = value;
    }

    internal void Set(ref DelegatedValue<T> delegatedValue)
    {
      if (_hasValue)
      {
        delegatedValue = _value;
      }
    }

    internal void Set<U>(ref DelegatedValue<U> delegatedValue, Func<T, U> map)
    {
      if (_hasValue)
      {
        delegatedValue = map(_value);
      }
    }

    public static implicit operator Optional<T>(T value) => new Optional<T>(value);
  }
}
