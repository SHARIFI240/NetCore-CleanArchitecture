
function ConvertCharToNumInput() {
    $(".Num").keyup(function (event) {

        if (event.which >= 37 && event.which <= 40) return;
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "");
        });
    });
}
function FDate(scope) {
    if (scope != null) {
        scope = "#" + scope + " ";
    }
    else {
        scope = "";
    }
    $(scope + ".FDate").each(function () {
        if ($(this).html().trim() != "") {
            var d = convert_to_Jalali($(this).html(), "fas");
            $(this).html(d);
        }
    });
    $(scope + ".fdate").each(function () {
        if ($(this).html().trim() != "") {
            var d = convert_to_Jalali($(this).html(), "fas");
            $(this).html(d);
        }
    });
    $(scope + ".FnDate").each(function () {
        if ($(this).html().trim() != "") {
            var d = convert_to_Jalali($(this).html(), "fan");
            $(this).html(d);
        }
    });
}

//========= doc ready ============

$(document).ready(() => {
    $("select").select2();
})


function BeforeFormSubmit() {
    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find("button[type='submit']").prop('disabled', true);
            Loading(true);
        }
        else {
            $(this).find("button[type='submit']").prop('disabled', false);
            Loading(false);
        }
    });
}
function EnableButtonSubmit() {
    $("form button[type='submit']").prop("disabled", false);
}

function ReplaceArabic(t) {
    if (t != null && t != "" && t != undefined) {
        if (t != null && t.length > 0) {
            return t.replace(/ي/g, 'ی').replace(/ك/g, 'ک');
        }
    } else {
        return "";
    }
}
function Loading(a) {
    if (a) {
        $(".loading").fadeIn();
        $(".loading").fadeIn();
    } else {
        $(".loading").fadeOut();
        $(".loading").fadeOut();
    }
}
function getQueryString(name) {
    var url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
ChangeSPI = (spi) => {
    ChangeUrl(spi);
}
CrtPaging = (CntRec) => {

    var string = "<li class='page-item' ><a class='page-link' onclick='ChangeSPI(1)' href='#'><i class='fa fa-angle-left'></i></a></li>";
    var number = 0;
    div = parseInt(CntRec) / parseInt(PgSize);
    if (parseFloat(parseInt(CntRec) % PgSize) == 0) { div--; }
    for (var i = 0; i <= div; i++) {
        number++;
        if (number == startPageIndex) {
            string = string + "<li class='active page-item'><a class='page-link' id='PgA" + number + "' onclick='ChangeSPI(" + number + ")' href='#' >" + number.toString() + "</a></li>";
        } else {
            string = string + "<li class='page-item'><a class='page-link' id='PgA" + number + "' onclick='ChangeSPI(" + number + ")' href='#' >" + number.toString() + "</a></li>";
        }
    }
    $("#Div_Paging").html(string + "<li class='page-item'><a  class='page-link'onclick='ChangeSPI(" + number + ")' href='#' ><i class='fa fa-angle-right'></i></a><li>");
    for (var j = 1; j < (number + 1); j++) {
        if (j < (parseInt(startPageIndex) + 14) && j > (parseInt(startPageIndex) - 14)) {
            $("#PgA" + j).show();
        }
        else {
            $("#PgA" + j).hide();
        }
    }

    Loading(false);

}
const FileUploader = (inputs, sendAddress, successFunction) => {

    let data = new FormData();

    inputs.map((item, index) => {
        let fileUpload = $(item).get(0);
        let files = fileUpload.files;
        if (files[0] !== undefined) {
            data.append(`file${index+1}`, files[0]);
            
        }

    })

    $.ajax({
        type: "POST",
        url: sendAddress,
        contentType: false,
        processData: false,
        data: data,
        async: false,
        beforeSend: function () {
            Loading(true);
        },
        success: function (response) {

            successFunction(response);

        },
        error: function (e) {
            notification(operationResult.faild, "مشکل در اپلود تصویر");
        },
    });

}
const ReadyForUpload = (items) => {

    items.map((item, index) => {
        $(`${item.label}`).on('change', function () {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(`${item.img}`).attr('src', e.target.result);
                }
                reader.readAsDataURL(this.files[0]);
            }
        });
    })
    
}
const ScrollToTop = () => {
    window.scrollTo(0, 0);
}