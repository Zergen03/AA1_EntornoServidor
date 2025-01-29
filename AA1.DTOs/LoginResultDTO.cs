using AA1.Models;

namespace AA1.DTOs;

public record LoginResultDTO{
    required public Users User {get; set;}
    required public int RemainingLife {get; set;}
    required public int DamageTaken {get; set;}
}