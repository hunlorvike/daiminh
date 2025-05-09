$(function () {
    const theme = getCookie("daiminhTheme");

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
        content_style: `
        .mce-content-body[data-mce-placeholder]:not(.mce-visualblocks)::before {
          content: attr(data-mce-placeholder);
          display: block;
          padding: 10px;
          color: #999;
          font-style: italic;
          position: absolute;
          top: 0;
          left: 0;
          width: 100%;
          pointer-events: none;
        }
      
        body.mce-content-body {
          padding: 10px !important;
          font-family: 'Inter', sans-serif !important;
          position: relative;
        }
      `,
        statusbar: false,
    };

    hugerte.init(options);
});
