using Microsoft.EntityFrameworkCore;
using Rtech_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rtech_App.Data
{
    public class DataContext : DbContext
    {

        //omda
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }


    public DbSet<User> Users { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    //}

}

}
    

