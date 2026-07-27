using CleanArchitecture.Domain.Entities;

public class Phrases1000Dto
{
    public int Id { get; set; }
    public required string Phrases { get; set; }
    public required string Meaning { get; set; }
    public int GroupId { get; set; }
    public required string Level { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Phrases1000, Phrases1000Dto>();
        }
    }
}
