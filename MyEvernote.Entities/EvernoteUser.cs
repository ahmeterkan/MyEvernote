using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [StringLength(25, ErrorMessage = "{0} alanı max. {1} karakte olmalıdır."), DisplayName("Ad")]
        public string Name { get; set; }

        [StringLength(25, ErrorMessage = "{0} alanı max. {1} karakte olmalıdır."), DisplayName("Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakte olmalıdır."), DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakte olmalıdır."), DisplayName("E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakte olmalıdır."), DisplayName("Şifre")]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }


        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }



        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }


    }
}
