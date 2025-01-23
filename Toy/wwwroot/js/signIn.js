document.querySelector(".user-form")?.addEventListener("invalid", (event) => {
    let elem = event.target;
    if (elem.id === "password")
        elem.setCustomValidity('Пароль має бути не менше 4 символів!');
    if (elem.id === "contact-info")
        elem.setCustomValidity('Введіть дані у вказаному форматі!');
}, true);

document.querySelector(".user-form")?.addEventListener("input", (event) => {
    let elem = event.target;
    if (elem.id === "password" && elem.value.length >= 4)
        elem.setCustomValidity('');

    if (elem.id === "contact-info") {
        let pattern = new RegExp(elem.getAttribute("pattern"));
        if (pattern.test(elem.value))
            elem.setCustomValidity('');
    }
});