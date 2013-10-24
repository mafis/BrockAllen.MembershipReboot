using System.ComponentModel.DataAnnotations;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Models
{
    public class PasswordResetInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}