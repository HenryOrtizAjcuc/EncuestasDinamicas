﻿using Acme.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace Acme.Controllers
{
    public class EncuestasController : Controller
    {
        private AcmeContext db = new AcmeContext();

        // GET: Encuestas
        public ActionResult Index()
        {
            var campos = db.Campos.ToList();
            var encuestas = db.Encuestas.ToList();
            return View(encuestas);
        }

        // GET: Encuestas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            var campos = db.Campos.Where(x => x.EncuestaId == id);

            foreach (Campo item in campos)
            {
                encuesta.Campos.Add(item);
            }

            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        public async Task<ActionResult> MostrarEncuesta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            var campos = db.Campos.Where(x => x.EncuestaId == id);
            foreach (Campo campo in campos)
            {
                encuesta.Campos.Add(campo);
            }

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
                return RedirectToAction($"Details/{encuesta.Id}");
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

        public async Task<ActionResult> CreateUrl(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (String.IsNullOrEmpty(encuesta.Url))
            {
                encuesta.Url = new Guid().ToString();
                db.Entry(encuesta).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public ActionResult AgregarCampo(int id)
        {
            Campo campo = new Campo();
            campo.EncuestaId = id;
            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarCampo([Bind(Include = "Id,Nombre,Titulo,EsRequerido,TipoCampo,EncuestaId")] Campo campo)
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
