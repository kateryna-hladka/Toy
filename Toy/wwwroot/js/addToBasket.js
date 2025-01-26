let addBasket = document.querySelector(".add-basket");
addBasket?.addEventListener("click", function (event) {
    let output = document.querySelector("output");
    let outputValue = parseInt(output.value);
    let elem = event.target;

    if (localStorage.length === 0 || localStorage.getItem(`product-${elem.getElementsByTagName("BUTTON")[0]?.getAttribute("data-product-id")}-in-basket`)) {
        if (elem.classList.contains("add-product")) {
            if (outputValue + 1 <= parseInt(output.getAttribute("maxValue")))
                output.value = ++outputValue;
        }
        if (elem.classList.contains("delete-product")) {
            if (outputValue - 1 >= parseInt(output.getAttribute("minValue")))
                output.value = --outputValue;
        }
    }
    if (elem.tagName === "BUTTON") {
        let productId = elem.getAttribute("data-product-id");
        sendProductToBasket(productId, elem, outputValue);
    }
});

document.addEventListener("DOMContentLoaded", function () {
    let cardProduct = document.querySelector(".card-product");
    if (localStorage.length !== 0) {
        let products = cardProduct?.getElementsByClassName("basket");
        if (products != null)
            for (let p of products) {
                let productId = p.getAttribute("data-product-id");

                if (!isNaN(Number(productId)) && productId === localStorage.getItem(`product-${productId}-in-basket`)) {
                    p.classList.add("send-to-basket")
                }
            }
        else {
            if (addBasket !== null) {
                let elem = addBasket.getElementsByTagName("BUTTON")[0];
                let productId = elem.getAttribute("data-product-id");
                if (!isNaN(Number(productId)) && productId === localStorage.getItem(`product-${productId}-in-basket`)) {
                    let output = document.querySelector("output");
                    output.innerHTML = localStorage.getItem(`product-${productId}-amount`);
                    elem.classList.add("send-to-basket");
                    elem.innerHTML = "В кошику";
                }
            }
        }
    }
    cardProduct?.addEventListener("click", function (event) {
        let target = event.target;
        if (target.closest(".basket")) {
            let basketElement = target.closest(".basket");
            let productId = basketElement.getAttribute("data-product-id");
            sendProductToBasket(productId, basketElement);
        }
    });
});

function sendProductToBasket(productId, element, amount = null) {
    if (!isNaN(Number(productId)) && !element.classList.contains("send-to-basket") &&
        ((amount != null && !isNaN(Number(amount))) || amount == null)) {

        fetch('/Basket/Add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ productId: productId, amount: amount })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    element.classList.add("send-to-basket");
                    localStorage.setItem(`product-${productId}-in-basket`, `${productId}`);
                    localStorage.setItem(`product-${productId}-amount`, `${amount ?? 1}`);
                } else {
                    console.error('Failed to add product to basket');
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}