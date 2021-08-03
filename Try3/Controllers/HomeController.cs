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

        public ActionResult Index()
        {
            List<Automobiles> automobiles = new List<Automobiles>();
            List<Purchases> purchases = new List<Purchases>();
            using (IDbConnection db = new SqlConnection(conString))
            {
                automobiles = db.Query<Automobiles>("Select * From Automobiles").ToList();
                purchases = db.Query<Purchases>("Select * From Purchases").ToList();
            }
            ViewBag.Automobiles = automobiles;
            ViewBag.Purchases = purchases;
            return View();
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
        public ActionResult DeletePurchase(int? id)
        {
            ViewBag.DeletingId = id;
            return View();
        }

        [HttpPost]
        public string DeletePurchase(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("delete from Purchases where Purchase_Id=@Id", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Purchase deleted" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }


        [HttpPost]
        public string UpdatePurchase(int Purchase_Id, string Client_Surname, int Auto_Id, DateTime Date_Of_Purchase)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("update Purchases set Client_Surname=@Client_Surname, Auto_Id = @Auto_Id, Date_Of_Purchase = @Date_Of_Purchase where Purchase_Id = @Purchase_Id", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Client_Surname", Client_Surname);
                    cmd.Parameters.AddWithValue("@Auto_Id", Auto_Id);
                    cmd.Parameters.AddWithValue("@Date_Of_Purchase", Date_Of_Purchase);
                    cmd.Parameters.AddWithValue("@Purchase_Id", Purchase_Id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Purchase updated" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }

        [HttpGet]
        public ActionResult UpdatePurchase(int? id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(conString))
                {
                    Purchases purchase = db.Query<Purchases>("Select * from Purchases where Purchase_Id=" + id).First();
                    ViewBag.Purchase_Id = purchase.Purchase_Id;
                    ViewBag.Client_Surname = purchase.Client_Surname;
                    ViewBag.Auto_Id = purchase.Auto_Id;
                    ViewBag.Date_Of_Purchase = purchase.Date_Of_Purchase.ToString("yyyy-MM-dd");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult UpdateCar(int? id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(conString))
                {
                    Automobiles automobile = db.Query<Automobiles>("Select * from Automobiles where Auto_id=" + id).First();
                    ViewBag.Auto_Id = automobile.Auto_Id;
                    ViewBag.Model = automobile.Model;
                    ViewBag.Manufacturer = automobile.Manufacturer;
                }
                return View();
            }
            catch
            {
                return View();
            }            
        }

        [HttpPost]
        public string UpdateCar(int Auto_id, string Model,string Manufacturer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("update automobiles set Model=@model, Manufacturer = @Manufacturer where Auto_id = @auto_id", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Model", Model);
                    cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                    cmd.Parameters.AddWithValue("@Auto_id", Auto_id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Car updated" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }

            [HttpGet]
        public ActionResult BuyCar(int? id)
        {
            ViewBag.BuyingId = id;
            return View();
        }

        [HttpPost]
        public string BuyCar(int Auto_Id, string Client_Surname)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("insert into Purchases values(@Surname,@Auto_Id,@Date)", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Surname", Client_Surname);
                    cmd.Parameters.AddWithValue("@Auto_Id", Auto_Id);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Car bought" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }

        [HttpGet]
        public ActionResult DeleteCar(int? id)
        {
            ViewBag.DeletingId = id;
            return View();
        }

        [HttpPost]
        public string DeleteCar(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("delete from Automobiles where Auto_Id=@Id", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Car deleted" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }

        [HttpGet]
        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public string AddCar(string model, string manufacturer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("insert into Automobiles values(@Model, @Manufacturer)", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Model", model);
                    cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Car added" + " <a href=' / '>Return to main page</a>";
            }
            catch
            {
                return "Something went wrong";
            }
        }
    }
}