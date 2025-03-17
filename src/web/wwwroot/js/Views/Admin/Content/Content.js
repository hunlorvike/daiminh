$(document).ready(function () {
    $(document).on('shown.bs.modal', '#create-content--modal, #edit-content--modal', function (e) {
        let options = {
            lang: 'vi-VN',
            placeholder: 'Nhập nội dung bài viết',
            tabsize: 2,
            height: 300,
            minHeight: 200,
            maxHeight: 500
        };
        $('#ContentBody').summernote(options);
    });
});

