using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IDashBoardService
    {
        public Task<DashBoardInformationViewModel> GetDashBoardInformation();
        public Task<List<OrderSoldViewModel>> GetNumberOfOrdersSoldAsync(int? year);
        public Task<List<NumberOfProfitViewModel>> GetNumberOfProfitAsync(int? year);
    }
}
