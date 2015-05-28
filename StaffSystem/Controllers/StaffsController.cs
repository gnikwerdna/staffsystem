using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StaffSystem.ViewModels;
using System.Data.Entity.Infrastructure;

namespace StaffSystem.Models
{
    public class StaffsController : Controller
    {
        private dbStaffSystem db = new dbStaffSystem();

        // GET: Staffs
        public ActionResult Index()
        {
            return View(db.Staff.ToList());
        }

        // GET: Staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TypeId")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staff.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            PopulateAssignedComplianceType(staff);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }
        private void PopulateAssignedComplianceType(Staff staff)
        {
            //var allCompliancetypes = db.complianceitems;
            var allCompliancetypes = db.ComplianceItems
                .OrderBy(cl => cl.grp)
                .ThenBy(cl => cl.order)
               
                ;
            var stafftoupdate = db.Staff
                //    .Include(s => s.ComplianceItems)
                // .Where(s => s.Id ==Id)
                // .Single()
         ;



            var staffcompliance = new HashSet<int>(staff.ComplianceItems.Select(c => c.ComplianceID));
            var viewModel = new List<AssignedComplianceData>();

            foreach (var comp in allCompliancetypes)
            {
                viewModel.Add(new AssignedComplianceData
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
        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,TypeId")] Staff staff)
        public ActionResult Edit(int? id, string[] SelectedComp, string[] SelectedCompSubItem_1, string[] SelectedCompSubItem_2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stafftoupdate = db.Staff
                 .Include(s => s.ComplianceItems)
                 .Where(s => s.Id == id)
                 .Single();
            if (TryUpdateModel(stafftoupdate, "", new string[] { "Id", "Name", "TypeId" }))
            {
                try
                {
                    resetStaffCompliance(SelectedComp, stafftoupdate);
                    UpdateStaffCompliance(SelectedComp, stafftoupdate);
                    UpdateStaffCompliance(SelectedCompSubItem_1, stafftoupdate);
                    UpdateStaffCompliance(SelectedCompSubItem_2, stafftoupdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }

            PopulateAssignedComplianceType(stafftoupdate);
            return View(stafftoupdate);
        }
        private void resetStaffCompliance(string []selectComp, Staff staffToUpdate)
        {

            if (selectComp == null)
            {
                staffToUpdate.ComplianceItems = new List<ComplianceItems>();
                return;

            }


            var selectedCompHS = new HashSet<string>(selectComp);
            var staffCompliances = new HashSet<int>(staffToUpdate.ComplianceItems.Select(c => c.ComplianceID));
            foreach (var comp in db.ComplianceItems)
            {
                staffToUpdate.ComplianceItems.Remove(comp);
            }

        }
        private void UpdateStaffCompliance(string[] selectedComp, Staff staffToUpdate)
        {
            if (selectedComp == null)
            {
                staffToUpdate.ComplianceItems = new List<ComplianceItems>();
                return;

            }
            var selectedCompHS = new HashSet<string>(selectedComp);
            var staffCompliances = new HashSet<int>(staffToUpdate.ComplianceItems.Select(c => c.ComplianceID));
            foreach (var comp in db.ComplianceItems)
            {
                if (selectedComp.Contains(comp.ComplianceID.ToString()))
                {
                    if (!staffCompliances.Contains(comp.ComplianceID))
                    {
                        staffToUpdate.ComplianceItems.Add(comp);

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
        
        // GET: Staffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staff.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
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
