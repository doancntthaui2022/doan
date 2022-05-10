const SYSTEM_DATE_FORMAT = "yyyy/MM/dd";
const SYSTEM_TIME_FORMAT = "hh:mm";

const PDF_FILENAME_DATE_FORMAT = "yyyyMMdd";

const RESPONSE_JSON_STATUS = {
    ERROR: 0,
    OK: 1,
};
const DIALOG_DISPLAY_TYPE = {
    MESSAGE: 0,
    CONFIRM: 1,
};

const DIALOG_MESSAGE_TYPE = {
    SUCCESS: 0,
    WARNING: 1,
    ERROR: 2,
};

const TABLE_PREFIX = {
    RESERVATION: "reserve",
};

const INTENDED_EXAMINATION_STATUS = {
    NEW: 0,
    ASSIGN: 1,
    LATCH_ASSIGN: 2,
    DONE: 3,
    CANCEL: 4
};

const RESERVATION_STATUS = {
    NotExamined: 1,
    Examined: 2,
    Canceled: 3
};

var COMMON_ERROR_AJAX = CreateNotifyMessage(Resources["W_C_006_01"], DIALOG_MESSAGE_TYPE.ERROR);
var isSearching = false;

const popUpData = new Map();

var languages = {
    sEmptyTable: Resources["sEmptyTable"],
    sInfo: Resources["sInfo"],
    sInfoEmpty: Resources["sInfoEmpty"],
    sInfoFiltered: Resources["sInfoFiltered"],
    sLengthMenu: Resources["sLengthMenu"],
    sLoadingRecords: Resources["sLoadingRecords"],
    sProcessing: Resources["sProcessing"],
    sSearch: Resources["sSearch"],
    sZeroRecords: Resources["sZeroRecords"],
    oPaginate: {
        sFirst: Resources["sFirst"],
        sLast: Resources["sLast"],
        sNext: Resources["sNext"],
        sPrevious: Resources["sPrevious"],
    },
    sPaginationType: 'ellipses'
};

function SetDatatable(urlAction, columns, async = true, drawCallBack = null) {
    return {
        processing: false,
        serverSide: true,
        ordering: false,
        dom: "rtip",
        ajax: {
            url: GetUrl(urlAction),
            type: "POST",
            async: async,
            error: function (xhr) {
                HideSearchLoading();
                ShowCallAjaxError(xhr);
            },
        },
        drawCallback: function () {
            HideSearchLoading(false);
            if (typeof drawCallBack === "function") drawCallBack();
            // Setting up popup for cell contents
            $("[data-popup-id]").each((_, ele) => {
                let eleContent = ele;
                //let eleContent = ele.firstElementChild;
                //if (!eleContent) eleContent = ele;
                if (eleContent.scrollWidth === eleContent.clientWidth && eleContent.scrollHeight === eleContent.clientHeight) return;

                //eleContent.textContent = `${popUpData.get($(ele).attr("data-popup-id")).split('\n')[0].trim()}...`;
                $(ele).popup({
                    content: popUpData.get($(ele).attr("data-popup-id")),
                    context: "#popups",
                    distanceAway: -15,
                    position: "top center",
                });
            });

            isSearching = false;
        },
        autoWidth: false,
        language: languages,
        columns: columns,
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, "All"],
        ],
        pagingType: "full_numbers",
        pageLength: 25,

    };
}

