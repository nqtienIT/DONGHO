$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblUsers").DataTable({
        "responsive": true,
        "autoWidth": false,
    });
});