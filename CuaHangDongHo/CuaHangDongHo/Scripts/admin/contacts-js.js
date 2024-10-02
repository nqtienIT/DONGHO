$(document).ready(function () {
    $("#btnPreview").click(function () {
        var map = $("#Map").val()
        $("#previewMap").html(map)
    });
});