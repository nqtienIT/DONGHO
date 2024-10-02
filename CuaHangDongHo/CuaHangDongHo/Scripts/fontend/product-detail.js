$(document).ready(function () {
    $("#btnComment").click(function (e) {
        e.preventDefault()
        $("#errComment").text("")
        $("#successComment").text("")
        if ($("#txtComment").val() == "" || $("#txtComment").val() == null) {
            $("#errComment").text("Vui lòng nhập Comment.")
        }
        else {
            $(this).addClass("disabled");
            ShowDisplayLoading()
            $.ajax({
                method: "POST",
                url: "/Comment/ProductComment",
                data: {
                    Created_by: $("#userId").val(),
                    ProductId: $("#productId").val(),
                    Detail: $("#txtComment").val()
                }
            })
                .done(function (data) {
                    if (data.result == 0) {
                        HideDisplayLoading()
                        $("#txtComment").val("");
                        $("#successComment").text("Bình luận thành công! Đang duyệt!")
                    }
                })
                .done(function () {
                    $("#btnComment").removeClass("disabled");
                })
        }
    })
})
