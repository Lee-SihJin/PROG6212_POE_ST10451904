// Models/ViewModels/AdminReportsViewModel.cs
namespace ContractMonthlyClaimSystem.Models.ViewModels
{
    public class AdminReportsViewModel
    {
        public int TotalClaimsCount { get; set; }
        public int TotalApprovedClaimsCount { get; set; }
        public int TotalRejectedClaimsCount { get; set; }
        public int TotalPendingClaimsCount { get; set; }
        public decimal? TotalAmountApproved { get; set; }

        public List<MonthlyStat> MonthlyStats { get; set; } = new List<MonthlyStat>();
        public List<LecturerStat> LecturerStats { get; set; } = new List<LecturerStat>();
        public List<CoordinatorStat> CoordinatorStats { get; set; } = new List<CoordinatorStat>();
    }

    public class MonthlyStat
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
        public int SubmittedCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class LecturerStat
    {
        public int LecturerId { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public int TotalClaims { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AverageAmount { get; set; }
    }

    public class CoordinatorStat
    {
        public int CoordinatorId { get; set; }
        public string CoordinatorName { get; set; } = string.Empty;
        public int TotalProcessed { get; set; }
        public double AverageProcessingDays { get; set; }
    }
}