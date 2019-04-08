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
    }
}
