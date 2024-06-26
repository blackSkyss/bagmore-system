﻿using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IShippingAddressService
    {
        Task<List<ShippingAddressViewModel>> GetShippingAddressAsync(string name);
        Task AddShippingAddressesAsync(string userName, ShippingAddressViewModel shippingAddressViewModel);
    }
}
