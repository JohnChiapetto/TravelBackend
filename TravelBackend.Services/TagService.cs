using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;
using TravelBackend.Models;

namespace TravelBackend.Services
{
    public class TagService : AbstractService
    {
        public TagService() : base() { }
        public TagService(Guid id) : base(id) { }

        public Tag GetTagById(Guid id) => GetSingleTag(e => e.TagId == id);
        public Tag GetSingleTag(Expression<Func<Tag,bool>> x) => GetTags(x)[0];
        public Tag[] GetTags(Expression<Func<Tag,bool>> x) => Context.Tags.Where(x).ToArray();
        public bool CreateTag(TagCreate model)
        {
            Context.Tags.Add(
                new Tag
                {
                    TagId = Guid.NewGuid(),
                    TagName = model.TagName,
                }
            );
            return Context.SaveChanges() == 1;
        }
        public bool CreateTag(TagCreate model,out Guid id)
        {
            var t = new Tag
            {
                TagId = Guid.NewGuid(),
                TagName = model.TagName
            };
            Context.Tags.Add(t);
            id = t.TagId;
            return Context.SaveChanges() == 1;
        }
        public bool UpdateTag(TagEdit model)
        {
            var ent = GetTagById(model.TagId);
            ent.TagName = model.TagName;
            return Context.SaveChanges() != 0; 
        }
        public bool DeleteTag(Guid id)
        {
            Context.Tags.Remove(GetTagById(id));
            return Math.Abs(Context.SaveChanges()) == 1;
        }
    }
}
