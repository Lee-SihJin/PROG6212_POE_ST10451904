using System.ComponentModel.DataAnnotations;
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class CreateUserViewModel
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
    [Display(Name = "Email/Username")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please select a user type")]
    [Display(Name = "User Type")]
    public User_Type UserType { get; set; }

    public List<SelectListItem>? UserTypes { get; set; }

    [Display(Name = "Phone Number")]
    [StringLength(15)]
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

    // Programme Coordinator specific field
    [StringLength(100)]
    public string? Department { get; set; }

    [Display(Name = "Active")]
    public int IsActive { get; set; } = 1; // Default to active

    [Required(ErrorMessage = "Temporary password is required")]
    [Display(Name = "Temporary Password")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string TemporaryPassword { get; set; }
}