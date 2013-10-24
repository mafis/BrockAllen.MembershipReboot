using System.ComponentModel.DataAnnotations;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Models
{
    public class ChangeMobileFromCodeInputModel
    {
        [Required]
        public string Code { get; set; }
    }
    
}