using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using KlijentApp.Models;
using System.Data.Entity.Validation;

namespace KlijentApp
{
    public class CompaniesController : Controller
    {
        private ModelEF db = new ModelEF();

        // GET: Companies
        public ActionResult Index(string sortOrder,string selektovanaFirma, int numberOfDocTypesIn = -1, int numberOfDocTypesOut = -1, bool xmlIn = true, bool pdfIn = true, string iiliin = "", bool xmlOut = true, bool pdfOut = true, string iiliout = "", bool aktivni = true)
        {
      
            var kompanije = this.db.Companies.Where(x => x.Active == aktivni);
            var firme = kompanije.Where(x => (x.CompanyDescription == selektovanaFirma || selektovanaFirma == "" || selektovanaFirma == null)).OrderByDescending(x => x.UpdatedOn).AsEnumerable();
            if (numberOfDocTypesIn != -1)
            {
                firme = firme.Where(x => (x.NumberOfDocTypesIn == numberOfDocTypesIn));}
            if (numberOfDocTypesOut != -1)
            {
                firme = firme.Where(x => (x.NumberOfDocTypesOut == numberOfDocTypesOut));
            }
            if (iiliin != "") { firme = iiliin == "i" ? firme.Where(x => x.PdfIn == pdfIn & x.XmlIn == xmlIn).AsQueryable() : firme.Where(x => x.PdfIn == pdfIn | x.XmlIn == xmlIn).AsQueryable(); }
            if (iiliout != "") { firme = iiliout == "i" ? firme.Where(x => x.PdfOut == pdfOut & x.XmlOut == xmlOut).AsQueryable() : firme.Where(x => x.PdfOut == pdfOut | x.XmlOut == xmlOut).AsQueryable();}
          
            List<string> listica = new List<string>();
            listica.Add("ili");
            listica.Add("i");
            ViewBag.IIliIn = listica.AsEnumerable();
            ViewBag.IIliOut = listica.AsEnumerable();
            ViewBag.Opisi = kompanije.Select(x => x.CompanyDescription).Distinct();

            ViewBag.PdfoviIn = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(firme.Select(x => x.PdfIn ).Distinct());
            ViewBag.XmloviIn = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(firme.Select(x => x.XmlIn).Distinct());


            ViewBag.PdfoviOut = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(firme.Select(x => x.PdfOut ).Distinct());
            ViewBag.XmloviOut = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(firme.Select(x => x.PdfOut ).Distinct());


            ViewBag.Aktivni = Prenosna.PretvoriListuBoolUDaNeSelektAjtem(db.Companies.AsEnumerable().Select(x => x.Active).Distinct());


            ViewBag.SortingUpdatedOn = String.IsNullOrEmpty(sortOrder) ? "UpdatedOn" : "";
            ViewBag.SortingName = sortOrder == "Name" ? "Name_Desc" : "Name";
            //   ViewBag.SortingUpdatedOn = sortOrder == "UpdatedOn" ? "UpdatedOn_Desc" : "UpdatedOn";
            ViewBag.SortingMesecnaNaknada = sortOrder == "MesecnaNaknada" ? "MesecnaNaknada_Desc" : "MesecnaNaknada";
   
            ViewBag.SortingMod = sortOrder == "Mod" ? "Mod_Desc" : "Mod";
            ViewBag.SortingPozivNaBroj = sortOrder == "PozivNaBroj" ? "PozivNaBroj_Desc" : "PozivNaBroj";

            ViewBag.SortingNumberOfDocTypesOut = sortOrder == "NumberOfDocTypesOut" ? "NumberOfDocTypesOut_Desc" : "NumberOfDocTypesOut";
            ViewBag.SortingNumberOfDocTypesIn = sortOrder == "NumberOfDocTypesIn" ? "NumberOfDocTypesIn_Desc" : "NumberOfDocTypesIn";
            ViewBag.SortingXmlOut = sortOrder == "XmlOut" ? "XmlOut_Desc" : "XmlOut";
            ViewBag.SortingXmlIn = sortOrder == "XmlIn" ? "XmlIn_Desc" : "XmlIn";
            ViewBag.SortingPdfOut = sortOrder == "PdfOut" ? "PdfOut_Desc" : "PdfOut";
            ViewBag.SortingPdfIn = sortOrder == "PdfIn" ? "PdfIn_Desc" : "PdfIn";
            ViewBag.SortingOtherFormatIn = sortOrder == "OtherFormatIn" ? "OtherFormatIn_Desc" : "OtherFormatIn";
            ViewBag.SortingOtherFormatOut = sortOrder == "OtherFormatOut" ? "OtherFormatOut_Desc" : "OtherFormatOut";
            ViewBag.SortingNeededConversion = sortOrder == "NeededConversion" ? "NeededConversion_Desc" : "NeededConversion";
            ViewBag.SortingComment = sortOrder == "Comment" ? "Comment_Desc" : "Comment";
            ViewBag.SortingActive = sortOrder == "Active" ? "Active_Desc" : "Active";
      
            switch (sortOrder)
            {
                case "Name":
                    firme = firme.OrderBy(stu => stu.CompanyDescription);
                    break;
                case "Name_Desc":
                    firme = firme.OrderByDescending(stu => stu.CompanyDescription);
                    break;

                case "PozivNaBroj":
                    firme = firme.OrderBy(stu => stu.RefNumber);
                    break;
                case "PozivNaBroj_Desc":
                    firme = firme.OrderByDescending(stu => stu.RefNumber);
                    break;

                case "Mod":
                    firme = firme.OrderBy(stu => stu.Mod );
                    break;
                case "Mod_Desc":
                    firme = firme.OrderByDescending(stu => stu.Mod);
                    break;

                case "MesecnaNaknada":
                    firme = firme.OrderBy(stu => Prenosna.RacunajRatu(stu));
                    break;
                case "MesecnaNaknada_Desc":
                    firme = firme.OrderByDescending(stu => Prenosna.RacunajRatu(stu));
                    break;

             

                case "UpdatedOn":
                    firme = firme.OrderBy(stu => stu.UpdatedOn);
                    break;            
                case "NumberOfDocTypesOut":
                    firme = firme.OrderBy(stu => stu.NumberOfDocTypesOut);
                    break;
                case "NumberOfDocTypesOut_Desc":
                    firme = firme.OrderByDescending(stu => stu.NumberOfDocTypesOut);
                    break;
                case "NumberOfDocTypesIn":
                    firme = firme.OrderBy(stu => stu.NumberOfDocTypesIn);
                    break;
                case "NumberOfDocTypesIn_Desc":
                    firme = firme.OrderByDescending(stu => stu.NumberOfDocTypesIn);
                    break;
                case "XmlOut":
                    firme = firme.OrderBy(stu => stu.XmlOut);
                    break;
                case "XmlOut_Desc":
                    firme = firme.OrderByDescending(stu => stu.XmlOut);
                    break;
                case "XmlIn":
                    firme = firme.OrderBy(stu => stu.XmlIn);
                    break;
                case "XmlIn_Desc":
                    firme = firme.OrderByDescending(stu => stu.XmlIn);
                    break;
                case "PdfOut":
                    firme = firme.OrderBy(stu => stu.PdfOut);
                    break;
                case "PdfOut_Desc":
                    firme = firme.OrderByDescending(stu => stu.PdfOut);
                    break;
                case "PdfIn":
                    firme = firme.OrderBy(stu => stu.PdfIn);
                    break;
                case "PdfIn_Desc":
                    firme = firme.OrderByDescending(stu => stu.PdfIn);
                    break;
                case "OtherFormatIn":
                    firme = firme.OrderBy(stu => stu.OtherFormatIn);
                    break;
                case "OtherFormatIn_Desc":
                    firme = firme.OrderByDescending(stu => stu.OtherFormatIn);
                    break;
                case "OtherFormatOut":
                    firme = firme.OrderBy(stu => stu.OtherFormatOut);
                    break;
                case "OtherFormatOut_Desc":
                    firme = firme.OrderByDescending(stu => stu.OtherFormatOut);
                    break;
                case "NeededConversion":
                    firme = firme.OrderBy(stu => stu.NeededConversion);
                    break;
                case "NeededConversion_Desc":
                    firme = firme.OrderByDescending(stu => stu.NeededConversion);
                    break;
                case "Comment":
                    firme = firme.OrderBy(stu => stu.Comment);
                    break;
                case "Comment_Desc":
                    firme = firme.OrderByDescending(stu => stu.Comment);
                    break;
                case "Active":
                    firme = firme.OrderBy(stu => stu.Active);
                    break;
                case "Active_Desc":
                    firme = firme.OrderByDescending(stu => stu.Active);
                    break;
                
                default:
                    firme = firme.OrderByDescending(stu => stu.UpdatedOn);
                    break;
            }

            return View(firme.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Company company)
        {
            if (ModelState.IsValid)
            {
                company.Active = true;    
                company.UpdatedOn = DateTime.Now;
                company.BalanceCheckDate = new DateTime(2000, 1, 1);
                db.Companies.Add(company);
                Prenosna.SnimiDb(db);

                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Company company)
        {
            if (ModelState.IsValid)
            {
                company.UpdatedOn = DateTime.Now;              
                db.Entry(company).State = EntityState.Modified;
                company.BalanceCheckDate = new DateTime(2000, 1, 1);
                Prenosna.SnimiDb(db);



                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
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
