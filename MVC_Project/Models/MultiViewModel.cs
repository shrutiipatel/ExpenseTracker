using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.Models
{
    public class MultiViewModel
    {
        public List<ExCategory> exCategories= new List<ExCategory>();

        public List<Expense> Expenses= new List<Expense>();

        public List<totalLimit> totalLimits = new List<totalLimit>();
    }
}