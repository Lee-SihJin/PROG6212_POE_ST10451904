// Controllers/AdminController.cs
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: /Admin/CreateUser
        public IActionResult CreateUser()
        {
            var model = new CreateUserViewModel
            {
                UserTypes = Enum.GetValues(typeof(User_Type))
                               .Cast<User_Type>()
                               .Where(ut => ut != User_Type.Administrator) // Exclude Administrator from creation
                               .Select(ut => new SelectListItem
                               {
                                   Value = ((int)ut).ToString(),
                                   Text = ut.ToString()
                               }).ToList()
            };
            return View(model);
        }

        // POST: /Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate UserTypes for the dropdown
                model.UserTypes = Enum.GetValues(typeof(User_Type))
                                   .Cast<User_Type>()
                                   .Where(ut => ut != User_Type.Administrator)
                                   .Select(ut => new SelectListItem
                                   {
                                       Value = ((int)ut).ToString(),
                                       Text = ut.ToString()
                                   }).ToList();
                return View(model);
            }

            try
            {
                // Check if email already exists across all user types
                if ((await _context.Users.CountAsync(u => u.Email == model.Email)) > 0)
                {
                    ModelState.AddModelError("Email", "Email address already exists.");
                    model.UserTypes = Enum.GetValues(typeof(User_Type))
                                       .Cast<User_Type>()
                                       .Where(ut => ut != User_Type.Administrator)
                                       .Select(ut => new SelectListItem
                                       {
                                           Value = ((int)ut).ToString(),
                                           Text = ut.ToString()
                                       }).ToList();
                    return View(model);
                }

                object createdEntity = null;
                int? entityId = null;

                // Create the specific user type entity
                switch (model.UserType)
                {
                    case User_Type.Lecturer:
                        var lecturer = new Lecturer
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            HourlyRate = model.HourlyRate,
                            ContractStartDate = model.ContractStartDate ?? DateTime.UtcNow,
                            ContractEndDate = model.ContractEndDate,
                            IsActive = model.IsActive
                        };

                        _context.Lecturers.Add(lecturer);
                        await _context.SaveChangesAsync();
                        createdEntity = lecturer;
                        entityId = lecturer.LecturerId;
                        break;

                    case User_Type.ProgrammeCoordinator:
                        var coordinator = new ProgrammeCoordinator
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Department = model.Department,
                            IsActive = model.IsActive
                        };

                        _context.ProgrammeCoordinators.Add(coordinator);
                        await _context.SaveChangesAsync();
                        createdEntity = coordinator;
                        entityId = coordinator.CoordinatorId;
                        break;

                    case User_Type.AcademicManager:
                        var manager = new AcademicManager
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsActive = model.IsActive
                        };

                        _context.AcademicManagers.Add(manager);
                        await _context.SaveChangesAsync();
                        createdEntity = manager;
                        entityId = manager.ManagerId;
                        break;

                    default:
                        ModelState.AddModelError("UserType", "Invalid user type selected.");
                        model.UserTypes = Enum.GetValues(typeof(User_Type))
                                           .Cast<User_Type>()
                                           .Where(ut => ut != User_Type.Administrator)
                                           .Select(ut => new SelectListItem
                                           {
                                               Value = ((int)ut).ToString(),
                                               Text = ut.ToString()
                                           }).ToList();
                        return View(model);
                }

                // Create user account if requested
                if (!string.IsNullOrEmpty(model.TemporaryPassword))
                {
                    var user = new User
                    {
                        UserName = model.Email?.Trim(),
                        NormalizedUserName = model.Email?.Trim().ToUpperInvariant(),
                        Email = model.Email?.Trim(),
                        NormalizedEmail = model.Email?.Trim().ToUpperInvariant(),
                        FirstName = model.FirstName?.Trim(),
                        LastName = model.LastName?.Trim(),
                        User_Type = model.UserType,
                        CreatedDate = DateTime.UtcNow,

                        // === CORRECT IDENTITY PROPERTIES ===
                        EmailConfirmed = true,           // Good - skip email verification
                        PhoneNumberConfirmed = false,    // Change to false (not true)
                        TwoFactorEnabled = false,        // Change to false (not true) 
                        LockoutEnabled = true,           // Good
                        AccessFailedCount = 0,           // Good
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        PhoneNumber = null
                    };

                    // Set foreign key
                    switch (model.UserType)
                    {
                        case User_Type.Lecturer:
                            user.LecturerId = entityId;
                            break;
                        case User_Type.ProgrammeCoordinator:
                            user.CoordinatorId = entityId;
                            break;
                        case User_Type.AcademicManager:
                            user.ManagerId = entityId;
                            break;
                    }

                    var result = await _userManager.CreateAsync(user, model.TemporaryPassword);

                    if (result.Succeeded)
                    {
                        // Add to appropriate role based on user type
                        string roleName = model.UserType.ToString();
                        await _userManager.AddToRoleAsync(user, roleName);
                        _logger.LogInformation("User account created for {UserType} {Email}", model.UserType, model.Email);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create user account for {UserType} {Email}. Errors: {Errors}",
                            model.UserType, model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));

                        // Continue without user account - entity was created successfully
                        TempData["WarningMessage"] = $"{model.UserType} created but user account creation failed. You can create the user account later.";
                    }
                }

                _logger.LogInformation("{UserType} {FirstName} {LastName} created successfully", model.UserType, model.FirstName, model.LastName);
                TempData["SuccessMessage"] = $"{model.UserType} {model.FirstName} {model.LastName} created successfully.";

                return RedirectToAction("CreateUser"); // You'll need to create this action to list all users
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating {UserType}", model.UserType);
                ModelState.AddModelError("", $"An error occurred while creating the {model.UserType}. Please try again.");

                // Repopulate UserTypes for the dropdown
                model.UserTypes = Enum.GetValues(typeof(User_Type))
                                   .Cast<User_Type>()
                                   .Where(ut => ut != User_Type.Administrator)
                                   .Select(ut => new SelectListItem
                                   {
                                       Value = ((int)ut).ToString(),
                                       Text = ut.ToString()
                                   }).ToList();
                return View(model);
            }
        }

            // GET: /Admin/Reports
