var RoomController = {
    Index: {
        urlAction: null,
        statusEnum: null,
        Init: function () {
            var statusEnum = RoomController.Index.statusEnum;
            // Common
            $("#btnCancel").click(function () {
                $("#txtRoomCodeAdd").val('');
                $("#roomTypeAddId").val(0).change();

                $("#txt-RoomCode-error").text('');
                $("#txt-RoomTypeId-error").text('');
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
                        if (data.status == 1 || data.status == 4)
                            return buildDropdown(statusEnum, data)
                        return statusEnum[data.status];
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            statusEnum[cellData.status],
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="RoomController.Index.deleteRoom(${data.roomId})"><i class="trash icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    width: "10%",
                },

            ];
            var urlAction = RoomController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns, true, initCallBack);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableRooms").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify({
                        RoomCode: $("#txtRoomCode").val(),
                        RoomTypeId: $("#roomTypeId").val(),
                        Status: $("#room_status").val(),
                    })
                ).draw();
            });

            function buildDropdown(statusEnum,data) {
                var dropdown = "<select id=" + data.roomId + ">";
                $.each(statusEnum, function (key, value) {
                    if (key == 1 || key == 4) {
                        var option = '';
                        if (key == data.status) {
                            option = "<option value=\"" + key + "\" selected>" + value + "</option>";
                        }
                        else option = "<option value=\"" + key + "\">" + value + "</option>";
                        
                        dropdown = dropdown + option;
                    }
                });
                dropdown = dropdown + "</select>";
                return dropdown;
            }
            function initCallBack() {
                $("#tableRooms td select").on('change', function () {
                    CallAction("/Admin/Room/UpdateStatus?id=" + $(this).attr('id') + "&status=" + $(this).val(), "GET", null, (response) => {
                        ShowDialog(response.message, () => { $("#tableRooms").DataTable().ajax.reload(); })
                    })
                });
            }
            $("#add-room").click(function () {
                $("#add-room-modal").modal("show");
            });
            $("#roomTypeAddId").on('change', function () {
                if ($("#roomTypeAddId :selected").val() != 0) {
                    CallAction("/Admin/Room/GetName?roomTypeId=" + $("#roomTypeAddId :selected").val(), "GET", null, (data) => {
                        $("#txtRoomCodeAdd").val($("#roomTypeAddId :selected").text() + " - R" + (data+1));
                    })
                }
                else {
                    $("#txtRoomCodeAdd").val('');
                }
            });
            $("#btnAdd").on('click', () => {
                var data = new FormData();
                data.append("RoomCode", $("#txtRoomCodeAdd").val())
                data.append("RoomTypeId", $("#roomTypeAddId :selected").val())
                CallAction("/Admin/Room/InsertRoom", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            $("#txtRoomCodeAdd").val('');
                            $("#roomTypeAddId").val(0).change();
                            $("#tableRooms").DataTable().ajax.reload();
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txt-RoomCode-error").text('');
                        $("#txt-RoomTypeId-error").text('');
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
                                $("#txtRoomCodeAdd").val('');
                                $("#roomTypeAddId").val(0).change();
                                $("#tableRooms").DataTable().ajax.reload();
                            })
                        }
                    }
                })
            })
        },
        deleteRoom: function (roomId) {
            var data = new FormData();
            data.append("id", roomId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/Room/DeleteRoom", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableRooms").DataTable().ajax.reload();
                    })
                })
            })
        },
    },
};
