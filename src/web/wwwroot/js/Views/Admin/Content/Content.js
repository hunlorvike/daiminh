$(document).ready(function () {
    $(document).on('shown.bs.modal', '#create-content--modal, #edit-content--modal', function (e) {
        let optionsSummernote = {
            lang: 'vi-VN',
            placeholder: 'Nhập nội dung bài viết',
            tabsize: 2,
            height: 300,
            minHeight: 200,
            maxHeight: 500
        };
        $('#ContentBody').summernote(optionsSummernote);

        $('.tom-select-tags').each(function () {
            if (this.tomselect) {
                this.tomselect.destroy();
            }
        });

        $('.tom-select-tags').each(function () {
            new TomSelect(this, {
                plugins: ['remove_button', 'clear_button'],
                maxItems: null,
                create: false,
                allowEmptyOption: false,
                closeAfterSelect: false,
                hidePlaceholder: true,
                selectOnTab: true,
            });
        });
    });
});
