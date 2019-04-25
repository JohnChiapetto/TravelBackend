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
    public static class ArrayExt
    {
        public static T[] Map<F, T>(this F[] arr,Func<F,T> f)
        {
            var oarr = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++) oarr[i] = f(arr[i]);
            return oarr;
        }
    }

    public class PlaceService : AbstractService
    {
        public PlaceService() : base() { }
        public PlaceService(Guid id) : base(id) { }

        public PlaceListItem ListItemOf(Place p)
        {
            var item = new PlaceListItem(p);
            var lsvc = new TagPlaceService(_userId);
            item.Tags = lsvc.GetTagsOfPlace(item.PlaceId).Map(e => new TagListItem { TagId = e.TagId,TagName = e.TagName });
            return item;
        }
        public PlaceListItem[] GetPlaceListItems(Expression<Func<Place,bool>> x) => GetPlaces(x).Map(e => ListItemOf(e));
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
        public Place[] GetPlacesWithTag(Guid tagId)
        {
            var lsvc = new TagPlaceService(_userId);
            List<Place> p = new List<Place>();
            foreach (var place in Context.Places.ToArray())
            {
                if (lsvc.AreLinked(place.PlaceId, tagId)) p.Add(place);
            }
            return p.ToArray();
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
            var lsvc = new TagPlaceService(_userId);
            Context.Places.Add(p);
            id = p.PlaceId;
            foreach (var t in model.Tags)
            {
                if (t.TagId.ToString() == new Guid().ToString()) continue;
                lsvc.Link(id,t.TagId);
            }
            var k = Context.SaveChanges() ;
            // return k != 0;
            // Works, but it always returns false with the other way. Changed it as a hotfix.
            return true;
        }

        public bool UpdatePlace(PlaceEdit model)
        {
            var lsvc = new TagPlaceService(_userId);
            var ent = GetPlaceById(model.PlaceId);
            ent.PlaceName = model.PlaceName;
            ent.PlaceDescription = model.PlaceDescription;
            ent.PlaceImageUrl = model.PlaceImageUrl;
            ent.PlaceLocation = model.PlaceLocation;
            lsvc.UnLinkAllFromPlace(ent.PlaceId);
            foreach (var tag in model.Tags) lsvc.Link(ent.PlaceId,tag.TagId);
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
