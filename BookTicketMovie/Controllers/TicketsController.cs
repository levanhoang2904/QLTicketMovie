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
using System.Diagnostics;

namespace BookTicketMovie.Controllers
{
    public class TicketsController : Controller
    {
        private readonly BookTicketMovieContext _context;
        private readonly ICommonDataService<Movie> _movieContext;
        private readonly ICommonDataService<Showtime> _showTimeService;
        private readonly ICommonDataService<Chair> _chairService;
        private readonly ICommonDataService<Ticket> _ticketService;
        private const int PAGE_SIZE = 5;

        public TicketsController(BookTicketMovieContext context, ICommonDataService<Movie> movieContext,
            ICommonDataService<Showtime> showTimeService, ICommonDataService<Chair> chairService, ICommonDataService<Ticket> ticketService)
        {
            _context = context;
            _movieContext = movieContext;
            _showTimeService = showTimeService;
            _chairService = chairService;
            _ticketService = ticketService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string searchValue = "", int pageIndex = 1)
        {
            var tickets = await _ticketService.GetAllAsync();
            HttpContext.Session.SetString("searchValueTicket", searchValue);
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                tickets = tickets.Where(s => s.Movie!.Title.ToUpper().Contains(searchValue.ToUpper())).ToList();
            }

            var ticketsPage = tickets.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
            int number = tickets.Count % PAGE_SIZE;
            ViewBag.TotalPage = 1;
            if (number == 0)
            {
                ViewBag.TotalPage = (int)(tickets.Count / (double)PAGE_SIZE);
            }
            else
            {
                ViewBag.TotalPage = (int)(tickets.Count / (double)PAGE_SIZE) + 1;
            }

            ViewBag.pageIndex = pageIndex;
            ViewBag.searchValue = HttpContext.Session.GetString("searchValue");
            return View(ticketsPage);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id, int? MovieId)
        {
            if (id == null || MovieId == null)
            {
                return NotFound();
            }

            var movie = await _movieContext.GetByIdAsync(MovieId);
            var dateTime = await _showTimeService.GetByIdAsync(id);
            if (movie == null || dateTime == null)
            {
                return NotFound();
            }
            var ticketView = new Ticket
            {
                Movie = movie,
                Showtime = dateTime,
                
            };

            return View(ticketView);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create(int? id, int? MovieId)
        {
            if (id == null || MovieId == null)
            {
                return NotFound();
            }
            ViewBag.Chairs = await _chairService.GetAllAsync();
            var movie = await _movieContext.GetByIdAsync(MovieId);
            var dateTime = await _showTimeService.GetByIdAsync(id);
            if (movie == null || dateTime == null)
            {
                return NotFound();
            }
            var ticket = new Ticket
            {
                Movie = movie,
                Showtime = dateTime,
            };
            List<int> NumberChairs = new List<int> { 1, 2, 3, 4, 5, 6 };
            ViewData["ChairId"] = new SelectList(_context.Chair, "Id", "NameChair");
           
            ViewData["NumberChairs"] = new SelectList(NumberChairs);
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowtimeId,ChairId,MovieId,SeatNumber,PurchaseDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var chair = await _chairService.GetByIdAsync(ticket.ChairId);
                if (chair == null) return NotFound();
                ticket.Price = chair.Price;
                ticket.PurchaseDate = DateTime.Now;
                _context.Add(ticket);
                var movie = await _movieContext.GetByIdAsync(ticket.MovieId);
                movie.Revenue += ticket.Price;
                await _movieContext.EditAsync(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ChairId"] = new SelectList(_context.Chair, "Id", "NameChair", ticket.ChairId);
            //ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", ticket.MovieId);
            //ViewData["ShowtimeId"] = new SelectList(_context.Showtime, "Id", "Id", ticket.ShowtimeId);
            return Ok(ticket);
        }

        // GET: Tickets/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ticket = await _context.Ticket.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ChairId"] = new SelectList(_context.Chair, "Id", "NameChair", ticket.ChairId);
        //    ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", ticket.MovieId);
        //    ViewData["ShowtimeId"] = new SelectList(_context.Showtime, "Id", "Id", ticket.ShowtimeId);
        //    return View(ticket);
        //}

        //// POST: Tickets/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ShowtimeId,ChairId,MovieId,SeatNumber,PurchaseDate")] Ticket ticket)
        //{
        //    if (id != ticket.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(ticket);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TicketExists(ticket.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ChairId"] = new SelectList(_context.Chair, "Id", "NameChair", ticket.ChairId);
        //    ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", ticket.MovieId);
        //    ViewData["ShowtimeId"] = new SelectList(_context.Showtime, "Id", "Id", ticket.ShowtimeId);
        //    return View(ticket);
        //}

        //// GET: Tickets/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ticket = await _context.Ticket
        //        .Include(t => t.Chair)
        //        .Include(t => t.Movie)
        //        .Include(t => t.Showtime)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (ticket == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ticket);
        //}

        //// POST: Tickets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var ticket = await _context.Ticket.FindAsync(id);
        //    if (ticket != null)
        //    {
        //        _context.Ticket.Remove(ticket);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
