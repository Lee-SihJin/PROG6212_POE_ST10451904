// Models/ViewModels/AdminDashboardViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace ContractMonthlyClaimSystem.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalLecturersCount { get; set; }
        public int TotalCoordinatorsCount { get; set; }
        public int TotalManagersCount { get; set; }
        public int TotalUsersCount { get; set; }
        public int ActiveLecturersCount { get; set; }
        public int InactiveLecturersCount { get; set; }

        public List<Lecturer> RecentLecturers { get; set; } = new List<Lecturer>();
        public List<User> RecentUsers { get; set; } = new List<User>();
    }

    public class CreateLecturerViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Required]
        [Display(Name = "Contract Start Date")]
        [DataType(DataType.Date)]
        public DateTime ContractStartDate { get; set; } = DateTime.Today;

        [Display(Name = "Contract End Date")]
        [DataType(DataType.Date)]
        public DateTime? ContractEndDate { get; set; }

        [Display(Name = "Active")]
        public int IsActive { get; set; } 

        // User account creation
        [Display(Name = "Create User Account")]
        public bool CreateUserAccount { get; set; } = true;

        [DataType(DataType.Password)]
        [Display(Name = "Temporary Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? TemporaryPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("TemporaryPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}