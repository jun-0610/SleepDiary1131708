using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SleepDiary1131708.Models;

namespace SleepDiary1131708.Controllers
{
    public class SleepLogController : Controller
    {
        private readonly SleepDiaryDbContext _context;
        private const int PageSize = 5;

        public SleepLogController(SleepDiaryDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserID");
        }

        public IActionResult Index(int page = 1)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Redirect("/index.html");
            }

            if (page < 1)
            {
                page = 1;
            }

            var query = _context.SleepRecords
                .Where(s => s.UserID == userId.Value)
                .OrderByDescending(s => s.SleepDate);

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var logs = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var recentLogs = _context.SleepRecords
                .Where(s => s.UserID == userId.Value)
                .OrderByDescending(s => s.SleepDate)
                .Take(7)
                .ToList();

            var avgHours = recentLogs.Any() ? recentLogs.Average(s => s.SleepHours) : 0m;
            var avgQuality = recentLogs.Any() ? recentLogs.Average(s => s.QualityScore) : 0d;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.AvgSleepHours = Math.Round(avgHours, 1);
            ViewBag.AvgQualityScore = Math.Round(avgQuality, 1);
            ViewBag.Name = HttpContext.Session.GetString("Name");

            var chartData = recentLogs.OrderBy(s => s.SleepDate).ToList();
            ViewBag.ChartDates = chartData.Select(s => s.SleepDate.ToString("MM/dd")).ToArray();
            ViewBag.ChartHours = chartData.Select(s => s.SleepHours).ToArray();
            ViewBag.ChartQuality = chartData.Select(s => s.QualityScore).ToArray();

            return View(logs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DateTime sleepDate, decimal sleepHours, int qualityScore, string? reason, string? remark)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Redirect("/index.html");
            }

            if (sleepHours < 0 || sleepHours > 24 || qualityScore < 1 || qualityScore > 10)
            {
                TempData["ErrorMessage"] = "睡眠時數或品質分數超出範圍。";
                return RedirectToAction(nameof(Index));
            }

            var existingLog = _context.SleepRecords
                .FirstOrDefault(s => s.UserID == userId.Value && s.SleepDate == sleepDate.Date);

            if (existingLog != null)
            {
                TempData["ErrorMessage"] = "這一天已經有睡眠紀錄。";
                return RedirectToAction(nameof(Index));
            }

            var log = new SleepLog
            {
                UserID = userId.Value,
                SleepDate = sleepDate.Date,
                SleepHours = sleepHours,
                QualityScore = qualityScore,
                Reason = reason,
                Remark = remark
            };

            _context.SleepRecords.Add(log);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "新增成功。";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DateTime sleepDate, decimal sleepHours, int qualityScore, string? reason, string? remark)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Redirect("/index.html");
            }

            if (sleepHours < 0 || sleepHours > 24 || qualityScore < 1 || qualityScore > 10)
            {
                TempData["ErrorMessage"] = "睡眠時數或品質分數超出範圍。";
                return RedirectToAction(nameof(Index));
            }

            var log = _context.SleepRecords
                .FirstOrDefault(s => s.RecordID == id && s.UserID == userId.Value);

            if (log == null)
            {
                TempData["ErrorMessage"] = "找不到要修改的紀錄。";
                return RedirectToAction(nameof(Index));
            }

            log.SleepDate = sleepDate.Date;
            log.SleepHours = sleepHours;
            log.QualityScore = qualityScore;
            log.Reason = reason;
            log.Remark = remark;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "修改成功。";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Redirect("/index.html");
            }

            var log = _context.SleepRecords
                .FirstOrDefault(s => s.RecordID == id && s.UserID == userId.Value);

            if (log == null)
            {
                TempData["ErrorMessage"] = "找不到要刪除的紀錄。";
                return RedirectToAction(nameof(Index));
            }

            _context.SleepRecords.Remove(log);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "刪除成功。";
            return RedirectToAction(nameof(Index));
        }
    }
}
