using BAL.Services.Interfaces;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class DescribeProductService: IDescribeProductService
    {
        private UnitOfWork _unitOfWork;
        public DescribeProductService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }
    }
}
