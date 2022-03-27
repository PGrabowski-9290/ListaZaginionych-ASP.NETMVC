using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ListaZaginionych.Data.Models;

namespace ListaZaginionych.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<LostPeopleModel> LostPeople { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
