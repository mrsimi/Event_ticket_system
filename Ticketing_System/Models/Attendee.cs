using System.ComponentModel.DataAnnotations;

namespace Ticketing_System.Models
{
    public class Attendee
    {

        public int Id { get; set; }

        [StringLength(20, ErrorMessage ="The name must be between 4 and 20 character", MinimumLength =2)]
        [Required(ErrorMessage ="The First name is Required")]
        [Display(Name ="First Name")]
        public string  FirstName { get; set; }

        [StringLength(20, ErrorMessage = "The name must be between 4 and 20 character", MinimumLength = 2)]
        [Required(ErrorMessage = "The Last name is Required")]
        [Display(Name = "Last Name")]
        public string  LastName  { get; set; }

        [Required(ErrorMessage ="Please enter your email")]
        [Display(Name ="Email")]
        [DataType("Email")]
        public string  Email  { get; set; }
        public string SeatNumber { get; set; }
    }
}
