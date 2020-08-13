using System.ComponentModel.DataAnnotations;

namespace FinalExam.Models
{
    public class Group
    {
        [Key]
        public int GroupId {get; set;}
        public int UserId {get; set;}
        public int ActivityId {get; set;}
        public User Host {get; set;}
        public ActivityCenter Event  {get; set;}
    }
}