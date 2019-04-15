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
    public class PlaceService : AbstractService
    {
        public PlaceService() : base() { }
        public PlaceService(Guid id) : base(id) { }

        public Place GetPlaceById(Guid id) => GetSinglePlace(e => e.PlaceId == id);
        public Place GetSinglePlace(Expression<Func<Place,bool>> x) => GetPlaces(x)[0];
        public Place[] GetPlaces(Expression<Func<Place,bool>> x) => Context.Places.Where(x).ToArray();
        public bool CreatePlace(PlaceCreate model)
        {
            Context.Places.Add(
                new Place
                {
                    PlaceName = model.PlaceName,
                    PlaceDescription = model.PlaceDescription,
                    PlaceImageUrl = model.PlaceImageUrl,
                    PlaceLocation = model.PlaceLocation,
                    SubmittedUTC = DateTimeOffset.Now,
                    SubmittingUserId = _userId
                }
            );
            return Context.SaveChanges() == 1;
        }
        public bool CreatePlace(PlaceCreate model,out Guid id)
        {
            var p = new Place
            {
                PlaceName = model.PlaceName,
                PlaceDescription = model.PlaceDescription,
                PlaceImageUrl = model.PlaceImageUrl,
                PlaceLocation = model.PlaceLocation,
                SubmittedUTC = DateTimeOffset.Now,
                SubmittingUserId = _userId
            };
            Context.Places.Add(p);
            id = p.PlaceId;
            return Context.SaveChanges() == 1;
        }

        public bool UpdatePlace(PlaceEdit model)
        {
            var ent = GetPlaceById(model.PlaceId);
            ent.PlaceName = model.PlaceName;
            ent.PlaceDescription = model.PlaceDescription;
            ent.PlaceImageUrl = model.PlaceImageUrl;
            ent.PlaceLocation = model.PlaceLocation;
            ent.ModifiedUTC = DateTimeOffset.Now;
            ent.ModifyingUserId = _userId;
            return Context.SaveChanges() != 0;
        }

        public bool DeletePlace(Guid id)
        {
            Context.Places.Remove(GetPlaceById(id));
            return Math.Abs(Context.SaveChanges()) == 1;
        }
    }
}
