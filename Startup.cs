using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FullStack.API.Startup))]

namespace FullStack.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // vvv 20230419 Diptesh QnCC No. 11088
            //ConfigureAuth(app);
            // ^^^ 20230419 Diptesh QnCC No. 11088
        }
    }
}
