$(document).ready(function () {
// Khởi tạo Summernote cho từng phần tử
$('.summernote').each(function (index) {
    // Tạo id duy nhất cho mỗi editor nếu chưa có
    var editorId = $(this).attr('id');
    if (!editorId) {
        editorId = 'summernote-' + index;
        $(this).attr('id', editorId);
    }

    // Bọc mỗi summernote trong một container riêng biệt
    $(this).wrap('<div class="summernote-container" id="container-' + editorId + '"></div>');

    // Khởi tạo summernote với cấu hình không sử dụng toolbarContainer chung
    $(this).summernote({
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
});
});