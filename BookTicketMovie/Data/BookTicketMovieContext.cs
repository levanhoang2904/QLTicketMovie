using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookTicketMovie.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookTicketMovie.Data
{
    public class BookTicketMovieContext : IdentityDbContext<UserDb>
    {
        public BookTicketMovieContext (DbContextOptions<BookTicketMovieContext> options)
            : base(options)
        {
        }

        public DbSet<BookTicketMovie.Models.Movie> Movie { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UserDb>().Property(u => u.Initials).HasMaxLength(5);
            modelBuilder.HasDefaultSchema("identity");
            modelBuilder.Entity<MovieGenre>()
                .HasKey(bg => new { bg.MovieId, bg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(bg => bg.Movie)
                .WithMany(b => b.MovieGenres)
                .HasForeignKey(bg => bg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Showtimes)
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Showtimes)
                .HasForeignKey(s => s.RoomId);

            modelBuilder.Entity<Ticket>()
                .HasOne(s => s.Showtime)
                .WithMany(r => r.Tickets)
                .HasForeignKey(s => s.ShowtimeId);
            modelBuilder.Entity<Ticket>()
                .HasOne(s => s.Chair)
                .WithMany(r => r.Tickets)
                .HasForeignKey(s => s.ChairId);
            modelBuilder.Entity<Ticket>()
               .HasOne(s => s.Movie)
               .WithMany(r => r.Tickets)
               .HasForeignKey(s => s.MovieId);
        }
        public DbSet<BookTicketMovie.Models.Genre> Genre { get; set; } = default!;
        public DbSet<BookTicketMovie.Models.MovieGenre> MovieGenre { get; set;} = default!;
        public DbSet<BookTicketMovie.Models.Room> Room { get; set; } = default!;
        public DbSet<BookTicketMovie.Models.Chair> Chair { get; set; } = default!;
        public DbSet<BookTicketMovie.Models.Showtime> Showtime { get; set; } = default!;
        public DbSet<BookTicketMovie.Models.Ticket> Ticket { get; set; } = default!;
        public DbSet<BookTicketMovie.Models.User> User { get; set; } = default!;
    }
}
