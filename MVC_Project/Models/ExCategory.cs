using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Project.Models
{
    [Table("tbl_Category")]
    public class ExCategory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Expense Limit Must be Entered")]
        public long Ex_Limit { get; set; }
    }
}