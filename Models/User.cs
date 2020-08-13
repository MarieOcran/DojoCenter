using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="Must be at least 2 characters")]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2,ErrorMessage="Must be at least 2 characters")]
        public string LastName {get;set;}
    
        // [Required(ErrorMessage="Email is required")]
        // [DataType(DataType.EmailAddress)]
        // [EmailAddress]
        [DataType(DataType.EmailAddress)]  
        [Required(ErrorMessage = "Email required")]  
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")] 
        public string Email {get;set;}
       
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [DataType ("Password")]
        [StrongPassword]
        public string Password {get;set;}

        
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType ("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<ActivityCenter> MyActivities {get ;set;}
        public List<Group> MyGroups {get ;set;}
    }
}