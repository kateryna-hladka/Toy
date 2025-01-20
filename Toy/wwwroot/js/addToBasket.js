let addBasket = document.querySelector(".add-basket");
addBasket.addEventListener("click", function (event) {
    let output = document.querySelector("output");
    let outputValue = parseInt(output.value);
    if (event.target.classList.contains("add-product")) {
        if (outputValue + 1 <= parseInt(output.getAttribute("maxValue")))
            output.value = ++outputValue;
    }
    if (event.target.classList.contains("delete-product")) {
        if (outputValue - 1 >= parseInt(output.getAttribute("minValue")))
            output.value = --outputValue;
    }
});