var BedsController = {
    Index: {
        urlAction: null,
        Init: function () {
            // Common
            $("#btnCancel").click(function () {
                $("#txtAddBeds").val('');
                $("#txtAddBedType-error").text('');
                $(".ui.modal").modal("hide");
            });
            $("#btnCancel1").click(function () {
                location.reload();
            });
            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.bedType;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.bedType,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="BedsController.Index.updateBeds(${data.bedId})"><i class="edit icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-update-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Update"])
                    },
                    width: "10%",
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="BedsController.Index.deleteBeds(${data.bedId})"><i class="trash icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    width: "10%",
                },

            ];
            var urlAction = BedsController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableBeds").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify($("#txtSearch").val())
                ).draw();
            });

            $("#add-bed").click(function () {
                $("#setting-modal").modal("show");
            });

            $("#btnAdd").on('click', () => {
                var data = new FormData();
                data.append("bedType", $('#txtAddBeds').val())
                CallAction("/Admin/Beds/InsertBeds", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            $('#txtAddBeds').val('')
                            $("#txtAddBedType-error").text('');
                            $("#tableBeds").DataTable().ajax.reload();
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txtAddBedType-error").text('');
                        if (response.data !== null) {
                            $.each(response.data, function (key, error) {
                                $.each(error.errors, function (key1, message) {
                                    let strKey = error.key.replaceAll("[", "").replaceAll("]", "").replaceAll(".", "");
                                    $(`#txtAdd${strKey}-error`).text(
                                        message
                                    );
                                });
                            });
                        }
                        else {
                            ShowDialog(response.message, () => {
                                $("#txtAddBeds").val('');
                                $("#txtAddBedType-error").text('');
                                $("#tableBeds").DataTable().ajax.reload();
                            })
                        }
                    }
                })
            })
        },
        updateBeds: function (bedId) {
            CallAction("/Admin/Beds/GetBedUpdate?id=" + bedId, "GET", null, (result) => {
                $("#update-modal").modal("show");
                if (result.status == RESPONSE_JSON_STATUS.OK) {
                    $("#oldBedType").val(result.data.bedType);
                    $("#btnUpdate").on('click', () => {
                        var data = new FormData();
                        data.append("BedType", $('#newBedType').val())
                        data.append("bedId", result.data.bedId)
                        CallAction("/Admin/Beds/UpdateBeds", "POST", data, function (response) {
                            $("#txtUpdateBedType-error").text('');
                            if (response.status == RESPONSE_JSON_STATUS.OK) {
                                ShowDialog(response.message, () => {
                                    $("#txtUpdateBedType-error").text('');
                                    location.reload();
                                    $("#tableBeds").DataTable().ajax.reload();
                                })
                            }
                            if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                                if (response.data !== null) {
                                    $.each(response.data, function (key, error) {
                                        $.each(error.errors, function (key1, message) {
                                            let strKey = error.key.replaceAll("[", "").replaceAll("]", "").replaceAll(".", "");
                                            $(`#txtUpdate${strKey}-error`).text(
                                                message
                                            );
                                        });
                                    });
                                }
                                else {
                                    ShowDialog(response.message, () => {
                                        $("#txtUpdateBedType-error").text('');
                                        location.reload();
                                        $("#tableBeds").DataTable().ajax.reload();
                                    })
                                }
                            }
                        })
                    })
                }
                else {
                    ShowDialog(result.message)
                }
            })
        },
        deleteBeds: function (bedId) {
            var data = new FormData();
            data.append("id", bedId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/Beds/DeleteBed", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableBeds").DataTable().ajax.reload();
                    })
                })
            })

        }

    },
};
