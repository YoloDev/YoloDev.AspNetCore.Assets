using System;

namespace YoloDev.AspNetCore.Assets.Options
{
  public class DelegatedValue<T>
  {
    private readonly DelegatedValue<T> _parent;
    private readonly T _value;

    internal DelegatedValue(DelegatedValue<T> parent)
    {
      _parent = parent ?? throw new ArgumentNullException(nameof(parent));
      _value = default(T);
    }

    private DelegatedValue(T value)
    {
      _parent = null;
      _value = value;
    }

    public bool IsSet => _parent == null;
    public T Value => _parent == null ? _value : _parent.Value;

    public static implicit operator T(DelegatedValue<T> delegatedValue) => delegatedValue.Value;
    public static implicit operator DelegatedValue<T>(T value) => new DelegatedValue<T>(value);
  }
}
