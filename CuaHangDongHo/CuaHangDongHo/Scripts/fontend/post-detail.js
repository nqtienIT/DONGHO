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
                url: "/Comment/PostComment",
                data: {
                    Created_by: $("#userId").val(),
                    PostId: $("#postId").val(),
                    Detail: $("#txtComment").val()
                }
            })
                .done(function (data) {
                    if (data.result == 0) {
                        HideDisplayLoading()
                        $("#successComment").text("Bình luận thành công! Đang duyệt!")
                    }
                });
        }
    })
})
