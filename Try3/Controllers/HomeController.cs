using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Try3.Models;

namespace Try3.Controllers
{
    public class HomeController : Controller
    {
        string conString = @"Server=DESKTOP-6R3R8GU\SQLEXPRESS;Database=Automobile_Sales;Trusted_Connection=True;";
        MyDatabase myDb = new MyDatabase();
        public ActionResult Index()
        {
            try
            {
                //List<Automobiles> automobiles = new List<Automobiles>();
                //List<Purchases> purchases = new List<Purchases>();
                using (var db = new SqlConnection(conString))
                {
                    myDb.automobiles = db.Query<Automobiles>("Select * From Automobiles").ToList();
                    myDb.purchases = db.Query<Purchases>("Select * From Purchases").ToList();
                }
                //ViewBag.Automobiles = automobiles;
                //ViewBag.Purchases = purchases;
                return View(myDb);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        public ActionResult Error(Error err)
        {
            return View(err);
        }


        [HttpGet]
        public ActionResult DeletePurchase(int id)
        {
            try
            {
                var purchase = new Purchases();
                using (var db = new SqlConnection(conString))
                {
                    purchase = db.Query<Purchases>("Select * from Purchases where Purchase_Id=@Id", new { Id = id }).First();
                }
                return View(purchase);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpPost]
        public ActionResult DeletePurchase(Purchases purchase)
        {
            var sql = "delete from Purchases where Purchase_Id=@Id";
            try
            {
                using (var db = new SqlConnection(conString))
                {
                    db.Execute(sql, new { Id = purchase.Purchase_Id });
                }
                return View("PurchaseDeleted");
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }


        [HttpPost]
        public ActionResult UpdatePurchase(Purchases purchase)
        {
            try
            {
                var sql = "update Purchases set Client_Surname=@Client_Surname, Auto_Id = @Auto_Id, Date_Of_Purchase = @Date_Of_Purchase where Purchase_Id = @Purchase_Id";
                if (ModelState.IsValid)
                {

                    using (var db = new SqlConnection(conString))
                    {
                        db.Execute(sql, new { Client_Surname = purchase.Client_Surname, Auto_Id = purchase.Auto_Id, Date_Of_Purchase = purchase.Date_Of_Purchase, Purchase_Id = purchase.Purchase_Id });
                    }
                    return View("PurchaseUpdated");
                }
                else
                {
                    var sql2 = "select Auto_Id, Model from automobiles";
                    using (var db = new SqlConnection(conString))
                    {
                        var selList = db.Query<Automobiles>(sql2);
                        ViewBag.aList = new SelectList(selList, "Auto_Id", "Model");
                        ViewBag.DPurchase = purchase.Date_Of_Purchase.ToString("yyyy-MM-dd");
                    }
                    return View(purchase);
                }
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpGet]
        public ActionResult UpdatePurchase(int id)
        {
            var sql = "select Auto_Id, Model from automobiles";
            var purchase = new Purchases();
            try
            {
                using (var db = new SqlConnection(conString))
                {
                    purchase = db.Query<Purchases>("Select * from Purchases where Purchase_Id=@Id", new { Id = id }).First();
                    var selList = db.Query<Automobiles>(sql);
                    ViewBag.aList = new SelectList(selList, "Auto_Id", "Model");
                    ViewBag.DPurchase = purchase.Date_Of_Purchase.ToString("yyyy-MM-dd");
                    /*                    ViewBag.Purchase_Id = purchase.Purchase_Id;
                                        ViewBag.Client_Surname = purchase.Client_Surname;
                                        ViewBag.Auto_Id = purchase.Auto_Id;
                                        */
                }
                return View(purchase);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }


        [HttpGet]
        public ActionResult UpdateCar(int? id)
        {
            var automobile = new Automobiles();
            try
            {
                using (var db = new SqlConnection(conString))
                {
                    automobile = db.Query<Automobiles>("Select * from Automobiles where Auto_id=@A_Id", new { A_Id = id }).First();
                }
                return View(automobile);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpPost]
        public ActionResult UpdateCar(Automobiles auto)
        {
            var sql = "update automobiles set Model=@Mod, Manufacturer = @Manufact where Auto_id = @A_id";
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new SqlConnection(conString))
                    {
                        db.Execute(sql, new { Mod = auto.Model, Manufact = auto.Manufacturer, A_id = auto.Auto_Id });
                    }
                    return View("Updated");
                }
                catch (Exception ex)
                {
                    var err = new Error
                    {
                        Message = ex.Message
                    };
                    return View("Error", err);
                }
            }
            return View(auto);
        }

        [HttpGet]
        public ActionResult BuyCar()
        {
            try
            {
                var sql = "select Auto_Id, Model from automobiles";
                var purchases = new Purchases();
                using (var db = new SqlConnection(conString))
                {
                    var selList = db.Query<Automobiles>(sql);
                    ViewBag.aList = new SelectList(selList, "Auto_Id", "Model");
                }
                return View(purchases);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpPost]
        public ActionResult BuyCar(Purchases purchase)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sql = "insert into Purchases values(@Surname, @A_Id, @Date)";

                    using (var db = new SqlConnection(conString))
                    {
                        db.Execute(sql, new { Date = DateTime.Now, Surname = purchase.Client_Surname, A_Id = purchase.Auto_Id });
                    }
                    return View("CarBought");
                }
                else
                {

                    var sql2 = "select Auto_Id, Model from automobiles";
                    using (var db = new SqlConnection(conString))
                    {
                        var selList = db.Query<Automobiles>(sql2);
                        ViewBag.aList = new SelectList(selList, "Auto_Id", "Model");
                    }
                    return View(purchase);
                }
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpGet]
        public ActionResult DeleteCar(int id)
        {
            try
            {
                var auto = new Automobiles();
                using (var db = new SqlConnection(conString))
                {
                    auto = db.Query<Automobiles>("Select * from Automobiles where Auto_Id=@Id", new { Id = id }).First();
                }
                return View(auto);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpPost]
        public ActionResult DeleteCar(Automobiles auto)
        {
            try
            {
                var sql = "delete from Automobiles where Auto_Id=@Id";
                using (var db = new SqlConnection(conString))
                {
                    db.Execute(sql, new { Id = auto.Auto_Id });
                }
                return View("Deleted");
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }

        [HttpGet]
        public ActionResult AddCar()
        {
            var auto = new Automobiles();
            return View(auto);
        }

        [HttpPost]
        public ActionResult AddCar(Automobiles auto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sql = "insert into Automobiles values(@Mod, @Manufact)";
                    /*                using (var db = new SqlConnection(conString))
                                    {
                                        db.Execute(sql, new { Mod = auto.Model, Manufact = auto.Manufacturer });
                                    }
                                    return View(auto);*/
                    using (var db = new SqlConnection(conString))
                    {
                        db.Execute(sql, new { Mod = auto.Model, Manufact = auto.Manufacturer });
                    }
                    return View(auto);
                }
                return View(auto);
            }
            catch (Exception ex)
            {
                var err = new Error
                {
                    Message = ex.Message
                };
                return View("Error", err);
            }
        }
    }
}