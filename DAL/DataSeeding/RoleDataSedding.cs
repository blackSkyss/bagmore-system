using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataSeeding
{
    public static class RoleDataSedding
    {
        public static void SeedingRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "Admin"},
                new Role() { Id = 2, Name = "User"}
                );
        }
    }
}
