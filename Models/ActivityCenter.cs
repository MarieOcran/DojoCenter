using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExam.Models
{
    public class ActivityCenter
    {
        [Key]
        public int ActivityId {get ; set;}
        
        [Required]
        [MinLength(2,ErrorMessage="Must be at least 2 characters")]
        public string Title {get ; set;}

        [Required]
        [DataType(DataType.Date)]
        [FutureDateAttribute]
        public DateTime Date {get ; set;}
        
        [Required]
        [DataType(DataType.Time)]
       
        public DateTime Time  {get ; set;}
        
        [Required]
        [Range(1,60)]
        public int Duration {get ; set;}
        
        [Required]
        [MinLength(10,ErrorMessage="Must be at least 10 characters")]
        public string Description {get ; set;}

        public int UserId {get; set;}
        public User Coordinator {get; set;}
        public List<Group> Participants {get ;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}