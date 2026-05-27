$(document).ready(() => {
    let pathName = window.location.pathname;

    switch (pathName) {
        //=========================================
        //=========== Home Page =============
        case '':
        case '/':
        case '/Home':
        case '/Home/':
        case '/Home/Index':
        case '/Home/Index/':
            $(".Page_Home").addClass("active");
            break;
        //=========================================
        //=========== Article Page ====================
        case '/Article':
        case '/Article/Index':
        case '/Article/Index/':
        case '/Article/':
            $(".Page_Article").addClass("active");
            break;
        //=========================================
        //=========== Gallery Page ================
        case '/Gallery':
        case '/Gallery/':
            $(".Page_Gallery").addClass("active");
            break;
        //=========================================
        //=========== About Us Page ===============
        case '/About-Us':
        case '/About-Us/':
            $(".Page_AboutUs").addClass("active");
            break;
        //=========================================
        //=========== Contact Us Page =============
        case '/Contact-Us':
        case '/Contact-Us/':
            $(".Page_ContactUs").addClass("active");
            break;

        default:
            break;
    }

})
const CreateTimer = (timerStr = "2:00", endFunction) => {
    $('#Timer').show("slow");
    // Reverse Timer
    var interval = setInterval(function () {
        var timer = timerStr.split(':');

        var minutes = parseInt(timer[0], 10);
        var seconds = parseInt(timer[1], 10);
        --seconds;
        minutes = (seconds < 0) ? --minutes : minutes;
        if (minutes < 0) clearInterval(interval);
        seconds = (seconds < 0) ? 59 : seconds;
        seconds = (seconds < 10) ? '0' + seconds : seconds;

        let Str = minutes + ':' + seconds;

        if (Str == "-1:59") {
            endFunction();
        } else {
            $('#Timer').html(Str);
        }

        timerStr = minutes + ':' + seconds;
    }, 1000);
}