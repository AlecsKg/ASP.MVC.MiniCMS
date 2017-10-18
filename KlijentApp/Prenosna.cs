using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc ;
using System.Data.Entity;
using System.Text;
using System.Data.Entity.Validation;
using static System.Math;
namespace KlijentApp
{
    public static class Prenosna
    {
        public static DateTime StariDatum;
        public static object PrenesiNesto;
        public static string PretvoridANe(bool vrednost)
        {
            if (vrednost == true)
            {
                return PrevodSrb.Da;
            }
            else
            {
                return PrevodSrb.Ne;
            }
        }

        public static void SnimiDb(ModelEF db) {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Greskonja(dbEx);
            }
        }
        private class DatumIznosTransakcije  {
            public DateTime datum;
            public double Iznos;
            public string Tip;


        };
        public static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

       public static String RGBConverter(System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
        public static Double RacunajRatu(Models.Company komp)
        {
            double rata = 0;
            try {
                using (ModelEF db = new ModelEF())
                {
                    var prvi = db.Companies.Include(x => x.Transactions).FirstOrDefault(x => x.Active && x.CompanyId == komp.CompanyId);
                    if (prvi != null && prvi.Transactions != null && prvi.Transactions.Count > 0 )
                    {
                        var trki = prvi.Transactions.Where(x => x.Active && x.TransactionType == PrevodSrb.Produkcija);
                        if (trki.Count() > 0)
                        {
                            rata = trki.OrderByDescending(x => x.TransactionDate).FirstOrDefault().Amount;
                        }
                       
                    }

                }
            }
            catch { };


            return rata;
        }
        public static KeyValuePair<DateTime,double> RacunajSaldo (Models.Company komp)
        {
            Double Zbir1 = 0;
            Double Zbir2 = 0;
            double Zbir3 = 0;
            DateTime Vreme = new DateTime(2000, 1, 1);
         
                //var kom = db.Companies.Include(x => x.Transactions).AsEnumerable().FirstOrDefault(x => x.Active && x.CompanyId == komp.CompanyId); ;
                var kom = komp;
                if (kom.Transactions != null && kom.Transactions.Count > 0)
                {
                var Listica = kom.Transactions.Where(x => x.Active && x.Amount != 0 && (x.TransactionType == PrevodSrb.Naplaćeno || x.TransactionType == PrevodSrb.Naplaćeno_delimično || x.TransactionType == PrevodSrb.Stornirano || x.TransactionType == PrevodSrb.Produkcija)).Select(x=> new DatumIznosTransakcije {Iznos = x.Amount,datum = x.TransactionDate, Tip = x.TransactionType}).ToList();
                if (Listica.Count > 0) {
                    var ProdDatum = kom.Transactions.Where(x => x.Active && x.TransactionType == PrevodSrb.Produkcija).OrderBy(x => x.TransactionDate).FirstOrDefault().TransactionDate;
                    var PocetakMeseca = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (ProdDatum < PocetakMeseca) { };
                    Zbir1 = Listica.Where(x=> (x.Tip == PrevodSrb.Naplaćeno || x.Tip == PrevodSrb.Naplaćeno_delimično)).Sum(x => Abs(x.Iznos));
                                        Zbir2 = Listica.Where(x=> (x.Tip == PrevodSrb.Stornirano)).Sum(x => -Abs(x.Iznos));
                    var Prvi = Listica.OrderByDescending(x => x.datum).FirstOrDefault();
                    if (Prvi != null) { Vreme = Prvi.datum ; }
                    var ListaProdukcije = Listica.Where(x => x.Tip == PrevodSrb.Produkcija).ToList();
                    if (ListaProdukcije.Count>0)
                    {
                        var ListaDatuma = ListaProdukcije.Select(x => x.datum).ToList();
                        ListaDatuma.Add(DateTime.Now);
                        for (int i = 0; i <= ListaDatuma.Count - 2; i++)
                        {
                            int RazMes = ListaDatuma[i + 1].Month - ListaDatuma[i].Month;
                            if (RazMes > 0)
                            {
                                Zbir3 += -Abs(ListaProdukcije[i].Iznos) * RazMes;
                            }
                        }
                    }
                   
                }
            }
               
          


            return new KeyValuePair<DateTime, double>(Vreme, Zbir1 + Zbir2 + Zbir3);
        }

        public static void Greskonja(DbEntityValidationException dbEx)
        {
          
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
           

        }

        public static int DajJuzerAjDi(Object obj)
        {
            string sesija = obj.ToString();
 return Convert.ToInt32(sesija);
        } 
        public static IEnumerable<SelectListItem> PretvoriListuBoolUDaNeSelektAjtem(IEnumerable<bool> ulaz)
        {
            return ulaz.Select(x => new SelectListItem(){Value = x.ToString(), Text = PretvoridANe(x)});
        }

        public static string Poruka(String tekst)
        {
            return "<script>alert('" + tekst + "');</script>";
        }
    }
}