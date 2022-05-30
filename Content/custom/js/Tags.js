$(function () {
    Tags.configure();   
});

var Tags = {};

Tags.configure = function () {
    $('.chips__choice .chip').on("click", function () {
        $('.chips__choice .chip').removeClass('chip--active');
        $(this).addClass("chip--active");
    });

    $('.chips__choice .chip_solucao').on("click", function () {
        let readonly = $(this).attr("data-readonly");

        if (readonly) {
            return;
        }

        $('.chips__choice .chip_solucao').removeClass('chip--active');
        $(this).addClass("chip--active");
    });
}
