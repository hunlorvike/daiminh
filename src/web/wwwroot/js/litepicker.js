const LitepickerManager = {
    init: function () {
        const $datepickerList = $('.datepicker');

        const litepickerList = $datepickerList.map(function () {
            return new Litepicker({
                element: this,
                buttonText: {
                    previousMonth: `<i class="ti ti-chevron-left"></i>`,
                    nextMonth: `<i class="ti ti-chevron-right"></i>`,
                },
            });
        }).get();

        return litepickerList;
    }
};

$(function () {
    LitepickerManager.init();
});
