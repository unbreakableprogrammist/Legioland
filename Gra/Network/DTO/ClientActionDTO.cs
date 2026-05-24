namespace Gra.Network.DTO;

public class ClientActionDto
{
    public int PlayerId { get; set; }
    // Typ akcji (ruch, wybor itp.)
    public string ActionType { get; set; } 
        
    // Dodatkowe parametry (przydatne np. przy ruchu)
    public int Dx { get; set; }
    public int Dy { get; set; }
}