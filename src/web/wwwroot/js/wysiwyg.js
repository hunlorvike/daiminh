$(function () {
    const theme = getCookie("daiminh-theme");

    const contentCssFiles = [
        '/lib/tabler/core/dist/css/tabler.min.css',
        ...(theme === 'dark' ? ['dark'] : []),
        '/fonts/inter/inter.css'
    ];

    const options = {
        selector: '.wysiwyg',
        height: 500,
        language: 'vi',
        language_url: '/js/vi.js',
              plugins: [
            'advlist anchor autolink autosave charmap code codesample',
            'directionality emoticons fullscreen help image importcss insertdatetime',
            'link lists media nonbreaking preview save searchreplace table template',
            'visualblocks visualchars wordcount'
        ].join(' '),
        toolbar: [
            'undo redo | bold italic underline strikethrough |',
            'alignleft aligncenter alignright alignjustify |',
            'bullist numlist outdent indent | link image media |',
            'code | forecolor backcolor |',
            'emoticons charmap | removeformat | preview fullscreen'
        ].join(' '),
        skin: theme === 'dark' ? 'oxide-dark' : 'oxide',
        content_css: contentCssFiles,
        content_style: "body { font-family: 'Inter', sans-serif; }",
    };

    hugerte.init(options);
});
