
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KlijentApp.Models;

public partial class Transaction
{
    [Key()]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

   
    [ForeignKey("Company")]
    public int CompanyId{ get; set; }

    [DisplayName("Tip transakcije")]
    public string TransactionType { get; set; }
    [DisplayName("Vreme unosa")]
    [Column(TypeName = "smalldatetime")]
    public DateTime TransactionDate { get; set; }

    [DisplayName("Iznos")]
    public double Amount { get; set; }

    [DisplayName("Ulaz")]
    public bool In { get; set; }
    [DisplayName("Izlaz")]
    public bool Out { get; set; }

    [DisplayName("Odgovorna osoba")]
    [ForeignKey("SystemUser")]
    public int ResponsiblePerson { get; set; }
    [DisplayName("Komentar")]
    public string Comment { get; set; }
    [DisplayName("Aktivan")]
    public bool Active { get; set; }


    [DisplayName("Kontakt")]
    public string Contact { get; set; }



    public SystemUser SystemUser { get; set; }

    public Company Company { get; set; }


}