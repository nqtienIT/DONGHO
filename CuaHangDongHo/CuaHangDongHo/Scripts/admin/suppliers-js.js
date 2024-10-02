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

    // set disabled khi khong chon quan/huyen
    let districstId = $("#DistricstId").val()
    if (districstId == undefined || districstId == null || districstId == "") {
        $("#DistricstId").attr("disabled", true)

        // set disabled khi khong chon xa
        let communeId = $("#CommuneId").val()
        if (communeId == undefined || communeId == null || communeId == "") {
            $("#CommuneId").attr("disabled", true)
        }
    }

    setShowHideInput()

    $("#ProvinceId").change(function () {
        getDistricts(this.value)
    });

    $("#DistricstId").change(function () {
        getCommunes(this.value)
    });

});

function getDistricts(ProvinceId) {
    $.ajax({
        method: "POST",
        url: "/AjaxCommon/AjaxGetDistricts",
        data: { str: ProvinceId }
    })
        .done(function (data) {
            if (data.length > 0) {
                $("#DistricstId").attr("disabled", false)
                $('#DistricstId').empty(); //remove all child nodes

                $.each(data, function (index, item) {
                    let newOption = $('<option value="' + item.Id + '">' + item.Name + '</option>');
                    $('#DistricstId').append(newOption);
                });
            }
            else {
                setDistrictsDefault()
            }
            setCommunesDefault()
            setShowHideInput()
        });
}

function getCommunes(DistrictId) {
    $.ajax({
        method: "POST",
        url: "/AjaxCommon/AjaxGetCommunes",
        data: { str: DistrictId }
    })
        .done(function (data) {
            if (data.length > 0) {
                $("#CommuneId").attr("disabled", false)
                $('#CommuneId').empty(); //remove all child nodes

                $.each(data, function (index, item) {
                    let newOption = $('<option value="' + item.Id + '">' + item.Name + '</option>');
                    $('#CommuneId').append(newOption);
                });
            }
            else {
                setCommunesDefault()
            }
            setShowHideInput()
        });
}

function setDistrictsDefault() {
    $("#DistricstId").attr("disabled", true)
    $('#DistricstId').empty();
    let newOption = $('<option value="">-- Chọn Quận / Huyện--</option>');
    $('#DistricstId').append(newOption);
}

function setCommunesDefault() {
    $("#CommuneId").attr("disabled", true)
    $('#CommuneId').empty();
    let newOption = $('<option value="">-- Chọn Xã --</option>');
    $('#CommuneId').append(newOption);
}

function setShowHideInput() {
    $("#DetailAddress").attr("disabled", $("#CommuneId").is(':disabled'))
}