using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelBackend.Data;

namespace TravelBackend.Services
{
    public class TagPlaceService : AbstractService
    {
        public TagPlaceService() : base() { }

        public TagPlace[] GetTagPlaces(Expression<Func<TagPlace,bool>> x) => Context.TagPlaces.Where(x).ToArray();
        public TagPlace GetSingleTagPlace(Expression<Func<TagPlace,bool>> x) => GetTagPlaces(x)[0];

        public Place[] GetPlacesWithTag(Guid tagId)
        {
            var k = GetTagPlaces(e=>e.TagId==tagId);
            List<Place> places = new List<Place>();
            var psvc = new PlaceService();
            foreach (var g in k) places.Add(psvc.GetPlaceById(g.PlaceId));
            return places.ToArray();
        }
        public Tag[] GetTagsOfPlace(Guid placeId)
        {
            var p = GetTagPlaces(e => e.PlaceId == placeId);
            List<Tag> tags = new List<Tag>();
            var tsvc = new TagService();
            foreach (var g in p) tags.Add(tsvc.GetTagById(g.TagId));
            return tags.ToArray();
        }
        public bool Link(Guid placeId,Guid tagId)
        {
            var ent = new TagPlace
            {
                TagPlaceId = Guid.NewGuid(),
                PlaceId = placeId,
                TagId = tagId
            };
            Context.TagPlaces.Add(ent);
            return Context.SaveChanges() > 0;
        }
        public bool UnLink(Guid placeId,Guid tagId)
        {
            var ents = GetTagPlaces(e => e.PlaceId == placeId && e.TagId == tagId);
            foreach (var ent in ents) Context.TagPlaces.Remove(ent);
            return Context.SaveChanges() != 0;
        }
        public bool AreLinked(Guid placeId,Guid tagId) => GetTagPlaces(e => e.PlaceId == placeId && e.TagId == tagId).Length > 0;
    }
}
