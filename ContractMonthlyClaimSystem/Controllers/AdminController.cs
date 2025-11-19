// Controllers/AdminController.cs
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<AdminController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new AdminDashboardViewModel
                {
                    TotalLecturersCount = await _context.Lecturers.CountAsync(),
                    TotalCoordinatorsCount = await _context.ProgrammeCoordinators.CountAsync(),
                    TotalManagersCount = await _context.AcademicManagers.CountAsync(),
                    TotalUsersCount = await _context.Users.CountAsync(),
                    ActiveLecturersCount = await _context.Lecturers.CountAsync(l => l.IsActive == 1),
                    InactiveLecturersCount = await _context.Lecturers.CountAsync(l => l.IsActive == 0),
                    RecentLecturers = await _context.Lecturers
                        .OrderByDescending(l => l.LecturerId)
                        .Take(5)
                        .ToListAsync(),
                    RecentUsers = await _context.Users
                        .OrderByDescending(u => u.CreatedDate)
                        .Take(5)
                        .ToListAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard");
                TempData["ErrorMessage"] = "Error loading dashboard data.";
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: /Admin/Lecturers
        public async Task<IActionResult> Lecturers()
        {
            var lecturers = await _context.Lecturers
                .OrderBy(l => l.LastName)
                .ThenBy(l => l.FirstName)
                .ToListAsync();
            return View(lecturers);
        }

        // GET: /Admin/CreateLecturer
        public IActionResult CreateLecturer()
        {
            return View();
        }

        // POST: /Admin/CreateLecturer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLecturer(CreateLecturerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Check if employee number already exists
                if (await _context.Lecturers.AnyAsync(l => l.EmployeeNumber == model.EmployeeNumber))
                {
                    ModelState.AddModelError("EmployeeNumber", "Employee number already exists.");
                    return View(model);
                }

                // Check if email already exists
                if (await _context.Lecturers.AnyAsync(l => l.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email address already exists.");
                    return View(model);
                }

                // Create new lecturer
                var lecturer = new Lecturer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmployeeNumber = model.EmployeeNumber,
                    PhoneNumber = model.PhoneNumber,
                    HourlyRate = model.HourlyRate,
                    ContractStartDate = model.ContractStartDate,
                    ContractEndDate = model.ContractEndDate,
                    IsActive = model.IsActive
                };

                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();

                // Create user account if requested
                if (model.CreateUserAccount && !string.IsNullOrEmpty(model.TemporaryPassword))
                {
                    var user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        User_Type = User_Type.Lecturer,
                        LecturerId = lecturer.LecturerId,
                        CreatedDate = DateTime.UtcNow,
                        //IsActive = 1
                    };

                    var result = await _userManager.CreateAsync(user, model.TemporaryPassword);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Lecturer");
                        _logger.LogInformation("User account created for lecturer {Email}", model.Email);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create user account for lecturer {Email}. Errors: {Errors}",
                            model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));

                        // Continue without user account - lecturer was created successfully
                        TempData["WarningMessage"] = "Lecturer created but user account creation failed. You can create the user account later.";
                    }
                }

                _logger.LogInformation("Lecturer {FirstName} {LastName} created successfully", model.FirstName, model.LastName);
                TempData["SuccessMessage"] = $"Lecturer {model.FirstName} {model.LastName} created successfully.";

                return RedirectToAction(nameof(Lecturers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lecturer");
                ModelState.AddModelError("", "An error occurred while creating the lecturer. Please try again.");
                return View(model);
            }
        }

        // GET: /Admin/EditLecturer/{id}
        public async Task<IActionResult> EditLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new CreateLecturerViewModel
            {
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                Email = lecturer.Email,
                EmployeeNumber = lecturer.EmployeeNumber,
                PhoneNumber = lecturer.PhoneNumber,
                HourlyRate = lecturer.HourlyRate,
                ContractStartDate = lecturer.ContractStartDate,
                ContractEndDate = lecturer.ContractEndDate,
                IsActive = lecturer.IsActive,
                CreateUserAccount = false // Don't show user creation options in edit
            };

            ViewData["LecturerId"] = id;
            return View(model);
        }

        // POST: /Admin/EditLecturer/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturer(int id, CreateLecturerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["LecturerId"] = id;
                return View(model);
            }

            try
            {
                var lecturer = await _context.Lecturers.FindAsync(id);
                if (lecturer == null)
                {
                    return NotFound();
                }

                // Check if employee number already exists (excluding current lecturer)
                if (await _context.Lecturers.AnyAsync(l => l.EmployeeNumber == model.EmployeeNumber && l.LecturerId != id))
                {
                    ModelState.AddModelError("EmployeeNumber", "Employee number already exists.");
                    ViewData["LecturerId"] = id;
                    return View(model);
                }

                // Check if email already exists (excluding current lecturer)
                if (await _context.Lecturers.AnyAsync(l => l.Email == model.Email && l.LecturerId != id))
                {
                    ModelState.AddModelError("Email", "Email address already exists.");
                    ViewData["LecturerId"] = id;
                    return View(model);
                }

                // Update lecturer
                lecturer.FirstName = model.FirstName;
                lecturer.LastName = model.LastName;
                lecturer.Email = model.Email;
                lecturer.EmployeeNumber = model.EmployeeNumber;
                lecturer.PhoneNumber = model.PhoneNumber;
                lecturer.HourlyRate = model.HourlyRate;
                lecturer.ContractStartDate = model.ContractStartDate;
                lecturer.ContractEndDate = model.ContractEndDate;
                lecturer.IsActive = model.IsActive;

                _context.Lecturers.Update(lecturer);
                await _context.SaveChangesAsync();

                // Update associated user if exists
                var user = await _context.Users.FirstOrDefaultAsync(u => u.LecturerId == id);
                if (user != null)
                {
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    await _userManager.UpdateAsync(user);
                }

                _logger.LogInformation("Lecturer {FirstName} {LastName} updated successfully", model.FirstName, model.LastName);
                TempData["SuccessMessage"] = $"Lecturer {model.FirstName} {model.LastName} updated successfully.";

                return RedirectToAction(nameof(Lecturers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lecturer with ID {LecturerId}", id);
                ModelState.AddModelError("", "An error occurred while updating the lecturer. Please try again.");
                ViewData["LecturerId"] = id;
                return View(model);
            }
        }

        // POST: /Admin/DeleteLecturer/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            try
            {
                var lecturer = await _context.Lecturers.FindAsync(id);
                if (lecturer == null)
                {
                    return NotFound();
                }

                // Check if lecturer has any claims
                var hasClaims = await _context.MonthlyClaims.AnyAsync(mc => mc.LecturerId == id);
                if (hasClaims)
                {
                    TempData["ErrorMessage"] = "Cannot delete lecturer who has existing claims. You can deactivate the lecturer instead.";
                    return RedirectToAction(nameof(Lecturers));
                }

                // Delete associated user if exists
                var user = await _context.Users.FirstOrDefaultAsync(u => u.LecturerId == id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                _context.Lecturers.Remove(lecturer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Lecturer {FirstName} {LastName} deleted successfully", lecturer.FirstName, lecturer.LastName);
                TempData["SuccessMessage"] = $"Lecturer {lecturer.FirstName} {lecturer.LastName} deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lecturer with ID {LecturerId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the lecturer. Please try again.";
            }

            return RedirectToAction(nameof(Lecturers));
        }

        // POST: /Admin/ToggleLecturerStatus/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLecturerStatus(int id)
        {
            try
            {
                var lecturer = await _context.Lecturers.FindAsync(id);
                if (lecturer == null)
                {
                    return NotFound();
                }

               if(lecturer.IsActive == 1)
                {
                    lecturer.IsActive = 0;
                }
               else
                {
                    lecturer.IsActive = 1;
                }
                _context.Lecturers.Update(lecturer);
                await _context.SaveChangesAsync();

                var status = lecturer.IsActive == 1 ? "activated" : "deactivated";
                _logger.LogInformation("Lecturer {FirstName} {LastName} {Status}", lecturer.FirstName, lecturer.LastName, status);
                TempData["SuccessMessage"] = $"Lecturer {lecturer.FirstName} {lecturer.LastName} {status} successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling lecturer status with ID {LecturerId}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the lecturer status. Please try again.";
            }

            return RedirectToAction(nameof(Lecturers));
        }
    }
}