function SetOrderableDatatable(urlAction, columns, orderableInfo, async = true, drawCallBack = null) {
    return {
        processing: false,
        serverSide: true,
        ordering: orderableInfo.orderable,
        order: orderableInfo.default,
        dom: "rtip",
        ajax: {
            url: GetUrl(urlAction),
            type: "POST",
            async: async,
            error: function (xhr) {
                HideSearchLoading();
                ShowCallAjaxError(xhr);
            },
        },
        drawCallback: function () {
            HideSearchLoading(false);
            if (typeof drawCallBack === "function") drawCallBack();
            // Setting up popup for cell contents
            $("[data-popup-id]").each((_, ele) => {
                let eleContent = ele.firstElementChild;
                if (!eleContent) eleContent = ele;
                if (eleContent.scrollWidth === eleContent.clientWidth && eleContent.scrollHeight === eleContent.clientHeight) return;

                //eleContent.textContent = `${popUpData.get($(ele).attr("data-popup-id")).split('\n')[0].trim()}...`;
                $(ele).popup({
                    content: popUpData.get($(ele).attr("data-popup-id")),
                    context: "#popups",
                    distanceAway: -15,
                    position: "top center",
                });

                isSearching = false;
            });
        },
        autoWidth: false,
        language: languages,
        columns: columns,
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, "All"],
        ],
        pagingType: "full_numbers",
        pageLength: 25,
    };
}

function CreateTablePopUpContent(td, content, tableName, row, col) {
    popUpData.set(`${tableName}_${row}_${col}`, content);
    td.setAttribute("data-popup-id", `${tableName}_${row}_${col}`);
}

function ShowCallAjaxError(xhr) {
    switch (xhr.status) {
        case 401:
            var prefix = $("#prefix").data("prefix");
            var returnUrl = window.location.pathname + window.location.search;
            if (prefix) returnUrl = window.location.pathname.replace(`/${prefix}`, '') + window.location.search;
            RedirectToPage(`/Account/PreLogin?returnUrl=${encodeURIComponent(encodeURIComponent(returnUrl))}`);
            break;
        case 403:
            RedirectToPage("/handleError/403");
            break;
        case 404:
            RedirectToPage("/handleError/404");
            break;
        default:
            HideAllLoading();
            ShowMessageBox(COMMON_ERROR_AJAX);
            break;
    }
}

function FormatDate(date, format, isUTC = false) {
    if (!(date instanceof Date) || isNaN(date)) return null;
    var datetime = {
        hours: isUTC ? date.getUTCHours() : date.getHours(),
        minutes: isUTC ? date.getUTCMinutes() : date.getMinutes(),
        years: isUTC ? date.getUTCFullYear() : date.getFullYear(),
        months: isUTC ? date.getUTCMonth() + 1 : date.getMonth() + 1,
        dates: isUTC ? date.getUTCDate() : date.getDate(),
    };
    Object.keys(datetime).forEach(function (k) {
        datetime[k] = datetime[k] < 10 ? `0${datetime[k]}` : datetime[k];
    });
    switch (format) {
        case "yyyy/MM/dd hh:mm:ss":
            return (
                datetime.years +
                "/" +
                datetime.months +
                "/" +
                datetime.dates +
                " " +
                datetime.hours +
                ":" +
                datetime.minutes +
                ":00"
            );
        case "yyyy/MM/dd hh:mm":
            return (
                datetime.years +
                "/" +
                datetime.months +
                "/" +
                datetime.dates +
                " " +
                datetime.hours +
                ":" +
                datetime.minutes
            );
        case SYSTEM_DATE_FORMAT:
            return datetime.years + "/" + datetime.months + "/" + datetime.dates;
        case "yyyy/dd/MM":
            return datetime.years + "/" + datetime.dates + "/" + datetime.months;
        case "yyyy/MM/dd":
            return datetime.years + "/" + datetime.months + "/" + datetime.dates;
        case "yyyy-MM-dd":
            return datetime.years + "-" + datetime.months + "-" + datetime.dates;
        case SYSTEM_TIME_FORMAT:
            return datetime.hours + ":" + datetime.minutes;
        case "yyyy年mm月dd日":
            return `${datetime.years}年${datetime.months}月${datetime.dates}日`;
        case PDF_FILENAME_DATE_FORMAT:
            return `${datetime.years}${datetime.months}${datetime.dates}`;
        default:
            return null;
    }
}

function RedirectToPage(href) {
    window.location.href = GetUrl(href);
}

