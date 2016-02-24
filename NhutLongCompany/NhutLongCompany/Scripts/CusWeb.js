jQuery(document).ready(function ($) {

    $(window).scroll(function () {

        var scrollTop = $(window).scrollTop();
        if ($('.favorite-fcsgroup').length > 0) {


            var elementOffset = $('.favorite-fcsgroup').offset().top;
            var distance = (elementOffset - scrollTop);

            // document.title = distance;

            if (distance < 30) {

                $('.cussticky').css('position', 'fixed').css('top', '0').removeClass('navbar-fixed-top').addClass('navbar-fixed-top').css("padding-top", "70px");
                $('.cussticky').removeClass('hidden');
            } else {

                $('.cussticky').css('position', 'static').removeClass('navbar-fixed-top').css("padding-top", "0px");
                $('.cussticky').addClass('hidden');
            }
        }

    });
});