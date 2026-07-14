using CleanArchitecture.Domain.Entities;
using AutoMapper;
namespace CleanArchitecture.Application.DetailConversations.Queries;

public class DetailConversationDto
{
    public int Id { get; set; }
    public int ConversationId { get; set; } = 0;
    public string SideA { get; set; } = string.Empty;
    public string SideAVN { get; set; } = string.Empty;
    public string SideB { get; set; } = string.Empty;
    public string SideBVN { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DetailConversation, DetailConversationDto>();
        }
    }
}