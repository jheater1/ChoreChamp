namespace ChoreChamp.API.Domain;

public record Points
{
    public int Value { get; init; }

    public Points(int value)
    {
        Value = value;
    }

    public Points Add(int amount) => new(Value + amount);

    public Points Subtract(int amount) => new(Value - amount);
}