function GetObjCalendar(type, subSystem) {
    var popupClass = "hs-table-master";

    if (subSystem === "booking") {
        popupClass = "hs-table-booking";
    } else if (subSystem === "result") {
        popupClass = "hs-table-result";
    }
    var labelText = {
        days: Resources["calendarDaysText"].split(" "),
        months: Resources["calendarMonthsText"].split(" "),
        monthsShort: Resources["calendarShortMonthsText"].split(" "),
        today: Resources["calendarTodayText"],
        now: Resources["calendarNowText"],
        year: Resources["year"],
    };
    return {
        type: type,
        monthFirst: true,
        popupOptions: {
            observeChanges: false,
        },
        formatter: {
            header: function (date, mode, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                var month = date.getMonth();
                return year + labelText.year + labelText.months[month];
            },
            date: function (date, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                if (year > 9999)
                    return "9999/12/31";
                return FormatDate(date, "yyyy/MM/dd");
            },
            time: function (time, settings) {
                if (!time) return "";
                return FormatDate(time, "hh:mm");
            },
        },
        //parser: {
        //    date: function (text, settings) {
        //        if (type === "date") {
        //            var parts = text.split("/");
        //            var date = {
        //                year: parts[0],
        //                month: parts[2],
        //                day: parts[1]
        //            }
        //            var newDateStr = `${date.year}/${date.month}/${date.day}`;
        //            return new Date(newDateStr);
        //        }
        //        return new Date(`${FormatDate(new Date(), "yyyy/MM/dd")} ${text}`);
        //    }
        //},
        ampm: false,
        text: labelText,
        className: {
            popup: `ui popup ${popupClass}`,
        },
        onChange: function () {
            $(this).find($("input[check-change]")).trigger("change");
        },
    };
}

function GetObjCalendarCustomPopUp(type, subSystem, popupOptions) {
    var popupClass = "hs-table-master";

    if (subSystem === "booking") {
        popupClass = "hs-table-booking";
    } else if (subSystem === "result") {
        popupClass = "hs-table-result";
    }
    var labelText = {
        days: Resources["calendarDaysText"].split(" "),
        months: Resources["calendarMonthsText"].split(" "),
        monthsShort: Resources["calendarShortMonthsText"].split(" "),
        today: Resources["calendarTodayText"],
        now: Resources["calendarNowText"],
        year: Resources["year"],
    };
    return {
        type: type,
        monthFirst: true,
        popupOptions: popupOptions,
        formatter: {
            header: function (date, mode, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                var month = date.getMonth();
                return year + labelText.year + labelText.months[month];
            },
            date: function (date, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                if (year > 9999)
                    return "9999/12/31";
                return FormatDate(date, "yyyy/MM/dd");
            },
            time: function (time, settings) {
                if (!time) return "";
                return FormatDate(time, "hh:mm");
            },
        },
        ampm: false,
        text: labelText,
        className: {
            popup: `ui popup ${popupClass}`,
        },
        onChange: function () {
            $(this).find($("input[check-change]")).trigger("change");
        },
    };
}

function GetObjCalendarOnChange(type, subSystem, config, onChange = undefined) {
    var popupClass = "hs-table-master";
    if (subSystem === "booking") {
        popupClass = "hs-table-booking";
    } else if (subSystem === "result") {
        popupClass = "hs-table-result";
    }
    var labelText = {
        days: Resources["calendarDaysText"].split(" "),
        months: Resources["calendarMonthsText"].split(" "),
        monthsShort: Resources["calendarShortMonthsText"].split(" "),
        today: Resources["calendarTodayText"],
        now: Resources["calendarNowText"],
        year: Resources["year"],
    };

    return {
        type: type,
        minDate: config.minDate,
        maxDate: config.maxDate,
        monthFirst: true,
        formatInput: false,
        popupOptions: {
            observeChanges: false,
        },
        formatter: {
            header: function (date, mode, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                var month = date.getMonth();
                return year + labelText.year + labelText.months[month];
            },
            date: function (date, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                if (year > 9999)
                    return "9999/12/31";
                return FormatDate(date, "yyyy/MM/dd");
            },
            time: function (time, settings) {
                if (!time) return "";
                return FormatDate(time, "hh:mm");
            },
        },
        ampm: false,
        text: labelText,
        className: {
            popup: `ui popup ${popupClass}`,
        },
        onChange: function (date, text, model) {
            $(this).find($("input[check-change]")).trigger("change");
            if (onChange !== undefined) {
                onChange(this, date, text, model);
            }

        }
    };
}

