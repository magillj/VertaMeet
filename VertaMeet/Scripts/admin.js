var defaultFailFunction = function (jqXHR, textStatus, errorThrown) {
    alert("Failed: " + jqXHR.responseText);
}

window.onload = function () {

    attachPostRequest("#create_user", "api/User/CreateUser", function (data, textStatus, jqXHR) {
        alert("User successfully created");
        location.reload();
    }, defaultFailFunction);

    attachPostRequest("#delete_user", "api/User/DeleteUser", function (data, textStatus, jqXHR) {
        alert("User was successfully deleted");
        location.reload();
    }, defaultFailFunction);

    attachPostRequest("#create_interestgroup", "api/interestgroup/CreateInterestGroup", function (data, textStatus, jqXHR) {
        alert("Interest group successfully created");
        location.reload();
    }, defaultFailFunction);

    attachPostRequest("#adduserto_interestgroup", "api/interestgroup/AddUserToInterestGroup", function (data, textStatus, jqXHR) {
        alert("User was successfully added");
        location.reload();
    }, defaultFailFunction); 

    attachPostRequest("#create_event", "api/Event/CreateEvent", function (data, textStatus, jqXHR) {
        alert("Event was successfully added");
        location.reload();
    }, defaultFailFunction);

    attachPostRequest("#adduserto_event", "api/Event/AddUserToEvent", function (data, textStatus, jqXHR) {
        alert("User was successfully added");
        location.reload();
    }, defaultFailFunction);
};

