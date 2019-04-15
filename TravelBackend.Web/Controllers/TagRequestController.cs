 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelBackend.Models;
using TravelBackend.Services;

namespace TravelBackend.Web.Controllers
{
    public class TagRequestController : ApiController
    {
        private TagRequestService CreateTagRequestService() => new TagRequestService();

        //Get List
        public IHttpActionResult Get()
        {
            var svc = CreateTagRequestService();
            var model = svc.GetTagRequests(e => true);
            return Ok(model);
        }

        //Get By ID
        public IHttpActionResult Get(Guid id)
        {
            var svc = CreateTagRequestService();
            var model = svc.GetSingleTagRequest(e => e.TagRequestId == id);
            return Ok(model);
        }

        // Create
        public IHttpActionResult Post(TagRequestCreate model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagRequestService();
            Guid newId;
            return svc.CreateTagRequest(model, out newId) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully created Tag Request \"" + model.TagRequestName + "\" (" + newId + ")" }) : (IHttpActionResult)InternalServerError(new Exception("Error Creating Tag Request"));
        }

        //Edit
        public IHttpActionResult Post(Guid id, TagRequestEdit model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreateTagRequestService();
            model.TagRequestId = id;
            return svc.UpdateTagRequest(model) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully updated Tag Request" }) : (IHttpActionResult)InternalServerError(new Exception("Error Updating Tag Request"));
        }

        //Delete
        public IHttpActionResult Delete(Guid id)
        {
            var svc = CreateTagRequestService();
            return svc.DeleteTagRequest(id) ? (IHttpActionResult)Ok(new { success = true, message = "Successfully deleted Tag Request" + id + "!" }) : (IHttpActionResult)InternalServerError(new Exception("Error Deleting Tag Request"));
        }
    }
}
