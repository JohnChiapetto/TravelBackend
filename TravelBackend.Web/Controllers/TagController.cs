using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelBackend.Data;
using TravelBackend.Models;
using TravelBackend.Services;

namespace TravelBackend.Web.Controllers
{
    class TagAlphaSorter : IComparer<Tag>
    {
        public int Compare(Tag x, Tag y)
        {
            return x.TagName.CompareTo(y.TagName);
        }
    }

    static class ListExt
    {
        public static List<E> Sorted<E>(this List<E> list, IComparer<E> c)
        {
            list.Sort(c);
            return list;
        }
    }

    public class TagController : AbstractController
    {
        private TagService CreateTagService() => new TagService(GetUserId());

        //Get List
        public IHttpActionResult Get()
        {
            var svc = CreateTagService();
            var model = svc.GetTags(e => true).ToList().Sorted(new TagAlphaSorter());
            return Ok(model);
        }

        // Get By ID
        public IHttpActionResult Get(Guid id)
        {
            var svc = CreateTagService();
            var model = svc.GetSingleTag(e => e.TagId == id);
            return Ok(model);
        }

        // Create
        public IHttpActionResult Post(TagCreate model)
        {
            if (!(User.Identity.IsAuthenticated && IsUserAdmin)) return StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagService();
            Guid newId;
            return svc.CreateTag(model, out newId) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully created tag \"" + model.TagName + "\" (" + newId + ")" }) : (IHttpActionResult)InternalServerError(new Exception("Error Creating Tag"));
        }

        // Edit
        public IHttpActionResult Post(Guid id, TagEdit model)
        {
            if (!(User.Identity.IsAuthenticated && IsUserAdmin)) return StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            model.TagId = id;
            var svc = CreateTagService();
            return svc.UpdateTag(model) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully updated place" }) : (IHttpActionResult)InternalServerError(new Exception("Error Updating Tag"));
        }

        // Delete
        public IHttpActionResult Delete(Guid id)
        {
            if (!(User.Identity.IsAuthenticated && IsUserAdmin)) return StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagService();
            return svc.DeleteTag(id) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully deleted tag " + id + "!" }) : (IHttpActionResult)InternalServerError(new Exception("Error Deleting Tag"));
        }
    }
}
