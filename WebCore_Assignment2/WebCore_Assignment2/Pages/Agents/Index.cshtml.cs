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
    public class IndexModel : PageModel
    {
        private readonly WebCore_Assignment2.Models.SeAssignment2DbContext _context;

        public IndexModel(WebCore_Assignment2.Models.SeAssignment2DbContext context)
        {
            _context = context;
        }

        public IList<Agent> Agent { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Agent = await _context.Agents.ToListAsync();
        }
    }
}
