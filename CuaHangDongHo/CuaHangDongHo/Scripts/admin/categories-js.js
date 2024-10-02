$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblCategories").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

    $('#Name').keyup(delay(function (e) {
        var text = this.value;

        $.ajax({
            method: "POST",
            url: "/Admin/Categories/AjaxCreateSlugName",
            data: { str: text }
        })
        .done(function (data) {
            if (data.result != "") {
                $("#Slug").val(data.result)
            }
        });
    }, 500));

});

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}