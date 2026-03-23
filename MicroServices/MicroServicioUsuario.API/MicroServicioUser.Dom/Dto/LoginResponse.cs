using System.ComponentModel.DataAnnotations;

namespace MicroServicioUser.Dom.Dto;

public class LoginResponse
{
    [Required]
    public bool Ok { get; set; }
    [Required]
    public string ? Error { get; set; }
    [Required]
    public string ? Token { get; set; }
    [Required]
    public string ? TokenType { get; set; }
}

