using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages.Agents
{
    public class DetailsModel : PageModel
    {
        private readonly WebCore_Assignment2.Models.SeAssignment2DbContext _context;

        public DetailsModel(WebCore_Assignment2.Models.SeAssignment2DbContext context)
        {
            _context = context;
        }

        public Agent Agent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents.FirstOrDefaultAsync(m => m.AgentId == id);

            if (agent is not null)
            {
                Agent = agent;

                return Page();
            }

            return NotFound();
        }
    }
}
