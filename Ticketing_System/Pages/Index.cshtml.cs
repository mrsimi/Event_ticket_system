using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Ticketing_System.Data;
using Ticketing_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ticketing_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Attendee Attendee { get; set;  }
      public async Task<IActionResult> OnPostAsync()
        {            
            if (!ModelState.IsValid)
            {
                return Page();
            }

                var count = await _context.Attendees.CountAsync();
                if (count >= 50)
                {
                        ViewData["Message"] = "Sorry! The provision of seats made for the events is filled. Thank you!";
                        return Page();
                }
                if(_context.Attendees.Any(m => m.Email == Attendee.Email))
                {
                    ViewData["Message"] = "Sorry! But this user has been awarded a ticket before. Thank you";
                    return Page();
                }
            else
            {
                Attendee.SeatNumber = String.Concat("0", count.ToString());

                _context.Attendees.Add(Attendee);
                await _context.SaveChangesAsync();

                EmailService emailService = new EmailService(Attendee.FirstName, Attendee.LastName, Attendee.SeatNumber, Attendee.Email);
                emailService.CreatPdf();
                emailService.SendEmaill();

              ViewData["Message"] = "The ticket has been sent to your e-mail. Thank you!";

                return Page();
            }
        }

    }

}