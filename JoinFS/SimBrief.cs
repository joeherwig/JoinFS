using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JoinFS
{
    /// <summary>
    /// SimBrief OFP import - authoritative flight-plan source when a username is configured,
    /// since it already separates callsign/registration/route cleanly (unlike SimConnect)
    /// </summary>
    public static class SimBrief
    {
        static readonly HttpClient httpClient = new();

        /// <summary>
        /// Fetch the given pilot's latest SimBrief OFP and apply it to the flight plan
        /// </summary>
        /// <returns>true if a usable OFP was found and applied</returns>
        public static async Task<bool> FetchAsync(string username, Sim.FlightPlan plan)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            try
            {
                string url = "https://www.simbrief.com/api/xml.fetcher.php?username=" + Uri.EscapeDataString(username) + "&json=1";
                string json = await httpClient.GetStringAsync(url);
                JObject ofp = JObject.Parse(json);

                string departure = ofp["origin"]?["icao_code"]?.ToString() ?? "";
                string destination = ofp["destination"]?["icao_code"]?.ToString() ?? "";
                // SimBrief can return a 200 with an empty/error payload (no origin/destination) when
                // the username has no active OFP - treat that the same as a failed fetch
                if (string.IsNullOrEmpty(departure) || string.IsNullOrEmpty(destination))
                {
                    return false;
                }

                plan.callsign = ofp["atc"]?["callsign"]?.ToString() ?? "";
                plan.registration = ofp["aircraft"]?["reg"]?.ToString() ?? "";
                plan.icaoType = ofp["aircraft"]?["icaocode"]?.ToString() ?? "";
                plan.departure = departure;
                plan.destination = destination;
                plan.alternate = ofp["alternate"]?["icao_code"]?.ToString() ?? "";
                plan.route = ofp["atc"]?["route"]?.ToString() ?? "";
                plan.remarks = ofp["atc"]?["section18"]?.ToString() ?? "";
                plan.rules = (ofp["atc"]?["flight_rules"]?.ToString() ?? "I").Equals("V", StringComparison.OrdinalIgnoreCase) ? "VFR" : "IFR";
                plan.altitude = ofp["general"]?["initial_altitude"]?.ToString() ?? "";

                return true;
            }
            catch
            {
                // no username configured, request/network error, or malformed response - treat as "not available"
                return false;
            }
        }
    }
}
