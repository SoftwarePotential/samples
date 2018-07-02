// Exists - to check if an element exists on the page.
$.fn.exists = function () { return this.length > 0; };

$(document).ready(function () {

    $('a.expander').live('click', function () {

        var ctrls = $('#' + $(this).attr('data-target'));
        ctrls.toggleClass('hidden');

        if ($(this).text() == $(this).attr('data-wordmore'))
            $(this).text($(this).attr('data-wordless'));
        else
            $(this).text($(this).attr('data-wordmore'));
    });

    $("form").submit(function () {
        var b = $("input[type=submit]", this);
        b.attr('disabled', 'disabled');
    });

    // links to info dialogs
    $(".button.info.mini").button({ icons: { primary: 'ui-icon-info' }, text: false });

    // more info boxes: a div with an 'a' to trigger and an 'info_text' to hold content
    if ($('#more_info').exists()) {

        $('#more_info').css('display', 'inline');
        $('#info_text').dialog({ autoOpen: false, position: "center", title: "Help" });
        $('#more_info .button').on('click', function () {
            $('#info_text').dialog('open');
        });


        $(".button.help.mini").button({ icons: { primary: 'ui-icon-help' }, text: false });
    }

    $("#save_file_button").attr("disabled", !$("#ManualActivationRequest").val());

    displayNamedUserLink = function () {
        if (window.localStorage.hasOwnProperty("hasNamedUserLicenses")) {
            var hasNamedUserLicenses = JSON.parse(window.localStorage.getItem("hasNamedUserLicenses"));
            if (hasNamedUserLicenses) {
                $('#namedUserLink').show();
            }
            else {
                $('#namedUserLink').hide();
            }
        }
        else {
            getNamedUsersProducts();
        }
    }

    getNamedUsersProducts = function () {
        var namedUserUrl = "http://" + window.location.host + "/Web/NamedUser";

        $.get(namedUserUrl).success(function (data) {
            productList = { products: JSON.parse(data) };
            window.localStorage.setItem("hasNamedUserLicenses", JSON.stringify(productList.products.length != 0))
            displayNamedUserLink();
        });
    }

    $('#activateLicense').click(function () {
        clearHasNamedUserLicenses()
    });

    $('#installLicense').click(function () {
        clearHasNamedUserLicenses()
    });

    clearHasNamedUserLicenses = function () {
        // Removed cached named user value to allow for first time activation of a named user license displaying menu
        window.localStorage.removeItem("hasNamedUserLicenses");
    }

    displayNamedUserLink();
});