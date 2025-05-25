const TomSelectManager = {
    init: function () {
        const $tomSelectElements = $('.tom-selected');

        $tomSelectElements.each((index, el) => {
            new TomSelect(el, {
                copyClassesToDropdown: false,
                dropdownParent: 'body',
                controlInput: '<input>',
                render: {
                    item: function (data, escape) {
                        if (data.customProperties) {
                            return `<div><span class="dropdown-item-indicator">${data.customProperties}</span>${escape(data.text)}</div>`;
                        }
                        return `<div>${escape(data.text)}</div>`;
                    },
                    option: function (data, escape) {
                        if (data.customProperties) {
                            return `<div><span class="dropdown-item-indicator">${data.customProperties}</span>${escape(data.text)}</div>`;
                        }
                        return `<div>${escape(data.text)}</div>`;
                    },
                },
            });

            el.addEventListener('change', function () {
                if (el.tomselect) {
                    el.tomselect.setValue(this.value, true);
                }
            });
            el.addEventListener('input', function () {
                if (el.tomselect) {
                    el.tomselect.setValue(this.value, true);
                }
            });
        });

        setTimeout(() => {
            $tomSelectElements.each((index, el) => {
                if (el.tomselect) {
                    el.tomselect.setValue(el.value, true);
                }
            });
        }, 300);
    },
};

$(function () {
    TomSelectManager.init();
});
