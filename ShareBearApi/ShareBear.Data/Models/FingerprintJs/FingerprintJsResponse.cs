using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Models.FingerprintJs
{
    public class BrowserDetails
    {
        public string browserName { get; set; }
        public string browserMajorVersion { get; set; }
        public string browserFullVersion { get; set; }
        public string os { get; set; }
        public string osVersion { get; set; }
        public string device { get; set; }
        public string userAgent { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    public class Confidence
    {
        public double score { get; set; }
    }

    public class Continent
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class FirstSeenAt
    {
        public string global { get; set; }
        public string subscription { get; set; }
    }

    public class IpLocation
    {
        public int accuracyRadius { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string postalCode { get; set; }
        public string timezone { get; set; }
        public City city { get; set; }
        public Continent continent { get; set; }
        public Country country { get; set; }
        public List<Subdivision> subdivisions { get; set; }
    }

    public class LastSeenAt
    {
        public string global { get; set; }
        public object subscription { get; set; }
    }

    public class FingerprintJsResponse
    {
        public string visitorId { get; set; }
        public List<Visit> visits { get; set; }
        public long lastTimestamp { get; set; }
    }

    public class Subdivision
    {
        public string isoCode { get; set; }
        public string name { get; set; }
    }

    public class Visit
    {
        public string requestId { get; set; }
        public bool incognito { get; set; }
        public string linkedId { get; set; }
        public string time { get; set; }
        public long timestamp { get; set; }
        public string url { get; set; }
        public string ip { get; set; }
        public IpLocation ipLocation { get; set; }
        public BrowserDetails browserDetails { get; set; }
        public Confidence confidence { get; set; }
        public bool visitorFound { get; set; }
        public FirstSeenAt firstSeenAt { get; set; }
        public LastSeenAt lastSeenAt { get; set; }
    }


}
