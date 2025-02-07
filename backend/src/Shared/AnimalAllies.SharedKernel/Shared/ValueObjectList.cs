using System.Collections;
using System.Text.Json.Serialization;
using AnimalAllies.SharedKernel.Shared.Objects;

namespace AnimalAllies.SharedKernel.Shared;

public class ValueObjectList<T> : ValueObject, IReadOnlyList<T>
{
    public IReadOnlyList<T> Values { get; } = null!;
    public T this[int index] => Values[index];

    [JsonIgnore]
    public int Count => Values.Count;
    
    public string TypeName => typeof(T).Name;
    
    private ValueObjectList(){}
    
    [JsonConstructor]

    public ValueObjectList(IEnumerable<T> values)
    {
        Values = new List<T>(values);
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