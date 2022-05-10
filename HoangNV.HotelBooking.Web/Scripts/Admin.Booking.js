var BookingRoomController = {
    Index: {
        urlAction: null,
        Init: function () {
            // Common
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
            $("#costNum").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('');
            });
            $("#area").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('');
            });
            $("#perNum").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('');
            });

            $("#add-booking").on('click', function () {
                RedirectToPage("/Admin/Booking/BookingDetail")
            });
            //table
            $("#btnCancel").click(function () {
                $(".ui.modal").modal("hide");
            });
            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.roomCode;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.roomCode,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.cost.toLocaleString();
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.cost,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.area.toLocaleString();
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.area,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.numOfPer;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.numOfPer,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },

            ];
            var urlAction = BookingRoomController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableRooms").DataTable(dataTablesConfig);
            function formatDate(date) {
                return [
                    padTo2Digits(date.getDate()),
                    padTo2Digits(date.getMonth() + 1),
                    date.getFullYear(),
                ].join('/');
            }
            function padTo2Digits(num) {
                return num.toString().padStart(2, '0');
            }
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
                    startDate = formatDate(new Date());
                }
                if (endDate == '') {
                    endDate = formatDate(new Date());
                }
                var date1 = startDate.split('/')
                var newDate1 = date1[1] + '/' + date1[0] + '/' + date1[2];
                var date2 = endDate.split('/')
                var newDate2 = date2[1] + '/' + date2[0] + '/' + date2[2];
                table.search(
                    JSON.stringify({
                        CheckInTime: newDate1,
                        CheckOutTime: newDate2,
                        SumCost: $('#costNum').val(),
                        NumOfPer: $('#perNum').val(),
                        RoomTypeId: $('#roomTypeId').val(),
                        Area: $('#area').val(),
                    })
                ).draw();
            });
        },
    },
    Detail: {
        Init: function () {
            $("#btnCancel").on('click', () => {
                RedirectToPage("/Admin/Booking")
            })
            $("#CostSum").toLocaleString();
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
                endCalendar: $('#endDateCalendar'),
                minDate: new Date(),
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
                minDate: new Date(),
            });
            $("#btnSearch").on('click', () => {
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
                    startDate = formatDate(new Date());
                }
                if (endDate == '') {
                    endDate = formatDate(new Date());
                }
                var date1 = startDate.split('/')
                var newDate1 = date1[1] + '/' + date1[0] + '/' + date1[2];
                var date2 = endDate.split('/')
                var newDate2 = date2[1] + '/' + date2[0] + '/' + date2[2];
                CallAction(`/Admin/Booking/GetRoomTypeNamesInsertJson?startDate=${newDate1.toString()}&endDate=${newDate2.toString()}`, "GET", null,
                    (data) => {

                        var dropDownlist = $('#roomTypes');
                        dropDownlist.children().remove();
                        dropDownlist.append(`<option></option>`);
                        for (var i = 0; i < data.length; i++) {
                            dropDownlist.append('<option value="' + data[i].value + '">' + data[i].text + "</option>");
                        }
                        $("#CostSum").val('');
                    })
            });



            $("#btnAddRoomType").click(() => {
                var checkAll = false;
                var optionValues = $("#roomTypes").val();
                var arrRoomTypes = optionValues.toString().split('-')
                optionValues.forEach(optionValue => {
                    if (!optionValue || $(`#room-type-table tbody tr[id="${optionValue}"]`).length != 0) return;
                    this.createRowDepartment(optionValue, (optionValue.split('-'))[1]);
                    $(`#roomTypes option[value="${optionValue}"]`).remove();
                    $(`#dropDownDepartmentSchedule a[data-value="${optionValue}"]`).remove();
                })
                if ($("#roomTypes option").length === 0) {
                    $("#btnAddRoomType").prop('disabled', true);
                    $("#roomTypes").empty();
                }
            });
            $("#roomTypes").change(() => {
                $("#btnAddRoomType").prop("disabled", $("#roomTypes").val().length == 0);
            });
            $("#btnSum").on('click', () => {
                var sum = 0;
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
                    startDate = formatDate(new Date());
                }
                if (endDate == '') {
                    endDate = formatDate(new Date());
                }
                var date1 = startDate.split('/')
                var newDate1 = date1[1] + '/' + date1[0] + '/' + date1[2];
                var date2 = endDate.split('/')
                var newDate2 = date2[1] + '/' + date2[0] + '/' + date2[2];
                const date1utc = Date.UTC(date1[2], date1[1], date1[0]);
                const date2utc = Date.UTC(date2[2], date2[1], date2[0]);
                day = 1000 * 60 * 60 * 24;
                var datesum = (date2utc - date1utc) / day +1;
                $.each($("#room-type-table tbody tr"), function (key, val) {
                    if ($(val).find("input[name=nameRoomNumberInsert]").val() != null && $(val).find("input[name=nameRoomNumberInsert]").val()!='') {
                        var cost = parseFloat(($(val).find("input[name=departmentCode]").val().split('-'))[2]);
                        sum = sum + cost * parseInt($(val).find("input[name=nameRoomNumberInsert]").val()) * datesum;
                    }
                });
                $("#CostSum").val(sum.toLocaleString());
            });

            $("#btnSubmit").on('click', function () {
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
                    startDate = formatDate(new Date());
                }
                if (endDate == '') {
                    endDate = formatDate(new Date());
                }
                var date1 = startDate.split('/')
                var newDate1 = date1[1] + '/' + date1[0] + '/' + date1[2];
                var date2 = endDate.split('/')
                var newDate2 = date2[1] + '/' + date2[0] + '/' + date2[2];
                var sum = 0;
                const date1utc = Date.UTC(date1[2], date1[1], date1[0]);
                const date2utc = Date.UTC(date2[2], date2[1], date2[0]);
                day = 1000 * 60 * 60 * 24;
                var datesum = (date2utc - date1utc) / day + 1;
                $.each($("#room-type-table tbody tr"), function (key, val) {
                    if ($(val).find("input[name=nameRoomNumberInsert]").val() != null && $(val).find("input[name=nameRoomNumberInsert]").val() != '') {
                        var cost = parseFloat(($(val).find("input[name=departmentCode]").val().split('-'))[2]);
                        sum = sum + cost * parseInt($(val).find("input[name=nameRoomNumberInsert]").val()) * datesum;
                    }
                });
                if (sum == 0) {
                    ShowDialog(CreateNotifyMessage(Resources["W_B_007_35"], DIALOG_MESSAGE_TYPE.ERROR));
                }
                else {
                    var data = new FormData();
                    $.each($("#room-type-table tbody tr"), function (key, val) {
                        if ($(val).find("input[name=nameRoomNumberInsert]").val() != null && $(val).find("input[name=nameRoomNumberInsert]").val() != '') {
                            data.append("RoomTypeId", parseInt(($(val).find("input[name=departmentCode]").val().split('-'))[0]));
                            data.append("NumOfRoom", parseInt($(val).find("input[name=nameRoomNumberInsert]").val()));
                        }
                    });
                    data.append("CostSum", sum);
                    data.append("CheckInTime", newDate1);
                    data.append("CheckOutTime", newDate2);
                    data.append("CustomerName", $("#CustomerName").val());
                    data.append("PhoneNumber", $("#PhoneNumber").val());
                    data.append("Email", $("#Email").val());
                    data.append("CheckInPersonCode", $("#CheckInPersonCode").val());
                    data.append("CheckInPersonName", $("#CheckInPersonName").val());
                    CallAction("/Admin/Booking/Booked", "POST", data, (response) => {
                        if (response.status == RESPONSE_JSON_STATUS.OK) {
                            ShowDialog(response.message, () => {
                                RedirectToPage("/Admin/Booking/ConfirmBooked")
                            })
                        }
                        if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                            $("#txt-CustomerName-error").text('');
                            $("#txt-Email-error").text('');
                            $("#txt-CheckInPersonName-error").text('');
                            $("#txt-CheckInPersonCode-error").text('');
                            if (response.data !== null) {
                                $.each(response.data, function (key, error) {
                                    $.each(error.errors, function (key1, message) {
                                        let strKey = error.key.replaceAll("[", "").replaceAll("]", "").replaceAll(".", "");
                                        $(`#txt-${strKey}-error`).text(
                                            message
                                        );
                                    });
                                });
                            }
                            else {
                                ShowDialog(response.message, () => {
                                    RedirectToPage("/Admin/Booking/ConfirmBooked")
                                })
                            }
                        }
                    })
                }
            });
        },
        createRowDepartment: function (departmentCode, maxPatient) {
            var html = `<tr id="${departmentCode}">
                            <td>
                                <input name="departmentCode" value="${departmentCode}" hidden />
                                <span>${$(`#roomTypes option[value="${departmentCode}"]`).text()}</span>
                            </td>
                            <td class="left aligned">
                                <input required maxlength="5" min="1" max="${maxPatient}" name="nameRoomNumberInsert" type="number" />
                                <span class="field hs-text-red" data-err-of="NumberPatientOfDepartments[$index]MaxPatient"></span>
                            </td>
                            <td class="center aligned"><i onclick="BookingRoomController.Detail.deleteDepartment('${departmentCode}')" class="trash alternate icon"></i></td>
                        </tr>`;
            $("#room-type-table tbody").append(html);
            ValidateNumber();
        },
        clearDepartments: function (departments) {
            $("#room-type-table tbody tr").remove();
            var optionElement = "";
            departments.forEach(department => {
                optionElement = optionElement.concat(`<option value=${department.Value}>${department.Text}</option>`);
            })
            $("#roomTypes").empty();
            $("#roomTypes").append(optionElement);
            //$("#departments").val("00000000000").change();
            $("#btnAddRoomType").prop('disabled', true);
        },
        deleteDepartment: function (departmentCode) {
            var departmentName = $(`#room-type-table tbody tr[id="${departmentCode}"] td span`).text();
            $("#roomTypes").append(`<option value=${departmentCode}>${departmentName}</option>`);
            $(`#room-type-table tbody tr[id="${departmentCode}"]`).remove();
            var arrDepartments = [];
            $("#roomTypes option").each(optionValue => {
                arrDepartments.push({ code: $(`#roomTypes option`).eq(optionValue).val(), text: $(`#roomTypes option`).eq(optionValue).text() });
            })
            arrDepartments.sort(function (a, b) {
                var nameA = a.text.toLowerCase();
                var nameB = b.text.toLowerCase();
                var codeA = a.code.toLowerCase();
                var codeB = b.code.toLowerCase();
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }
                if (nameA === nameB) {
                    if (codeA < codeB) return -1;
                    if (codeA > codeB) return 1;
                    return 0;
                }
                return 0;
            });
            var optionElement = "";
            arrDepartments.forEach(department => {
                optionElement = optionElement.concat(`<option value=${department.code}>${department.text}</option>`);
            })
            $("#roomTypes").empty();
            $("#roomTypes").append(optionElement);
            //$("#departments").val("00000000000").change();
            $("#btnAddDepartment").prop('disabled', true);
        },
    },
    Exit: {
        urlAction: null,
        Init: function () {
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
                        return data.customerName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.customerName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.phoneNumber;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.phoneNumber,
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
                        return `<div class="ui icon" onclick="location.href='/Admin/Booking/CheckingDetail/${data.bookingId}'"><i class="address book icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                    },
                    width: "10%",
                },
            ];
            var urlAction = BookingRoomController.Exit.urlAction;
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
                        CustomerName: $("#customerName").val(),
                        PhoneNumber: $("#phone").val(),
                    })
                    
                ).draw();
            });

            function padTo2Digits(num) {
                return num.toString().padStart(2, '0');
            }
            $("#btn-download-csv").click(function () {
                var param = JSON.stringify(BookingRoomController.getSearchData());
                RedirectToPage('/Admin/Booking/DowloadCSV?searchValue=' + param);
            });
            function formatDate(date) {
                return [
                    padTo2Digits(date.getDate()),
                    padTo2Digits(date.getMonth() + 1),
                    date.getFullYear(),
                ].join('/');
            }
        },
        Detail: function (bookingId) {
            RedirectToPage("/Admin/Booking/CheckingDetail?id=" + bookingId);
        }
    },
    getSearchData: function () {
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
        return {
            CheckInTime: newDate1,
            CheckOutTime: newDate2,
            CustomerName: $("#customerName").val(),
            PhoneNumber: $("#phone").val(),
        };
    },
};
