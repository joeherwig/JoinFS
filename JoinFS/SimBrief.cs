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
        /// <param name="main">optional, used only to log DIAG detail about the fetch</param>
        /// <returns>true if a usable OFP was found and applied</returns>
        public static async Task<bool> FetchAsync(string username, Sim.FlightPlan plan, Main main = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                main?.MonitorEvent("DIAG SimBrief: no username configured");
                return false;
            }

            try
            {
                string url = "https://www.simbrief.com/api/xml.fetcher.php?username=" + Uri.EscapeDataString(username) + "&json=1";
                main?.MonitorEvent("DIAG SimBrief: fetching " + url);
                string json = await httpClient.GetStringAsync(url);
                main?.MonitorEvent("DIAG SimBrief: received " + json.Length + " bytes");
                JObject ofp = JObject.Parse(json);

                string departure = (ofp["origin"]?["icao_code"]?.ToString() ?? "").ToUpperInvariant();
                string destination = (ofp["destination"]?["icao_code"]?.ToString() ?? "").ToUpperInvariant();
                main?.MonitorEvent("DIAG SimBrief: origin.icao_code='" + departure + "' destination.icao_code='" + destination + "'");

                // SimBrief can return a 200 with an empty/error payload (no origin/destination) when
                // the username has no active OFP - treat that the same as a failed fetch
                if (string.IsNullOrEmpty(departure) || string.IsNullOrEmpty(destination))
                {
                    string fetchStatus = ofp["fetch"]?["status"]?.ToString() ?? "";
                    main?.MonitorEvent("DIAG SimBrief: no origin/destination in response - fetch.status='" + fetchStatus + "'");
                    return false;
                }

                plan.callsign = ofp["atc"]?["callsign"]?.ToString() ?? "";
                plan.registration = ofp["aircraft"]?["reg"]?.ToString() ?? "";
                plan.icaoType = ofp["aircraft"]?["icaocode"]?.ToString() ?? "";
                plan.departure = departure;
                plan.destination = destination;
                plan.alternate = (ofp["alternate"]?["icao_code"]?.ToString() ?? "").ToUpperInvariant();
                plan.route = ofp["atc"]?["route"]?.ToString() ?? "";
                plan.remarks = ofp["atc"]?["section18"]?.ToString() ?? "";
                plan.rules = (ofp["atc"]?["flight_rules"]?.ToString() ?? "I").Equals("V", StringComparison.OrdinalIgnoreCase) ? "VFR" : "IFR";
                plan.altitude = ofp["general"]?["initial_altitude"]?.ToString() ?? "";

                return true;
            }
            catch (Exception ex)
            {
                // no username configured, request/network error, or malformed response - treat as "not available"
                main?.MonitorEvent("DIAG SimBrief: fetch failed - " + ex.Message);
                return false;
            }
        }
    }
}
