var Loading = {};

Loading.start = function () {
    $.blockUI({
        message: $('<img src="/images/loading.gif"/>')        
    });
}

Loading.done = function () {
    $.unblockUI();
}