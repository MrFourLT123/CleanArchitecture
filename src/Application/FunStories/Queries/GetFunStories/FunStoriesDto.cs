using CleanArchitecture.Domain.Entities;

public class FunStoriesDTO
{
    public int Id { get; set; }
    public required string TitleEN { get; set; }
    public required string TitleVN { get; set; }
    public required string ContentEN { get; set; }
    public required string ContentVN { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<FunStory, FunStoriesDTO>();
        }
    }
}
