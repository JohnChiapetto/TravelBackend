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
    public class PlaceController : ApiController
    {
        private PlaceService CreatePlaceService() => new PlaceService();

        // Get List
        public IHttpActionResult Get()
        {
            var svc = CreatePlaceService();
            var model = svc.GetPlaces(e => true);
            return Ok(model);
        }

        // Get By ID
        public IHttpActionResult Get(Guid id)
        {
            var svc = CreatePlaceService();
            var model = svc.GetSinglePlace(e => e.PlaceId == id);
            return Ok(model);
        }

        // Create
        public IHttpActionResult Post(PlaceCreate model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreatePlaceService();
            Guid newId;
            return svc.CreatePlace(model,out newId) ? (IHttpActionResult)Ok(new { success = true,message = "Successfully created place \"" + model.PlaceName + "\" ("+newId+")" }) : (IHttpActionResult)InternalServerError(new Exception("Error Creating Place"));
        }

        // Edit
        public IHttpActionResult Post(Guid id,PlaceEdit model)
        {
            if (!ModelState.IsValid) return InternalServerError(new Exception("Invalid Model"));
            var svc = CreatePlaceService();
            return svc.UpdatePlace(model) ? (IHttpActionResult)Ok(new { success=true,message="Successfully updated place" }) : (IHttpActionResult)InternalServerError(new Exception("Error Updating Place")) ;
        }

        // Delete
        public IHttpActionResult Delete(Guid id)
        {
            var svc = CreatePlaceService();
            return svc.DeletePlace(id) ? (IHttpActionResult)Ok(new { success = true,message = "Successfully deleted place " + id + "!" }) : (IHttpActionResult)InternalServerError(new Exception("Error Deleting Place"));
        }
    }
}
