using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı username kontrolü 
            //kullanıcı e-posta kontolü 
            //kayıt işlemi 
            //akitvasyon e-postaso gönderimi


            EvernoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);//girilen kullanıcı adı ve email ile kayıtlı hesap varsa user değişkenine kaydediliyor
            BusinessLayerResult<EvernoteUser> layerresult = new BusinessLayerResult<EvernoteUser>();

            if (user != null)
            {
                //girilen kullanıcı adı ve email den hangisinin kayıtlı olduğunu ve hata mesajlarını layer.result.errors e kayıt ediliyor
                if (user.Username == data.Username)
                {
                    layerresult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.EMail)
                {
                    layerresult.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı");

                }
            }
            else
            {
                //girilen kullanıcı adı ve email ile kayıtlı kullanıcı yoksa girilen bilgileri veri tabanına kayıt ediliyor
                int dbResult = repo_user.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });
                if (dbResult >= 1)//kayıt başarılı ise if in içerisine girecek
                {
                    //kayıt edilen kullanıcı veri tabanından çekilip layer.result a kaydedilir.
                    layerresult.Result = repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);

                    //aktivasyon maili atılacak
                    //layerresult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerresult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerresult.Result.Username};<br/><br/>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerresult.Result.Email, "MyEvernote Hesap Aktifleştirme");

                }
            }
            return layerresult;

        }


        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //giriş kontrolü ..
            //hesap aktive edilmiş mi?

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);//girilen kullanıcı adı ve email ile kayıtlı hesap varsa user değişkenine kaydediliyor


            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-postanızı kontrol ediniz");

                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor");
            }

            return res;
        }


        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid == id);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActivate, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                else
                {
                    res.Result.IsActive = true;
                    repo_user.Update(res.Result);
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return res;
        }

    }
}
