
let Page = 1;
let PageSize = 10;
let SearchTerm = "";

$(document).ready(async function () {
    await ShowData();
});

// Mở modal thêm nhóm thực đơn
$(document).on("click", "#btnAdd", function (event) {
    event.preventDefault();

    $(".modal-title").text("Thêm mới nhóm thực đơn");

    $(".modal-footer").html(`
        <button type="button" class="btn btn-danger btn-tone m-r-5"
            data-dismiss="modal">Thoát</button>
        <button type="button" class="btn btn-success btn-tone m-r-5"
            id="btnSaveAdd">Lưu</button>
    `);

    $("#bd-example-modal-lg").modal("show");
});

// Lưu nhóm thực đơn
$(document).on("click", "#btnSaveAdd", async function (event) {
    event.preventDefault();
    await CreateData();
});

// Thêm dữ liệu
async function CreateData() {
    const tenmenu = $("#ten-nhom-menu").val()?.trim();

    if (!tenmenu) {
        Swal.fire({
            icon: "warning",
            title: "Vui lòng nhập tên nhóm thực đơn"
        });
        return;
    }

    try {
        const res = await ajaxAsync({
            url: `${BASE_URL}/group-menu-dat-com/create-group-menu`,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                ten_group_menu: tenmenu
            })
        });

        if (res.success) {
            await Swal.fire({
                title: res.message,
                icon: "success"
            });

            $("#bd-example-modal-lg").modal("hide");
            $("#ten-nhom-menu").val("");

            await ShowData();
        } else {
            Swal.fire({
                icon: "error",
                title: "Không thể thêm dữ liệu",
                text: res.message
            });
        }
    } catch (error) {
        console.error("CreateData error:", error);

        Swal.fire({
            icon: "error",
            title: "Lỗi",
            text: "Không thể thêm nhóm thực đơn."
        });
    }
}

// Hiển thị danh sách nhóm thực đơn
async function ShowData() {
    const table = $("#GroupMenuTable");

    try {
        const res = await ajaxAsync({
            url: `${BASE_URL}/group-menu-dat-com/view-group-menu`,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                SearchTerm: SearchTerm,
                Page: Page,
                PageSize: PageSize
            })
        });

        if (!res.success) {
            Swal.fire({
                icon: "error",
                title: "Không thể tải dữ liệu"
            });
            return;
        }

        const theadHtml = `
            <tr>
                <th scope="col">Số thứ tự</th>
                <th scope="col">ID Group Menu</th>
                <th scope="col">Tên Group Menu</th>
                <th scope="col">Ngày tạo</th>
                <th scope="col">Ngày cập nhật</th>
                <th scope="col">Chức năng</th>
            </tr>
        `;

        let tbodyHtml = "";

        if (res.data && res.data.length > 0) {
            res.data.forEach((item, index) => {
                tbodyHtml += `
                    <tr>
                        <td class="formatSo">
                            ${(Page - 1) * PageSize + index + 1}
                        </td>
                        <td class="formatSo">${item.id}</td>
                        <td>${item.ten_group_menu ?? ""}</td>
                      
                        <td>
                            <button
                                type="button"
                                class="btn btn-icon btn-hover btn-sm btn-rounded"
                                id="btnEdit"
                                data-id="${item.id}">
                                <i class="anticon anticon-edit"></i>
                            </button>

                            <button
                                type="button"
                                class="btn btn-icon btn-hover btn-sm btn-rounded"
                                id="btnDelete"
                                data-id="${item.id}">
                                <i class="anticon anticon-delete"></i>
                            </button>
                        </td>
                    </tr>
                `;
            });
        } else {
            tbodyHtml = `
                <tr>
                    <td colspan="6" class="text-center">
                        Không có dữ liệu
                    </td>
                </tr>
            `;
        }

        table.find("thead").html(theadHtml);
        table.find("tbody").html(tbodyHtml);

    } catch (error) {
        console.error("ShowData error:", error);

        Swal.fire({
            icon: "error",
            title: "Lỗi",
            text: "Không thể tải danh sách nhóm thực đơn."
        });
    }
}
