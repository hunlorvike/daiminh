const themeStorageKey = "daiminhTheme"
const defaultTheme = "light"

const getTheme = () => {
    const urlTheme = new URLSearchParams(window.location.search).get('theme')
    if (urlTheme) {
        localStorage.setItem(themeStorageKey, urlTheme)
        return urlTheme
    }
    return localStorage.getItem(themeStorageKey) || defaultTheme
}

const applyTheme = (theme) => {
    if (theme === 'dark') {
        $('body').attr('data-bs-theme', theme)
    } else {
        $('body').removeAttr('data-bs-theme')
    }
}

$(document).ready(() => {
    const selectedTheme = getTheme()
    applyTheme(selectedTheme)
})