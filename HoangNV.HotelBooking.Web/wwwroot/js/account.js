var AccountBooking = {
    Index: {
        urlAction: null,
        statusEnum: null,
        Init: function () {
            var statusEnum = AccountBooking.Index.statusEnum;
            $("#startDateCalendar").calendar({
                type: 'date',
                formatter: {
                    date: function (date, settings) {
                        if (!date) return '';
                        var day = date.getDate() + '';
                        if (day.length < 2) {
                            day = '0' + day;
                        }
                        var month = (date.getMonth() + 1) + '';
                        if (month.length < 2) {
                            month = '0' + month;
                        }
                        var year = date.getFullYear();
                        return day + '/' + month + '/' + year;
                    }
                },
                className: {
                    table: 'ui celled center aligned unstackable table',
                },
                text: {
                    days: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                    months: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                    monthsShort: ['Th.1', 'Th.2', 'Th.3', 'Th.4', 'Th.5', 'Th.6', 'Th.7', 'Th.8', 'Th.9', 'Th.10', 'Th.11', 'Th.12'],
                    today: 'Hôm nay',
                    now: 'Hiện tại',
                },
                endCalendar: $('#endDateCalendar')
            });
            $("#endDateCalendar").calendar({
                type: 'date',
                formatter: {
                    date: function (date, settings) {
                        if (!date) return '';
                        var day = date.getDate() + '';
                        if (day.length < 2) {
                            day = '0' + day;
                        }
                        var month = (date.getMonth() + 1) + '';
                        if (month.length < 2) {
                            month = '0' + month;
                        }
                        var year = date.getFullYear();
                        return day + '/' + month + '/' + year;
                    }
                },
                text: {
                    days: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                    months: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                    monthsShort: ['Th.1', 'Th.2', 'Th.3', 'Th.4', 'Th.5', 'Th.6', 'Th.7', 'Th.8', 'Th.9', 'Th.10', 'Th.11', 'Th.12'],
                    today: 'Hôm nay',
                    now: 'Hiện tại',
                },
                className: {
                    table: 'ui celled center aligned unstackable table',
                },
                startCalendar: $('#startDateCalendar'),
            });
            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.bookingId;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.bookingId,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return formatDate(new Date(data.checkInTime));
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.checkInTime,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return formatDate(new Date(data.checkOutTime));
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.checkOutTime,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.sumCost.toLocaleString();
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.sumCost,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return statusEnum[data.bookingStatus];
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            statusEnum[cellData.bookingStatus],
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" style="cursor:pointer" onclick="AccountBooking.Index.RedictToAction('${data.bookingId}')"><i class="address book icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                    },
                    width: "10%",
                },
            ];
            var urlAction = AccountBooking.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableExitConfig").DataTable(dataTablesConfig);
          
            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                var startDate = $("#startDate").val();
                var endDate = $("#endDate").val();
                if (startDate != '') {
                    var dateParts = startDate.split("/");
                    var dateObject = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
                    if (endDate == '' && dateObject > (new Date())) {
                        endDate = startDate;
                    }
                }
                if (endDate != '') {
                    var dateEndParts = endDate.split("/");
                    var dateEndObject = new Date(+dateEndParts[2], dateEndParts[1] - 1, +dateEndParts[0]);
                    if (startDate == '' && dateEndObject < (new Date())) {
                        startDate = endDate;
                    }
                }
                if (startDate == '') {
                    startDate = formatDate(new Date(-8640000000000000));
                }
                if (endDate == '') {
                    endDate = formatDate(new Date(8640000000000000));
                }
                var date1 = startDate.split('/')
                var newDate1 = date1[1] + '/' + date1[0] + '/' + date1[2];
                var date2 = endDate.split('/')
                var newDate2 = date2[1] + '/' + date2[0] + '/' + date2[2];
                table.search(
                    JSON.stringify({
                        CheckInTime: newDate1,
                        CheckOutTime: newDate2,
                    })

                ).draw();
            });
            function padTo2Digits(num) {
                return num.toString().padStart(2, '0');
            }
            function formatDate(date) {
                return [
                    padTo2Digits(date.getDate()),
                    padTo2Digits(date.getMonth() + 1),
                    date.getFullYear(),
                ].join('/');
            }
        },
        RedictToAction: function (id) {
            window.location.href = "/Account/BookingDetail?id=" + id;
        }
    }
}
