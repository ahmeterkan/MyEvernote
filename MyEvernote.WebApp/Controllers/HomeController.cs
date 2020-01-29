using System;
using MyEvernote.BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using System.Net;
using MyEvernote.Entities.ValueObject;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        //  Ok
        public ActionResult Index()
        {
            //test test = new test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();

            //test.CommentTest();


            //Başka bir  controller ile bir category deki notlaro listeleme yapmak istersek TempData kullanımı
            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();

            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        // Ok
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        //Ok
        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        //Ok
        public ActionResult About()
        {
            return View();
        }

        //Ok
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }
                else
                {
                    //sessiona kullnaıcı bilgi saklama
                    //yönlendirme..

                    Session["login"] = res.Result;
                    return RedirectToAction("Index");
                }
            }


            return View();
        }


        //Ok
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            // bu kısımı business layerda yazılacak
            //if (ModelState.IsValid)
            //{
            //    if (model.Username == "aaa")
            //    {
            //        ModelState.AddModelError("", "Kullanıcı adı kullanılıyor.");
            //    }
            //    if (model.EMail == "aaa@aa.com")
            //    {
            //        ModelState.AddModelError("", "E-posta adı kullanılıyor.");
            //    }
            //    foreach (var item in ModelState)
            //    {
            //        if (item.Value.Errors.Count > 0)
            //        {
            //            return View(model);
            //        }
            //    }

            //    return RedirectToAction("RegisterOk");
            //}


            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                return RedirectToAction("RegisterOk");
            }


            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }



        public ActionResult UserActivate(Guid activate_id)
        {

            //kullanıcı aktivasyonu sağlanacak
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}