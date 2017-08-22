using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZenLeap.Api.Data;

namespace ZenLeap.Api.Pages
{
    public abstract class BasePageModel: PageModel
    {
		protected UnitOfWork _unitOfWork;
		protected DataContext _context;

        public BasePageModel()
            :base()
        {
			_context = new DataContext();
			_unitOfWork = new UnitOfWork(_context);
        }
    }
}
