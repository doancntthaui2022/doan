var AccountController = {
    Index: {
        urlAction: null,
        Init: function () {
            $(".ui.dropdown").dropdown({
                fullTextSearch: true,
                match: "text",
                message: { noResults: "" },
            });
            $(".ui.search.selection.dropdown input").prop("maxLength", 50);
            $("#btnCancel").click(function () {
                $(".ui.modal").modal("hide");
            });

            var columns = [
                {
                    data: null,
                    render: function (data) {
                        return data.userName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.userName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                    width: "20%",
                },
                {
                    data: null,
                    render: function (data) {
                        return data.fullName;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.fullName,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                    width: "25%",
                },
                {
                    data: null,
                    render: function (data) {
                        return data.email;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.email,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                    width: "20%",
                },
                {
                    data: null,
                    render: function (data) {
                        return data.role;
                    },
                    className: "hs-overflow-ellipsis",
                    createdCell: function (td, cellData, _rowData, row, col) {
                        CreateTablePopUpContent(
                            td,
                            cellData.role,
                            TABLE_PREFIX.CONVENIENT_TYPE,
                            row,
                            col
                        );
                    },
                    width: "20%",
                },
                {
                    data: null,
                    render: function (data) {
                        return `<div class="ui icon" onclick="AccountController.Index.deleteAccount(\'${data.userId}\')"><i class="trash icon"></i></div>`;
                    },
                    createdCell: function (td, cellData, _rowData, row, col) {
                        UpdatePopUp(td, '', Resources["Delete"])
                    },
                    className: "hs-overflow-ellipsis  dt-body-center hs-delete-convenient-type",
                    width: "10%",
                },

            ];
            var urlAction = AccountController.Index.urlAction;
            const dataTablesConfig = SetDatatable(urlAction, columns);
            dataTablesConfig.language.sEmptyTable = null;
            var table = $("#table-account").DataTable(dataTablesConfig);

            $("#btnSearch").on('click', function () {
                ShowSearchLoading();
                table.search(
                    JSON.stringify({
                        UserName: $('#txtUserName').val(),
                        FullName: $("#txtFullName").val(),
                        Role: $("#role").val(),
                    })
                ).draw();
            });

            $("#add-user").on('click', () => {
                RedirectToPage("/Admin/Account/Insert")
            });

        },
        deleteAccount: function (accountId) {
            var data = new FormData();
            data.append("id", accountId)
            ShowDialog(CreateConfirmMessage(`${Resources["C_R_002"]}`, DIALOG_MESSAGE_TYPE.WARNING), () => {
                CallAction("/Admin/Account/DeleteAccount", "POST", data, function (response) {
                    ShowDialog(response.message, () => {
                        $("#table-account").DataTable().ajax.reload();
                    })
                })
            })

        }

        
    },
    Insert: {
        Init: function () {
            $("#btnCancel").on('click', () => {
                RedirectToPage("/Admin/Account")
            });

            $("#btnSubmit").on('click', () => {
                var data = new FormData();
                data.append("FullName", $('#FullName').val())
                data.append("Email", $('#Email').val())
                data.append("UserName", $('#UserName').val())
                data.append("PassWord", $('#PassWord').val())
                data.append("PassWordSecond", $('#PassWordSecond').val())
                data.append("RoleId", $('#RoleId').val())
                CallAction("/Admin/Account/Insert", "POST", data, function (response) {
                    console.log(response)
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {

                            RedirectToPage("/Admin/Account");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txt-FullName-error").text('');
                        $("#txt-Email-error").text('');
                        $("#txt-UserName-error").text('');
                        $("#txt-PassWord-error").text('');
                        $("#txt-PassWordSecond-error").text('');
                        $("#txt-role-error").text('');
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
                                $("#txt-FullName-error").text('');
                                $("#txt-Email-error").text('');
                                $("#txt-UserName-error").text('');
                                $("#txt-PassWord-error").text('');
                                $("#txt-PassWordSecond-error").text('');
                                $("#txt-role-error").text('');
                                RedirectToPage("/Admin/Account");
                            })
                        }
                    }
                })
            })
        },
        
    },
    Detail: {
        Init: function () {
            $("#btnCancel").on('click', () => {
                RedirectToPage("/Admin/Home")
            });

            $("#btnSubmit").on('click', () => {
                console.log('a')
                var data = new FormData();
                data.append("FullName", $('#FullName').val())
                data.append("UserId", $('#UserId').val())
                data.append("Email", $('#Email').val())
                data.append("UserName", $('#UserName').val())
                CallAction("/Admin/Account/Update", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/Home");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txt-FullName-error").text('');
                        $("#txt-Email-error").text('');
                        $("#txt-UserName-error").text('');
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
                                $("#txt-FullName-error").text('');
                                $("#txt-Email-error").text('');
                                $("#txt-UserName-error").text('');
                                RedirectToPage("/Admin/Home");
                            })
                        }
                    }
                })
            })
        },

    },
    UpdatePassWord: {
        Init: function () {
            $("#btnCancel").on('click', () => {
                RedirectToPage("/Admin/Home")
            });

            $("#btnSubmit").on('click', () => {
                console.log('a')
                var data = new FormData();
                data.append("OldPassWord", $('#OldPassWord').val())
                data.append("UserId", $('#UserId').val())
                data.append("PassWord", $('#PassWord').val())
                data.append("PassWordSecond", $('#PassWordSecond').val())
                CallAction("/Admin/Account/UpdatePassword", "POST", data, function (response) {
                    if (response.status == RESPONSE_JSON_STATUS.OK) {
                        ShowDialog(response.message, () => {
                            RedirectToPage("/Admin/Home");
                        })
                    }
                    if (response.message.messageType == RESPONSE_JSON_STATUS.ERROR) {
                        $("#txt-OldPassWord-error").text('');
                        $("#txt-PassWord-error").text('');
                        $("#txt-PassWordSecond-error").text('');
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
                                $("#txt-OldPassWord-error").text('');
                                $("#txt-PassWord-error").text('');
                                $("#txt-PassWordSecond-error").text('');
                                RedirectToPage("/Admin/Home");
                            })
                        }
                    }
                })
            })
        },

    },
};
