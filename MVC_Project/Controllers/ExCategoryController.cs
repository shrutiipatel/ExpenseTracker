using MVC_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.Controllers
{
    public class ExCategoryController : Controller
    {
        DatabaseConnection Db_Conn = new DatabaseConnection();
        // GET: ExCategory
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(ExCategory e)
        {
            decimal cat_sum = Db_Conn.Model_Category.ToList().Sum(h => h.Ex_Limit);
            totalLimit tl = Db_Conn.Model_totalLimit.FirstOrDefault(h => h.Id == 1);

            cat_sum = cat_sum + e.Ex_Limit;

            if (cat_sum > tl.Total_Limit)
            {
                TempData["AlertMsg"] = "total limit first edited...";
                return RedirectToAction("ListLimit");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ExCategory e1 = new ExCategory();
                    // e1.Name = "Harsh";
                    //e1.Ex_Limit = 2;
                    e1.Name = e.Name;
                    e1.Ex_Limit = e.Ex_Limit;

                    Db_Conn.Model_Category.Add(e1);
                    TempData["AlertMsg"] = "New Category Inserted...";
                    Db_Conn.SaveChanges();
                    return RedirectToAction("ListCategory");
                }
                ModelState.Clear();
            }
            return View("Create");
        }

        public ActionResult ListCategory()
        {

            decimal cat_sum = Db_Conn.Model_Category.ToList().Sum(h => h.Ex_Limit);
            totalLimit tl = Db_Conn.Model_totalLimit.FirstOrDefault(h => h.Id == 1);

            decimal remain = tl.Total_Limit - cat_sum;
            TempData["Remain"] = remain;

            var list = Db_Conn.Model_Category.ToList();
            Db_Conn.SaveChanges();

            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var del = Db_Conn.Model_Category.Where(h => h.Id == id).First();
            Db_Conn.Model_Category.Remove(del);
            Db_Conn.SaveChanges();
            TempData["AlertMsg"] = "Record Deleted...";

            return RedirectToAction("ListCategory");

            //var list = Db_Conn.Model_Category.ToList();
            //Db_Conn.SaveChanges();

            return View("Create");
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var row = Db_Conn.Model_Category.Where(h => h.Id == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult EditCategory(ExCategory e)
        {
            ExCategory e1 = new ExCategory();
            if (ModelState.IsValid)
            {
                e1.Id = e.Id;
                e1.Name = e.Name;
                e1.Ex_Limit = e.Ex_Limit;

                Db_Conn.Entry(e1).State = EntityState.Modified;
                Db_Conn.SaveChanges();
                TempData["AlertMsg"] = "Record Updated Successful...";

                return RedirectToAction("ListCategory");
            }
            ModelState.Clear();
            return View("Create");
            // var row = Db_Conn.Model_Category.Where(h => h.Id == id).FirstOrDefault();
        }


        //--------Expense Action--------------

        public ActionResult CreateExp()
        {
            var delist = Db_Conn.Model_Category.ToList();
            ViewBag.Id = new SelectList(delist, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult AddExpense(Expense ex)
        {
            Expense e_obj = new Expense();

            decimal total_exp = Db_Conn.Model_Expense.Where(h => h.Id == ex.Id).ToList().Sum(h => h.Amount);
            ExCategory exCat = Db_Conn.Model_Category.FirstOrDefault(h => h.Id == ex.Id);

            total_exp = total_exp + ex.Amount;

            if (total_exp > exCat.Ex_Limit)
            {
                TempData["AlertMsg"] = "Edit Expense Limit...";
                return RedirectToAction("ListCategory");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var delist = Db_Conn.Model_Category.ToList();
                    ViewBag.Id = new SelectList(delist, "Id", "Name");

                    e_obj.Title = ex.Title;
                    e_obj.Description = ex.Description;
                    e_obj.Amount = ex.Amount;
                    e_obj.Id = ex.Id;
                    e_obj.Date = DateTime.Now;

                    Db_Conn.Model_Expense.Add(e_obj);
                    TempData["AlertMsg"] = "New Category Inserted...";
                    Db_Conn.SaveChanges();
                    return RedirectToAction("ListExpense");
                }
                ModelState.Clear();
            }

            return View("CreateExp");
        }

        public ActionResult ListExpense()
        {
            //var delist = Db_Conn.Model_Category.ToList();
            //ViewBag.Id = new SelectList(delist, "Id", "Name");

            //var list = Db_Conn.Model_Expense.ToList();
            //Db_Conn.SaveChanges();

            List<Expense> exp = Db_Conn.Model_Expense.ToList();
            List<ExCategory> excat = Db_Conn.Model_Category.ToList();

            var list = from e in exp
                       join d in excat on e.Id equals d.Id into Table1
                       from d in Table1.ToList()
                       select new ViewModel
                       {
                           expense = e,
                           exCategory = d
                       };

            return View(list);
        }





        public ActionResult CatNameSearch(int id)
        {
            //var delist = Db_Conn.Model_Category.ToList();
            //ViewBag.Id = new SelectList(delist, "Id", "Name");

            //var list = Db_Conn.Model_Expense.Where(h=>h.Id == id).ToList();
            //Db_Conn.SaveChanges();

            decimal cat_sum = Db_Conn.Model_Expense.Where(h => h.Id == id).ToList().Sum(h=>h.Amount);
            ExCategory explimit = Db_Conn.Model_Category.FirstOrDefault(h=>h.Id == id);

            decimal remian = explimit.Ex_Limit - cat_sum;
            TempData["Remain"] = remian;

            List<Expense> exp = Db_Conn.Model_Expense.ToList();
            List<ExCategory> excat = Db_Conn.Model_Category.ToList();



            var list = from e in exp
                       join d in excat on e.Id equals d.Id into Table1
                       from d in Table1.Where(h => h.Id == id).ToList()
                       select new ViewModel
                       {
                           expense = e,
                           exCategory = d
                       };

            //ViewBag.totalrow = Db_Conn.Model_Category.Count();


            return View(list);
        }



        public ActionResult DeleteEx(int exp_id)
        {
            var del = Db_Conn.Model_Expense.Where(h => h.Exp_Id == exp_id).First();
            Db_Conn.Model_Expense.Remove(del);
            Db_Conn.SaveChanges();
            TempData["AlertMsg"] = "Record Deleted...";
            return RedirectToAction("ListExpense");

            return View("CreateExp");
        }

        [HttpGet]
        public ActionResult EditExpense(int exp_id)
        {
            var delist = Db_Conn.Model_Category.ToList();
            ViewBag.Id = new SelectList(delist, "Id", "Name");


            var row = Db_Conn.Model_Expense.Where(h => h.Exp_Id == exp_id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult EditExpense(Expense ex)
        {
            var delist = Db_Conn.Model_Category.ToList();
            ViewBag.Id = new SelectList(delist, "Id", "Name");


            Expense e1 = new Expense();
            if (ModelState.IsValid)
            {
                e1.Exp_Id = ex.Exp_Id;
                e1.Title = ex.Title;
                e1.Description = ex.Description;
                e1.Amount = ex.Amount;
                e1.Id = ex.Id;
                e1.Date = DateTime.Now;

                Db_Conn.Entry(e1).State = EntityState.Modified;
                Db_Conn.SaveChanges();
                TempData["AlertMsg"] = "Record Updated Successful...";

                return RedirectToAction("ListExpense");
            }
            ModelState.Clear();

            return View();
        }

        public ActionResult CreateLimit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddLimit(totalLimit tl)
        {
            if (ModelState.IsValid)
            {
                totalLimit t = new totalLimit();

                t.Total_Limit = tl.Total_Limit;

                Db_Conn.Model_totalLimit.Add(t);
                TempData["AlertMsg"] = "Total Expense Limit Created...";
                Db_Conn.SaveChanges();
                return RedirectToAction("ListLimit");
            }

            return View("CreateLimit");
        }

        public ActionResult ListLimit()
        {
            var list = Db_Conn.Model_totalLimit.Where(h => h.Id == 1).ToList();
            Db_Conn.SaveChanges();

            return View(list);
        }

        [HttpGet]
        public ActionResult EditLimit(int id)
        {
            var row = Db_Conn.Model_totalLimit.Where(h => h.Id == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult EditLimit(totalLimit tl)
        {
            totalLimit t1 = new totalLimit();
            if (ModelState.IsValid)
            {
                t1.Id = tl.Id;
                t1.Total_Limit = tl.Total_Limit;


                Db_Conn.Entry(t1).State = EntityState.Modified;
                Db_Conn.SaveChanges();
                TempData["AlertMsg"] = "Limit Updated Successful...";

                return RedirectToAction("ListLimit");
            }
            ModelState.Clear();
            return View("CreateLimit");

        }

        public ActionResult Dashboard()
        {
            ViewBag.Exp = Db_Conn.Model_Expense.Sum(h => h.Amount);

            var ExeCat = GetCategories();
            var Exp = GetExpenses();
            var totLimit = GettotalLimits();

            MultiViewModel list = new MultiViewModel();
            list.Expenses = Exp;
            list.exCategories = ExeCat;
            list.totalLimits = totLimit;

            //var list = new MultiViewModel();

            // list.Expenses = Db_Conn.Model_Expense.ToList();
            // list.exCategories = Db_Conn.Model_Category.ToList();
            // list.totalLimits = Db_Conn.Model_totalLimit.ToList();
            return View(list);
            //ViewBag.ExeCate = ExeCat;
            //ViewBag.Expe = Exp;
            //ViewBag.totLimit = totLimit;

            //            
        }

        public List<ExCategory> GetCategories()
        {
            return Db_Conn.Model_Category.ToList();
        }
        public List<Expense> GetExpenses()
        {
            return Db_Conn.Model_Expense.ToList();
        }
        public List<totalLimit> GettotalLimits()
        {
            return Db_Conn.Model_totalLimit.ToList();
        }



    }
}