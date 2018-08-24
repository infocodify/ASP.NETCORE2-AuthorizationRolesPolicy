using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAuth.Pages
{

    //Authorization Required to access Tutorials
    //[Authorize(Roles = "Admin")]

    [Authorize(Policy = "CanadianOrAdmin")]
    public class TutorialsModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}
