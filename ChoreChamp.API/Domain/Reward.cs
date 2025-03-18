namespace ChoreChamp.API.Domain;

public class Reward
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int PointCost { get; private set; }
    public int? Limit { get; private set; }
    public bool IsAvailable { get; private set; }
    public object Points { get; set; }

    // EF Core constructor
    private Reward() { }

    public Reward(string name, string? description, int pointCost, int? limit = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.");

        if (pointCost <= 0)
            throw new ArgumentException("Point cost cannot be less than or equal to 0.");

        if (limit is not null && limit < 0)
            throw new ArgumentException("Limit cannot be negative.");

        this.Name = name;
        this.Description = description;
        this.PointCost = pointCost;
        this.Limit = limit;
        this.IsAvailable = true;
    }

    public void UpdateAvailability()
    {
        this.IsAvailable = !this.IsAvailable;
    }

    public void UpdateLimit(int limit)
    {
        if (limit < 0)
            throw new ArgumentException("Limit cannot be negative.");

        this.Limit = limit;
    }
}
