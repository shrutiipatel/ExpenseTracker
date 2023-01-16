using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MVC_Project.Models
{
    [Table("tbl_Expense")]
    public class Expense
    {
        [Key]
        public int Exp_Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Amount is Required")]
        public long Amount { get; set; }

        [Display(Name = "tbl_Category")]
        [Required(ErrorMessage = "Select Any Category,If Null Then first Add Category")]
        public virtual int Id { get; set; }

        [ForeignKey("Id")]
        public virtual ExCategory ExCat { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public DateTime Date { get; set; }
    }
}