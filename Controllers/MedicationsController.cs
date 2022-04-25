using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Samples.IntegrationLayer.Models;

namespace Samples.IntegrationLayer.Controllers
{
    public class MedicationsController : Controller
    {
        private readonly MedicationDBContext db = new MedicationDBContext();

        // GET: Medications
        public ActionResult Index()
        {
            return View(db.Medications.ToList());
        }

        // GET: Medications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var medication = db.Medications.Find(id);
            if (medication == null) return HttpNotFound();
            return View(medication);
        }

        // GET: Medications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicationId,MemberId,Name,Expiry")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                medication.MedicationId = Guid.NewGuid();
                db.Medications.Add(medication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medication);
        }

        // GET: Medications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var medication = db.Medications.Find(id);
            if (medication == null) return HttpNotFound();
            return View(medication);
        }

        // POST: Medications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicationId,MemberId,Name,Expiry")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medication);
        }

        // GET: Medications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var medication = db.Medications.Find(id);
            if (medication == null) return HttpNotFound();
            return View(medication);
        }

        // POST: Medications/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var medication = db.Medications.Find(id);
            db.Medications.Remove(medication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}