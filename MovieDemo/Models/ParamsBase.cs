using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MovieDemo.Models
{
    public class ParamsBase
    {       

        public ParamsBase()
        {
            page = "1";
            api_key = "6a824db996cc590d4644109dfbac4e01";
            sort_by = "popularity.des";
        }                
        public string api_key { get; set; }
        public string sort_by { get; set; }
        public string page { get; set; }
        public string GetParamsJSON(ParamsBase oDerivedClass)
        {
            //// SerializeObject
            //return JsonConvert.SerializeObject(this);
            string retParamsJSON = string.Empty;
            string paramValue;

            IEnumerable<PropertyInfo> propertiesInfos = oDerivedClass.GetType().GetRuntimeProperties();

            foreach (PropertyInfo propInfo in propertiesInfos)
            {
                paramValue = GetValueOrEmptyString(propInfo.GetValue(this));

                if (paramValue != string.Empty)
                {
                    if (retParamsJSON == string.Empty)
                    {
                        retParamsJSON = "?";
                    }
                    else
                    {
                        retParamsJSON += "&";
                    }
                    retParamsJSON += propInfo.Name + "=" + paramValue;
                }
            }
            return retParamsJSON;
        }

        private string GetValueOrEmptyString(object objProp)
        {
            if (objProp == null)
            {
                return string.Empty;
            }
            else
            {
                return (string)objProp;
            }
        }

    }
}
