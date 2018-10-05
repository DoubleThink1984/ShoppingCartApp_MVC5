$(function () {
    $('.carousel').carousel({
        interval: 5000
    })

    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });

    $('#fileUpload').validate({
        rules: {
            file: {
                required: true,
            }
        }
    }); // end validate 

    $('#menu-wrap').prepend('<div id="menu-trigger"><a href="http://www.jqueryscript.net/menu/">Menu</a></div>');

    $('#menu-trigger').on('click', function () {
        $('#menu').slideToggle();
    });
      
    //Back button on Product/Details page takes user back to previous page on click
    $('#backButton').click(function () {
        parent.history.back();
        return false;
    });

    // Changes product count in shopping cart on Shoppingcart page
    $(".RemoveLink").change(function () {
        // Get the id from the link
        var recordToDelete = $(this).attr("data-id");
        var quantity = $(this).val();
        if (recordToDelete != '') {
            $.post("/ShoppingCart/ChangeItemCount",
                { "id": recordToDelete, "quantity": quantity },
                function (data) {
                    if (data.ProductQuantity == 0) {
                        $('#row-' + data.DeleteID).fadeOut('slow');
                        $('#hr-' + data.DeleteID).fadeOut('slow');
                    }
                    else {
                        $("#item-count-" + data.DeleteID).text("Quantity: " + data.ProductQuantity);
                        $("#cartItems").text("View Cart (" + data.CartQuantity + " items)");
                    }
                    if (data.CartTotal == '$0.00') {
                        $(location).attr('href', '/ShoppingCart/Index');
                    }
                    else {
                        $("#cart-total").text("Subtotal: " + data.CartTotal);
                        $("#cartTotal").text(data.CartTotal);
                    }
                });
        }
    });


    // Deletes all products from shopping cart on Shoppingcart page
    $(".DeleteLink").click(function () {
        var recordToDelete = $(this).attr("data-id");
        if (recordToDelete != '') {
            $.post("/ShoppingCart/RemoveItem",
                { "id": recordToDelete },
                function (data) {
                    $('#row-' + data.DeleteID).fadeOut('slow');
                    $('#hr-' + data.DeleteID).fadeOut('slow');
                    if (data.CartTotal == '$0.00') {
                        $(location).attr('href', '/ShoppingCart/Index');
                    }
                    else {
                        $("#cart-total").text("Subtotal: " + data.CartTotal);
                        $("#cartItems").text("View Cart (" + data.CartQuantity + " items)");
                        $("#cartTotal").text(data.CartTotal);
                    }
                });
        }
    });

    // Copies shipping address to payment address on Checkout Index page
    $('#copyInfo').click(function () {
        $("#payName").val($("#shipName").val());
        $("#payAddr").val($("#shipAddr").val());
        $("#payCountry").val($("#shipCountry").val());
        $("#payCity").val($("#shipCity").val());
        $("#payState").val($("#shipState").val());
        $("#payPostCode").val($("#shipPostCode").val());
    });

});


