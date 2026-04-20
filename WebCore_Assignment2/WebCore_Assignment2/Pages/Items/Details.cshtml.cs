using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages.Items
{
    public class DetailsModel : PageModel
    {
        private readonly WebCore_Assignment2.Models.SeAssignment2DbContext _context;

        public DetailsModel(WebCore_Assignment2.Models.SeAssignment2DbContext context)
        {
            _context = context;
        }

        public Item Item { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FirstOrDefaultAsync(m => m.ItemId == id);

            if (item is not null)
            {
                Item = item;

                return Page();
            }

            return NotFound();
        }
    }
}
