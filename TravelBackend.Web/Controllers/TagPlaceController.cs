using Microsoft.AspNet.Identity;
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
    public class TagPlaceController : ApiController
    {
        private TagPlaceService CreateTagePlaceService() => new TagPlaceService(Guid.Parse(User.Identity.GetUserId()));

        public IHttpActionResult Get() => Ok(CreateTagePlaceService().GetTagPlaces(e=>true));
        public IHttpActionResult Get(bool placesWithTag,Guid id)
        {
            var svc = CreateTagePlaceService();
            return ( placesWithTag ? (IHttpActionResult)Ok(svc.GetPlacesWithTag(id)) : (IHttpActionResult)Ok(svc.GetTagsOfPlace(id)) );
        }
        public IHttpActionResult Post(TagPlaceCreate model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model!"));
            if (CreateTagePlaceService().Link(model.PlaceId,model.TagId)) {
                return Ok(new { success=true,message="Successfully linked place to tag!" });
            }
            return InternalServerError(new Exception("Error saving link"));
        }
        public IHttpActionResult Delete(TagPlaceDelete model) {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model!"));
            if (CreateTagePlaceService().UnLink(model.PlaceId,model.TagId))
            {
                return Ok(new { success = true,message = "Successfully un-linked place from tag!" });
            }
            return InternalServerError(new Exception("Error removing link"));
        }
    }
}
