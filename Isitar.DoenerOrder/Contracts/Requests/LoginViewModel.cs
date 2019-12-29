using System.ComponentModel.DataAnnotations;

namespace Isitar.DoenerOrder.Contracts.Requests
{
    public class LoginViewModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}