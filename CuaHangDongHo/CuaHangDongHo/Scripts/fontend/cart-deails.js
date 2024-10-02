var CartItem = [];

$(document).ready(function () {
    $("#btnUpdateCart").click(function () {
        //ShowDisplayLoading();

        console.log(CartItem)

        //$.ajax({
        //    method: "POST",
        //    url: "/Cart/UpdateCart",
        //    data: { id: id }
        //})
        //    .done(function (data) {
        //        if (data.err == 1) {
        //            window.location.href = "Auth/Login"
        //        }
        //        else if (data.err == 0) {
        //            $('span#lblCartCount').text(data.msg)
        //        }
        //        console.log(data)
        //        HideDisplayLoading();
        //    });
    })
})

function UpdateQuantity(id) {
    let quantity = $("#quantity_" + id).val();
    if (quantity < 1) {
        return;
    }
    ShowDisplayLoading();
    $.ajax({
        method: "POST",
        url: "/Cart/UpdateCart",
        data: { id: id, quantity: quantity }
    })
        .done(function (data) {
            if (data.err == 0) {
                $("#total_price_" + id).text(data.msg.price)
                $("#total_money").text(data.msg.totalPrice)
                HideDisplayLoading()
            }
        });
}

function SubQuantity(id) {
    let quantity = $("#quantity_" + id).val();

    if (quantity == 1) {
        return;
    }
    ShowDisplayLoading();
    let newQuantity = (Number(quantity) - 1);
    $.ajax({
        method: "POST",
        url: "/Cart/UpdateCart",
        data: { id: id, quantity: newQuantity }
    })
        .done(function (data) {
            if (data.err == 0) {
                $("#quantity_" + id).val(newQuantity)
                $("#total_price_" + id).text(data.msg.price)
                $("#total_money").text(data.msg.totalPrice)
                HideDisplayLoading()
            }
        });
}

function AddQuantity(id) {
    ShowDisplayLoading();
    let oldQuantity = $("#quantity_" + id).val();
    let newQuantity = (Number(oldQuantity) + 1);

    $.ajax({
        method: "POST",
        url: "/Cart/UpdateCart",
        data: { id: id, quantity: newQuantity }
    })
        .done(function (data) {
            if (data.err == 0) {
                $("#quantity_" + id).val(newQuantity)
                $("#total_price_" + id).text(data.msg.price)
                $("#total_money").text(data.msg.totalPrice)
                //UpdateMoney(id);
                //UpdateAddTotalMoney(id)
                HideDisplayLoading()
            }
        });
}

function DeleteProduct(id) {
    $("#row_" + id).remove()
    ShowDisplayLoading();
    $.ajax({
        method: "POST",
        url: "/Cart/DeleteItemCart",
        data: { id: id }
    })
        .done(function (data) {
            if (data.err == 0)
                if (data.msg.totalPrice == undefined || data.msg.totalPrice == 0) {
                    $("#cart_items").remove();
                    $("#do_action").remove();
                    $("#empty_cart").show();
                }
            $("#total_money").text(data.msg.totalPrice)
            HideDisplayLoading()
        });
}

function ReplaceFormatVND(str) {
    return str.replace(/[, VND]+/g, "");
}

function FormatVND(str) {
    let formatStr = str.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    return formatStr + " VND";
}

function UpdateMoney(id) {
    // cap nhat tien
    let newQuantity = $("#quantity_" + id).val();
    let price = ReplaceFormatVND($("#price_" + id).text());
    let newPrice = FormatVND(Number(newQuantity) * price);
    $("#total_price_" + id).text(newPrice)
}

function UpdateAddTotalMoney(id) {
    let oldTotalMoney = ReplaceFormatVND($("#total_money").text())
    let price = ReplaceFormatVND($("#price_" + id).text());
    let newTotalMoney = Number(oldTotalMoney) + Number(price);
    $("#total_money").text(FormatVND(newTotalMoney))
}

function UpdateSubTotalMoney(id) {
    let oldTotalMoney = ReplaceFormatVND($("#total_money").text())
    let price = ReplaceFormatVND($("#price_" + id).text());
    let newTotalMoney = Number(oldTotalMoney) - Number(price);
    $("#total_money").text(FormatVND(newTotalMoney))
}

function UpdateDeleteTotalMoney(id) {
    let oldTotalMoney = ReplaceFormatVND($("#total_money").text())
    let price = ReplaceFormatVND($("#total_price_" + id).text());
    let newTotalMoney = Number(oldTotalMoney) - Number(price);
    $("#total_money").text(FormatVND(newTotalMoney))
}



