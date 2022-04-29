document.addEventListener("DOMContentLoaded", function () {
    function bindConnectionMessage(connection) {
        var messageCallback = function (message) {
            console.log("message" + message);
            if (!message) return;
            var userCountSpan = document.getElementById("users");
            userCountSpan.innerText = message;
        };
        connection.on("updateCount", messageCallback);
        connection.onclose(onConnectionError);
    }
    function onConnected(connection) {
        console.log("connection started");
    }
    function onConnectionError(error) {
        if (error && error.message) {
            console.error(error.message);
        }
    }
    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    bindConnectionMessage(connection);
    connection
        .start()
        .then(function () {
            onConnected(connection);
        })
        .catch(function (error) {
            console.error(error.message);
        });
});