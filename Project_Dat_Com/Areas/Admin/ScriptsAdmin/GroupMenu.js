let Page = 1;
let PageSize = 10;

async function ShowData(SearchTerm) {
    const table = $("#GroupMenuTable");
    const res = await ajaxAsync({
        url: `${BASE_URL}/group-menu-dat-com/view-group-menu`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            SearchTerm: SearchTerm,
            Page: Page,
            PageSize: PageSize
        }),
    });
    if (res.success) {
        let theadHtml = `
        <tr>
            <th scope="col">Số Thứ Tự</th>
            <th scope="col">ID Group Menu</th>
            <th scope="col">Tên Group Menu</th>
            <th scope="col">Ngày tạo</th>
            <th scope="col">Ngày cập nhật</th>
            <th scope="col">Chức năng</th>
        </tr>
    `;
        let tbodyHtml = ``;
        table.find("thead").html(theadHtml);
        res.data.forEach
    }
}