using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Acme.Models;

namespace Acme.Controllers
{
    public class EncuestasController : Controller
    {
        private AcmeContext db = new AcmeContext();

        // GET: Encuestas
        public async Task<ActionResult> Index()
        {
            return View(await db.Encuestas.ToListAsync());
        }

        // GET: Encuestas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        // GET: Encuestas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Encuestas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Descripcion")] Encuesta encuesta)
        {
            if (ModelState.IsValid)
            {
                db.Encuestas.Add(encuesta);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(encuesta);
        }

        // GET: Encuestas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        // POST: Encuestas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Descripcion")] Encuesta encuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encuesta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(encuesta);
        }

        // GET: Encuestas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        // POST: Encuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            db.Encuestas.Remove(encuesta);
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
