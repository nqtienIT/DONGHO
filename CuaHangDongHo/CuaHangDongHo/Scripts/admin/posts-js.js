$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblPost").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

    // Summernote
    $('.detail_summernote').summernote({
        lang: 'vi-VN'
    })

    $('#Title').keyup(delay(function (e) {
        var text = this.value;

        $.ajax({
            method: "POST",
            url: "/Admin/Products/AjaxCreateSlugName",
            data: { str: text }
        })
            .done(function (data) {
                if (data.result != "") {
                    $("#Slug").val(data.result)
                }
            });
    }, 500));

    $("#Img").change(function () {
        previewImg(this, "#previewImg");
        $("#previewImg").attr("height", 200)
    });
});