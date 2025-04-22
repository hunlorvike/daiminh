const themeConfig = {
    cookieKey: "daiminhTheme",
    defaultTheme: "light",
    cookieDays: 365
};

const ThemeManager = {
    getCookie: function (name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        return parts.length === 2 ? parts.pop().split(';').shift() : null;
    },

    setCookie: function (name, value, days = themeConfig.cookieDays) {
        const expires = new Date(Date.now() + days * 864e5).toUTCString();
        document.cookie = `${name}=${value}; path=/; expires=${expires}`;
    },

    getTheme: function () {
        const urlTheme = new URLSearchParams(window.location.search).get("theme");
        if (urlTheme) {
            this.setCookie(themeConfig.cookieKey, urlTheme);
            return urlTheme;
        }
        return this.getCookie(themeConfig.cookieKey) || themeConfig.defaultTheme;
    },

    applyTheme: function (theme) {
        if (theme === "dark") {
            $('body').attr('data-bs-theme', 'dark');
        } else {
            $('body').removeAttr('data-bs-theme');
        }
    },

    init: function () {
        const selectedTheme = this.getTheme();
        this.applyTheme(selectedTheme);
    }
};

$(function () {
    ThemeManager.init();
});
