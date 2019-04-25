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
    public class AMap : Dictionary<string, dynamic>
    {
        public new dynamic this[string key]
        {
            get => base[key];
            set
            {
                if (ContainsKey(key)) base[key] = value;
                else Add(key, value);
            }
        }

        public AMap(object o)
        {
            foreach (var prop in o.GetType().GetProperties())
            {
                this[prop.Name] = prop.GetValue(o);
            }
        }

        // public static implicit operator AMap(object o) => new AMap(o);
    }

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

        public IHttpActionResult Put(TagListItem item)
        {
            var res = CreatePlaceService().GetPlacesWithTag(item.TagId);
            return Ok(res);
        }
        public IHttpActionResult Patch(Guid tagId)
        {
            var res = new TagPlaceService(GetUserId()).GetPlacesWithTag(tagId);
            return Ok(res);
        }
        //public IHttpActionResult Put(Guid[] tagIds) {
        //    var lsvc = new TagPlaceService(GetUserId());
        //    var m = lsvc.GetPlacesWithTags(tagIds);
        //    var psvc = new PlaceService(GetUserId());
        //    return Ok(m.Map(e=>psvc.ListItemOf(e)));
        //}

        // Create
        public IHttpActionResult Post(PlaceCreate model)
        {
            //if (!IsUserSignedIn) return this.StatusCode(HttpStatusCode.Forbidden);
            //if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreatePlaceService();
            Guid newId;
            if (svc.CreatePlace(model, out newId))
            {
                return Ok(new { success = true, message = "Successfully created place \"" + model.PlaceName + "\" (" + newId + ")" });
            }
            return InternalServerError(new Exception("Error Creating Place"));
        }

        // Edit
        public IHttpActionResult Post(Guid id,PlaceEdit model)
        {
            //if (!IsUserSignedIn) return this.StatusCode(HttpStatusCode.Forbidden);
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            model.PlaceId = id;
            var svc = CreatePlaceService();
            if (svc.UpdatePlace(model))
            {
                var modelx = new {
                    success = true,
                    message = "Successfully updated place"
                };
                return Ok(modelx);
            }
            return InternalServerError(new Exception("Error Updating Place")) ;
        }

        // Delete
        public IHttpActionResult Delete(Guid id)
        {
            //if (!IsUserSignedIn) return this.StatusCode(HttpStatusCode.Forbidden);
            var svc = CreatePlaceService();
            if (svc.DeletePlace(id))
            {
                var model = new {
                    success = true,
                    message = "Successfully deleted place " + id + "!"
                };
                return Ok(model);
            }
            return InternalServerError(new Exception("Error Deleting Place"));
        }
    }
}
