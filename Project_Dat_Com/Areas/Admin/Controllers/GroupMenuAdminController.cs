using Project_Dat_Com.Models;
using Project_Dat_Com.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Project_Dat_Com.Areas.Admin.Controllers
{
    [RoutePrefix("api/v1/admin/group-menu-dat-com")]
    public class GroupMenuAdminController : ApiController
    {
        QLDatComEntities db = new QLDatComEntities();
        private int unixTimestamp;
        public GroupMenuAdminController()
        {
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        [HttpPost]
        [Route("view-group-menu")]
        public async Task<IHttpActionResult> LoadDanhSachGroupMenu([FromBody] ViewGroupMenuAdmin items)
        {
            var query = db.Group_menu.AsQueryable();
            if (!string.IsNullOrEmpty(items.SearchTerm))
            {
                query = query.Where(x => x.ten_group_menu.Contains(items.SearchTerm));
            }
            var groupmenu = await query
                .Skip((items.Page -1) * items.PageSize)
                .Take(items.PageSize)
                .ToList
            return Ok();
        }
    }
}
