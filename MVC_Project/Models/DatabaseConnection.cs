using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Project.Models
{
    public class DatabaseConnection :DbContext
    {
        public DatabaseConnection() :base("Conn") 
        {
        } 

        public DbSet<ExCategory> Model_Category { get; set; }

        public DbSet<Expense> Model_Expense { get; set; }

        public DbSet<totalLimit> Model_totalLimit { get; set; }

    }

    public class ViewModel
    {
        public ExCategory exCategory { get; set; }
        public Expense expense { get; set; }
        public totalLimit totallimit { get; set; }
    }

}