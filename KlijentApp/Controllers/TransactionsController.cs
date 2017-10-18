using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;

namespace KlijentApp
{
    public class TransactionsController : Controller
    {
       ModelEF db = new ModelEF();
      
        public List<SelectListItem > ListaSelekta = new List<SelectListItem>
        {
            new SelectListItem(){Value = PrevodSrb.Prvi_kontakt, Text = PrevodSrb.Prvi_kontakt},
            new SelectListItem(){Value = PrevodSrb.Potpisan_ugovor, Text = PrevodSrb.Potpisan_ugovor},
            new SelectListItem(){Value = PrevodSrb.Priprema_za_test, Text = PrevodSrb.Priprema_za_test},
            new SelectListItem(){Value = PrevodSrb.Testiranje, Text = PrevodSrb.Testiranje},
            new SelectListItem(){Value = PrevodSrb.Priprema_za_produkciju, Text =PrevodSrb.Priprema_za_produkciju},
            new SelectListItem(){Value = PrevodSrb.Produkcija, Text = PrevodSrb.Produkcija},
            new SelectListItem(){Value = PrevodSrb.Naplaćeno, Text = PrevodSrb.Naplaćeno},
            new SelectListItem(){Value = PrevodSrb.Naplaćeno_delimično, Text = PrevodSrb.Naplaćeno_delimično},
            new SelectListItem(){Value = PrevodSrb.Nije_plaćeno, Text = PrevodSrb.Nije_plaćeno},
            new SelectListItem(){Value = PrevodSrb.Pauzirana_saradnja, Text = PrevodSrb.Pauzirana_saradnja},
            new SelectListItem(){Value = PrevodSrb.Nastavljena_saradnja, Text = PrevodSrb.Nastavljena_saradnja},
            new SelectListItem(){Value = PrevodSrb.Poslata_opomena, Text = PrevodSrb.Poslata_opomena},
            new SelectListItem(){Value = PrevodSrb.Raskid_ugovora, Text = PrevodSrb.Raskid_ugovora},
            new SelectListItem(){Value = PrevodSrb.Razno__pogledati_komentar_, Text = PrevodSrb.Razno__pogledati_komentar_},
             new SelectListItem(){Value = PrevodSrb.Stornirano , Text = PrevodSrb.Stornirano}
        };



