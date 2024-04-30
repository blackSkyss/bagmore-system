using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataSeeding
{
    public static class SizeDataSeeding
    {
        public static void SeedingSize(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Size>().HasData(
                new Size() { Id = 1, Name = "S", Status= 1},
                new Size() { Id = 2, Name = "M", Status= 1},
                new Size() { Id = 3, Name = "L", Status= 1},
                new Size() { Id = 4, Name = "XL", Status= 1},
                new Size() { Id = 5, Name = "XXL", Status= 1}
                );
        }
    }
}
