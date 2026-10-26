using Project_Dat_Com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project_Dat_Com.Controllers
{
    [RoutePrefix("api/v1/group-menu-dat-com")]
    public class GroupMenuController : ApiController
    {
        QLDatComEntities db = new QLDatComEntities();
        private int unixTimestamp;
        public GroupMenuController()
        {
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

    }
}
