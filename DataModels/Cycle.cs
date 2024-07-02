using System.Collections;
using SharpAstrology.Enums;

namespace SharpAstrology.Vedic.DataModels;

internal sealed class Cycle<T> : IEnumerable<T>
{
    private readonly List<T> _values;
    private readonly int _offset;

    public int Length => _values.Count;
    
    public Cycle(IEnumerable<T> values, int startIndex = 0)
    {
        _values = values.ToList();
        _offset = startIndex;
    }
    
    public Cycle(IEnumerable<T> values, T startItem)
    {
        _values = values.ToList();
        _offset = _values.IndexOf(startItem);
    }

    public T this[int index] => _values[(index + _offset) % _values.Count];
    
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < _values.Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}