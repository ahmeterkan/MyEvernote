using System;
using MyEvernote.BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using System.Net;
using MyEvernote.Entities.ValueObject;
using MyEvernote.Entities.Messages;
using MyEvernote.WebApp.ViewModels;

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
        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotfyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotfyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.GetUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            if (ProfileImage != null && (ProfileImage.ContentType == "image/jpeg" || ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/png"))
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                model.ProfileImageFilename = filename;
            }
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res =eum.UpdateProfile(model);

            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyOBj = new ErrorViewModel()
                {
                    Items=res.Errors,
                    Title= "Profile Güncellenemedi",
                    RedirectingUrl = "/Home/EditProfile"
                };
                return View("Error", errorNotifyOBj);
            }
            Session["login"] = res.Result;// Profile güncellendiği için session güncellendi.
            return RedirectToAction("ShowProfile");
        }
        public ActionResult DeleteProfile()
        {
            EvernoteUser currentuser = Session["login"] as EvernoteUser;
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.RemoveUserById(currentuser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
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

            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel notfyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notfyObj.Items.Add("Lütfen e-posta adresinize gönderidiğimiz aktivasyon linkine tıklayarak hasabınızı aktive ediniz.Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notfyObj);
            }


            return View(model);
        }


        //Ok
        public ActionResult UserActivate(Guid id)
        {

            //kullanıcı aktivasyonu sağlanacak
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotfObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                TempData["errors"] = res.Errors;
                return View("Error", errorNotfObj);
            }
            OkViewModel okNotfyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };
            okNotfyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok", okNotfyObj);
        }


        //Ok
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}