using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDemo.Interface
{
    public interface RefreshCallback
    {
        void CallRefreshApi(int webserviceType);
    }
}
