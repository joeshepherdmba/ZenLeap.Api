using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Pages
{
    public class AboutModel : BasePageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            
        }
    }
}
