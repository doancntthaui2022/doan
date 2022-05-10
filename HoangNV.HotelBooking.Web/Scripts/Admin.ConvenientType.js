var ConvenientTypeController = {
    Index: {
        urlAction: null,
        Init: function () {
            // Common
            $("#btnCancel").click(function () {
                $("#txtAddConvenientType").val('');
                $("#txtAddConvenientType-error").text('');
                $(".ui.modal").modal("hide");
            });
            $("#btnCancel1").click(function () {
                $(".ui.modal").modal("hide");
                $("#txtUpdateConvenientType-error").text('');
                $('#newConvenientName').val('')
                location.reload();
            });
            var columns = [
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
                        return `<div class="ui icon" onclick="ConvenientTypeController.Index.updateConvenientType(${data.convenientTypeId},'${data.convenientTypeName}')"><i class="edit icon"></i></div>`;
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
                        return `<div class="ui icon" onclick="ConvenientTypeController.Index.deleteConvenientType(${data.convenientTypeId})"><i class="trash icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    width: "10%",
                },

            ];
            var urlAction = ConvenientTypeController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableConvenientType").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify($("#txtSearch").val())
                ).draw();
            });
            $("#add-convenient-type").click(function () {
                $("#setting-modal").modal("show");
            });
            $("#btnAdd").on('click', function () {
                var data = new FormData();
                if ($("#txtAddConvenientType").val().trim().length == 0 || $("#txtAddConvenientType").val().trim().length > 50) {
                    var message = CreateNotifyMessage(Resources["E_B_002"], DIALOG_MESSAGE_TYPE.ERROR);
                    $("#txtAddConvenientType-error").text(message.message);
                }
                else {
                    data.append("convenientTypeName", $("#txtAddConvenientType").val())
                    CallAction("/Admin/ConvenientTypes/InsertConvenientType", "POST", data, function (response) {
                        ShowDialog(response.message, () => {
                            $("#txtAddConvenientType-error").text('');
                            $("#tableConvenientType").DataTable().ajax.reload();
                        })
                    })
                }

                $("#txtAddConvenientType").val('');
            });
        },
        deleteConvenientType: function (convenientId) {
            var data = new FormData();
            data.append("id", convenientId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_001"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/ConvenientTypes/DeleteConvenientType", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableConvenientType").DataTable().ajax.reload();
                    })
                })
            })
        },
        updateConvenientType: function (convenientId, convenientTypeName) {
            $('#oldConvenientName').val(convenientTypeName)
            $("#update-modal").modal("show");

            $('#btnUpdate').on('click', function () {
                if ($('#newConvenientName').val().trim() == '' || $('#newConvenientName').val().trim() == null || $('#newConvenientName').val().trim().length > 50) {
                    var message = CreateNotifyMessage(Resources["E_B_002"], DIALOG_MESSAGE_TYPE.ERROR);
                    $("#txtUpdateConvenientType-error").text(message.message);
                    $('#newConvenientName').val('')
                    location.reload();
                }
                else {
                    var data = new FormData();
                    data.append("id", convenientId);
                    data.append("newName", $('#newConvenientName').val().trim());
                    CallAction("/Admin/ConvenientTypes/UpdateConvenientType", "POST", data, function (response) {
                        ShowDialog(response.message, () => {
                            location.reload();
                            $('#newConvenientName').val('')
                            $("#txtUpdateConvenientType-error").text('');
                            $("#tableConvenientType").DataTable().ajax.reload();
                        })
                    })
                }
            })
        },
    },
};
