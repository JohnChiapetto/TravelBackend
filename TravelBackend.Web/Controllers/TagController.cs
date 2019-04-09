using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TravelBackend.Web.Controllers
{
    public class TagController : ApiController
    {
        private TagService CreateTagService() => new TagService();

        //Get List
        public IHttpActionResult Get()
        {
            var svc = CreateTagService();
            var model = svc.GetTags(e => true);
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
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagService();
            Guid newId;
            return svc.CreateTag(model, out newId) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully created tag \"" + model.TagName + "\" (" + newId + ")" }) : (IHttpActionResult)InternalServerError(new Exception("Error Creating Tag"));
        }

        // Edit
        public IHttpActionResult Post(Guid id, TagEdit model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagService();
            return svc.UpdateTag(model) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully updated place" }) : (IHttpActionResult)InternalServerError(new Exception("Error Updating Tag"));
        }

        // Delete
        public IHttpActionResult Delete(Guid id)
        {
            var svc = CreateTagService();
            return svc.DeleteTag(id) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully deleted tag " + id + "!" }) : (IHttpActionResult)InternalServerError(new Exception("Error Deleting Tag"));
        }
    }
}
