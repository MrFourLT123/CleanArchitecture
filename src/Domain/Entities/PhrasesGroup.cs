namespace CleanArchitecture.Domain.Entities;

public class PhrasesGroup
{
    public int Id { get; set; }
    public required string GroupName { get; set; }
    public required string GroupNameVN { get; set; }
    public required string ImageName { get; set; }
}
