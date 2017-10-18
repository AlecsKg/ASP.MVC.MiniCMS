using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlijentApp.Models
{
    public class PrikazNaplate
    {
    
        public bool Selected { get; set; }

        [DisplayName("Id")]
        public int FirmaId { get; set; }

        public double Rata { get; set; }
        public string Opis { get; set; }
        public double Saldo { get; set; }

        [DisplayName("Saldo datum")]
        public DateTime SaldoDatum { get; set; }

        public System.Drawing.Color Kolor {get;set;}

    }
}