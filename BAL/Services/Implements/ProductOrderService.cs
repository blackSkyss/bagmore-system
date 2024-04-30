using BAL.Services.Interfaces;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class ProductOrderService: IProductOrderService
    {
        private UnitOfWork _unitOfWork;
        public ProductOrderService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }
    }
}
