//=========== Global Variables ===========
let startPageIndex = 1, PgSize = 20;
const operationResult = {
    success: 1,
    validationError: -1,
    duplicateData: -2,
    notFound: -3,
    faild: -4,
    unauthorized: -5,
};
function notification(response, message) {

    let type = "";

    switch (response) {

        case operationResult.success:
            type = "success";
            break;

        case operationResult.validationError:
        case operationResult.faild:
        case operationResult.unauthorized:
        case operationResult.notFound:
            type = "error";
            break;

        case operationResult.duplicateData:
            type = "warning";
            break;


        default:
    }

    Swal.fire(
        "",
        message,
        type
    );
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
//--------convert_to_Jalali--------------
//type en fan fas
function convert_to_Jalali(strDate, type) {
    var d = new Date(strDate);
    return gregorian_to_jalali(d.getFullYear(), (d.getMonth() + 1), d.getDate(), d.getDay(), type);
}
function gregorian_to_jalali(g_y, g_m, g_d, g_day, type) {
    var g_days_in_month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var j_days_in_month = [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];
    var gy = g_y - 1600;
    var gm = g_m - 1;
    var gd = g_d - 1;

    var g_day_no = 365 * gy + div_fun(gy + 3, 4) - div_fun(gy + 99, 100) + div_fun(gy + 399, 400);

    for (var i = 0; i < gm; ++i)
        g_day_no += g_days_in_month[i];
    if (gm > 1 && ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)))
        /* leap and after Feb */
        g_day_no++;
    g_day_no += gd;

    var j_day_no = g_day_no - 79;

    var j_np = div_fun(j_day_no, 12053);
    /* 12053 = 365*33 + 32/4 */
    j_day_no = j_day_no % 12053;

    var jy = 979 + 33 * j_np + 4 * div_fun(j_day_no, 1461);
    /* 1461 = 365*4 + 4/4 */

    j_day_no %= 1461;

    if (j_day_no >= 366) {
        jy += div_fun(j_day_no - 1, 365);
        j_day_no = (j_day_no - 1) % 365;
    }
    for (var i = 0; i < 11 && j_day_no >= j_days_in_month[i]; ++i)
        j_day_no -= j_days_in_month[i];
    var jm = i + 1;
    var jd = j_day_no + 1;


    var full_str = "";
    if (type == "en") {
        full_str = jy.toString() + "/" + addZiroToDate(jm) + "/" + addZiroToDate(jd);
    }
    else if (type == "fan") {
        full_str = jy.toString() + "/" + addZiroToDate(jm) + "/" + addZiroToDate(jd);
    }
    else if (type == "fas") {
        full_str = "" + get_persian_day(g_day) + " " + number_to_fa(jd.toString()) + " " + get_persian_month(jm) + " " + number_to_fa(jy.toString());
    }

    return full_str;

}
function div_fun(a, b) {
    return parseInt((a / b));
}
function addZiroToDate(a) {
    if (a.toString().length == 1) {
        a = "0" + a.toString();
    }
    return a.toString();
}
function get_persian_month(month) {
    switch (month) {
        case 1:
            return "فروردین";
            break;
        case 2:
            return "اردیبهشت";
            break;
        case 3:
            return "خرداد";
            break;
        case 4:
            return "تیر";
            break;
        case 5:
            return "مرداد";
            break;
        case 6:
            return "شهریور";
            break;
        case 7:
            return "مهر";
            break;
        case 8:
            return "آبان";
            break;
        case 9:
            return "آذر";
            break;
        case 10:
            return "دی";
            break;
        case 11:
            return "بهمن";
            break;
        case 12:
            return "اسفند";
            break;
    }
}
function get_persian_day(day) { switch (day) { case 0: return "یکشنبه"; break; case 1: return "دوشنبه"; break; case 2: return "سه شنبه"; break; case 3: return "چهارشنبه"; break; case 4: return "پنجشنبه"; break; case 5: return "جمعه"; break; case 6: return "شنبه"; break; } } function main_code() { $(".HasPrice").keyup(function (event) { if (event.which >= 37 && event.which <= 40) return; $(this).val(function (index, value) { return value.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ","); }); }); $('.HasPrice').focusout(function () { var val = $(this).val(); val = val.replace(/,/g, ""); $(this).val(val); }); $(".Num").keyup(function (event) { if (event.which >= 37 && event.which <= 40) return; $(this).val(function (index, value) { return value.replace(/\D/g, ""); }); }); } function getQueryString(name) { var url = window.location.href; name = name.replace(/[\[\]]/g, "\\$&"); var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"), results = regex.exec(url); if (!results) return null; if (!results[2]) return ''; return decodeURIComponent(results[2].replace(/\+/g, " ")); }
//================= Convert Shamsi To Miladi
JalaliDate = {
    g_days_in_month: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31],
    j_days_in_month: [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29]
};
JalaliDate.jalaliToGregorian = function (j_y, j_m, j_d) {
    j_y = parseInt(j_y);
    j_m = parseInt(j_m);
    j_d = parseInt(j_d);
    var jy = j_y - 979;
    var jm = j_m - 1;
    var jd = j_d - 1;

    var j_day_no = 365 * jy + parseInt(jy / 33) * 8 + parseInt((jy % 33 + 3) / 4);
    for (var i = 0; i < jm; ++i) j_day_no += JalaliDate.j_days_in_month[i];

    j_day_no += jd;

    var g_day_no = j_day_no + 79;

    var gy = 1600 + 400 * parseInt(g_day_no / 146097); /* 146097 = 365*400 + 400/4 - 400/100 + 400/400 */
    g_day_no = g_day_no % 146097;

    var leap = true;
    if (g_day_no >= 36525) /* 36525 = 365*100 + 100/4 */ {
        g_day_no--;
        gy += 100 * parseInt(g_day_no / 36524); /* 36524 = 365*100 + 100/4 - 100/100 */
        g_day_no = g_day_no % 36524;

        if (g_day_no >= 365) g_day_no++;
        else leap = false;
    }

    gy += 4 * parseInt(g_day_no / 1461); /* 1461 = 365*4 + 4/4 */
    g_day_no %= 1461;

    if (g_day_no >= 366) {
        leap = false;

        g_day_no--;
        gy += parseInt(g_day_no / 365);
        g_day_no = g_day_no % 365;
    }

    for (var i = 0; g_day_no >= JalaliDate.g_days_in_month[i] + (i == 1 && leap); i++)
        g_day_no -= JalaliDate.g_days_in_month[i] + (i == 1 && leap);
    var gm = i + 1;
    var gd = g_day_no + 1;

    gm = gm < 10 ? "0" + gm : gm;
    gd = gd < 10 ? "0" + gd : gd;

    return [gy, gm, gd];
}
function ConvertShamsiToMilad(myDate) {
    myDate = number_to_en(myDate),
        dateSplitted = myDate.split("/"),
        jD = JalaliDate.jalaliToGregorian(dateSplitted[0], dateSplitted[1], dateSplitted[2]);
    jResult = jD[0] + "/" + jD[1] + "/" + jD[2];
    return number_to_en(jResult);
}
function number_to_en(a) {
    if (a != null && a.length > 0) {
        return a = a.replace(/۱/g, "1").replace(/۲/g, "2").replace(/۳/g, "3").replace(/۴/g, "4").replace(/۵/g, "5").replace(/۶/g, "6").replace(/۷/g, "7").replace(/۸/g, "8").replace(/۹/g, "9").replace(/۰/g, "0")
    }
}
function number_to_fa(a) {
    if (a != null && a.length > 0) {
        return a = a.replace(/1/g, "۱").replace(/2/g, "۲").replace(/3/g, "۳").replace(/4/g, "۴").replace(/5/g, "۵").replace(/6/g, "۶").replace(/7/g, "۷").replace(/8/g, "۸").replace(/9/g, "۹").replace(/0/g, "۰")
    }
}