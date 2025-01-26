let addBasket = document.querySelector(".add-basket");
addBasket?.addEventListener("click", function (event) {
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

document.addEventListener("DOMContentLoaded", function () {
    let cardProduct = document.querySelector(".card-product");
    cardProduct?.addEventListener("click", function (event) {
        let target = event.target;
        if (target.closest(".basket")) {
            let basketElement = target.closest(".basket");
            let productId = basketElement.getAttribute("data-product-id");

            if (!isNaN(Number(productId))) {

                fetch('/Basket/Add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ productId: productId })
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        basketElement.style.backgroundColor = "red";
                    } else {
                        console.error('Failed to add product to basket');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                });
            }
        }
    });
});
