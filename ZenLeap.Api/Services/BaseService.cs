using System;
using Microsoft.EntityFrameworkCore;
using ZenLeap.Api.Data;

namespace ZenLeap.Api.Services
{
    /// <summary>
    /// Uses Unit of Work to call appropriate Repos.
    /// Uses data transfer opbjects and handles the transformation of data models to view models. 
    /// </summary>
    public class BaseService
    {
        protected UnitOfWork _unitOfWork;

        public BaseService(DataContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
    }
}
