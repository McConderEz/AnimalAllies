using System.Buffers;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Common;

public class ValueObjectList<T> : ValueObject, IReadOnlyList<T>
{
    public IReadOnlyList<T> Values { get; } = null!;
    public T this[int index] => Values[index];

    [JsonIgnore]
    public int Count => Values.Count;
    
    public string TypeName => typeof(T).Name;
    
    private ValueObjectList(){}

    public ValueObjectList(IEnumerable<T> values)
    {
        Values = new List<T>(values).AsReadOnly();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Values;
    }

    public IEnumerator<T> GetEnumerator()
        => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Values.GetEnumerator();
    
    public static implicit operator ValueObjectList<T>(List<T> list)
        => new(list);

    public static implicit operator List<T>(ValueObjectList<T> list)
        => list.Values.ToList();

    
}