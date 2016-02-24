
using System.ComponentModel.DataAnnotations;

namespace NhutLongCompany.Models
{
    public class SendEmail
    {
        [Display(Name = "Số điện thoại")]
        [Required ]
        public string sodienthoai { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Họ và tên")]
        [Required]
        public string hovaten { get; set; }
     
        public string content { get; set; }
      
    }
}