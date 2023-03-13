using Data.Entities;

namespace Extensions.MyExtentions
{
    public static class CarFilter
    {
        public static IQueryable<T> DistanceFilter<T>(this IQueryable<T> source, double lat, double lon) where T : Car
        {
            return source.Where(car => car.Location != null ?
            6371 * Math.Acos(Math.Sin(lat * Math.PI / 180) *
            Math.Sin(car.Location.Latitude * Math.PI / 180) + Math.Cos(lat * Math.PI / 180) *
            Math.Cos(car.Location.Latitude *
            Math.PI / 180) * Math.Cos((lon - car.Location.Longitude) *
            Math.PI / 180)) <= 5 : true);
        }
    }
}
