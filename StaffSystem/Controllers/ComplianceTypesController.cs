﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StaffSystem.Models;

namespace StaffSystem.Controllers
{
    public class ComplianceTypesController : Controller
    {
        private dbStaffSystem db = new dbStaffSystem();

        // GET: ComplianceTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ComplianceTypes.ToListAsync());
        }

        // GET: ComplianceTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplianceTypes complianceTypes = await db.ComplianceTypes.FindAsync(id);
            if (complianceTypes == null)
            {
                return HttpNotFound();
            }
            return View(complianceTypes);
        }

        // GET: ComplianceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComplianceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title")] ComplianceTypes complianceTypes)
        {
            if (ModelState.IsValid)
            {
                db.ComplianceTypes.Add(complianceTypes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(complianceTypes);
        }

        // GET: ComplianceTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplianceTypes complianceTypes = await db.ComplianceTypes.FindAsync(id);
            if (complianceTypes == null)
            {
                return HttpNotFound();
            }
            return View(complianceTypes);
        }

        // POST: ComplianceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title")] ComplianceTypes complianceTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(complianceTypes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(complianceTypes);
        }

        // GET: ComplianceTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplianceTypes complianceTypes = await db.ComplianceTypes.FindAsync(id);
            if (complianceTypes == null)
            {
                return HttpNotFound();
            }
            return View(complianceTypes);
        }

        // POST: ComplianceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ComplianceTypes complianceTypes = await db.ComplianceTypes.FindAsync(id);
            db.ComplianceTypes.Remove(complianceTypes);
            await db.SaveChangesAsync();
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
