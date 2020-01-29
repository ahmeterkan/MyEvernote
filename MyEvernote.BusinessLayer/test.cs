using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class test
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment= new Repository<Comment>();
        private Repository<Note> repo_note= new Repository<Note>();

        public test()
        {
            List<Category> categories = repo_category.List();
            List<Category> categories_filtered = repo_category.List(x => x.Id > 5);
        }

        public void InsertTest()
        {

            int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "aaa",
                Surname = "bb",
                Email = "ahmeterkan48@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "aabb",
                Password = "111",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "aabb"
            });
        }

        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "aabb");
            if (user != null)
            {
                user.Username = "xxx";
                int result = repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "xxx");
            if (user != null)
            {
                int result = repo_user.Delete(user);
            }
        }


        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);
            Comment comment = new Comment()
            {
                Text = "Bu birtestdir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName="ahmeterkan",
                Note = note,
                Owner = user
            };

            repo_comment.Insert(comment);
        }
    }
}
