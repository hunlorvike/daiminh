const themeConfig = {
    defaults: {
        "theme": "light",
        "theme-base": "gray",
        "theme-font": "sans-serif",
        "theme-primary": "blue",
        "theme-radius": "1",
    },
    cookiePrefix: "daiminh-",
    cookieDays: 365
};

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    return parts.length === 2 ? parts.pop().split(';').shift() : null;
}

function setCookie(name, value, days = themeConfig.cookieDays) {
    const expires = new Date(Date.now() + days * 864e5).toUTCString();
    document.cookie = `${name}=${value}; path=/; expires=${expires}`;
}

function applyAttribute(key, value, defaultValue) {
    const attr = 'data-bs-' + key;
    if (value !== defaultValue) {
        $('html').attr(attr, value);
    } else {
        $('html').removeAttr(attr);
    }
}

function getParam(key) {
    return new URLSearchParams(window.location.search).get(key);
}

; (function ($) {
    function initTheme() {
        $.each(themeConfig.defaults, function (key, defaultValue) {
            const cookieName = themeConfig.cookiePrefix + key;
            const param = getParam(key);
            let selectedValue;

            if (param) {
                setCookie(cookieName, param);
                selectedValue = param;
            } else {
                selectedValue = getCookie(cookieName) || defaultValue;
            }

            applyAttribute(key, selectedValue, defaultValue);
        });
    }

    $(document).ready(initTheme);

})(jQuery);
