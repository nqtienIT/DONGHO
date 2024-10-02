$(document).ready(function () {

})

function ChangeStatus(elm, id) {
    if (elm.value == 4) {
        $("#cancel_order_" + id).show();
    }
    else {
        $("#cancel_order_" + id).hide();
    }
}

function CancelOrder(id) {
    ShowDisplayLoading()
    $.ajax({
        method: "POST",
        url: "/Cart/CancelOrder",
        data: {
            id: id,
            description: $("#description_" + id).val()
        }
    })
        .done(function (data) {
            if (data.err == 1) {
                $("#description_" + id).addClass("is-invalid")
                $("#invalid_text_" + id).text(data.msg)
            }
            else if (data.err == 0) {
                $("td#divStatus_" + id).find("select, #cancel_order_" + id).remove();
                $("td#divStatus_" + id).html(`<p>${data.msg.status}</p><small class="text-success">${data.msg.noti}</small>`)
            }
            console.log(data)
            HideDisplayLoading();
        });
}
