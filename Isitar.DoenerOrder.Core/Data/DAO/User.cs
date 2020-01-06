using Microsoft.AspNetCore.Identity;

namespace Isitar.DoenerOrder.Core.DAO
{
public class User : IdentityUser<int>
{
    [PersonalData]
    public string Firstname { get; set; }

    [PersonalData]
    public string Lastname { get; set; }
}
}