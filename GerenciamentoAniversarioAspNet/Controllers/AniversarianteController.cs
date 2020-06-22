using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoAniversarioAspNet.Models;
using GerenciamentoAniversarioAspNet.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoAniversarioAspNet.Controllers
{
    public class AniversarianteController : Controller
    {
        private AniversarianteRepository AniversarianteRepository { get; set; }
        public AniversarianteController(AniversarianteRepository aniversarianteRepository)
        {
            this.AniversarianteRepository = aniversarianteRepository;
        }

        // GET: AniversarianteController
        public ActionResult Index()
        {
            var model = this.AniversarianteRepository.GetAll();
            return View(model);
        }

        // GET: AniversarianteController/Details/5
        public ActionResult Details(int id)
        {
            var model = this.AniversarianteRepository.GetAniversarianteById(id);
            return View(model);
        }

        // GET: AniversarianteController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search([FromQuery] string query)
        {
            var model = this.AniversarianteRepository.Search(query);
            return View("Index", model);
        }


        // POST: AniversarianteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aniversariante aniversariante)
        {
            try
            {
                this.AniversarianteRepository.Save(aniversariante);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AniversarianteController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = this.AniversarianteRepository.GetAniversarianteById(id);
            return View(model);
        }

        // POST: AniversarianteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Aniversariante model)
        {
            try
            {
                model.Id = id;
                this.AniversarianteRepository.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AniversarianteController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = this.AniversarianteRepository.GetAniversarianteById(id);
            return View(model);
        }

        // POST: AniversarianteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Aniversariante model)
        {
            try
            {
                model.Id = id;
                this.AniversarianteRepository.Delete(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
