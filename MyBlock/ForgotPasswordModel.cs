using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlock
{
    public class ForgotPasswordModel
    {
        [Required]
        public string EmailAddress { get; set; }


        [Required]
        [Compare(nameof(EmailAddress), ErrorMessage = "Emails mismatch")]
        public string VerifiedEmailAddress { get; set; }
    }
}
