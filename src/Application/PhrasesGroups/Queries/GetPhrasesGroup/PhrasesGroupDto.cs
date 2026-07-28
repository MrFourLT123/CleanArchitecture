using CleanArchitecture.Domain.Entities;

public class PhrasesGroupDto
{
    public int Id { get; set; }
    public required string GroupName { get; set; }
    public required string GroupNameVN { get; set; }
    public required string ImageName { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PhrasesGroup, PhrasesGroupDto>();
        }
    }
}
