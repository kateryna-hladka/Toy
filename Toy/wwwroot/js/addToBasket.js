let localStorageLengthNotZero = localStorage.length !== 0;
let addBasket = document.querySelector(".add-basket");
let productExistInBasket = "Товар вже додано до кошика";

addBasket?.addEventListener("click", function (event) {
    let elem = event.target;
    let output = document.querySelector("output");
    let outputValue = parseInt(output.value);

    let productId = getProductId(addBasket.getElementsByTagName("BUTTON")[0]);

    if (localStorageIsNull(productId)) {
        if (elem.classList.contains("add-product")) {
            if (outputValue + 1 <= parseInt(output.getAttribute("maxValue")))
                output.value = ++outputValue;
        }
        if (elem.classList.contains("delete-product")) {
            if (outputValue - 1 >= parseInt(output.getAttribute("minValue")))
                output.value = --outputValue;
        }
        if (elem.tagName === "BUTTON") {
            displayChoosePanel();
            sendProductToBasket(productId, elem, outputValue);
            changeButtonText(elem);
        }
    }
    else
        alert(productExistInBasket);
});

document.addEventListener("DOMContentLoaded", function () {
    let cardProduct = document.querySelector(".card-product");

    if (localStorageLengthNotZero) {
        let products = cardProduct?.getElementsByClassName("basket");
        if (products != null)
            for (let p of products) {
                let productId = getProductId(p);

                if (!isNaN(Number(productId)) && localStorageHasProduct(productId))
                    elemMarkAddBasketClass(p);
            }
        else {
            if (addBasket !== null) {
                let elem = addBasket.getElementsByTagName("BUTTON")[0];
                let productId = getProductId(elem);
                if (!isNaN(Number(productId)) && localStorageHasProduct(productId)) {
                    displayChoosePanel();
                    elemMarkAddBasketClass(elem);
                    changeButtonText(elem);
                }
            }
        }
    }
    cardProduct?.addEventListener("click", function (event) {
        let target = event.target;
        if (target.closest(".basket")) {
            let basketElement = target.closest(".basket");
            let productId = getProductId(basketElement);
            if (localStorageIsNull(productId))
                sendProductToBasket(productId, basketElement);
            else
                alert(productExistInBasket);
        }
    });
    setTimeout(function () {
        if (sessionStorage.getItem("clear") === null) {
            checkUserLoginStatus();
            
        }
    });
});

function localStorageIsNull(productId) {
    return (!localStorageLengthNotZero || localStorage.getItem(`product-${productId}-in-basket`) === null)
}

function sessionStorageIsNull(productId) {
    return (!sessiongStorageLengthNotZero || sessionStorage.getItem(`product-${productId}-in-basket`) === null)
}

function localStorageHasProduct(productId) {
    return (productId === localStorage.getItem(`product-${productId}-in-basket`))
}
function sessionStorageHasProduct(productId) {
    return (productId === sessionStorage.getItem(`product-${productId}-in-basket`))
}

function getProductId(elem) {
    return elem?.getAttribute("data-product-id");
}

function elemMarkAddBasketClass(elem) {
    elem.classList.add("send-to-basket");
}

function displayChoosePanel() {
    let chooseAmount = addBasket.getElementsByClassName("choose-amount")[0];
    chooseAmount.style.display = "none";
}

function changeButtonText(elem) {
    elem.innerHTML = "В кошику";
}

function sendProductToBasket(productId, element, amount = null) {
    if (!isNaN(Number(productId)) &&
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
                    elemMarkAddBasketClass(element);
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

function checkUserLoginStatus() {
    fetch('/api/check-login', {
        method: 'GET',
        credentials: 'same-origin'
    })
        .then(response => response.json())
        .then(data => {
            if (!data.loggedIn) {
                localStorage.clear();

                sessionStorage.setItem("clear", "yes");
            }
        })
        .catch(error => {
            console.error("Помилка перевірки статусу логіну:", error);
        });
}
