window.onload = function () {
    $("#create_user").on("submit", function (e) {
        var postData = $("#create_user").serializeArray();
        $.ajax(
        {
            url: "api/User",
            type: "POST",
            data: postData,
            mimeType: "Application/JSON",
            success: function (data, textStatus, jqXHR) {
                alert("User successfully created");
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("User creation failed. Error: " + errorThrown);
            }
        });

        e.preventDefault();
    });
};