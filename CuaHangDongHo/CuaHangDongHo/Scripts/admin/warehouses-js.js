$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblSupplier").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

    //Initialize Select2 Elements
    $('.select2').select2()

    showHideSupplier()

    $("#Type").change(function () {
        showHideSupplier()
    });

});

function showHideSupplier() {
    if ($("#Type").val() == 1) {
        $("#Supplier").css("display", "none")
    }
    else {
        $("#Supplier").css("display", "")
    }
}