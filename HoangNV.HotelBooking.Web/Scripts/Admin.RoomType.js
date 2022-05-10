var RoomTypeController = {
    Index: {
        urlAction: null,
        Init: function () {
            // Common
            $("#btnCancel").click(function () {
                $(".ui.modal").modal("hide");
            });
            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.roomTypeCode;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.roomTypeCode,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.roomTypeName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.roomTypeName,
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
                        return data.numOfRoom;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.numOfRoom,
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
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="RoomTypeController.Index.updateRoomType(${data.roomTypeId})"><i class="edit icon"></i></div>`;
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
                        return `<div class="ui icon" onclick="RoomTypeController.Index.deleteRoomType(${data.roomTypeId})"><i class="trash icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    width: "10%",
                },

            ];
            var urlAction = RoomTypeController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableRoomType").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify({
                        RoomTypeCode: $("#txtRoomTypeCode").val(),
                        RoomTypeName: $("#txtRoomTypeName").val(),
                    })
                ).draw();
            });
            $("#add-room-type").on('click', () => {
                RedirectToPage("/Admin/RoomType/Insert");
            });
        },
        deleteRoomType: function (roomTypeId) {
            var data = new FormData();
            data.append("id", roomTypeId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/RoomType/DeleteRoomType", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableRoomType").DataTable().ajax.reload();
                    })
                })
            })
        },
        updateRoomType: function (roomTypeId) {
            RedirectToPage("/Admin/RoomType/Update?id=" + roomTypeId);
        }
    },
    Insert: {
        Init: function () {
            $(".ui.dropdown").dropdown();
            $('.ui.accordion').accordion("setting", { exclusive: false });
            $('.ui.eight.item.menu .item').click(function () {
                $('.ui.menu .item').removeClass("active")
                $(this).addClass("active");
            });
            $("#Cost").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $("#Area").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $("#NumOfPer").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $(".num-of-bed").on("input", function () {
                $(this).val(+($(this).val()));
                if ($(this).val() == null || $(this).val() == '')
                    $(this).val('0');
            });
            //image
            var fileData = new FormData();

            $("#btnCancel").on('click', function () {
                RedirectToPage("/Admin/RoomType");
            });

            $(document).on('click', '.remove-image', function () {
                var nameFile = $(this).closest('.img-container').children('img').attr('id');
                var getFile = new FormData();
                var dem = 0;
                for (var value of fileData.values()) {
                    if (value.name != nameFile) {
                        getFile.append("fileInput", value);
                    }
                    else {
                        dem++;
                    }
                    if (dem > 1 && value.name == nameFile) {
                        getFile.append("fileInput", value);
                    }
                }
                fileData.delete("fileInput")
                for (var value of getFile.values()) {
                    fileData.append("fileInput", value);
                }
                $(this).closest('.img-container').remove();
            })
            $("#imgAdder").on('click', function () {
                $("#imageUploader").trigger('click');

            });
            $("#imageUploader").on('change', function (event) {
                var files = this.files;
                var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
                var i = 0,
                    len = files.length;
                (function readFile(n) {
                    var reader = new FileReader();
                    var f = files[n];
                    if (regex.test(f.name.toLowerCase())) {
                        fileData.append("fileInput", files[n]);
                        reader.onload = function (e) {
                            var dv = $('<div/>', { class: 'img-container img-div' });
                            dv.append($('<img>', { src: e.target.result, class: 'img-div img-add img-add-hotel', id: f.name }));
                            dv.append('<input type="button" value="Xóa" class="remove-image"/>');
                            $('#imgViewer').append(dv);
                            if (n < len - 1) readFile(++n)
                        };
                        reader.readAsDataURL(f);
                    }
                }(i));
            });

            $("#btnSubmit").on('click', function () {
                var data = new FormData();
                data.append("RoomTypeCode", $('#RoomTypeCode').val())
                data.append("RoomTypeName", $('#RoomTypeName').val())
                data.append("Cost", parseFloat($('#Cost').val().replace(/,/g, '')))
                data.append("Area", parseFloat($('#Area').val().replace(/,/g, '')))
                data.append("NumOfPer", parseInt($('#NumOfPer').val().replace(/,/g, '')))
                data.append("NumOfRoom", $('#NumOfRoom').val())
                data.append("Description", $('#Description').val())
                var listCheckbox = $("input:checked");
                for (var checkbox of listCheckbox) {
                    data.append("ConvenientId", $(checkbox).attr("id"));
                }
                var bedNumbers = $(".bed-count");
                for (var bedNum of bedNumbers) {
                    data.append("BedNumber",$(bedNum).attr("id").substring($(bedNum).attr("id").indexOf('-')+1) + '-' + $(bedNum).val())
                }
                for (var value of fileData.values()) {
                    data.append("ListImages", value);
                }
                CallAction("/Admin/RoomType/InsertRoomTypes", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/RoomType");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $(`#txt-RoomTypeCode-error`).text('');
                        $(`#txt-RoomTypeName-error`).text('');
                        $(`#txt-NumOfRoom-error`).text('');
                        $(`#txt-Area-error`).text('');
                        $(`#txt-NumOfPer-error`).text('');
                        $(`#txt-Cost-error`).text('');
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
                                RedirectToPage("/Admin/RoomType");
                            })
                        }
                    }
                })
                
            });

        }
    },
    Update: {
        Init: function () {
            $("#Cost").val(parseInt($("#Cost").val()).toLocaleString());
            $("#Area").val(parseInt($("#Area").val()).toLocaleString());
            $("#NumOfPer").val(parseInt($("#NumOfPer").val()).toLocaleString());
            $("#num-of-bed").val(parseInt($("#num-of-bed").val()).toLocaleString());
            $(".ui.dropdown").dropdown();
            $('.ui.accordion').accordion("setting", { exclusive: false });
            $('.ui.eight.item.menu .item').click(function () {
                $('.ui.menu .item').removeClass("active")
                $(this).addClass("active");
            });
            $("#Cost").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $("#Area").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $("#NumOfPer").on("input", function () {
                var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                $(this).val(n.toLocaleString());
                if ($(this).val() == "NaN")
                    $(this).val('0');
            });
            $("#NumOfRoom").on("input", function () {
                if ($(this).val() == "")
                    $(this).val($(this).attr('min'));
            });

            $(".num-of-bed").on("input", function () {
                $(this).val(+($(this).val()));
                if ($(this).val() == null || $(this).val() == '')
                    $(this).val('0');
            });

            var fileData = new FormData();

            $("#btnCancel").on('click', function () {
                RedirectToPage("/Admin/RoomType");
            });

            $(document).on('click', '.remove-image', function () {
                var nameFile = $(this).closest('.img-container').children('img').attr('id');
                var getFile = new FormData();
                var dem = 0;
                for (var value of fileData.values()) {
                    if (value.name != nameFile) {
                        getFile.append("fileInput", value);
                    }
                    else {
                        dem++;
                    }
                    if (dem > 1 && value.name == nameFile) {
                        getFile.append("fileInput", value);
                    }
                }
                fileData.delete("fileInput")
                for (var value of getFile.values()) {
                    fileData.append("fileInput", value);
                }
                $(this).closest('.img-container').remove();
            })
            $("#imgAdder").on('click', function () {
                $("#imageUploader").trigger('click');

            });
            $("#imageUploader").on('change', function (event) {
                var files = this.files;
                console.log(files)
                var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
                var i = 0,
                    len = files.length;
                (function readFile(n) {
                    var reader = new FileReader();
                    var f = files[n];
                    if (regex.test(f.name.toLowerCase())) {
                        fileData.append("fileInput", files[n]);
                        reader.onload = function (e) {
                            var dv = $('<div/>', { class: 'img-container img-div' });
                            dv.append($('<img>', { src: e.target.result, class: 'img-div img-add img-add-hotel', id: f.name }));
                            dv.append('<input type="button" value="Xóa" class="remove-image"/>');
                            $('#imgViewer').append(dv);
                            if (n < len - 1) readFile(++n)
                        };
                        reader.readAsDataURL(f);
                    }
                }(i));
            });

            $("#btnSubmit").on('click', function () {
                var data = new FormData();
                console.log($('#Cost').val().replace(/,/g, ''))
                data.append("RoomTypeCode", $('#RoomTypeCode').val())
                data.append("RoomTypeId", $('#RoomTypeId').val())
                data.append("RoomTypeName", $('#RoomTypeName').val())
                data.append("Cost", parseFloat($('#Cost').val().replace(/,/g, '')))
                data.append("Area", parseFloat($('#Area').val().replace(/,/g, '')))
                data.append("NumOfPer", parseInt($('#NumOfPer').val().replace(/,/g, '')))
                data.append("NumOfRoom", $('#NumOfRoom').val())
                data.append("Description", $('#Description').val())
                var listCheckbox = $("input:checked");
                for (var checkbox of listCheckbox) {
                    data.append("ConvenientId", $(checkbox).attr("id"));
                }
                var bedNumbers = $(".bed-count");
                for (var bedNum of bedNumbers) {
                    data.append("BedNumber", $(bedNum).attr("id").substring($(bedNum).attr("id").indexOf('-') + 1) + '-' + $(bedNum).val())
                }
                for (var value of fileData.values()) {
                    data.append("ListImages", value);
                }
                for (var i = 0; i < $(".img-count").length; i++) {
                    data.append("ImageLinks", $($(".img-count")[i]).attr("id"))
                }
                CallAction("/Admin/RoomType/UpdateRoomType", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/RoomType");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $(`#txt-RoomTypeCode-error`).text('');
                        $(`#txt-RoomTypeName-error`).text('');
                        $(`#txt-NumOfRoom-error`).text('');
                        $(`#txt-Area-error`).text('');
                        $(`#txt-NumOfPer-error`).text('');
                        $(`#txt-Cost-error`).text('');
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
                                RedirectToPage("/Admin/RoomType");
                            })
                        }
                    }
                })
            });
        },
    },
};
