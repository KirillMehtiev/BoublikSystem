// JavaScript source code
$(function () {

    $("#applyId").click(function () {
        var countId = $("#countId").value;
        var productId = $("#hiddenId").value;
        $.ajax({
            url: "/sale/ChnageCount",
            success: function (result) {
                $("#selectedProducts2").html(result);
            },
            data: JSON.stringify({ productId: productId, productCount: countId })
            //dataType: 'json'
        });
    });

    $('#applyId').click(function () {
        $('#modDialog').modal('hide');
    });
});
