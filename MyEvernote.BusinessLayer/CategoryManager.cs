using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        NoteManager noteManager = new NoteManager();
        LikedManager likedManager = new LikedManager();
        CommentManager commentManager = new CommentManager();

        // kategori ile ilişkili notlaroon silinmesi gerekiyor
        public override int Delete(Category category)
        {
            foreach (Note note in category.Notes.ToList())
            {
                //Note ile ilişkili likeların silinmesi.
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                // note ile ilişkili commentlerin seilinmesi
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }

            return base.Delete(category);
        }
    }
}
