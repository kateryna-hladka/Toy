document.querySelector(".user-form")?.addEventListener("invalid", (event) => {
    let elem = event.target;
    switch (elem.id) {
        case "password":
        case "contact-info": 
        case "user-name": 
        case "user-surname": elem.setCustomValidity(elem.getAttribute("title")); break;
    }
}, true);

document.querySelector(".user-form")?.addEventListener("input", (event) => {
    let elem = event.target;
    switch (elem.id) {
        case "password":
            if(elem.value.length >= elem.getAttribute("minlength"))
                elem.setCustomValidity('');
            break;
        case "contact-info":
        case "user-name":
        case "user-surname":
            setRegExpCustomValidity(elem); break;
    }
});

function setRegExpCustomValidity(elem) {
    let pattern = new RegExp(elem.getAttribute("pattern"));
    if (pattern.test(elem.value))
        elem.setCustomValidity('');
}