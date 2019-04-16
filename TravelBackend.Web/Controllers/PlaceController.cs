using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TravelBackend.Models;
using TravelBackend.Services;

namespace TravelBackend.Web.Controllers
{
    public class PlaceController : AbstractController
    {
        private PlaceService CreatePlaceService() => new PlaceService(GetUserId());
        private TagPlaceService CreateLinkService() => new TagPlaceService(GetUserId());

        // Get List
        public IHttpActionResult Get()
        {
            var svc = CreatePlaceService();
            var model = svc.GetPlaceListItems(e => true);
            return Ok(model);
        }

        // Get By ID
        public IHttpActionResult Get(Guid id)
        {
            var tsvc = CreateLinkService();
            var svc = CreatePlaceService();
            var ent = svc.GetSinglePlace(e => e.PlaceId == id);
            var tagz = tsvc.GetTagsOfPlace(ent.PlaceId);
            var tags = new List<TagListItem>();
            foreach (var tag in tagz)
            {
                var tli = new TagListItem
                {
                    TagId = tag.TagId,
                    TagName = tag.TagName
                };
                tags.Add(tli);
            }
            var model = new PlaceDetails
            {
                PlaceId = ent.PlaceId,
                PlaceDescription = ent.PlaceDescription,
                PlaceImageUrl = ent.PlaceImageUrl,
                PlaceLocation = ent.PlaceLocation,
                PlaceName = ent.PlaceName,
                ModifiedUTC = ent.ModifiedUTC,
                ModifyingUserId = ent.ModifyingUserId,
                SubmittedUTC = ent.SubmittedUTC,
                SubmittingUserId = ent.SubmittingUserId,
                Tags = tags.ToArray()
            };
            return Ok(model);
        }

        // Create
        public IHttpActionResult Post(PlaceCreate model)
        {
            if (!User.Identity.IsAuthenticated) return this.StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreatePlaceService();
            Guid newId;
            return svc.CreatePlace(model,out newId) ? (IHttpActionResult)Ok(new { success = true,message = "Successfully created place \"" + model.PlaceName + "\" ("+newId+")" }) : (IHttpActionResult)InternalServerError(new Exception("Error Creating Place"));
        }

        // Edit
        public IHttpActionResult Post(Guid id,PlaceEdit model)
        {
            if (!User.Identity.IsAuthenticated) return this.StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            model.PlaceId = id;
            var svc = CreatePlaceService();
            return svc.UpdatePlace(model) ? (IHttpActionResult)Ok(new { success=true,message="Successfully updated place" }) : (IHttpActionResult)InternalServerError(new Exception("Error Updating Place")) ;
        }

        // Delete
        public IHttpActionResult Delete(Guid id)
        {
            if (!User.Identity.IsAuthenticated) return this.StatusCode(HttpStatusCode.Forbidden);
            var svc = CreatePlaceService();
            return svc.DeletePlace(id) ? (IHttpActionResult)Ok(new { success = true,message = "Successfully deleted place " + id + "!" }) : (IHttpActionResult)InternalServerError(new Exception("Error Deleting Place"));
        }
    }
}
