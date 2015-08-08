// JavaScript source code

$("#applyId").click(function () {
    var countId = document.getElementById("countId").value;
    var productId = document.getElementById("hiddenId").value;
    var elementToUpDate = "#update" + productId;

    $.ajax({
        type: "GET",
        url: "/sale/ChangeCount",
        success: function (result) {
            $(elementToUpDate).html(result);
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        },
        data: { productId: productId, productCount: countId }
    });

   $("#modDialog").modal("hide");

});

