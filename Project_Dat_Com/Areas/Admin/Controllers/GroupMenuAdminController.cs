using Project_Dat_Com.Models;
using Project_Dat_Com.Models.DTOs;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
namespace Project_Dat_Com.Areas.Admin.Controllers
{
    [RoutePrefix("api/v1/admin/group-menu-dat-com")]
    public class GroupMenuAdminController : ApiController
    {
        QLDatComEntities db = new QLDatComEntities();

        [HttpPost]
        [Route("view-group-menu")]
        public async Task<IHttpActionResult> LoadDanhSachGroupMenu(
            [FromBody] ViewGroupMenuAdmin items)
        {
            var query = db.Group_menu.AsQueryable();

            if (!string.IsNullOrEmpty(items.SearchTerm))
            {
                query = query.Where(x =>
                    x.ten_group_menu.Contains(items.SearchTerm));
            }
            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.id)
                .Skip((items.Page - 1) * items.PageSize)
                .Take(items.PageSize)
                .Select(x => new
                {
                    x.id,
                    x.ten_group_menu,
                    x.time_up,
                    x.time_cre
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                data,
                currentPage = items.Page,
                pageSize = items.PageSize,
                totalRecords,
                totalPages = (int)Math.Ceiling(totalRecords / (double)items.PageSize)
            });
        }
    }
}