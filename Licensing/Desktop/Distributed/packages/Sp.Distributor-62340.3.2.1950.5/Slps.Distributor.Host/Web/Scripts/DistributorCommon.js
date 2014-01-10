
$(document).ready(function () {

    $('a.expander').live('click', function () {

        var ctrls = $('#' + $(this).attr('data-target'));
        ctrls.toggleClass('hidden');

        if ($(this).text() == $(this).attr('data-wordmore'))
        	$(this).text( $(this).attr('data-wordless') );
        else
        	$(this).text( $(this).attr('data-wordmore') );
    });

    $("form").submit(function () {
        var b = $("input[type=submit]", this);
        b.attr('disabled', 'disabled');   
    });
});