$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblSlider").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

});