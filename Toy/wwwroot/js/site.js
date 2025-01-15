let dash = document.querySelector('header img[alt="Dash"]');
dash.addEventListener('click', (event) => {
    event.preventDefault();
let category = document.querySelector(".category");
    if (getComputedStyle(category).display === "none")
        category.style.display = "inline-block";
    else
        category.style.display = "none";

});
