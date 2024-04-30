using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface ISizeService
    {
        Task<List<SizeViewModel>> GetSizesAsync();
    }
}
