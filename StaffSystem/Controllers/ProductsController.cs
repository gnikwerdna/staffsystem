using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StaffSystem.Models;
using System.Data.Entity.Infrastructure;
using StaffSystem.ViewModels;

namespace StaffSystem.Controllers
{
    public class ProductsController : Controller
    {
        private dbStaffSystem db = new dbStaffSystem();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TypeId")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            PopulateAssignedComplianceType(products);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }



       
       
//**************************edit update starts here***********************************************************************************************
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,TypeId")] Products products)
        public ActionResult Edit(int? id, string[] SelectedComp, string[] SelectedCompSubItem_1, string[] SelectedCompSubItem_2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Producttoupdate = db.Products
                 .Include(s => s.ComplianceItems)
                 .Where(s => s.Id == id)
                 .Single();
            if (TryUpdateModel(Producttoupdate, "", new string[] { "Id", "Name", "TypeId" }))
            {
                try
                {


                    resetProductCompliance(SelectedComp, Producttoupdate);
                    UpdateStaffCompliance(SelectedComp, Producttoupdate);
                 //   UpdateStaffCompliance(SelectedCompSubItem_1, stafftoupdate);
                  //  UpdateStaffCompliance(SelectedCompSubItem_2, stafftoupdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }

           PopulateAssignedComplianceType(Producttoupdate);
            return View(Producttoupdate);
        }

        

        
        //------------------------------------functions------------------------------------------------------------------------------------------------------------------------------------------------
        //reset compliance relation table
        private void resetProductCompliance(string[] SelectedComp, Products Producttoupdate)
        {
            if (SelectedComp == null)
            {
                Producttoupdate.ComplianceItems = new List<ComplianceItems>();
                return;

            }


            var selectedCompHS = new HashSet<string>(SelectedComp);
            var staffCompliances = new HashSet<int>(Producttoupdate.ComplianceItems.Select(c => c.ComplianceID));
            foreach (var comp in db.ComplianceItems)
            {
                Producttoupdate.ComplianceItems.Remove(comp);
            }
        }

        //update complianceproduct relational table
        private void UpdateStaffCompliance(string[] SelectedComp, Products Producttoupdate)
        {
            if (SelectedComp == null)
            {
                Producttoupdate.ComplianceItems = new List<ComplianceItems>();
                return;

            }
            var selectedCompHS = new HashSet<string>(SelectedComp);
            var staffCompliances = new HashSet<int>(Producttoupdate.ComplianceItems.Select(c => c.ComplianceID));
            foreach (var comp in db.ComplianceItems)
            {
                if (SelectedComp.Contains(comp.ComplianceID.ToString()))
                {
                    if (!staffCompliances.Contains(comp.ComplianceID))
                    {
                        Producttoupdate.ComplianceItems.Add(comp);

                    }
                }
                else
                {
                    if (staffCompliances.Contains(comp.ComplianceID))
                    {
                        //staffToUpdate.ComplianceItems.Remove(comp);

                    }
                }
            }
        }

        
        

//*********************************** Assigned types
        private void PopulateAssignedComplianceType(Products Producttoupdate)
        {
            //var allCompliancetypes = db.complianceitems;
            var allCompliancetypes = db.ComplianceItems
                .OrderBy(cl => cl.grp)
                .ThenBy(cl => cl.order)

                ;
            var Prodtoupdate = db.Products
                //    .Include(s => s.ComplianceItems)
                // .Where(s => s.Id ==Id)
                // .Single()
         ;



            var staffcompliance = new HashSet<int>(Producttoupdate.ComplianceItems.Select(c => c.ComplianceID));
            var viewModel = new List<AssignedComplianceProductData>();

            foreach (var comp in allCompliancetypes)
            {
                viewModel.Add(new AssignedComplianceProductData
                {
                    ComplianceID = comp.ComplianceID,
                    Title = comp.Title,
                    Assigned = staffcompliance.Contains(comp.ComplianceID),
                    level = comp.level,
                    grp = comp.grp,
                    order = comp.order


                });

            }
            ViewBag.Compliance = viewModel;
        }
       

//***********************************end assigned types

//-----------------------------------------end functions---------------------------------------------------------------------------------------------------------------------------------------
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
