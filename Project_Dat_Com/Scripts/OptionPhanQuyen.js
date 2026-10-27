let BASE_URL = `/api/v1/admin`;
let BASE_URL_DV = `/api/v1/donvi`;
let BASE_URL_CTDT = `/api/v1/ctdt`;
let BASE_URL_DVCM = `/api/v1/dvcm`;

function ajaxAsync(options) {
    return new Promise((resolve, reject) => {
        $.ajax(options).done(resolve).fail(reject);
    });
}