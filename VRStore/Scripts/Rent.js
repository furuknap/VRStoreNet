$(document).ready(function () {
    getPrice();
});

$("#days").on("click", function (e) {
    $("#rentButton").attr("disabled", "disabled");
    getPrice();
}
);

// I've opted to get the price from the server so I don't have to store prices in multiple locations.
function getPrice() {
    $.ajax({
        url: "/Videos/GetPrice?videoID=" + $("#ID").val() + "&days=" + $("#days").val()
    }).done(function (response) {
        $("#price").html(response);
        $("#rentButton").removeAttr("disabled");

    });
}

$("#rentButton").on("click", function () {
    var formData = new FormData();

    formData.append("ID", $("#ID").val());
    formData.append("Days", $("#days").val());
    var data = JSON.stringify({
        ID: $("#ID").val(),
        Days: $("#days").val()
    });

    $.ajax({
        url: "/Videos/Rent",
        type: "POST",
        data: data,
        async: true,
        contentType: 'application/json',
        processData: false,
        error: function (a) {
            alert("We're sorry, but there was a problem renting this video. This may be because the last copy was just rented.");
        }
    }).done(function (response) {
        $("#price").html("Rented");
        $("#rentButton").attr("disabled", "disabled");
  });
})