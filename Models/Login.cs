using System;
using System.ComponentModel.DataAnnotations;


namespace FinalExam.Models
{
    public class Login
    {
        [DataType(DataType.EmailAddress)]  
        [Required(ErrorMessage = "Email required")]  
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")] 
        public string EmailLog {get;set;}

        [DataType ("Password")]       
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string PasswordLog {get;set;}
    }
}