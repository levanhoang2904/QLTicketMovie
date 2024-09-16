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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookTicketMovie.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookTicketMovieContext _context;
        private readonly ICommonUserService<User> _userService;
        private readonly ICommonDataService<User> _userCrudService;
        private readonly SignInManager<UserDb> _signInManger;
       private readonly UserManager<UserDb> _userManager;

        public AccountController(
            BookTicketMovieContext context,
            ICommonUserService<User> userService,
            ICommonDataService<User> userCrudService,
            SignInManager<UserDb> signInManger,
            UserManager<UserDb> userManager
 )
        {
            _context = context;
            _userService = userService;
            _userCrudService = userCrudService;
            _signInManger = signInManger;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            if (User.Identity != null)
            {
                if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public async Task<IActionResult> LoginUser(User user)
        {
           
            if (ModelState.IsValid)
            {
                //var userResult = await _userService.Login(user);
                //if (userResult != null)
                //{
                //    HttpContext.Session.SetString("Username", userResult.Name!);
                //    var claims = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.NameIdentifier,  userResult.Id!.ToString()),
                //        new Claim(ClaimTypes.Name,  userResult.Name!),
                //        new Claim(ClaimTypes.Email,  userResult.Email!),
                //        new Claim(ClaimTypes.Role,  userResult.Role!)
                //    };
                //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //    var principal = new ClaimsPrincipal(claimsIdentity);
                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //    return RedirectToAction("Index", "Home");
                //}    
                //ModelState.AddModelError("userWrong", "Tài khoản hoặc mật khẩu không chính xác");
                //return View(user);
                var rs = await _signInManger.PasswordSignInAsync(user.Email!, user.Password!, false, false);
                if (rs.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("userWrong", "Tài khoản hoặc mật khẩu không chính xác");
                return View(user);
            }
            return View(user);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User Input, string rePassword)
        {
            if (Input.Name == "" || Input.Name == null)
            {
                ModelState.AddModelError("Name", "Tên không được bỏ trống");
            }
            if (Input.Email == null)
            {
                ModelState.AddModelError("Email", "Email không được bỏ trống");
            }
            if (Input.Password == null)
            {
                ModelState.AddModelError("Password", "Password không được bỏ trống");
            }

            if (Input.Email != null)
            {
                var checkEmail = await _userService.MailIsExists(Input.Email);
                if (checkEmail)
                {
                    ModelState.AddModelError("CheckEmail", "Email đã tồn tại");
                }
            }

            if (Input.Password != rePassword)
            {
                ModelState.AddModelError("rePassWrong", "Mật khẩu nhập lại không chính xác");
            }

            if (!ModelState.IsValid)
            {
                return View(Input);
            }





            //if (ModelState.IsValid)
            //{
            //    Input.Role = "";
            //    await _userCrudService.CreateAsync(Input);
            //    return View(nameof(Login));
            //}
            if (ModelState.IsValid)
            {
                UserDb user = new UserDb
                {
                    
                    UserName = Input.Email,
                    Email = Input.Email,
                };
                var rs = await _userManager.CreateAsync(user, Input.Password!);
                if (rs.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                    if (roleResult.Succeeded)
                    {
                        return View(nameof(Login));
                    }
                }
                foreach (var error in rs.Errors)
                {
                    ModelState.AddModelError("formatPass", error.Description);
                }
            }

           



            return View(Input);
        }
        [Authorize(Roles = "Admin")]

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _userCrudService.EditAsync(user) != null)
                    return RedirectToAction(nameof(Index));
                return NotFound();
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userCrudService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            if (id == Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return RedirectToAction(nameof(Login));
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManger.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

	}
}
