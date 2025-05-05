window.setLanguageCookie = function (lang, reload) {
    document.cookie = `blazor-culture=${lang}; path=/; max-age=31536000`; // 1 year
    if (reload) {
        location.reload();
    }
};