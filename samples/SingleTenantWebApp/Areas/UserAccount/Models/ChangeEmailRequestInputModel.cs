using System.ComponentModel.DataAnnotations;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Models
{
    public class ChangeEmailRequestInputModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}