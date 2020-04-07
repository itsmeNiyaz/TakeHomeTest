using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RubytoCsharp
{
    public class ChartRequest
    {
        private string method;
        private Dictionary<string, string> chart_params;
        protected Dictionary<string, string> DEFAULT;

        private readonly IHttpContextAccessor httpContextAccessor;


        IList<Facilities> Facility;
        IList<HealthSystems> healthSystem;

        public ChartRequest(string method, Dictionary<string, string> params1, IHttpContextAccessor httpContextAccessor)
        {
            this.method = method;
            this.chart_params = DEFAULT.Concat(params1)
                       .ToDictionary(x => x.Key, x => x.Value);
            this.httpContextAccessor = httpContextAccessor;
    
        }

        public Uri url() {
            string HostName = Dns.GetHostName();

          var RemoteAddress =  this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var Port = this.httpContextAccessor.HttpContext.Connection.RemotePort.ToString();
            return new Uri("http://"+ RemoteAddress + ":"+Port+"/"+chart_path()+"?"+query_params());

        }
        public void results_name()
        {
            chart_params["as"] = method;
        }
        public string query_params()
        {
        Dictionary<string,string>  params1 = new Dictionary<string, string>() ;
            string[] date_range = parse_date_range(chart_params["date_range"].ToString());
            
      params1["start_date"] = date_range[0];
      params1["end_date"] = date_range[0];

            chart_params["ed"] = chart_params["er"];

            if (chart_params["er"] != null) {

                foreach(var adKey in additional_keys())
                {
                  if(chart_params.Count >0)
                    {
                        params1[adKey] = "1";
                    }
                }
            
    }

            if (chart_params["inactive_facilities"]!=null)
                params1["inactive_facilities"] = chart_params["inactive_facilities"];

            if (chart_params["date_filter_type"] != null)
                params1["date_filter_type"] = chart_params["date_filter_type"];

            if (chart_params["exclude_cancel"] != null)
                params1["exclude_cancel"] = chart_params["exclude_cancel"];
            else params1["exclude_cancel"] = "off";



            foreach(var chartResources in chartable_resources())
            {
                var val = chart_params[chartResources];

                if (val.GetType().IsArray)
                {
                    params1[chartResources] = val + ",";
                } else params1[chartResources] = val;


    }

            var url = string.Format("{0}",HttpUtility.UrlEncode(string.Join("&", params1.Select
                (kvp =>string.Format("{0}={1}", kvp.Key, kvp.Value)))));

            return url;
        }

    public string[] parse_date_range(string str)
        {
            if (chart_params["start_date"] != null && chart_params["end_date"] != null) {

            return new string[2] { chart_params["start_date"], chart_params["end_date"] };
            }
            else {
                var dates = str.Split(" - ");
            return new string[2] { dates[0], dates[1] };

        }
    }

        protected string[] additional_keys()
        {
            return new string[5] {"mobile", "ed", "pcp", "ucc", "inactive" };
        }
        private  string[] chartable_resources()
        {
            return new string[6] { "health_systems", "facilities", "locations", "providers", "regions", " state" };
        }

        private string chart_path()
        {
            var facility = new Facilities();
            var health_system = new HealthSystems();
            if ( chart_params["facility_id"] != null)
            {
                 facility = Facility.Where(c => c.id == chart_params["facility_id"]).FirstOrDefault();
            }else if(chart_params["health_system_id"] != null)
            {
                health_system = healthSystem.Where(x => x.id == chart_params["health_system_id"]).FirstOrDefault();
            }

            if (facility != null)
            {
                return Uri.EscapeDataString("" + method + "/" + facility.HealthSystems.id + "/" + @facility.id);
            }
            else if(health_system != null)
            {
               return Uri.EscapeDataString("" + method + "/" + health_system.id );

            }
            else return Uri.EscapeDataString("" + method );
        }

    }
}
