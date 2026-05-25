namespace Gra.Network.DTO;

public class ClientActionDto
{
    public int PlayerId { get; set; }
    public string ActionType { get; set; } 
        
    public int Dx { get; set; }
    public int Dy { get; set; }
}
