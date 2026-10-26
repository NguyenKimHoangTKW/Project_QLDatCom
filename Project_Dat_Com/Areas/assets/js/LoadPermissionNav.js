$(document).ready(async function () {
    await LoadPermissionNav();
});

async function LoadPermissionNav() {
    const res = await $.ajax({
        url: `/api/load-nav-nguoi-dung`,
        type: "GET"
    });

    const data = JSON.parse(res);
    let html = `
        <li class="nav-item">
            <a href="/trang-chu"><span class="title">Trang chủ</span></a>
        </li>
    `;

    data.forEach(navcha => {
        if (navcha.NavCon && navcha.NavCon.length > 0) {
            html += `
                <li class="nav-item dropdown" data-ma="Admin_${navcha.NavCha.replace(/\s+/g, '')}">
                    <a class="dropdown-toggle" href="javascript:void(0);">
                        <span class="title">${navcha.NavCha}</span>
                        <span class="arrow"><i class="fas fa-chevron-down"></i></span>
                    </a>
                    <ul class="dropdown-menu">
                        ${navcha.NavCon.map(con => `
                            <li><a href="${con.link_dieu_huong}">${con.ten_nav_con}</a></li>
                        `).join("")}
                    </ul>
                </li>`;
        } else {
            html += `
                <li class="nav-item" data-ma="Admin_${navcha.NavCha.replace(/\s+/g, '')}">
                    <a href="${navcha.URL_NC || 'javascript:void(0);'}">
                        <span class="title">${navcha.NavCha}</span>
                    </a>
                </li>`;
        }
    });

    html += `
        <li class="nav-item">
            <a href="javascript:void(0)" onclick="logout()">
                <span class="title">Đăng xuất</span>
            </a>
        </li>
    `;

    $(".side-nav-menu").html(html);
    $(".side-nav-menu").off("click", ".dropdown-toggle").on("click", ".dropdown-toggle", function (e) {
        e.preventDefault();
        e.stopPropagation();
        const parentLi = $(this).closest(".nav-item");
        const submenu = parentLi.find(".dropdown-menu").first();
        if (parentLi.hasClass("open")) {
            submenu.stop(true, true).slideUp(200);
            parentLi.removeClass("open");
        } else {
            $(".side-nav-menu .nav-item.open").removeClass("open").find(".dropdown-menu").slideUp(200);
            submenu.stop(true, true).slideDown(200);
            parentLi.addClass("open");
        }
    });

    $(".side-nav-menu").off("click", ".nav-item > a").on("click", ".nav-item > a", function () {
        $(".side-nav-menu .nav-item").removeClass("active");
        $(".side-nav-menu .dropdown-menu li").removeClass("active");
        $(this).closest(".nav-item").addClass("active");
    });

    $(".side-nav-menu").off("click", ".dropdown-menu li > a").on("click", ".dropdown-menu li > a", function () {
        $(".side-nav-menu .nav-item").removeClass("active");
        $(".side-nav-menu .dropdown-menu li").removeClass("active");
        $(this).parent("li").addClass("active"); 
        $(this).closest(".nav-item.dropdown").addClass("active open");
    });

    const currentUrl = window.location.pathname.toLowerCase();

    $(".side-nav-menu .dropdown-menu li > a").each(function () {
        if ($(this).attr("href") && $(this).attr("href").toLowerCase() === currentUrl) {
            $(this).parent("li").addClass("active");
            $(this).closest(".nav-item.dropdown").addClass("active open");
            $(this).closest(".dropdown-menu").show();
        }
    });

    $(".side-nav-menu .nav-item > a").each(function () {
        if ($(this).attr("href") && $(this).attr("href").toLowerCase() === currentUrl) {
            $(this).closest(".nav-item").addClass("active");
        }
    });
}