function CallAction(url, method, data, callBack, async = true) {
    url = GetUrl(url);
    $.ajax({
        url: url,
        type: method,
        contentType: false,
        processData: false,
        async: async,
        data: data,
        beforeSend: ShowLoading,
        success: function (result) {
            HideLoading();
            callBack && callBack(result);
        },
        error: function (xhr) {
            HideLoading();
            ShowCallAjaxError(xhr);
        },
    });
}

function CallActionWithJson(url, method, data, callBack, async = true) {
    url = GetUrl(url);
    $.ajax({
        url: url,
        type: method,
        processData: false,
        async: async,
        data: data,
        contentType: "application/json; charset=utf-8",
        beforeSend: ShowLoading,
        success: function (result) {
            HideLoading();
            callBack && callBack(result);
        },
        error: function (xhr) {
            HideLoading();
            ShowCallAjaxError(xhr);
        },
    });
}

function ShowLoading() {
    $("#loading-modal").css("display", "block");
}

function HideLoading() {
    $("#loading-modal").css("display", "none");
}

function ShowDownloadLoading() {
    $("#download-loading-modal").css("display", "block");
}

function HideDownloadLoading() {
    $("#download-loading-modal").css("display", "none");
}

function ShowSearchLoading() {
    isSearching = true;
    $("#loading-search-modal").css("display", "block");
}

function HideSearchLoading(includeUpdateSearching = true) {
    if (includeUpdateSearching) isSearching = false;
    $("#loading-search-modal").css("display", "none");
}

function HideAllLoading() {
    HideLoading();
    HideDownloadLoading();
    HideSearchLoading();
}


function UploadFile(url, inputId, callBack, otherParams) {
    var fileUpload = $(`#${inputId}`).get(0);
    var files = fileUpload.files;

    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    // Add other parameters to FormData object
    if (otherParams) {
        for (const [key, value] of Object.entries(otherParams)) {
            fileData.append(key, value);
        }
    }

    CallAction(url, "POST", fileData, callBack);
}

function UploadFileWithInput(url, files, callBack, otherParams) {
    // Create FormData object
    var fileData = new FormData();

    // Looping over all files and add it to FormData object
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    // Add other parameters to FormData object
    if (otherParams) {
        for (const [key, value] of Object.entries(otherParams)) {
            fileData.append(key, value);
        }
    }

    CallAction(url, "POST", fileData, callBack);
}

function CompareFormData(formData1, formData2) {
    for (let [key, value] of formData1.entries()) {
        if (formData2.get(key) !== value) return false;
    }
    return true;
}

function StringToBoolean(str) {
    return str.toLowerCase() === "true";
}
function RenderStringData(data) {
    return (data ?? "").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/\n/g, "<br>").replace(/"/g, "&quot;");
}
function GetObjCalendarMinMax(type, subSystem, minDate, maxDate, onChange) {
    var popupClass = "hs-table-master";

    if (subSystem === "booking") {
        popupClass = "hs-table-booking";
    } else if (subSystem === "result") {
        popupClass = "hs-table-result";
    }
    var labelText = {
        days: Resources["calendarDaysText"].split(" "),
        months: Resources["calendarMonthsText"].split(" "),
        monthsShort: Resources["calendarShortMonthsText"].split(" "),
        today: Resources["calendarTodayText"],
        now: Resources["calendarNowText"],
        year: Resources["year"],
    };
    return {
        type: type,
        minDate: minDate,
        maxDate: maxDate,
        monthFirst: true,
        popupOptions: {
            observeChanges: false,
        },
        formatter: {
            header: function (date, mode, settings) {
                if (!date) return "";
                var year = date.getFullYear();
                var month = date.getMonth();
                return year + labelText.year + labelText.months[month];
            },
            date: function (date, settings) {
                if (!date) return "";
                return FormatDate(date, "yyyy/MM/dd");
            },
            time: function (time, settings) {
                if (!time) return "";
                return FormatDate(time, "hh:mm");
            },
        },
        ampm: false,
        text: labelText,
        className: {
            popup: `ui popup ${popupClass}`,
        },
        onChange: onChange
    };
}

