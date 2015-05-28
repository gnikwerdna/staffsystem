using System;
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
    public class StaffTypesController : Controller
    {
        private dbStaffSystem db = new dbStaffSystem();

        // GET: StaffTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.StaffType.ToListAsync());
        }

        // GET: StaffTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffType staffType = await db.StaffType.FindAsync(id);
            if (staffType == null)
            {
                return HttpNotFound();
            }
            return View(staffType);
        }

        // GET: StaffTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TypeDesc")] StaffType staffType)
        {
            if (ModelState.IsValid)
            {
                db.StaffType.Add(staffType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(staffType);
        }

        // GET: StaffTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffType staffType = await db.StaffType.FindAsync(id);
            if (staffType == null)
            {
                return HttpNotFound();
            }
            return View(staffType);
        }

        // POST: StaffTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TypeDesc")] StaffType staffType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(staffType);
        }

        // GET: StaffTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffType staffType = await db.StaffType.FindAsync(id);
            if (staffType == null)
            {
                return HttpNotFound();
            }
            return View(staffType);
        }

        // POST: StaffTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StaffType staffType = await db.StaffType.FindAsync(id);
            db.StaffType.Remove(staffType);
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
