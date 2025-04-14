const themeCookieKey = "daiminhTheme";
const defaultTheme = "light";

// Hàm get cookie
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}

// Hàm set cookie
function setCookie(name, value, days = 365) {
    const expires = new Date(Date.now() + days * 864e5).toUTCString();
    document.cookie = `${name}=${value}; path=/; expires=${expires}`;
}

// Lấy theme từ cookie hoặc URL
function getTheme() {
    const urlTheme = new URLSearchParams(window.location.search).get("theme");
    if (urlTheme) {
        setCookie(themeCookieKey, urlTheme);
        return urlTheme;
    }
    return getCookie(themeCookieKey) || defaultTheme;
}

// Apply theme (nếu muốn xử lý lại client-side sau khi chuyển theme mà không reload)
function applyTheme(theme) {
    if (theme === "dark") {
        $('body').attr('data-bs-theme', 'dark');
    } else {
        $('body').removeAttr('data-bs-theme');
    }
}

// Khởi tạo
$(document).ready(() => {
    const selectedTheme = getTheme();
    applyTheme(selectedTheme);
});
