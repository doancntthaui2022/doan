function ShowDialog(message, yesCallBack, noCallBack) {
    if (message.displayType === 0) ShowMessageBox(message, yesCallBack);
    else ConfirmDialog(message, yesCallBack, noCallBack);
}

function ShowMessageBox(message, callback) {
    initUI(message);
    $("#message-box-modal .ui.button").click(function () {
        if (callback !== undefined) {
            callback();
        }
        $("#message-box-modal").modal("hide");
    });
}

function ConfirmDialog(message, yesCallBack, noCallBack) {
    initUI(message);
    $("#confirm-dialog .ui.button").click(function () {
        switch ($(this).data("value")) {
            case "Yes":
                if (yesCallBack !== undefined) {
                    yesCallBack();
                }
                break;
            case "No":
                if (noCallBack !== undefined) {
                    noCallBack();
                }
                break;
        }
        $("#confirm-dialog").modal("hide");
    });
}

function initUI(message) {
    var header = GetHeaderStyle(message.messageType);
    var popUpId =
        message.displayType === 0 ? "message-box-modal" : "confirm-dialog";
    clearUI(popUpId);

    $(`#${popUpId} #icon`).addClass(header.iconClass);
    $(`#${popUpId} .header`).attr("style", `background-color: ${header.color}`);
    $(`#${popUpId} #header-content`).text(header.headerContent);
    $(`#${popUpId} #message`).html(message.message);
    $(`#${popUpId} .ui.button`).attr(
        "style",
        `background-color: ${header.color}`
    );
    $(`#${popUpId}`).modal("show");
}

function clearUI(popUpId) {
    $(`#${popUpId} #icon`).removeAttr("class");
    $(`#${popUpId} .ui.button`).unbind();
}

function GetHeaderStyle(messageType) {
    switch (messageType) {
        case DIALOG_MESSAGE_TYPE.SUCCESS:
            return {
                iconClass: "info circle large icon",
                color: "#0275d8",
                headerContent: Resources["Header Success Message"],
            };
            break;
        case DIALOG_MESSAGE_TYPE.WARNING:
            return {
                iconClass: "exclamation triangle large icon",
                color: "#f0ad4e",
                headerContent: Resources["Header Warning Message"],
            };
            break;
        case DIALOG_MESSAGE_TYPE.ERROR:
            return {
                iconClass: "minus circle large icon",
                color: "#d9534f",
                headerContent: Resources["Header Error Message"],
            };
        default:
            return {
                iconClass: "",
                color: "",
                headerContent: "",
            };
    }
}

function CreateMessage(message, type, displayType) {
    return {
        message: message,
        displayType: displayType,
        messageType: type,
    };
}

function CreateConfirmMessage(message, type) {
    return CreateMessage(message, type, DIALOG_DISPLAY_TYPE.CONFIRM);
}

function CreateNotifyMessage(message, type) {
    return CreateMessage(message, type, DIALOG_DISPLAY_TYPE.MESSAGE);
}

function ShowMultiDialog(messages, i = 0, yesCallBack = undefined, noCallBack = undefined) {
    if (messages[i + 1]) {
        ShowDialog(messages[i], () => { setTimeout(() => { ShowMultiDialog(messages, i + 1, yesCallBack, noCallBack); }, 100) }, noCallBack);
    }
    else {
        ShowDialog(messages[i], yesCallBack, noCallBack);
    }
}
