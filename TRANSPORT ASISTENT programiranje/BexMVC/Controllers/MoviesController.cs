using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Demos.Club.Common;
using Demos.Club.DAL.EF.UOW;
using Demos.Club.Models;
using Demos.Club.MVC.Common;
using Demos.Club.MVC.Exceptions;

namespace Demos.ClubMVC.Controllers
{
    public class MoviesController : Controller
    {
        public MoviesController() : this(new ClubUow(), new ExceptionSolver())
        { }
        public MoviesController(
            IClubUow clubUow,
            IExceptionSolver exceptionSolver)
        {
            ClubUow = clubUow;
            ExceptionSolver = exceptionSolver;
        }

        // GET: /Movies/
        public ActionResult Index()
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            var moviesQuery = ClubUow.Movies.AllAsNoTracking;

            return View(moviesQuery.ToList());
        }

        // GET: /Movies/Details/1
        public ActionResult Details(int? id)
        {
            TempData.ToList().ForEach(keyValue =>
                ModelState.AddModelError("", keyValue.Value.ToString()));
            TempData.Clear();

            if (id == null)
            {
                TempData["ModelError_Index"] = "Bad request. Correct request needs value for id";
                return RedirectToAction("Index");
            }

            var movie = ClubUow.Movies.Find(m => m.Id == id, m => m.Binary);
            if (movie == null)
            {
                TempData["ModelError_Index"] = $"Movie with Id = {id} is not found.";
                return RedirectToAction("Index");
            }

            var loadedPosts = ClubUow.Posts
                .GetAll(p => p.MovieId == id).ToList();
            movie.Posts = loadedPosts;

            return View(movie);
        }

        public ActionResult Create() =>
            View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                ClubUow.Movies.Add(movie);

                var commandResult = ClubUow.SubmitChanges();

                if (commandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = movie.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, commandResult);
            }
            else
            { ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }

            return View(movie);
        }

        // GET: /Movies/Edit/1
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["ModelError_Index"] = "Bad request. Correct request needs value for id";
                return RedirectToAction("Index");
            }

            var movie = ClubUow.Movies.Find(id);
            if (movie == null)
            {
                TempData["ModelError_Index"] = $"Movie with Id = {id} is not found.";
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                ClubUow.Movies.Update(movie);

                var uowCommandResult = ClubUow.SubmitChanges();

                if (uowCommandResult.IsSuccessful)
                { return RedirectToAction("Details", new { id = movie.Id }); }

                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
            }
            else
            { ModelState.AddModelError("", "MVC_Handled_ModelValidation"); }

            return View(movie);
        }

        // GET: /Movies/Delete/1
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["ModelError_Index"] = "Bad request. Correct request needs value for id";
                return RedirectToAction("Index");
            }

            var movie = ClubUow.Movies.Find(id);
            if (movie == null)
            {
                TempData["ModelError_Index"] = $"Movie with Id = {id} is not found.";
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubUow.Posts.Remove(
                p => p.MovieId == id,
                p => p.Comments);
            ClubUow.Movies.Remove(m => m.Id == id);

            var uowCommandResult = ClubUow.SubmitChanges();

            if (!uowCommandResult.IsSuccessful)
            {
                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                ExceptionSolver.PrepareTempData(TempData, ModelState);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { ClubUow.Dispose(); }
            base.Dispose(disposing);
        }

        private IExceptionSolver ExceptionSolver { get; }
        private IClubUow ClubUow { get; }
    }
}
