using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;
using TravelBackend.Models;

namespace TravelBackend.Services
{
    public class TagRequestService : AbstractService
    {
        public TagRequestService() : base() { }
        public TagRequestService(Guid uid) : base(uid) { }

        public TagRequest GetTagRequestById(Guid id) => GetSingleTagRequest(e => e.TagRequestId == id);
        public TagRequest GetSingleTagRequest(Expression<Func<TagRequest,bool>> x) => GetTagRequests(x)[0];
        public TagRequest[] GetTagRequests(Expression<Func<TagRequest, bool>> x) => Context.TagRequests.Where(x).ToArray();
        Func<Func<Func<Func<Func<Task<Func<Func<string,bool>>>>>>>> GetFunc() => () => null;
        public bool CreateTagRequest(TagRequestCreate model)
        {
            var ent = new TagRequest
            {
                TagRequestName = model.TagRequestName,
                TagRequestPlace = model.TagRequestPlace,
                TagRequestDate = DateTimeOffset.Now,
                TagRequestUserId = _userId
            };
            var rsvc = CreateRoleService();
            Context.TagRequests.Add(ent);
            if (rsvc.IsUserInRole(_userId,Guid.Parse(rsvc.GetRole(e => e.Name == "Admin").Id)))
            {
                GrantTagRequest(ent.TagRequestId);
            }
            return Context.SaveChanges() != 0;
        }
        public bool GrantTagRequest(Guid id)
        {
            var model = GetTagRequestById(id);
            var ent = new Tag
            {
                TagId = Guid.NewGuid(),
                TagName = model.TagRequestName,
            };
            DeleteTagRequest(id);
            Context.Tags.Add(ent);
            return Context.SaveChanges() != 0;
        }
        public bool DenyTagRequest(Guid id) => DeleteTagRequest(id);
        public bool CreateTagRequest(TagRequestCreate model, out Guid id)
        {
            var psvc = new PlaceService();
            var t = new TagRequest
            {
                TagRequestName = model.TagRequestName,
                TagRequestPlace = model.TagRequestPlace,
                TagRequestDate = DateTimeOffset.Now,
                TagRequestUserId = _userId
            };
            Context.TagRequests.Add(t);
            id = t.TagRequestId;
            return Context.SaveChanges() == 1;
        }
        public bool UpdateTagRequest(TagRequestEdit model)
        {
            var ent = GetTagRequestById(model.TagRequestId);
            ent.TagRequestName = model.TagRequestName;
            ent.TagRequestPlace = model.TagRequestPlace;
            ent.TagRequestDate = DateTimeOffset.Now;
            ent.TagRequestUserId = _userId;
            return Context.SaveChanges() != 0;
        }
        public bool DeleteTagRequest(Guid id)
        {
            Context.TagRequests.Remove(GetTagRequestById(id));
            return Math.Abs(Context.SaveChanges()) == 1;
        }
    }
}
