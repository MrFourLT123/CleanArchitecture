using CleanArchitecture.Domain.Entities;

namespace Application.VocabCards.Queries.GetVocabCards;

public class VocabCardDto
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public string? Definition { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? Ipa { get; set; }
    public string? Synonyms { get; set; }
    public string? Level { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            _ = CreateMap<VocabCard, VocabCardDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        }
    }

}