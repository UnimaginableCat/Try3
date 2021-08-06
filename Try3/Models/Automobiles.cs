using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Try3.Models
{
    public class Automobiles
    {
        public int Auto_Id { get; set; }
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Manufacturer is required.")]
        public string Manufacturer { get; set; }
    }
}