const BackToTop = {
    init: function () {
        const $backToTopButton = $('.back-to-top');
        if ($backToTopButton.length) {
            $backToTopButton.on('click', function () {
                $('html, body').animate({ scrollTop: 0 }, 'smooth');
            });
        }
    }
};

$(document).ready(function () {
    BackToTop.init();
});