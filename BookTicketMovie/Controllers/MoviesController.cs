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
using BookTicketMovie.Services.Genres;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using BookTicketMovie.Models;
using Microsoft.AspNetCore.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using BookTicketMovie.Migrations;

namespace BookTicketMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly BookTicketMovieContext _context;
        private readonly ICommonDataService<Movie> _movieService;
        private readonly ICommonDataService<Genre> _genreService;
        private readonly ICommonDataService<MovieEditView> _movieEditViewService;
        private readonly IWebHostEnvironment _hostEnvironment;

        private const int PAGE_SIZE = 5;
        public MoviesController(BookTicketMovieContext context, ICommonDataService<Movie> movieService,
            ICommonDataService<Genre> genreService, ICommonDataService<MovieEditView> movieEditViewService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _movieService = movieService;
            _genreService = genreService;
            _movieEditViewService = movieEditViewService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchValue = "", int movieGenre = 0, int pageIndex = 1)
        {

            var movies = await _movieService.GetAllAsync();
            HttpContext.Session.SetString("searchValue", searchValue);
            HttpContext.Session.SetInt32("searchGenre", movieGenre);
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                movies = movies.Where(s => s.Title!.ToUpper().Contains(searchValue.ToUpper())).ToList();
            }


            if (movieGenre > 0)
            {
                movies = movies.Where(x => x.MovieGenres.Any(mg => mg.GenreId == movieGenre)).ToList();
            }
            var moviesPage = movies.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
            int number = movies.Count % PAGE_SIZE;
            ViewBag.TotalPage = 1;
            if (number == 0)
            {
                ViewBag.TotalPage = (int)(movies.Count / (double)PAGE_SIZE);
            }
            else
            {
                ViewBag.TotalPage = (int)(movies.Count / (double)PAGE_SIZE) + 1;
            }


            ViewBag.searchValue = HttpContext.Session.GetString("searchValue");

            ViewBag.searchGenre = (int)HttpContext.Session.GetInt32("searchGenre");

            var movieView = new MovieViewModel
            {
                Movies = moviesPage,
                Genres = new SelectList(await _genreService.GetAllAsync(), "Id", "name"),
            };
            ViewBag.pageIndex = pageIndex;
            return View(movieView);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var genres = await _genreService.GetAllAsync();
            var movieEditView = new MovieEditView
            {
                Genres = genres,
            };

            return View(movieEditView);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieEditView movieEditView, IFormFile? uploadPhoto)
        {
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(_hostEnvironment.WebRootPath, "images\\movies");
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                movieEditView.Movie.Photo = fileName;
            }
           if (ModelState.IsValid)
            {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var movie = await _movieService.CreateAsync(movieEditView.Movie);
                        //throw new Exception();
                        await _movieEditViewService.CreateAsync(movieEditView);
                        transaction.Commit();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Đã có lỗi xảy ra! Vui lòng thử lại.");
                    }
                }
            }
           movieEditView.Genres =  await _genreService.GetAllAsync();
            return View(movieEditView);
             
            
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetByIdAsync(id);
            var genres = await _genreService.GetAllAsync();

            if (movie == null)
            {
                return NotFound();
            }

            var MovieEditView = new MovieEditView
            {
                Movie = movie,
                Genres = genres,
                idGenres = movie.MovieGenres.Select(mg => mg.GenreId).ToList()
            };
            return View(MovieEditView);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieEditView movieEditView, IFormFile? uploadPhoto)
        {
            if (id != movieEditView.Movie.Id)
            {
                return NotFound();
            }
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(_hostEnvironment.WebRootPath, "images\\movies");
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                movieEditView.Movie.Photo = fileName;
            }


            if (ModelState.IsValid) {
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await _movieService.EditAsync(movieEditView.Movie);
                        //throw new Exception();
                        await _movieEditViewService.EditAsync(movieEditView);
                        transaction.Commit();
                        if (result == null)
                        {
                            return NotFound();
                        }
                        return RedirectToAction(nameof(Index));

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Đã có lỗi xảy ra! Vui lòng thử lại.");
                    }

                }

            }
            var movie = await _movieService.GetByIdAsync(id);
            movieEditView.Genres = await _genreService.GetAllAsync();
            movieEditView.idGenres = movie.MovieGenres.Select(mg => mg.GenreId).ToList();
            return View(movieEditView);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewBag.AllowDelete = await _movieService.isUsed(id);
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _movieEditViewService.DeleteByIdAsync(id))
                if (await _movieService.DeleteByIdAsync(id))
                    return RedirectToAction(nameof(Index));
            return NotFound();
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
