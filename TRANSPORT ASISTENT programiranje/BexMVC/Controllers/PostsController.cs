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
    public class PostsController : Controller
    {
        public PostsController() : this(new ClubUow(), new ExceptionSolver())
        { }
        public PostsController(
            IClubUow clubUow,
            IExceptionSolver exceptionSolver)
        {
            ClubUow = clubUow;
            ExceptionSolver = exceptionSolver;
        }

        public ActionResult Create(int id = 0) // this id is movieId
        {
            var postsCount = 0;
            var title = $"Title {postsCount + 1}";
            var post = new Post
            {
                MovieId = id,
                Title = title,
                CreatedDate = DateTime.Now.Date,
                Content = $"Content of {title}",
            };

            ClubUow.Posts.Add(post);

            var uowCommandResult = ClubUow.SubmitChanges();

            if (!uowCommandResult.IsSuccessful)
            {
                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                ExceptionSolver.PrepareTempData(TempData, ModelState);
            }

            return RedirectToAction("Details", "Movies", new { id = id });
        }

        public ActionResult Edit(int id = 0)
        {
            var post = ClubUow.Posts.Find(id);
            var postsCount = 0;
            post.Title = $"Title {postsCount} (Edited)";

            ClubUow.Posts.Update(post);

            var uowCommandResult = ClubUow.SubmitChanges();

            if (!uowCommandResult.IsSuccessful)
            {
                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                ExceptionSolver.PrepareTempData(TempData, ModelState);
            }

            return RedirectToAction("Details", "Movies", new { id = post.MovieId });
        }

        public ActionResult Delete(int id = 0, int movieId = 0, string rowVersionString = null)
        {
            var rowVersion = Convert.FromBase64String(rowVersionString);

            var post = new Post { Key = id, Title = "Anything", RowVersion = rowVersion };
            ClubUow.Posts.Remove(post);

            var uowCommandResult = ClubUow.SubmitChanges();

            if (!uowCommandResult.IsSuccessful)
            {
                ExceptionSolver.PrepareModelState(ModelState, uowCommandResult);
                ExceptionSolver.PrepareTempData(TempData, ModelState);
            }

            return RedirectToAction("Details", "Movies", new { id = movieId });
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
