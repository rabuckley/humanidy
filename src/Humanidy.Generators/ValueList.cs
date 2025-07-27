using System.Collections;

namespace Humanidy.Generators;

/// <summary>
/// A list with sequence equality.
/// </summary>
public sealed class ValueList<T> : IList<T>, IEquatable<ValueList<T>>
    where T : IEquatable<T>
{
    private readonly IList<T> _inner;

    public ValueList()
    {
        _inner = new List<T>();
    }

    public ValueList(IList<T> list)
    {
        _inner = list ?? throw new ArgumentNullException(nameof(list));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _inner.Add(item);
    }

    public void Clear()
    {
        _inner.Clear();
    }

    public bool Contains(T item)
    {
        return _inner.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _inner.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _inner.Remove(item);
    }

    public int Count => _inner.Count;

    public bool IsReadOnly => _inner.IsReadOnly;

    public int IndexOf(T item)
    {
        return _inner.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _inner.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _inner.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _inner[index];
        set => _inner[index] = value;
    }

    public bool Equals(ValueList<T>? other)
    {
        return other is not null && _inner.SequenceEqual(other._inner);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueList<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _inner.Aggregate(0, (current, item) => current ^ item.GetHashCode());
    }
}