public async Task<IActionResult> Reports()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var model = new AdminReportsViewModel
                {
                    TotalClaimsCount = await _context.MonthlyClaims.CountAsync(),
                    TotalApprovedClaimsCount = await _context.MonthlyClaims.CountAsync(mc => mc.Status == ClaimStatus.ManagerApproved),
                    TotalRejectedClaimsCount = await _context.MonthlyClaims.CountAsync(mc => mc.Status == ClaimStatus.Rejected),
                    TotalPendingClaimsCount = await _context.MonthlyClaims.CountAsync(mc => mc.Status == ClaimStatus.CoordinatorApproved),
                    TotalAmountApproved = await _context.MonthlyClaims
                        .Where(mc => mc.Status == ClaimStatus.ManagerApproved)
                        .SumAsync(mc => mc.TotalAmount),

                    // Monthly statistics for current year
                    MonthlyStats = await _context.MonthlyClaims
                        .Where(mc => mc.SubmissionDate.Year == currentYear)
                        .GroupBy(mc => new { mc.SubmissionDate.Year, mc.SubmissionDate.Month })
                        .Select(g => new MonthlyStat
                        {
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            SubmittedCount = g.Count(),
                            ApprovedCount = g.Count(mc => mc.Status == ClaimStatus.ManagerApproved),
                            RejectedCount = g.Count(mc => mc.Status == ClaimStatus.Rejected),
                            TotalAmount = g.Where(mc => mc.Status == ClaimStatus.ManagerApproved).Sum(mc => mc.TotalAmount)
                        })
                        .OrderBy(ms => ms.Year)
                        .ThenBy(ms => ms.Month)
                        .ToListAsync(),

                    // Lecturer performance stats
                    LecturerStats = await _context.MonthlyClaims
                        .Include(mc => mc.Lecturer)
                        .Where(mc => mc.Status == ClaimStatus.ManagerApproved)
                        .GroupBy(mc => new { mc.LecturerId, mc.Lecturer.FirstName, mc.Lecturer.LastName })
                        .Select(g => new LecturerStat
                        {
                            LecturerId = g.Key.LecturerId,
                            LecturerName = g.Key.FirstName + " " + g.Key.LastName,
                            TotalClaims = g.Count(),
                            TotalAmount = g.Sum(mc => mc.TotalAmount),
                            AverageAmount = g.Average(mc => mc.TotalAmount)
                        })
                        .OrderByDescending(ls => ls.TotalAmount)
                        .Take(10)
                        .ToListAsync(),

                    // Coordinator approval stats
                    CoordinatorStats = await _context.MonthlyClaims
                        .Include(mc => mc.Coordinator)
                        .Where(mc => mc.CoordinatorId != null)
                        .GroupBy(mc => new { mc.CoordinatorId, mc.Coordinator.FirstName, mc.Coordinator.LastName })
                        .Select(g => new CoordinatorStat
                        {
                            CoordinatorId = g.Key.CoordinatorId.Value,
                            CoordinatorName = g.Key.FirstName + " " + g.Key.LastName,
                            TotalProcessed = g.Count(),
                            AverageProcessingDays = g.Average(mc =>
                                (mc.CoordinatorApprovalDate.Value - mc.SubmissionDate).TotalDays)
                        })
                        .OrderByDescending(cs => cs.TotalProcessed)
                        .ToListAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin reports");
                TempData["ErrorMessage"] = "Error loading reports data.";
                return View(new AdminReportsViewModel());
            }
        }

        // GET: /Admin/AllClaims
        public async Task<IActionResult> AllClaims(string status, string month, string lecturer, string coordinator)
        {
            try
            {
                var query = _context.MonthlyClaims
                    .Include(mc => mc.Lecturer)
                    .Include(mc => mc.Coordinator)
                    .Include(mc => mc.Manager)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<ClaimStatus>(status, out var statusEnum))
                    {
                        query = query.Where(mc => mc.Status == statusEnum);
                    }
                }

                if (!string.IsNullOrEmpty(month))
                {
                    var filterDate = DateTime.Parse(month + "-01");
                    query = query.Where(mc => mc.ClaimMonth.Year == filterDate.Year &&
                                             mc.ClaimMonth.Month == filterDate.Month);
                }

                if (!string.IsNullOrEmpty(lecturer))
                {
                    query = query.Where(mc => mc.Lecturer.FirstName.Contains(lecturer) ||
                                             mc.Lecturer.LastName.Contains(lecturer));
                }

                if (!string.IsNullOrEmpty(coordinator))
                {
                    query = query.Where(mc => mc.Coordinator.FirstName.Contains(coordinator) ||
                                             mc.Coordinator.LastName.Contains(coordinator));
                }

                var claims = await query
                    .OrderByDescending(mc => mc.SubmissionDate)
                    .ToListAsync();

                // Pass filter values to view
                ViewBag.CurrentStatus = status;
                ViewBag.CurrentMonth = month;
                ViewBag.CurrentLecturer = lecturer;
                ViewBag.CurrentCoordinator = coordinator;

                return View(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all claims with filters");
                TempData["ErrorMessage"] = "Error loading claims data.";
                return View(new List<MonthlyClaim>());
            }
        }

        // GET: /Admin/ClaimDetails/{id}
        public async Task<IActionResult> ClaimDetails(int id)
        {
            try
            {
                var claim = await _context.MonthlyClaims
                    .Include(mc => mc.Lecturer)
                    .Include(mc => mc.Coordinator)
                    .Include(mc => mc.Manager)
                    .Include(mc => mc.SupportingDocuments)
                    .Include(mc => mc.ClaimItems)
                    .FirstOrDefaultAsync(mc => mc.ClaimId == id);

                if (claim == null)
                {
                    TempData["ErrorMessage"] = "Claim not found.";
                    return RedirectToAction(nameof(AllClaims));
                }

                return View(claim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading claim details for claim {ClaimId}", id);
                TempData["ErrorMessage"] = "Error loading claim details.";
                return RedirectToAction(nameof(AllClaims));
            }
        }

        // POST: /Admin/ExportClaimsReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportClaimsReport(string reportType, string format, string status, string month)
        {
            try
            {
                var query = _context.MonthlyClaims
                    .Include(mc => mc.Lecturer)
                    .Include(mc => mc.Coordinator)
                    .Include(mc => mc.Manager)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(status) && Enum.TryParse<ClaimStatus>(status, out var statusEnum))
                {
                    query = query.Where(mc => mc.Status == statusEnum);
                }

                if (!string.IsNullOrEmpty(month))
                {
                    var filterDate = DateTime.Parse(month + "-01");
                    query = query.Where(mc => mc.ClaimMonth.Year == filterDate.Year &&
                                             mc.ClaimMonth.Month == filterDate.Month);
                }

                var claims = await query
                    .OrderByDescending(mc => mc.SubmissionDate)
                    .ToListAsync();

                if (format == "csv")
                {
                    return GenerateCsvReport(claims, reportType);
                }
                else if (format == "excel")
                {
                    return GenerateExcelReport(claims, reportType);
                }
                else
                {
                    TempData["ErrorMessage"] = "Unsupported export format.";
                    return RedirectToAction(nameof(Reports));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting claims report");
                TempData["ErrorMessage"] = "Error generating export file.";
                return RedirectToAction(nameof(Reports));
            }
        }

        private IActionResult GenerateCsvReport(List<MonthlyClaim> claims, string reportType)
        {
            var csv = new System.Text.StringBuilder();

            // Add headers based on report type
            if (reportType == "detailed")
            {
                csv.AppendLine("ClaimID,Lecturer,EmployeeNumber,ClaimMonth,Status,TotalHours,TotalAmount,SubmissionDate,Coordinator,CoordinatorApprovalDate,Manager,ManagerApprovalDate");

                foreach (var claim in claims)
                {
                    csv.AppendLine($"{claim.ClaimId},\"{claim.Lecturer?.FullName}\",\"{claim.Lecturer.LecturerId}\",\"{claim.DisplayMonth}\",{claim.Status},{claim.TotalHours},{claim.TotalAmount},\"{claim.SubmissionDate:yyyy-MM-dd}\",\"{claim.Coordinator?.FullName}\",\"{claim.CoordinatorApprovalDate?.ToString("yyyy-MM-dd")}\",\"{claim.Manager?.FullName}\",\"{claim.ManagerApprovalDate?.ToString("yyyy-MM-dd")}\"");
                }
            }
            else // summary
            {
                csv.AppendLine("Month,SubmittedCount,ApprovedCount,RejectedCount,PendingCount,TotalAmount");

                var summary = claims
                    .GroupBy(c => new { c.ClaimMonth.Year, c.ClaimMonth.Month })
                    .Select(g => new
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
                        SubmittedCount = g.Count(),
                        ApprovedCount = g.Count(c => c.Status == ClaimStatus.ManagerApproved),
                        RejectedCount = g.Count(c => c.Status == ClaimStatus.Rejected),
                        PendingCount = g.Count(c => c.Status == ClaimStatus.CoordinatorApproved),
                        TotalAmount = g.Where(c => c.Status == ClaimStatus.ManagerApproved).Sum(c => c.TotalAmount)
                    })
                    .OrderBy(s => s.Month);

                foreach (var item in summary)
                {
                    csv.AppendLine($"{item.Month},{item.SubmittedCount},{item.ApprovedCount},{item.RejectedCount},{item.PendingCount},{item.TotalAmount}");
                }
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"{reportType}-report-{DateTime.Now:yyyyMMdd}.csv");
        }

        private IActionResult GenerateExcelReport(List<MonthlyClaim> claims, string reportType)
        {
            // For simplicity, returning CSV as Excel
            // In a real application, you would use a library like EPPlus or ClosedXML
            return GenerateCsvReport(claims, reportType);
        }
    
    }
}