$(function () {
    const theme = getCookie("daiminh-theme");

    const contentCssFiles = [
        '/lib/tabler/core/dist/css/tabler.min.css',
        ...(theme === 'dark'
            ? ['dark']
            : []),
    ];

    const options = {
        selector: '.wysiwyg',
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media table paste help wordcount',
            'fontfamily'
        ].join(' '),
        toolbar: [
            'undo redo | formatselect |',
            'fontselect fontsizeselect |',
            'bold italic underline | alignleft aligncenter alignright |',
            'bullist numlist outdent indent | removeformat | preview'
        ].join(' '),
        skin: theme === 'dark' ? 'oxide-dark' : 'oxide',
        content_css: contentCssFiles,
    };

    hugerte.init(options);
});

// TODO: chưa cấu hình được font trong hugerte