//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using ZenLeap.Api.Data;
//using ZenLeap.Api.Models;
//using ZenLeap.Api.Repositories;

//namespace ZenLeap.Api.Filters
//{
//	// https://msdn.microsoft.com/en-us/magazine/mt767699.aspx
//	public class ValidateUserExistsAttribute : TypeFilterAttribute
//    {
//        protected UnitOfWork _unitOfWork;
//        DataContext _context;

//        public ValidateUserExistsAttribute()
//            : base(typeof(ValidateUserExistsFilterImpl))
//        {
//            _context = new DataContext();
//            _unitOfWork = new UnitOfWork(_context);
//        }

//        private class ValidateUserExistsFilterImpl : IAsyncActionFilter
//        {
//            private readonly IGenericRepository<User> _userRepository;
//            public ValidateUserExistsFilterImpl(IGenericRepository<User> userRepository)
//            {
//                _userRepository = userRepository;
//            }
//            public async Task OnActionExecutionAsync(ActionExecutingContext context,
//              ActionExecutionDelegate next)
//            {
//                if (context.ActionArguments.ContainsKey("id"))
//                {
//                    var id = context.ActionArguments["id"];
//                    if (id != null)
//                    {
//                        var u = await _userRepository.GetByIdAsync(id);
//                        if (u.Id != id.ToString())
//                        {
//                            context.Result = new NotFoundObjectResult(id.ToString());
//                            return;
//                        }
//                    }
//                }
//                await next();
//            }
//        }
//    }
//}
