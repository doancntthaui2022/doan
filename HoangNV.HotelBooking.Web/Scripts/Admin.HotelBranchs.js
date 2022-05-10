var HotelBranchsController = {
    Index: {
        urlAction: null,
        statusEnum: null,
        Init: function () {
            var statusEnum = HotelBranchsController.Index.statusEnum;

            // Common
            $("#btnCancel").click(function () {
                //$("#txtAddBeds").val('');
                //$("#txtAddBedType-error").text('');
                $(".ui.modal").modal("hide");
            });
            $("#btnCancel1").click(function () {
                location.reload();
            });
            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.hotelBranchCode;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.hotelBranchCode,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.hotelBranchName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.hotelBranchName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
                        return data.address;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.address,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                },
                {
                    data: null,
                    render: function (data) {
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
                        return `<div class="ui icon" onclick="HotelBranchsController.Index.updateHotel(${data.hotelBranchId})"><i class="edit icon"></i></div>`;
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
                        return `<div class="ui icon" onclick="HotelBranchsController.Index.deleteHotel(${data.hotelBranchId})"><i class="trash icon"></i></div>`;
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    width: "10%",
                },

            ];
            var urlAction = HotelBranchsController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#tableHotelBranchs").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify({
                        HotelBranchName: $("#txt_hotelbranch_name").val(),
                        HotelBranchCode: $("#txt_hotelbranch_code").val(),
                        Address: $("#txt_hotel_address").val(),
                        Status: $("#hotel_status").val(),
                    })
                ).draw();
            });

        },
        deleteHotel: function (hotelId) {
            var data = new FormData();
            data.append("id", hotelId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/HotelBranchs/DeleteHotelBranch", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#tableHotelBranchs").DataTable().ajax.reload();
                    })
                })
            })
        },
        updateHotel: function (hotelId) {
            RedirectToPage("/Admin/HotelBranchs/GetUpdate?id=" + hotelId);
        }
    },
    Insert: {
        Init: function () {
            var fileData = new FormData();
            
            $("#btnCancel").on('click', function () {
                RedirectToPage("/Admin/HotelBranchs");
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
                            dv.append($('<img>', { src: e.target.result, class: 'img-div img-add img-add-hotel',id: f.name }));
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
                data.append("HotelBranchName", $('#HotelBranchName').val())
                data.append("HotelBranchCode", $('#HotelBranchCode').val())
                data.append("Address", $('#Address').val())
                data.append("Description", $('#Description').val())
                for (var value of fileData.values()) {
                    data.append("ListImages", value);
                }
                CallAction("/Admin/HotelBranchs/InsertHotelBranchs", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/GetUpdate");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $(`#txt-HotelBranchCode-error`).text('');
                        $(`#txt-HotelBranchName-error`).text('');
                        $(`#txt-Address-error`).text('');
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
                                RedirectToPage("/Admin/GetUpdate");
                            })
                        }
                    }
                })
            });
        },
    },
    Update: {
        Init: function () {
            var fileData = new FormData();

            $("#btnCancel").on('click', function () {
                RedirectToPage("/Admin/Home");
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
                data.append("HotelBranchId", $('#HotelBranchId').val())
                data.append("HotelBranchName", $('#HotelBranchName').val())
                data.append("HotelBranchCode", $('#HotelBranchCode').val())
                data.append("Address", $('#Address').val())
                data.append("Email", $('#Email').val())
                data.append("PhoneNumber", $('#PhoneNumber').val())
                data.append("Description", $('#Description').val())
                for (var value of fileData.values()) {
                    data.append("ListImages", value);
                }
                console.log($(".img-count"))
                for (var i = 0; i < $(".img-count").length; i++) {
                    data.append("ImageLinks",$($(".img-count")[i]).attr("id"))
                }
                CallAction("/Admin/HotelBranchs/UpdateHotelBranch", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/HotelBranchs/GetUpdate");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $(`#txt-HotelBranchCode-error`).text('');
                        $(`#txt-HotelBranchName-error`).text('');
                        $(`#txt-Address-error`).text('');
                        $(`#txt-PhoneNumber-error`).text('');
                        $(`#txt-Email-error`).text('');
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
                                RedirectToPage("/Admin/HotelBranchs/GetUpdate");
                            })
                        }
                    }
                })
            });
        },
    },
};
