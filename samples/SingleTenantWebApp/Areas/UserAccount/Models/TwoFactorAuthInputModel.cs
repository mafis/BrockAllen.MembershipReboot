using System.ComponentModel.DataAnnotations;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Models
{
    public class TwoFactorAuthInputModel
    {
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}