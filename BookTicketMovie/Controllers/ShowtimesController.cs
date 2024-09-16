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
using BookTicketMovie.Services.Chairs;

namespace BookTicketMovie.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly BookTicketMovieContext _context;
        private readonly ICommonDataService<Showtime> _showTimeContext;
        private readonly ICommonDataService<Movie> _movieContext;
        private readonly ICommonDataService<Room> _roomContext;
        private readonly ICommonDataService<ShowTimeView> _showTimeVIewContext;
        
        private const int PAGE_SIZE = 5;

        public ShowtimesController(BookTicketMovieContext context, ICommonDataService<Showtime> showTimeContext,
            ICommonDataService<Movie> movieContext, ICommonDataService<Room> roomContext, ICommonDataService<ShowTimeView> showTimeVIewContext)
        {
            _context = context;
            _showTimeContext = showTimeContext;
            _movieContext = movieContext;
            _roomContext = roomContext;
            _showTimeVIewContext = showTimeVIewContext;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            var showtimes = await _showTimeContext.GetAllAsync();
                var showtimesPage = showtimes.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
            int number = showtimes.Count % PAGE_SIZE;
            ViewBag.TotalPage = 1;
            if (number == 0)
            {
                ViewBag.TotalPage = (int)(showtimes.Count / (double)PAGE_SIZE);
            }
            else
            {
                ViewBag.TotalPage = (int)(showtimes.Count / (double)PAGE_SIZE) + 1;
            }

            ViewBag.pageIndex = pageIndex;
            return View(showtimesPage);
        }

        // GET: Showtimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = await _showTimeContext.GetByIdAsync(id);
            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // GET: Showtimes/Create
        public async Task<IActionResult> Create()
        {
            var Movies = await _movieContext.GetAllAsync();
            var Rooms = await _roomContext.GetAllAsync();
            var showTimeView = new ShowTimeView
            {
                Movies = Movies,
                Rooms = Rooms,
            };
            return View(showTimeView);
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowTimeView showTimeView)
        {
            if (ModelState.IsValid)
            {
                await _showTimeVIewContext.CreateAsync(showTimeView);
                return RedirectToAction(nameof(Index));
            }
            return Ok(showTimeView);
        }

        // GET: Showtimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Movies = await _movieContext.GetAllAsync();
            var Rooms = await _roomContext.GetAllAsync();
            var showtime = await _context.Showtime.FindAsync(id);

            
            if (showtime == null)
            {
                return NotFound();
            }
            var showTimeView = new ShowTimeView
            {
                Movies = Movies,
                Rooms = Rooms,
                showTime = showtime
            };
            return View(showTimeView);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShowTimeView showTimeView)
        {
            if (id != showTimeView.showTime!.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _showTimeContext.EditAsync(showTimeView.showTime);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowtimeExists(showTimeView.showTime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(showTimeView);
        }

        // GET: Showtimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = await _context.Showtime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtime == null)
            {
                return NotFound();
            }
            ViewBag.AllowDelete = await _showTimeContext.isUsed(id);
            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _showTimeContext.DeleteByIdAsync(id);
            if (result)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

        private bool ShowtimeExists(int id)
        {
            return _context.Showtime.Any(e => e.Id == id);
        }
    }
}
