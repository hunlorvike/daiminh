const TomSelectManager = {
    init: function () {
        const $tomSelectElements = $('.tom-selected');

        $tomSelectElements.each((index, el) => {
            new TomSelect(el, {});
        });
    },
};

$(function () {
    TomSelectManager.init();
});
