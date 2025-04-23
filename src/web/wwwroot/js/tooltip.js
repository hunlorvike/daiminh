const TooltipManager = {
    init: function () {
        const $tooltipTriggerList = $('[title]');

        const tooltipList = $tooltipTriggerList.map(function () {
            return new tabler.Tooltip(this);
        }).get();

        return tooltipList;
    }
};

$(function () {
    TooltipManager.init();
});
