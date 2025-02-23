namespace Business.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ProjectNumber { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CustomerId { get; set; }
    public int StatusId { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
}
