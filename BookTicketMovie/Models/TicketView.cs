namespace BookTicketMovie.Models
{
    public class TicketView
    {
        public int Id { get; set; }

        public Ticket? Ticket { get; set; }

        public List<Chair>? Chairs { get; set; }

       
    }
}
