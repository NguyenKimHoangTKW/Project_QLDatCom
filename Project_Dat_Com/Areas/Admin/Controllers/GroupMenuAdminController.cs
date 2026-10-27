
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
        private readonly QLDatComEntities db = new QLDatComEntities();

        private readonly int unixTimestamp =
            (int)(DateTime.UtcNow -
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .TotalSeconds;

        // 1. Danh sách nhóm thực đơn + tìm kiếm + phân trang
        [HttpPost]
        [Route("view-group-menu")]
        public async Task<IHttpActionResult> LoadDanhSachGroupMenu(
            [FromBody] ViewGroupMenuAdmin items)
        {
            if (items == null)
            {
                return BadRequest("Dữ liệu gửi lên không hợp lệ.");
            }

            int page = Math.Max(1, items.Page);
            int pageSize = items.PageSize <= 0
                ? 10
                : Math.Min(items.PageSize, 100);

            var query = db.Group_menu.AsQueryable();

            if (!string.IsNullOrWhiteSpace(items.SearchTerm))
            {
                string search = items.SearchTerm.Trim();

                query = query.Where(x =>
                    x.ten_group_menu.Contains(search));
            }

            int totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                currentPage = page,
                pageSize,
                totalRecords,
                totalPages = (int)Math.Ceiling(
                    totalRecords / (double)pageSize)
            });
        }

        // 2. Thêm nhóm thực đơn
        [HttpPost]
        [Route("create-group-menu")]
        public async Task<IHttpActionResult> CreateGroupMenu(
            [FromBody] Group_menu items)
        {
            if (items == null ||
                string.IsNullOrWhiteSpace(items.ten_group_menu))
            {
                return Ok(new
                {
                    success = false,
                    message = "Không được bỏ trống tên nhóm thực đơn."
                });
            }

            string tenGroupMenu = items.ten_group_menu.Trim();

            bool exists = await db.Group_menu.AnyAsync(x =>
                x.ten_group_menu == tenGroupMenu);

            if (exists)
            {
                return Ok(new
                {
                    success = false,
                    message = "Tên nhóm thực đơn này đã tồn tại."
                });
            }

            var newGroup = new Group_menu
            {
                ten_group_menu = tenGroupMenu,
                time_cre = unixTimestamp,
                time_up = unixTimestamp
            };

            db.Group_menu.Add(newGroup);
            await db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Thêm nhóm thực đơn thành công.",
                data = new
                {
                    newGroup.id,
                    newGroup.ten_group_menu,
                    newGroup.time_cre,
                    newGroup.time_up
                }
            });
        }

        // 3. Lấy nhóm thực đơn theo ID
        [HttpPost]
        [Route("get-items-by-id")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var item = await db.Group_menu
                .Where(x => x.id == id)
                .Select(x => new
                {
                    x.id,
                    x.ten_group_menu,
                    x.time_cre,
                    x.time_up
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Không tồn tại nhóm thực đơn."
                });
            }

            return Ok(new
            {
                success = true,
                data = item
            });
        }

        // 4. Cập nhật nhóm thực đơn
        [HttpPost]
        [Route("update-group-menu")]
        public async Task<IHttpActionResult> UpdateGroupMenu(
            [FromBody] Group_menu items)
        {
            if (items == null ||
                string.IsNullOrWhiteSpace(items.ten_group_menu))
            {
                return Ok(new
                {
                    success = false,
                    message = "Không được bỏ trống tên nhóm thực đơn."
                });
            }

            var checkItems = await db.Group_menu
                .FirstOrDefaultAsync(x => x.id == items.id);

            if (checkItems == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Không tồn tại nhóm thực đơn."
                });
            }

            string tenGroupMenu = items.ten_group_menu.Trim();

            bool duplicate = await db.Group_menu.AnyAsync(x =>
                x.id != items.id &&
                x.ten_group_menu == tenGroupMenu);

            if (duplicate)
            {
                return Ok(new
                {
                    success = false,
                    message = "Tên nhóm thực đơn này đã tồn tại."
                });
            }

            checkItems.ten_group_menu = tenGroupMenu;
            checkItems.time_up = unixTimestamp;

            await db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Cập nhật nhóm thực đơn thành công."
            });
        }

        // 5. Xóa nhóm thực đơn
        [HttpPost]
        [Route("delete-group-menu")]
        public async Task<IHttpActionResult> DeleteGroupMenu(int id)
        {
            var checkItems = await db.Group_menu
                .FirstOrDefaultAsync(x => x.id == id);

            if (checkItems == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Không tồn tại nhóm thực đơn."
                });
            }

            db.Group_menu.Remove(checkItems);
            await db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Xóa nhóm thực đơn thành công."
            });
        }

        // 6. Giải phóng tài nguyên database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
