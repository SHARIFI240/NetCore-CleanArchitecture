$(document).ready(() => {
    $("#Page_Users").addClass("active");
    ReadByUrl();
    BeforeFormSubmit();
})
const ChangeUrl = (page) => {
    startPageIndex = page;

    let searchParam = "";

    if ($("#txt_SearchParam").val().trim() != "") {
        searchParam = "?searchParam=" + ReplaceArabic($("#txt_SearchParam").val());
    }

    history.pushState(null, null, "/Admin/Users/Index" + searchParam);
    ReadByUrl();
}
const ReadByUrl = () => {
    let searchParam = "";

    Loading(true);

    if (getQueryString('searchParam') != undefined && getQueryString('searchParam') != "") {
        searchParam = ReplaceArabic(getQueryString('searchParam').replace(/-/g, " "));
        $("#txt_SearchParam").val(searchParam);

    }
    else {
        $("#txt_SearchParam").val("");
    }


    GetAll(startPageIndex, searchParam);
}
const GetAll = (page, searchParam) => {

    $.ajax({
        type: "GET",
        url: `/Admin/Users/Select?page=${page}&searchParam=${searchParam}`,
        contentType: "application/json",
        success: function (result) {

            if (result.status === operationResult.success) {

                $("#tbody_users").html("");

                result.data.items.map((item, _) => {

                    let activeOrDeactiveClass = "btn-success";
                    let activeOrDeactiveName = "باز کردن حساب";

                    if (item.isActive) {
                        activeOrDeactiveClass = "btn-danger";
                        activeOrDeactiveName = "قفل کردن حساب";
                    }

                    let row = `<tr>
                                    <td>${item.row}</td>
                                    <td><span class="FDate">${item.registerDate}</span></td>
                                    <td>${item.phoneNumber}</td>
                                    <td>${item.fullName}</td>
                                    <td><span class="FDate">${item.lastLoginDate}</span></td>
                                     <td>${item.isActive ? `<span class="badge badge-success" >فعال است</span>` : `<span class="badge badge-danger" >قفل است</span>`}</td>
                                    <td>
                                        <button id="btn_ActiveOrDeactive_${item.id}" onclick="ActiveOrDeactive(${item.id})" class="btn ${activeOrDeactiveClass} btn-sm"><i class="fa fa-lock"></i> ${activeOrDeactiveName}</button>
                                        <button onclick="DeleteCategory(${item.id})" class="btn btn-primary btn-sm"><i class="fa fa-key"></i> تغییر رمز</button>
                                    </td>
                                </tr>`;

                    $("#tbody_users").append(row);

                })

                FDate("tbody_users");

                CrtPaging(result.data.totalCount);

            }


        }, error: function () {
            Loading(false);
            notification(operationResult.faild, "هنگام انجام عملیات خطایی رخ داد");
        }
    });

}
const ActiveOrDeactive = (userRef) => {

    $("#Modal_ActiveOrDeactive h5").html($(`#btn_ActiveOrDeactive_${userRef}`).html());
    $("#txt_userRef").val(userRef);
    $("#Modal_ActiveOrDeactive").modal("show");

}
const ClearSearch = () => {
    history.pushState(null, null, "/Admin/Users/Index");
    ReadByUrl(); 
}
function OnSuccessActiveOrDeactive(response) {
    ReadByUrl();
    Loading(false);
    $("#Modal_ActiveOrDeactive button").prop("disabled", false);
    $(".modal").modal("hide");
    notification(response.status, response.message);
}