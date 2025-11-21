using System.ComponentModel.DataAnnotations;

namespace ContractMonthlyClaimSystem.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }  // Changed from UserId to Id

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public User_Type UserType { get; set; }

        // Lecturer specific fields
        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Hourly Rate")]
        [Range(0, double.MaxValue)]
        public decimal? HourlyRate { get; set; }

        [Display(Name = "Contract Start Date")]
        [DataType(DataType.Date)]
        public DateTime? ContractStartDate { get; set; }

        [Display(Name = "Contract End Date")]
        [DataType(DataType.Date)]
        public DateTime? ContractEndDate { get; set; }

        // Coordinator specific field
        [Display(Name = "Department")]
        [StringLength(100)]
        public string? Department { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int IsActive { get; set; }

        [Display(Name = "New Password")]
        [StringLength(100, MinimumLength = 6)]
        public string? NewPassword { get; set; }
    }
}