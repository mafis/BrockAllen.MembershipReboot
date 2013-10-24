using System.ComponentModel.DataAnnotations;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Models
{
    public class ChangeUsernameInputModel
    {
        [Required]
        public string NewUsername { get; set; }
    }
}