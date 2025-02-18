(function (factory) {
    if (typeof module === "object" && module.exports) {
        module.exports = factory(require("jquery"));
    } else {
        factory(jQuery);
    }
})(function ($) {
    $.fn.toggleCommandDisabled = function (disabled) {
        return this.each(function () {
            let $command = $(this);
            if (disabled) {
                $command.prop('disabled', true);
            } else {
                setTimeout(() => {
                    $command.prop('disabled', false);
                }, 500);
            }
        });
    };

    return $;
});