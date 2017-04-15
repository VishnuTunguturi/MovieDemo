using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDemo.Models.Param
{
    public class ParamFavourite : ParamsBase
    {
        public ParamFavourite()
        {
            session_id = "2de39045e5ae5642c8bae4220514613a110e9eb9";
        }

        public string GetParamsJSON()
        {
            return base.GetParamsJSON(this);
        }

        public string session_id { get; set; }
    }
}
