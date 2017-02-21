$(function () {
    function ratingEnable() {





        $('#example-css').barrating({
            theme: 'css-stars',
            showSelectedRating: false
        });

   




    }

    function ratingDisable() {
        $('select').barrating('destroy');
    }

    $('.rating-enable').click(function (event) {
        event.preventDefault();

        ratingEnable();

        $(this).addClass('deactivated');
        $('.rating-disable').removeClass('deactivated');
    });

    $('.rating-disable').click(function (event) {
        event.preventDefault();

        ratingDisable();

        $(this).addClass('deactivated');
        $('.rating-enable').removeClass('deactivated');
    });

    ratingEnable();
});
