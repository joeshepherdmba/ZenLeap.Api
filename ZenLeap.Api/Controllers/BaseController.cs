using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenLeap.Api.Data;
using ZenLeap.Api.Filters;
using ZenLeap.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZenLeap.Api.Controllers
{
	//[Authorize]
	[ValidateModel]
    public class BaseController : Controller
    {
        protected UnitOfWork _unitOfWork;
        protected DataContext _context;
        protected IAuthorizationService _authorizationService; // TODO: may be able to move this to UOW
        protected readonly UserManager<User> _userManager;

		protected BaseController(DataContext context,
			IAuthorizationService authorizationService,
			UserManager<User> userManager)
            : base()
        {
            _context = new DataContext();
            _unitOfWork = new UnitOfWork(_context);
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
    }
}
