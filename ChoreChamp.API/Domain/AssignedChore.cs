namespace ChoreChamp.API.Domain;

public class AssignedChore
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ChoreId { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsApproved { get; private set; }

    // Navigation properties
    public User User { get; private set; }
    public Chore Chore { get; private set; }

    // EF Core constructor
    private AssignedChore() { }

    public AssignedChore(int userId, int choreId, DateTime dueDate)
    {
        this.UserId = userId;
        this.ChoreId = choreId;
        this.DueDate = dueDate;
        this.IsCompleted = false;
        this.IsApproved = false;
    }

    public void MarkCompleted()
    {
        this.IsCompleted = true;
    }

    public void Approve()
    {
        if (!IsCompleted)
        {
            throw new InvalidOperationException("Chore must be completed before approving.");
        }

        this.IsApproved = true;
    }

    public void Reject()
    {
        if (!IsCompleted)
        {
            throw new InvalidOperationException("Chore must be completed before rejecting.");
        }

        this.IsCompleted = false;
    }
}
