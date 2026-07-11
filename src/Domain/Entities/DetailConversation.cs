namespace CleanArchitecture.Domain.Entities;

public class DetailConversation
{
    public int DTID { get; set; }
    public int CID { get; set; } = 0;
    public string SideA { get; set; } = string.Empty;
    public string SideAVN { get; set; } = string.Empty;
    public string SideB { get; set; } = string.Empty;
    public string SideBVN { get; set; } = string.Empty;
}
