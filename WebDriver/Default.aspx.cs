using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDriver
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Cache["Foo"] = "Foo";

            Cache["Customer"] = new Customer() {FirstName = "Mohammad", LastName = "Azam"}; 


            Cache.Insert("Categories","Menu",new CacheDependency(Server.MapPath("XMLFile1.xml")),DateTime.Now.AddMinutes(20),TimeSpan.Zero);
                       

        }
    }
}
