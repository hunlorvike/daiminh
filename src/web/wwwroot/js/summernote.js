$(document).ready(function () {
    $('.summernote').summernote({
        placeholder: 'Nhập nội dung của bạn ở đây...',
        tabsize: 2,
        height: 300,
        lang: 'vi-VN',
        toolbarContainer: '.note-toolbar',
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
            mediaBrowser: function (context) {
                var ui = $.summernote.ui;
                var button = ui.button({
                    contents: '<i class="note-icon-picture"></i>',
                    tooltip: 'Chọn ảnh từ thư viện',
                    click: function () {
                        toastr.error("TODO: Open media");
                    }
                });
                return button.render();
            }
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
        }
    });

    $('<style>.note-toolbar { display: flex; flex-wrap: wrap; } .note-toolbar > .note-btn-group { margin-bottom: 5px; }</style>').appendTo('head');
});