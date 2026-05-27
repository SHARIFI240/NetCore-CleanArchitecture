$(document).ready(() => {
    $("#Page_Article").addClass("active");
    ReadByUrl();
});

const NewCategory = () => {
    $.ajax({
        type: "GET",
        url: `/Admin/Category/_Create`,
        contentType: "application/json",
        success: function (result) {

            $("#Modal_Create_Category").html(result).modal("show");
            BeforeFormSubmit();

        }, error: function () {
            Loading(false);
            notification("error", "هنگام انجام عملیات خطایی رخ داد");
        }
    });
}
const ChangeUrl = (page) => {
    startPageIndex = page;

    let searchParam = "";

    if ($("#txt_title").val().trim() != "") {
        searchParam = "?searchParam=" + ReplaceArabic($("#txt_title").val());
    }

    

    history.pushState(null, null, "/Admin/Category/Index" + searchParam);
    ReadByUrl();
}
const ReadByUrl = () => {
    let searchParam = "";

    Loading(true);

    if (getQueryString('searchParam') != undefined && getQueryString('searchParam') != "") {
        searchParam = ReplaceArabic(getQueryString('searchParam').replace(/-/g, " "));
        $("#txt_title").val(searchParam);

    }
    else {
        $("#txt_title").val("");
    }


    GetAll(startPageIndex, searchParam);
}
const GetAll = (page, searchParam) => {

    $.ajax({
        type: "GET",
        url: `/Admin/Category/Select?page=${page}&searchParam=${searchParam}`,
        contentType: "application/json",
        success: function (result) {

            if (result.status === operationResult.success) {

                $("#tbody_category").html("");

                result.data.items.map((item, _) => {

                    let row = `<tr>
                                    <td>${item.row}</td>
                                    <td>${item.title}</td>
                                    <td>
                                        <button onclick="EditCategory(${item.id})" class="btn btn-success btn-sm"><i class="fa fa-edit"></i> ویرایش</button>
                                        <button onclick="DeleteCategory(${item.id})" class="btn btn-danger btn-sm"><i class="fa fa-trash-alt"></i> حذف</button>

                                    </td>
                                </tr>`;

                    $("#tbody_category").append(row);

                })

                CrtPaging(result.data.totalCount);

            }


        }, error: function () {
            Loading(false);
            notification(operationResult.faild, "هنگام انجام عملیات خطایی رخ داد");
        }
    });

}
const EditCategory = (id) => {
    $.ajax({
        type: "GET",
        url: `/Admin/Category/_Edit?id=${id}`,
        contentType: "application/json",
        beforeSend: function () {
            Loading(true);
        },
        success: function (result) {

            $("#Modal_Create_Category").html(result).modal("show");
            Loading(false);
            BeforeFormSubmit();

        }, error: function () {
            Loading(false);
            notification(operationResult.faild, "هنگام انجام عملیات خطایی رخ داد");
        }
    });
}
const ClearSearch = () => {
    history.pushState(null, null, "/Admin/Category/Index");
    ReadByUrl();
}
const DeleteCategory = (id) => {
    $("#txt_delete_id").val(id);
    $("#Modal_Delete_Category").modal("show");
}
function OnSuccess(response) {
    Loading(false);
    EnableButtonSubmit();
    if (response.status === operationResult.success) {
        ReadByUrl();
        $(".modal").modal("hide");
    }
    notification(response.status, response.message);
}