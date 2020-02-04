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
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [Required(ErrorMessage ="{0} alanı gereklidir."), StringLength(50, ErrorMessage ="{0} alanı maz. {1} karakter içermeli."), DisplayName("Başlık")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessage = "{0} alanı maz. {1} karakter içermeli."), DisplayName("Açıklama")]
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();
        }

    }
}
