$(document).ready(function () {
    getNamedUsersProducts = function () {
        $.get("NamedUser").success(function (data) {
            productList = { products: JSON.parse(data) };
            var productTemplate = $('#product-template').html();
            var renderedProducts = Mustache.render(productTemplate, productList);
            $('#productList').html(renderedProducts);
            if (productList.products.length == 0) {
                $('#noProducts').show();
            }
            else {
                $('#noProducts').hide();
            }
        }).error(function (response) {
            console.log(response.responseText);
        });
    }
    getNamedUsersProducts();
});