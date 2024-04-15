using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOS
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        //[Required]
        //[Phone]
        //public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$"
            ,ErrorMessage ="Password Must Be Required"),]
        public string Password { get; set; }
    }
}
