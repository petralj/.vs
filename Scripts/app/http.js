function Http() {
    var self = this;

    self.constructURI = function constructURI(link, data) {
        var keys = Object.keys(data);
        if (keys.length > 0) {
            for (var i = 0; i < keys.length; i++) {
                if (i > 0) {
                    link += "?" + keys[i] + "=" + data[keys[i]];
                }
                else {
                    link += "&" + keys[i] + "=" + data[keys[i]];
                }
            }
        }
    };

    self.post = function post(link, postData, successCallback, errorCallback) {
        $.ajax({
            url: link,
            type: "POST",
            data: JSON.stringify(postData),
            contentType: "application/json;charset=utf-8",
            success: successCallback,
            error: errorCallback
        });
    };

    self.get = function post(link, data, successCallback, errorCallback) {
        self.constructURI(link, data);
        $.ajax({
            url: link,
            type: "GET",
            success: successCallback,
            error: errorCallback
        });
    };
}
var http = new Http();

