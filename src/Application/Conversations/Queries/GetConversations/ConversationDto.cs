using CleanArchitecture.Domain.Entities;

public class ConversationDto
{
    public int Id { get; set; }
    public required string ConName { get; set; }
    public string? ConNameVN { get; set; }
    public string? URL { get; set; }
    public string? Character1 { get; set; }
    public string? Character2 { get; set; } = null;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Conversation, ConversationDto>();
        }
    }
}