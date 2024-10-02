$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblProduct").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

    $("#Description").text(WriteHtmlDescription())

    // Summernote
    $('.detail_summernote').summernote({
        lang: 'vi-VN'
    })

    $('#Name').keyup(delay(function (e) {
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

    // Format So luong san pham
    if ($('#Number').val() != null) {
        FormatNumber($('#Price').val(), "#NumberFormat")
    }

    $('#Number').keyup(delay(function (e) {
        var money = this.value;
        FormatNumber(money, "#NumberFormat")
    }, 500));

    // Format Tien VND Gia
    if ($('#Price').val() != null) {
        FormatMoneyVND($('#Price').val(), "#PriceFormat")
    }

    $('#Price').keyup(delay(function (e) {
        var money = this.value;
        FormatMoneyVND(money, "#PriceFormat")
    }, 500));

    // Format Tien VND Gia giam
    if ($('#PriceSale').val() != null) {
        FormatMoneyVND($('#PriceSale').val(), "#PriceSaleFormat")
    }

    $('#PriceSale').keyup(delay(function (e) {
        var money = this.value;
        FormatMoneyVND(money, "#PriceSaleFormat")
    }, 500));

    //Initialize Select2 Elements
    $('.select2').select2()

    $("#Img").change(function () {
        previewImg(this, "#previewImg");
        $("#previewImg").attr("height", 200)
    });

    $('#Name').keyup(delay(function (e) {
        var money = this.value;
        CreateSlug(money, "#PriceSaleFormat")
    }, 500));

    $('#img_1, #img_2, #img_3, #img_4, #img_5, #img_6, #img_7, #img_8').change(function () {
        previewImg(this, "img#" + this.id);
    })
});

function WriteHtmlDescription() {
    var text = "<ul>";
    text += "<li>";
    text += "Tình trạng : ";
    text += "</li>";
    text += "<li>";
    text += "Giới tính : ";
    text += "</li>";
    text += "<li>";
    text += "Loại đồng hồ : ";
    text += "</li>";
    text += "<li>";
    text += "Loại dây : ";
    text += "</li>";
    text += "</ul>";
    return text;
}