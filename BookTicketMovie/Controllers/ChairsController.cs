using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTicketMovie.Data;
using BookTicketMovie.Models;
using BookTicketMovie.Services;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.Results;

namespace BookTicketMovie.Controllers
{
    [Authorize(Roles = "Admin")]
	public class ChairsController : Controller
    {
        private readonly BookTicketMovieContext _context;
        private readonly ICommonDataService<Chair> _chairService;
        public ChairsController(BookTicketMovieContext context, ICommonDataService<Chair> chairService)
        {
            _context = context;
            _chairService = chairService;
        }

        // GET: Chairs
        public async Task<IActionResult> Index()
        {
            return View(await _chairService.GetAllAsync());
        }

        // GET: Chairs/Details/5
        public async Task<IActionResult> Details(char? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chair = await _context.Chair
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chair == null)
            {
                return NotFound();
            }

            return View(chair);
        }

        // GET: Chairs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chair chair)
        {
            
            ChairValidator validator = new ChairValidator();

            ValidationResult result = validator.Validate(chair);

            if (result.IsValid)
            {
                await _chairService.CreateAsync(chair);
                    return RedirectToAction(nameof(Index));
                }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(chair);
        }

        // GET: Chairs/Edit/5
        public async Task<IActionResult> Edit(char? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chair = await _context.Chair.FindAsync(id);
            if (chair == null)
            {
                return NotFound();
            }
            return View(chair);
        }

        // POST: Chairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(char id, byte[] rowVersion)
        {
           
            var chairtoUpdate = _context.Chair.FirstOrDefault(c => c.Id == id);
            if (chairtoUpdate == null)
            {
                Chair  deletedDepartment = new Chair();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                return View(deletedDepartment);
            }

            _context.Entry(chairtoUpdate).Property("RowVersion").OriginalValue = rowVersion;
            if (await TryUpdateModelAsync<Chair>(
        chairtoUpdate,
        "",
        s => s.NameChair, s => s.Price))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Chair)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {

                        var databaseValues = (Chair)databaseEntry.ToObject();
                        if (databaseValues.Price != clientValues.Price)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Price:c}");
                        }
                        ModelState.AddModelError(string.Empty, "Bản ghi bạn cố chỉnh sửa "
                            + "đã được người dùng khác sửa đổi sau khi bạn nhận được giá trị ban đầu. "
                            + "Thao tác chỉnh sửa đã bị hủy và các giá trị hiện tại trong cơ sở dữ liệu đã được hiển thị. "
                            + "Nếu bạn vẫn muốn chỉnh sửa bản ghi này, hãy nhấp lại vào nút Lưu. Nếu không hãy nhấp vào siêu liên kết Quay lại danh sách. "
                            );
                        chairtoUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            return View();
        }

        // GET: Chairs/Delete/5
        public async Task<IActionResult> Delete(char? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chair = await _chairService.GetByIdAsync(id);
            if (chair == null)
            {
                return NotFound();
            }

            return View(chair);
        }

        // POST: Chairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(char id)
        {
            var result = await _chairService.DeleteByIdAsync(id);
            if (result)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

        private bool ChairExists(char id)
        {
            return _context.Chair.Any(e => e.Id == id);
        }
    }
}
