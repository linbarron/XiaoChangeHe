$(function () {
    $(".s_right").click(function () {
        var t = $(this).siblings("input");
        t.val(parseInt(t.val()) + 1);
        console($(this).attr("pid"));
    });

    $(".s_left").click(function () {
        var t = $(this).siblings("input");
        if (t.val() > 1)
            t.val(parseInt(t.val()) - 1);
        console($(this).attr("pid"));
    });

    //上面导航栏切换效果   

    $(".top_list ul li").click(function () {

        $(this).addClass("bg1").siblings().removeClass("bg1");
        var s = $(this).index();

        $(".menu>ul.level1:eq(" + s + ")").show().siblings().hide();
        $(".menu>ul.level1:eq(" + s + ")>li:eq(0)").addClass("bg2");
        $(".menu>ul.level1:eq(" + s + ") .level2:eq(0)").show();
    });
    $(".level1 li").click(function () {

        $(this).addClass("bg2").siblings().removeClass("bg2");
        $(this).next("ul").show();
        $(this).siblings().next("ul").hide();

    });

    var l1 = $(".level1>li").length;
    for (var i = 1; i <= l1; i++) {
        var s = i * 30;
        $(".level1>li").eq(i).css({ "margin-top": s, "background": "#f2f2f2" });
    }
    $(".level1>li").click(function () {
        $(this).css("background", "#fff").siblings("li").css("background", "#f2f2f2");
    });
    $(".level1>li:eq(1)").css({ "background": "#fff" });
});