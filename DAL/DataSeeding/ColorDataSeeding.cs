using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataSeeding
{
    public static class ColorDataSeeding
    {
        public static void SeedingColor(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>().HasData(
                new Color() { Id=1, Name = "White", ColorCode = "#ffffff", Status = 1},
                new Color() { Id =2, Name = "Black", ColorCode = "#000000", Status = 1},
                new Color() { Id = 3, Name = "Red", ColorCode = "#FF0000", Status = 1 },
                new Color() { Id =4, Name = "Pink", ColorCode = "#FFC0CB" , Status = 1 },
                new Color() { Id = 5, Name = "Brown", ColorCode = "#A52A2A", Status = 1 }
                );
        }
    }
}
