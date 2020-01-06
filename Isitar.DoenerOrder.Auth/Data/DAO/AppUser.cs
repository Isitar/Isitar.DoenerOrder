using Isitar.DoenerOrder.Core.Data.DAO;
using Microsoft.AspNetCore.Identity;

namespace Isitar.DoenerOrder.Auth.Data.DAO
{
    public class AppUser : IdentityUser<int>
    {
        public int UserId { get; set; }        
    }
}