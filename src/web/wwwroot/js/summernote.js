const SummernoteManager = {
    createMediaBrowserButton: function (context) {
        const ui = $.summernote.ui;
        const button = ui.button({
            contents: '<i class="note-icon-picture"></i>',
            tooltip: 'Chọn ảnh từ thư viện',
            click: function () {
                toastr.error("TODO: Open media");
            }
        });
        return button.render();
    },

    getSummernoteConfig: function () {
        return {
            placeholder: 'Nhập nội dung của bạn ở đây...',
            tabsize: 2,
            height: 300,
            lang: 'vi-VN',
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['fontname', ['fontname']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'video']],
                ['view', ['fullscreen', 'codeview', 'help']],
                ['fontsize', ['fontsize']],
                ['height', ['height']],
                ['misc', ['undo', 'redo']],
                ['format', ['strikethrough', 'superscript', 'subscript']],
                ['hr', ['hr']],
                ['custom', ['mediaBrowser']]
            ],
            buttons: {
                mediaBrowser: this.createMediaBrowserButton
            },
            popover: {
                image: [
                    ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                    ['float', ['floatLeft', 'floatRight', 'floatNone']],
                    ['remove', ['removeMedia']]
                ],
                link: [
                    ['link', ['linkDialogShow', 'unlink']]
                ],
                table: [
                    ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
                    ['delete', ['deleteRow', 'deleteCol', 'deleteTable']]
                ],
                air: [
                    ['color', ['color']],
                    ['font', ['bold', 'underline', 'clear']],
                    ['para', ['ul', 'paragraph']],
                    ['table', ['table']],
                    ['insert', ['link', 'picture']]
                ]
            },
            callbacks: {
                onBlur: function () {
                    const $editor = $(this);
                    if ($editor.summernote('isEmpty') || $editor.summernote('code') === '<p><br></p>') {
                        $editor.summernote('code', '');
                    }
                }
            }
        };
    },

    init: function () {
        const $summernoteElements = $('.summernote');

        $summernoteElements.each((index, element) => {
            const $element = $(element);
            let editorId = $element.attr('id');
            if (!editorId) {
                editorId = `summernote-${index}`;
                $element.attr('id', editorId);
            }

            const $container = $('<div>', {
                class: 'summernote-container',
                id: `container-${editorId}`
            });

            $element.before($container);
            $container.append($element);

            $element.summernote(this.getSummernoteConfig());
        });
    }
};

$(function () {
    SummernoteManager.init();
});
