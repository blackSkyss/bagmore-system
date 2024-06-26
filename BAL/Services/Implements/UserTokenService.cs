﻿using BAL.Services.Interfaces;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class UserTokenService : IUserTokenService
    {
        private UnitOfWork _unitOfWork;
        public UserTokenService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }
    }
}
