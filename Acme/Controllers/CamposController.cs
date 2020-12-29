using Acme.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Acme.Controllers
{
    [Authorize(Roles = "1,2")]
    public class CamposController : Controller
    {
        private AcmeContext db = new AcmeContext();

        // GET: Campos
        public async Task<ActionResult> Index()
        {
            var campos = db.Campos.Include(c => c.encuesta);
            return View(await campos.ToListAsync());
        }

        // GET: Campos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = await db.Campos.FindAsync(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            return View(campo);
        }

        // GET: Campos/Create
        public ActionResult Create()
        {
            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre");
            return View();
        }

        // POST: Campos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Titulo,EsRequerido,TipoCampo,EncuestaId")] Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Campos.Add(campo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        // GET: Campos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = await db.Campos.FindAsync(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        // POST: Campos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Titulo,EsRequerido,TipoCampo,EncuestaId")] Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        // GET: Campos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = await db.Campos.FindAsync(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            return View(campo);
        }

        // POST: Campos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Campo campo = await db.Campos.FindAsync(id);
            db.Campos.Remove(campo);
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
