$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblBrands").DataTable({
        "responsive": true,
        "autoWidth": false,
    });
});