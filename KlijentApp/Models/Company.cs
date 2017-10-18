using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KlijentApp.Models;

namespace KlijentApp.Models
{
    using System.Collections.Generic;

    public class Company
    {

        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [DisplayName("Opis firme")]
        public string CompanyDescription { get; set; }

        [DisplayName("Komentar")]
        public string Comment { get; set; }

        [DisplayName("Aktivan")]
        public bool Active { get; set; }


        [DisplayName("B.T.D. - Izlaz")]
        public int NumberOfDocTypesOut { get; set; }

        [DisplayName("XML - Izlaz")]
        public bool XmlOut { get; set; }

        [DisplayName("PDF - Izlaz")]
        public bool PdfOut { get; set; }

        [DisplayName("B.T.D. - Ulaz")]
        public int NumberOfDocTypesIn { get; set; }

        [DisplayName("XML - Ulaz")]
        public bool XmlIn { get; set; }

        [DisplayName("PDF - Ulaz")]
        public bool PdfIn { get; set; }

        [DisplayName("Drugi formati - Ulaz")]
        public bool OtherFormatIn { get; set; }

        [DisplayName("Drugi formati - Izlaz")]
        public bool OtherFormatOut { get; set; }

        [DisplayName("Potrebna konverzija")]
        public bool NeededConversion { get; set; }     


        [DisplayName("Model")]
        public string Mod { get; set; }

        [DisplayName("Poziv na broj")]
        public string RefNumber { get; set; }

        [DisplayName("Ažurirano")]
        [Column(TypeName = "smalldatetime")]
        public DateTime UpdatedOn { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime BalanceCheckDate { get; set; }

        public ICollection<Transaction> Transactions { set; get; }
       
    }
}