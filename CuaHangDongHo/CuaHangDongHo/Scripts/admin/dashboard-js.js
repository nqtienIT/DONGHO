$(document).ready(function () {
    $("#btnSave").click(function () {
        $(this).attr("disabled", true);
        $('form').submit();
    });

    $("#tblBrands").DataTable({
        "responsive": true,
        "autoWidth": false,
    });

    getDistricts();
});


function getDistricts() {
    $.ajax({
        method: "POST",
        url: "/AjaxCommon/AjaxGetDashboardAdmin",
    })
        .done(function (data) {

            // dashboard count
            $("#total_product").text(data.TotalProduct)
            $("#total_account").text(data.TotalAccount)
            $("#total_order").text(data.TotalOrders)
            $("#total_post").text(data.TotalPosts)


            let labels = [];
            let dataInThisMonth = [];
            let dataInPrevMonth = [];

            $.each(data.NewAccountsInThisMonth, function (index, value) {
                labels.push(value.Day);
                dataInThisMonth.push(value.Count);
            });

            $.each(data.NewAccountsInPrevMonth, function (index, value) {
                dataInPrevMonth.push(value.Count);
            });

            console.log(data)

            $("#count_new_account").text(data.CountNewAccount)

            if (data.RateRegisterAccount < 0) {
                $("#rate_account_down font").text(" " + data.RateRegisterAccount + "%")
                $("#rate_account_up").hide();
                $("#rate_account_down").show();
            }
            else {
                $("#rate_account_up font").text(" " + data.RateRegisterAccount + "%")
                $("#rate_account_up").show();
                $("#rate_account_down").hide();
            }

            labels = [];
            dataInThisMonth = [];
            $.each(data.PostInMonth, function (index, value) {
                labels.push(value.Day);
                dataInThisMonth.push(value.Count);
            });

            var ctx = document.getElementById('chartRegisterAccount').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Tháng này',
                        borderColor: [
                            'rgba(0, 123, 255, 1)'
                        ],
                        borderWidth: 1,
                        fill: false,
                        data: dataInThisMonth,
                    }, {
                        label: 'Tháng trước',
                        borderColor: [
                            'rgba(108, 117, 125, 1)',
                        ],
                        borderWidth: 1,
                        fill: false,
                        data: dataInPrevMonth,
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                stepSize: 1
                            }
                        }]
                    },
                    legend: {
                        display: false
                    }
                }
            });


            var ctx = document.getElementById('chartPosts').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Tháng này',
                        borderColor: [
                            'rgba(0, 123, 255, 1)'
                        ],
                        borderWidth: 1,
                        fill: false,
                        data: dataInThisMonth,
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                stepSize: 1
                            }
                        }]
                    },
                    legend: {
                        display: false
                    }
                }
            });

        });
}
