using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KlijentApp.Models;

namespace KlijentApp.Controllers
{
    public class PrikazBalansaPomocController : Controller
    {
        private ModelEF db = new ModelEF();
        // GET: PrikazNaplate
        private PrikazBalansaPomoc Pomoc = new PrikazBalansaPomoc();
        public ActionResult Index(string sortOrder, string selektovanaFirma, string saldo = null, DateTime? saldoDatum = null)
        {
            try
            {
                bool Poz = false;
                if (saldo == ">0") { Poz = true; }
                else { Poz = false; }

                var kompanije = this.db.Companies.Include(x => x.Transactions).Where(x => x.Active == true && x.Transactions.Any(y => y.TransactionType == PrevodSrb.Produkcija && y.Amount != 0));
                var firme = kompanije.AsEnumerable().Where(x => (x.CompanyDescription == selektovanaFirma || selektovanaFirma == "" || selektovanaFirma == null)).OrderByDescending(x => x.UpdatedOn).AsEnumerable();
                if (saldo != null && saldo != "") { firme = firme.Where(x => (Poz == true ? Prenosna.RacunajSaldo(x).Value >= 0 : Prenosna.RacunajSaldo(x).Value < 0)); }
                if (saldoDatum != null) { firme = firme.Where(x => Prenosna.RacunajSaldo(x).Key == (DateTime)saldoDatum); }
                //if (Rata!= null) { firme = firme.Where(x => x.MonthlyFee = Rata;)}
                ViewBag.Opisi = kompanije.AsEnumerable().Select(x => x.CompanyDescription).Distinct();
                List<string> a = new List<string>();
                a.Add(">0");
                a.Add("<0");
                ViewBag.Saldo = a;
                var b = kompanije.AsEnumerable().Select(x => Prenosna.RacunajSaldo(x).Key).Distinct();
                ViewBag.SaldoDatum = b;
                ViewBag.SortingSaldo = String.IsNullOrEmpty(sortOrder) ? "Saldo" : "";
                ViewBag.SortingName = sortOrder == "Name" ? "Name_Desc" : "Name";
                ViewBag.SortingRata = sortOrder == "Rata" ? "Rata_Desc" : "Rata";
                ViewBag.SortingSaldoDatum = sortOrder == "SaldoDatum" ? "SaldoDatum_Desc" : "SaldoDatum";
                switch (sortOrder)
                {
                    case "Name":
                        firme = firme.OrderBy(stu => stu.CompanyDescription);
                        break;
                    case "Name_Desc":
                        firme = firme.OrderByDescending(stu => stu.CompanyDescription);
                        break;
                    case "Rata":
                        firme = firme.OrderBy(stu => stu.CompanyDescription);
                        break;
                    case "Rata_Desc":
                        firme = firme.OrderByDescending(stu => stu.CompanyDescription);
                        break;
                    case "SaldoDatum":
                        firme = firme.OrderBy(stu => Prenosna.RacunajSaldo(stu).Key);
                        break;
                    case "SaldoDatum_Desc":
                        firme = firme.OrderByDescending(stu => Prenosna.RacunajSaldo(stu).Key);
                        break;

                    case "Saldo":
                        firme = firme.OrderBy(stu => Prenosna.RacunajSaldo(stu).Value);
                        break;

                    default:
                        firme = firme.OrderByDescending(stu => Prenosna.RacunajSaldo(stu).Value);
                        break;
                }
                List<Models.PrikazNaplate> Izlaz = new List<Models.PrikazNaplate>();
                foreach (Models.Company x in firme)
                {
                    var z = Prenosna.RacunajSaldo(x);
                    var rat = Prenosna.RacunajRatu(x);
                    Izlaz.Add(new Models.PrikazNaplate() { Opis = x.CompanyDescription, Rata = rat, FirmaId = x.CompanyId, Saldo = z.Value, SaldoDatum = z.Key, Selected = z.Value < 0 ? true : false, Kolor = z.Value < 0 ? System.Drawing.Color.Red : z.Value == 0 ? System.Drawing.Color.Black : System.Drawing.Color.Green });
                }
             
                Izlaz = Izlaz.OrderByDescending(x => x.Saldo).ToList();
                Pomoc.ListaNaplate = Izlaz;
                Prenosna.PrenesiNesto = this;
                return View(Pomoc);
            }
            catch { return null; }
        }
        [HttpPost]
        public ActionResult SubmitSelected(PrikazBalansaPomoc pbp)
        {
            var a = pbp;
            var list = pbp.ListaNaplate;
            foreach (var p in pbp.ListaNaplate)
            {
                if (p.Selected)
                {
                    var g = p.FirmaId;

                    // Do your logic}
                }
            }
                //  var b = a.getSelectedIds();
                // var c = (PrikazBalansaPomocController)Prenosna.PrenesiNesto;
                // var d = c.Pomoc.ListaNaplate;
                return null;
        }

    }
}