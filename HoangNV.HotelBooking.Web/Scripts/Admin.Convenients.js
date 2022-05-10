var ConvenientController = {
    Index: {
        urlAction: null,
        Init: function () {
            $(".ui.dropdown").dropdown({
                fullTextSearch: true,
                match: "text",
                message: { noResults: "" },
            });
            $(".ui.search.selection.dropdown input").prop("maxLength", 50);
            // Common
            $("#btnCancel").click(function () {
                $("#txtAddConvenient").val('');
                $("#txtAddConvenientName-error").text('');
                $("#txtAddConvenientTypeId-error").text('');
                $(".ui.modal").modal("hide");
                $('#convenientTypeNameAdd').val('0').change();
            });
            $("#btnCancel1").click(function () {
                $(".ui.modal").modal("hide");
                $("#txtUpdateConvenientName-error").text('');
                $("#txtUpdateConvenientTypeId-error").text('');
                location.reload();
            });

            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.convenientName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.convenientName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                    width: "40%",
                },
                {
                    data: null,
                    render: function (data) {
                        return data.convenientTypeName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.convenientTypeName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="ConvenientController.Index.updateConvenient(${data.convenientId})"><i class="edit icon"></i></div>`;
                    },
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Update"])
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-update-convenient-type",
                    width: "10%",
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="ConvenientController.Index.deleteConvenient(${data.convenientId})"><i class="trash icon"></i></div>`;
                    },
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    width: "10%",
                },

            ];
            var urlAction = ConvenientController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableConvenient").DataTable(dataTablesConfig);
           
            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify({
                        ConvenientName: $('#txtConvenientName').val(),
                        ConvenientTypeId: $("#convenientTypeName").val(),
                    })
                ).draw();
            });
            $("#add-convenient").click(function () {
                $("#setting-modal").modal("show");
            });

            $("#btnAdd").on('click', () => {
                var data = new FormData();
                data.append("ConvenientName", $('#txtAddConvenient').val())
                data.append("ConvenientTypeId", $('#convenientTypeNameAdd').val())
                CallAction("/Admin/Convenients/InsertConvenient", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            $('#txtAddConvenient').val('')
                            $("#txtAddConvenientName-error").text('');
                            $("#txtAddConvenientTypeId-error").text('');
                            $('#convenientTypeNameAdd').val('0').change();
                            $("#tableConvenient").DataTable().ajax.reload();
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txtAddConvenientName-error").text('');
                        $("#txtAddConvenientTypeId-error").text('');
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
                                $('#txtAddConvenient').val('')
                                $("#txtAddConvenientName-error").text('');
                                $("#txtAddConvenientTypeId-error").text('');
                                $('#convenientTypeNameAdd').val('0').change();
                                $("#tableConvenient").DataTable().ajax.reload();
                            })
                        }
                    }
                })
            })

        },
        updateConvenient: function (convenientId) {
            CallAction("/Admin/Convenients/GetConvenientUpdate?id=" + convenientId, "GET", null, (result) => {
                $("#update-modal").modal("show");
                if (result.status == RESPONSE_JSON_STATUS.OK) {
                    $("#txtUpdateConvenient").val(result.data.convenientName);
                    $("#convenientTypeNameUpdate").val(result.data.convenientTypeId).change();
                    $("#btnUpdate").on('click', () => {
                        var data = new FormData();
                        data.append("ConvenientName", $('#txtUpdateConvenient').val())
                        data.append("ConvenientTypeId", $('#convenientTypeNameUpdate').val())
                        data.append("ConvenientId", result.data.convenientId)
                        CallAction("/Admin/Convenients/UpdateConvenient", "POST", data, function (response) {
                            $("#txtUpdateConvenientName-error").text('');
                            $("#txtUpdateConvenientTypeId-error").text('');
                            if (response.status == RESPONSE_JSON_STATUS.OK) {
                                ShowDialog(response.message, () => {
                                    $("#txtUpdateConvenientName-error").text('');
                                    $("#txtUpdateConvenientTypeId-error").text('');
                                    location.reload();
                                    $("#tableConvenient").DataTable().ajax.reload();
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
                                        $("#txtUpdateConvenientName-error").text('');
                                        $("#txtUpdateConvenientTypeId-error").text('');
                                        location.reload();
                                        $("#tableConvenient").DataTable().ajax.reload();
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
        deleteConvenient: function (convenientId) {
            var data = new FormData();
            data.append("id", convenientId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/Convenients/DeleteConvenient", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableConvenient").DataTable().ajax.reload();
                    })
                })
            })

        }
    },
};
