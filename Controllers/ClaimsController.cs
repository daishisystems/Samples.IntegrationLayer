using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Samples.IntegrationLayer.Models;

namespace Samples.IntegrationLayer.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ClaimDBContext db = new ClaimDBContext();

        // GET: Claims
        public ActionResult Index()
        {
            return View(db.Claims.ToList());
        }

        // GET: Claims/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimId,MemberId,IssuedDate,Description")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                claim.ClaimId = Guid.NewGuid();
                db.Claims.Add(claim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claim);
        }

        // GET: Claims/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimId,MemberId,IssuedDate,Description")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claim);
        }

        // GET: Claims/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var claim = db.Claims.Find(id);
            if (claim == null) return HttpNotFound();
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
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