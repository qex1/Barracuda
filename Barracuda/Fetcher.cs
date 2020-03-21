using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Barracuda
{
    public class Fetcher
    {
        private Cacher DronCacher, CarCacher;
        private Coord[] _coord;
        private static String BaseUrl = "http://routes.maps.sputnik.ru/osrm/router/viaroute?loc={0},{1}&loc={2},{3}&alt=false&geometry=false";

        public Fetcher(int n, IEnumerable<Coord> list)
        {
            DronCacher = new Cacher();
            CarCacher = new Cacher();
            _coord = new Coord[n];
            int i = 0;
            foreach (var e in list)
            {
                _coord[i] = e;
                ++i;
            }
        }

        private double MakeRequest(Coord a, Coord b, bool car)
        {
            if (car)
            {
                string t = String.Format(BaseUrl, a.Latitude, a.Longtitude, b.Latitude, b.Longtitude);
                var request = WebRequest.Create(t);
                var response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                String resp = streamReader.ReadToEnd();
                var k = JsonSerializer.Deserialize<RootObject>(resp);
                return k.route_summary.total_distance;
            }
            else
            {
                double earth = 6372795;
                double lat1 = a.Latitude * Math.PI / 180.0;
                double lat2 = b.Latitude * Math.PI / 180.0;
                double long1 = a.Longtitude * Math.PI / 180.0;
                double long2 = b.Longtitude * Math.PI / 180.0;
                double cl1 = Math.Cos(lat1);
                double cl2 = Math.Cos(lat2);
                double sl1 = Math.Sin(lat1);
                double sl2 = Math.Sin(lat2);
                var delta = long2 - long1;
                var cdelta = Math.Cos(delta);
                var sdelta = Math.Sin(delta);

                double y = Math.Sqrt(Math.Pow(cl2 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
                double x = sl1 * sl2 + cl1 * cl2 * cdelta;

                var ad = Math.Atan2(y, x);
                return ad * earth;
            }
        }

        public double T1(int a, int b)  //car calculating
        {
            if (CarCacher.CheckPair(_coord[a], _coord[b]))
            {
                return CarCacher.GetDist(_coord[a], _coord[b]);
            }
            else
            {
                var d = MakeRequest(_coord[a], _coord[b], true);
                CarCacher.AddPair(_coord[a], _coord[b], d);
                return d;
            }
        }

        public double T2(int a, int b)  //dron calculating
        {
            if (DronCacher.CheckPair(_coord[a], _coord[b]))
            {
                return DronCacher.GetDist(_coord[a], _coord[b]);
            }
            else
            {
                var d = MakeRequest(_coord[a], _coord[b], false);
                DronCacher.AddPair(_coord[a], _coord[b], d);
                return d;
            }
        }

    }
}