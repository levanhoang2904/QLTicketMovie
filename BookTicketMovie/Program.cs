using BookTicketMovie.Data;
using BookTicketMovie.Models;
using BookTicketMovie.Services;
using BookTicketMovie.Services.Chairs;
using BookTicketMovie.Services.Genres;
using BookTicketMovie.Services.MovieGenres;
using BookTicketMovie.Services.Movies;
using BookTicketMovie.Services.Rooms;
using BookTicketMovie.Services.ShowTimes;
using BookTicketMovie.Services.Tickets;
using BookTicketMovie.Services.Users;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // This ensures the app listens on port 80
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BookTicketMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookTicketMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));
builder.Services.AddScoped<ICommonDataService<Movie>, MovieService>();
builder.Services.AddScoped<ICommonDataService<Genre>, GenreService>();
builder.Services.AddScoped<ICommonDataService<MovieEditView>, MovieGenreService>();
builder.Services.AddScoped<ICommonDataService<Room>, RoomService>();
builder.Services.AddScoped<ICommonDataService<Chair>, ChairService>();
builder.Services.AddScoped<ICommonDataService<Showtime>, ShowTimeService>();
builder.Services.AddScoped<ICommonDataService<ShowTimeView>, ShowTimeViewService>();
builder.Services.AddScoped<ICommonDataService<Ticket>, TicketService>();
builder.Services.AddScoped<ICommonUserService<User>, UserService>();
builder.Services.AddScoped<ICommonDataService<User>, UserService>();

builder.Services.AddIdentity<UserDb, IdentityRole>(
    options =>
    {
        
    }
    ).AddEntityFrameworkStores<BookTicketMovieContext>().AddDefaultTokenProviders();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var DB_HOST = Environment.GetEnvironmentVariable("DB_HOST");
var DB_NAME = Environment.GetEnvironmentVariable("DB_NAME");
var DB_SA_PASSWORD = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Users/Login"; 
                options.AccessDeniedPath = "/Users/AccessDenied";
            });
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DbInitializer.Seed(services);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
