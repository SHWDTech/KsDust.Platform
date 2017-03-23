var BaseInfo = {};

BaseInfo.IsIE = false;
BaseInfo.IsIE6 = false;
BaseInfo.IsIE7 = false;
BaseInfo.IsIE8 = false;

$(function () {
    BaseInfo.IsIE = (!!window.ActiveXObject || "ActiveXObject" in window);
    if (BaseInfo.IsIE) {
        BaseInfo.IsIE6 = navigator.appVersion.match(/6./i) === "6.";
        BaseInfo.IsIE7 = navigator.appVersion.match(/7./i) === "7.";
    }
});

var trimStr = function (str) {
    var re = /^\s+|\s+$/;
    return !str ? "" : str.replace(re, "");
}

var IsNullOrEmpty = function (obj) {
    try {
        if (obj.length === 0) {
            return true;
        }

        return false;
    }
    catch (e) {
        if (!obj) {
            return true;
        }
        if (typeof obj == "string" && trimStr(obj) === "") {
            return true;
        }

        if (typeof obj == "number" && isNaN(obj))
            return true;

        return false;
    }
}

var IsShow = function (targetId, targetClass) {
    if (targetId !== null) {
        return ($('#' + targetId).css('display') === 'none');
    } else {
        return ($('.' + targetClass).css('display') === 'none');
    }
};

// ReSharper disable once NativeTypePrototypeExtending
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (o.hasOwnProperty(k))
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1,
                    (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}