function DownloadFile(downloadUrl, funcCallBack = undefined) {
    ShowDownloadLoading();
    var req = new XMLHttpRequest();
    downloadUrl = GetUrl(downloadUrl);
    req.open("GET", downloadUrl, true);
    req.responseType = "blob";
    req.onload = function (event) {
        if (req.status == 200) {
            var blob = req.response;
            if (blob.type !== "application/octet-stream") {
                RedirectToPage(`/MasterMenu/Index`);
                return;
            }
            var fileName = GetFileNameInContentDisposition(req.getResponseHeader('content-disposition'));
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
            if(funcCallBack) funcCallBack();
        }
        else ShowCallAjaxError(req);
        HideDownloadLoading();
    };
    req.send();
}

function GetFileNameInContentDisposition(disposition) {
    if (!disposition) return null;
    const utf8FilenameRegex = /filename\*=UTF-8''([\w%\-\.]+)(?:; |$)/;
    const asciiFilenameRegex = /filename=(["'])(.*?[^\\])\1(?:; |$)/;
    var fileName = "";
    if (utf8FilenameRegex.test(disposition)) {
        fileName = decodeURIComponent(utf8FilenameRegex.exec(disposition)[1]);
    } else {
        const matches = asciiFilenameRegex.exec(disposition);
        if (matches != null && matches[2]) {
            fileName = matches[2];
        }
    }
    return fileName;
}

function GetUrl(url) {
    var prefix = $("#prefix").data("prefix");
    if (prefix != undefined && prefix != null && prefix != '' && url.indexOf("/" + prefix) < 0) {
        url = "/" + prefix + url;
    }
    return url;
}

// Check if user has pasted invalid content into input type="number"
function IsValidInputNumber(domElement) {
    return domElement.validity.valid;
}

function BackPrePage() {
    if (document.referrer) window.location.href = document.referrer;
    else history.back();
}

function ObjectifyForm(formArray) {
    //serialize data function
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

function SearchAfterInit(selector) {
    var interval = setInterval(() => {
        if (isSearching) return;
        $(selector).click();
        clearInterval(interval);
    }, 100);
}

function UpdatePopUp(td, screenName, tranfer, changStatus = null) {
    let eleContent = td.firstElementChild;
    if (!eleContent) return;
    var textHTML = "";
    if (changStatus === null)
        textHTML = "<b>" + screenName + "</b>" + tranfer;
    else textHTML = changStatus + "<b>" + screenName + "</b>" + tranfer;
    $(td).popup({
        content: popUpData.get($(td).attr("data-popup-id")),
        context: "#popups",
        distanceAway: -15,
        position: "top center",
        html: textHTML,
        variation: 'wide',
    });
}

function CheckValidDate(dateStr, minDate = null, maxDate = null) {
    if (isNaN(Date.parse(dateStr)) == false) {
        let date = new Date(dateStr);
        let isValid = true;
        if (minDate) {
            isValid = isValid && date >= minDate;
        }
        if (maxDate) {
            isValid = isValid && date <= maxDate;
        }
        return isValid;
    } else {
        return false;
    }
}

function DisabledDateOfCalendar(calendarObj, validDates) {
    let validDateStrs = [];
    for (var i = 0; i < validDates.length; i++) {
        validDateStrs.push(FormatDate(new Date(validDates[i]), SYSTEM_DATE_FORMAT));
    }
    calendarObj.isDisabled = function (date, mode) {
        return !validDateStrs.includes(FormatDate(date, SYSTEM_DATE_FORMAT));
    };
    return calendarObj;
}