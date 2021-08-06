using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Try3.Models
{
    public class Purchases
    {
        public int Purchase_Id { get; set; }
        [Required(ErrorMessage = "Client Surname is required.")]
        public string Client_Surname { get; set; }
        [Required(ErrorMessage = "Auto is required.")]
        public int Auto_Id { get; set; }
        public DateTime Date_Of_Purchase { get; set; }
    }
}