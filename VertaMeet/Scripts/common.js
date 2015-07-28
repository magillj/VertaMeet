/* Common JavaScript functions to either be run on or accessable to every page */

function setCookie(name, value, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = name + "=" + value + "; " + expires;
};

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
};

function attachPostRequest(formSelector, url, successCallback, errorCallback) {
    $(formSelector).on("submit", function (e) {
        var postData = $(formSelector).serializeArray();
        $.ajax(
        {
            url: url,
            type: "POST",
            data: postData,
            mimeType: "Application/JSON",
            success: successCallback,
            error: errorCallback
        });

        e.preventDefault();
    });
};

// Function run on every page to ensure user has a login cookie
// This way of logging in is totally unsafe and temporary, for our product demo
function checkCookie() {
    if (!getCookie("userId")) {
        // Ask them to sign up, then we'll assign a cookie
        $("#login-modal").display = "block";
        attachPostRequest("#initial-login-form", "api/User/CreateUser", function (data, textStatus, jqXHR) {
            $("#login-modal").display = "none";
            setCookie("userId", data.Id, 9999);
            setCookie("userName", data.Name, 9999);
            // Doing this avoids reloading the page
            $("current-user-name").html(data.name);
        }, function () {
            alert("An error occurred while processing your login. Please try again or refresh.");
        });
    }
};

// Run functions
checkCookie();