        // GET: Transactions/Index
        public  ViewResult Index(string sortOrder, string selektovanaFirma = "", string selektovanTip = "", bool aktivni = true, DateTime? datum1 = null, DateTime? datum2 = null, bool In = true, bool Out = true, string iili = "")
        {/**/
            var klijenti = db.Transactions.Include(c => c.SystemUser).Include(c=> c.Company).AsEnumerable()
                .Where(x => x.Active == aktivni).ToList();
            DateTime d1 = DateTime.MinValue;
            DateTime d2 = DateTime.MaxValue;
            if (datum1 == null)
            {datum1=d1;}
            if (datum2 == null)
            {datum2=d2;}

            var Transactions = klijenti.Where(x => (x.Company.CompanyDescription == selektovanaFirma || selektovanaFirma == "" || selektovanaFirma == null)
                                              && (x.TransactionType == selektovanTip || selektovanTip == "" ||
                                                  selektovanTip == null)
                  && x.TransactionDate >= datum1 && x.TransactionDate <= datum2).AsQueryable();
            if (iili != "") { Transactions = iili == "i" ? Transactions.Where(x => x.Out == Out & x.In == In).AsQueryable() : Transactions.Where(x => x.Out == Out | x.In == In).AsQueryable(); }
            List<string> listica = new List<string>();
            listica.Add("ili");
            listica.Add("i");
            ViewBag.IIli = listica.AsEnumerable();
            ViewBag.Opisi = klijenti.Select(x => x.Company.CompanyDescription).Distinct();
            ViewBag.Tipovi = Transactions.Select(x => x.TransactionType).Distinct();
            ViewBag.Odgovorni = Transactions.Select(x => x.ResponsiblePerson).Distinct();
            ViewBag.Datumi = Transactions.OrderBy(x => x.TransactionDate).Select(x => x.TransactionDate).Distinct();
            ViewBag.Datumi2 = Transactions.OrderBy(x => x.TransactionDate).Select(x => x.TransactionDate).Distinct();
          ViewBag.Inovi = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(Transactions.Select(x => x.In).Distinct());
           ViewBag.Outovi = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(Transactions.Select(x => x.Out).Distinct());
            ViewBag.Aktivni = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(db.Transactions.AsEnumerable().Select(x => x.Active).Distinct());
            ViewBag.ListaSelekta = ListaSelekta;

            ViewBag.SortingDate = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
           //ViewBag.SortingDate = sortOrder == "Date" ? "Date_Desc" : "Date";
            ViewBag.SortingName = sortOrder == "Name" ? "Name_Desc" : "Name";
            ViewBag.SortingAmount = sortOrder == "Amount" ? "Amount_Desc" : "Amount";
            ViewBag.SortingContact = sortOrder == "Contact" ? "Contact_Desc" : "Contact";
            ViewBag.SortingType = sortOrder == "Type" ? "Type_Desc" : "Type";
            ViewBag.SortingTypeDoc = sortOrder == "TypeDoc" ? "TypeDoc_Desc" : "TypeDoc";

            ViewBag.SortingIn = sortOrder == "In" ? "In_Desc" : "In";
            ViewBag.SortingOut = sortOrder == "Out" ? "Out_Desc" : "Out";

            ViewBag.SortingComment = sortOrder == "Comment" ? "Comment_Desc" : "Comment";
            ViewBag.SortingActive = sortOrder == "Active" ? "Active_Desc" : "Active";

            ViewBag.SortingNick = sortOrder == "Nick" ? "Nick_Desc" : "Nick";
            switch (sortOrder)
            {
                case "Name_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Company.CompanyDescription);
                    break;
                case "Name":
                    Transactions = Transactions.OrderBy(stu => stu.Company.CompanyDescription);
                    break;
                case "Contact_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Contact);
                    break;
                case "Contact":
                    Transactions = Transactions.OrderBy(stu => stu.Contact);
                    break;
                case "Amount_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Amount );
                    break;
                case "Amount":
                    Transactions = Transactions.OrderBy(stu => stu.Amount);
                    break;
                case "Date":
                    Transactions = Transactions.OrderBy(stu => stu.TransactionDate);
                    break;
                //case "Date_Desc":
                //    Transactions = Transactions.OrderByDescending(stu => stu.TransactionDate);
                //    break;
                case "Type":
                    Transactions = Transactions.OrderBy(stu => stu.TransactionType);
                    break;
                case "Type_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.TransactionType);
                    break;
                //case "TypeDoc":
                //    Transactions = Transactions.OrderBy(stu => stu.NumberOfDocTypes);
                //    break;
                //case "TypeDoc_Desc":
                //    Transactions = Transactions.OrderByDescending(stu => stu.NumberOfDocTypes);
                //    break;
                case "Out":
                    Transactions = Transactions.OrderBy(stu => stu.Out);
                    break;
                case "Out_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Out);
                    break;
                case "In":
                    Transactions = Transactions.OrderBy(stu => stu.In);
                    break;
                case "In_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.In);
                    break;
                case "Comment":
                    Transactions = Transactions.OrderBy(stu => stu.Comment);
                    break;
                case "Comment_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Comment);
                    break;
                case "Active":
                    Transactions = Transactions.OrderBy(stu => stu.Active);
                    break;
                case "Active_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.Active);
                    break;
                case "Nick":
                    Transactions = Transactions.OrderBy(stu => stu.SystemUser.Nick );
                    break;
                case "Nick_Desc":
                    Transactions = Transactions.OrderByDescending(stu => stu.SystemUser.Nick);
                    break;
                default:
                    Transactions = Transactions.OrderByDescending(stu => stu.TransactionDate);
                    break;
            }
            return View(Transactions.ToList());
          
        }
     
        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction Transaction = db.Transactions.Find(id);
            if (Transaction == null)
            {
                return HttpNotFound();
            }
            return View(Transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.Opisi = db.Companies.Where(x=> x.Active).Select(x=> x.CompanyDescription).Distinct();
            ViewBag.ListaSelekta = ListaSelekta;
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Transaction transaction,string selektovanaFirma = "")
        //[Bind(Include = "TransactionID,CompanyId,TransactionType,TransactionDate,In,Out,ResponsiblePerson,Comment,Active")]
        {
            if (ModelState.IsValid)
            {
             
                try
                {
                    transaction.Active = true;
                    var firstOrDefault = this.db.Companies
                        .FirstOrDefault(x => x.Active && x.CompanyDescription == selektovanaFirma);
                    if (firstOrDefault
                        != null)
                        transaction.CompanyId = firstOrDefault.CompanyId;
                    if (transaction.TransactionType == PrevodSrb.Produkcija )
                    {
                        if (transaction.Amount > 0) { firstOrDefault.UpdatedOn = DateTime.Now;
                        
                        }
                        else {
                            TempData["msg"] ="Ako upisujete transakciju produkcija, mora da se unese iznos koji je veći od nule";
                            return RedirectToAction("Index");
                        }


                    };

                    transaction.ResponsiblePerson = Prenosna.DajJuzerAjDi(Session["UserID"]);
              transaction.TransactionDate = DateTime.Now;
           
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["msg"] = Prenosna.Poruka(PrevodSrb.Niste_ulogovani);
                    return RedirectToAction("Login", "Login");
                    
                }
              
            }

           //ViewBag.ResponsiblePerson = new SelectList(db.Users, "UserId", "Nick", Transaction.ResponsiblePerson);
            return View(transaction);
        }
        private double StarIIznos = 0;
   
        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction Transaction = db.Transactions.Find(id);
            if (Transaction == null)
            {
                return HttpNotFound();
            }
           Prenosna.StariDatum = Transaction.TransactionDate;
            StarIIznos = Transaction.Amount;
            ViewBag.ListaSelekta = ListaSelekta;
            ViewBag.ResponsiblePerson = new SelectList(db.SystemUsers.Where(x=> x.Active && x.UserId == Transaction.ResponsiblePerson), "UserId", "Nick", Transaction.ResponsiblePerson);
            var systemUser = db.SystemUsers.Find(Transaction.ResponsiblePerson);
            if (systemUser != null)
                ViewBag.Napravio = systemUser.Nick;
            return View(Transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Transaction Transaction)
        //[Bind(Include = "TransactionID,CompanyId,TransactionType,TransactionDate,In,Out,ResponsiblePerson,Comment,Active")]
        {
            if (ModelState.IsValid )
            {
                if (Transaction == null | Session["UserID"] == null)
                {
                    return RedirectToAction("Login","Login");
                }
                else
                {
                    if (Transaction.ResponsiblePerson == Prenosna.DajJuzerAjDi(Session["UserID"]))
                    {
                        bool Udri = true;
                        if (Transaction.TransactionType == PrevodSrb.Produkcija) {
                            if (Transaction.Amount > 0)
                            {
                            }
                            else
                            {
                                TempData["msg"] = "Ako upisujete transakciju produkcija, mora da se unese iznos koji je veći od nule";
                                return RedirectToAction("Index");
                            }
                        }

                        Transaction.TransactionDate = Prenosna.StariDatum;
                       if (Udri) { db.Entry(Transaction).State = EntityState.Modified; }
                        else
                        {
                            db.Transactions.Add(Transaction);
                        }
                    //   db.Entry(Transaction).State = EntityState.Modified;
                        Prenosna.SnimiDb(db);
                 
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["msg"] = Prenosna.Poruka(PrevodSrb.Ne_mogu_se_menjati_unosi_drugog_korisnika_);
              }
                    
                }



            }
            ViewBag.ListaSelekta = ListaSelekta;

            //ViewBag.ResponsiblePerson = new SelectList(db.SystemUsers, "UserId", "Nick", Transaction.ResponsiblePerson);
          
            return View(Transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction Transaction = db.Transactions.Find(id);
            if (Transaction == null)
            {
                return HttpNotFound();
            }
        return View(Transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["msg"] = Prenosna.Poruka(PrevodSrb.Niste_ulogovani);
                return RedirectToAction("Index");
            } else
            {
                Transaction Transaction = db.Transactions.Find(id);
                if (Transaction != null && Transaction.ResponsiblePerson == Prenosna.DajJuzerAjDi(Session["UserID"]))
                {
                    db.Transactions.Remove(Transaction);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = Prenosna.Poruka(PrevodSrb.Ne_može_se_brisati_unos_drugog_korisnika);
                    return RedirectToAction("Index");
                }
            }
          
          
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
