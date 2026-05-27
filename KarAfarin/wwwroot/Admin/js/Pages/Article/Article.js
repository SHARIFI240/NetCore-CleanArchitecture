$(document).ready(() => {
    CreateNew();
})
const CreateNew = () => {
    $.ajax({
        type: "GET",
        url: "/Admin/Article/_Create",
        contentType: "application/json",
        beforeSend: function () {
            Loading(true);
        },
        success: function (response) {

            $("#Div_Create_Update").html(response);

            BeforeFormSubmit();

            $("select").select2();

            new Tagify(document.getElementById("Keywords"));



            InitSummernote();
            ReadyForUpload([
                {
                    label: ".custom-file-input1",
                    img: "#img1"
                }
            ]);

            ReadByUrl();

        },
        complete: function () {
            ScrollToTop();
            Loading(false);
        },
        error: function (e) {
            notification(operationResult.faild, "مشکل در اپلود تصویر");
        },
    });
}
function OnSuccess(response) {
    if (response.status === operationResult.success) {

        $("button").prop("disabled", false);

        if ($("#Image").val() !== '') {

            FileUploader(
                [$("#Image")],
                `/Admin/Article/UploadPostImage?articleCover=true&articleId=${response.data}`,
                (responseUpload) => {
                    Loading(false);
                    if (responseUpload.status === operationResult.success) {

                        Loading(false);
                        notification(response.status, response.message);
                        CreateNew();
                        
                    } else {
                        notification(operationResult.faild, "بارگذاری تصویر با خطا مواجه شد");
                    }
                }
            );

        } else {
            Loading(false);
            CreateNew();
            notification(response.status, response.message);
        }

    } else {
        Loading(false);
        notification(response.status, response.message);
    }
}
const InitSummernote = () => {


    $('#txt_article').summernote({
        tabsize: 2,
        height: 300,
        direction: 'rtl',
        lang: 'fa-IR',
        toolbar: [
            // گروهبندی دکمه‌هایی که می‌خواهید باقی بمانند
            ['style', ['style']],
            ['font', ['bold', 'italic', 'underline', 'clear']],
            ['fontname', []], // این بخش را خالی بگذارید یا کلاً حذف کنید تا آپشن فونت برود
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', []],
            ['view', []]
        ]
    });

    $(".note-toolbar .note-view").append(`
    <button onclick="ImageInputClick()" type="button" class="note-btn" tabindex="-1">
        <i class="fa fa-image"></i>
    </button>`);



    //====== Send Image To Server

    $("#inp_image").change(() => {

        if ($("#inp_image").val() !== '') {

            FileUploader(
                [$("#inp_image")],
                "/Admin/Article/UploadPostImage?articleCover=false",
                (response) => {
                    Loading(false);
                    if (response.status === operationResult.success) {

                        $(".note-editable").append(`<img src="/Upload${response.data[0].filePath}/${response.data[0].fileName}" />`);

                    } else {
                        notification(operationResult.faild,"بارگذاری تصویر با خطا مواجه شد");
                    }
                }
            );

        } else {
            notification(operationResult.faild, "لطفا تصویر مورد نظر را انتخاب کنید");
        }

    });

}
const ImageInputClick = () => {
    $("#inp_image").click();
}
//======= Grid
const ChangeUrl = (page) => {
    startPageIndex = page;

    let searchParam = "", categoryRef = "";

    if ($("#txt_title").val().trim() != "") {
        searchParam = "?searchParam=" + ReplaceArabic($("#txt_title").val());
    }

    if ($("#slc_category").val().trim() != "") {
        categoryRef = "?categoryRef=" + $("#slc_category").val();
    }
    

    history.pushState(null, null, "/Admin/Article/Index" + searchParam + categoryRef);
    ReadByUrl();
}
const ReadByUrl = () => {
    let searchParam = "", categoryRef = 0;

    Loading(true);

    if (getQueryString('searchParam') != undefined && getQueryString('searchParam') != "") {
        searchParam = ReplaceArabic(getQueryString('searchParam').replace(/-/g, " "));
        $("#txt_title").val(searchParam);
    }
    else {
        $("#txt_title").val("");
    }


    if (getQueryString('categoryRef') != undefined && getQueryString('categoryRef') != "") {
        categoryRef = getQueryString('categoryRef');
        $("#slc_category").val(categoryRef);
        let categoryTitle = $(`#slc_category option[value='${categoryRef}']`).html();
        $("#select2-slc_category-container").html(categoryTitle);
    }
    else {
        $("#slc_category").val("");
        $("#select2-slc_category-container").html("جستجو براساس دسته بندی");

    }


    GetAll(startPageIndex, categoryRef, searchParam);
}
const GetAll = (page, categoryRef, searchParam) => {

    $.ajax({
        type: "GET",
        url: `/Admin/Article/Select?page=${page}&categoryRef=${categoryRef}&searchParam=${searchParam}`,
        contentType: "application/json",
        success: function (result) {

            if (result.status === operationResult.success) {

                $("#tbody_article").html("");

                result.data.items.map((item, _) => {

                    let row = `<tr>
                                    <td>${item.row}</td>
                                    <td>${item.title}</td>
                                    <td>${item.categoryTitle}</td>
                                    <td><span class="FDate">${item.publishDate}</span></td>
                                    <td>${item.viewCount}</td>
                                    <td>
                                        <button onclick="EditArticle(${item.id})" class="btn btn-success btn-sm"><i class="fa fa-edit"></i> ویرایش</button>
                                        <button onclick="DeleteArticle(${item.id})" class="btn btn-danger btn-sm"><i class="fa fa-trash-alt"></i> حذف</button>
                                    </td>
                                </tr>`;

                    $("#tbody_article").append(row);

                })

                FDate("tbody_article");

                CrtPaging(result.data.totalCount);

            }


        }, error: function () {
            Loading(false);
            notification(operationResult.faild, "هنگام انجام عملیات خطایی رخ داد");
        }
    });
}
const ClearSearch = () => {
    history.pushState(null, null, "/Admin/Article/Index");
    ReadByUrl();
}
//====== Edit
const EditArticle = (id) => {
    $.ajax({
        type: "GET",
        url: `/Admin/Article/_Edit?id=${id}`,
        contentType: "application/json",
        beforeSend: function () {
            Loading(true);
        },
        success: function (response) {

            $("#Div_Create_Update").html(response);

            BeforeFormSubmit();

            $("select").select2();

            new Tagify(document.getElementById("Keywords"));



            InitSummernote();
            ReadyForUpload([
                {
                    label: ".custom-file-input1",
                    img: "#img1"
                }
            ]);

            ReadByUrl();

        },
        complete: function () {
            ScrollToTop();
            Loading(false);
        },
        error: function (e) {
            notification(operationResult.faild, "مشکل در اپلود تصویر");
        },
    });
}
//====== Delete
const DeleteArticle = (id) => {  
    $("#txt_delete_id").val(id);
    $("#Modal_Delete_Article").modal("show");
}
function OnSuccessDelete(response) {
    Loading(false);
    ReadByUrl();
    $("#Modal_Delete_Article").modal("hide");
    $("#Modal_Delete_Article button").prop("disabled", false);
    notification(response.status, response.message);
}