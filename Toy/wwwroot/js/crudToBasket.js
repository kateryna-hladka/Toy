let localStorageLengthNotZero = localStorage.length !== 0;
let addBasket = document.querySelector(".add-basket");
let productExistInBasket = "Товар вже додано до кошика";

addBasket?.addEventListener("click", function (event) {
    let elem = event.target;
    let productId = getProductId(addBasket.getElementsByTagName("BUTTON")[0]);

    if (localStorageIsNull(productId) && !elemHasClass(elem)) {

        let output = document.querySelector("output");
        let outputValue = parseInt(output.value);
        changeProductAmount(elem, output, outputValue);

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
    let productInBusket = document.getElementsByClassName("in-basket")[0];
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
            if (localStorageIsNull(productId) && !elemHasClass(basketElement))
                sendProductToBasket(productId, basketElement);
            else
                alert(productExistInBasket);
        }
    });

    productInBusket?.addEventListener("click", function (event) {
        let elem = event.target;
        if (elem.closest(".basket-parent")) {

            let chooseElem = elem.closest(".basket-parent")
            let output = chooseElem.querySelector("output");
            let productId = getProductId(chooseElem);
            if (!elem.classList.contains("rubbish-bin")) {
                changeProductAmount(elem, output, parseInt(output.value));
                updateProductCount(productId, elem, parseInt(output.value));
            }
            else {
                if (confirm("Ви дійсно бажаєте видалити цей елемент з кошика?"))
                    deleteProduct(productId);
            }
        }

    });

    setTimeout(function () {
        if (sessionStorage.getItem("clear") === null) {
            checkUserLoginStatus();
        }
    });
});

function elemHasClass(elem) {
    return elem.classList.contains("send-to-basket");
}
function changeProductAmount(elem, output, outputValue) {
    if (elem.classList.contains("add-product")) {
        if (outputValue + 1 <= parseInt(output.getAttribute("maxValue")))
            output.value = ++outputValue;
    }
    if (elem.classList.contains("delete-product")) {
        if (outputValue - 1 >= parseInt(output.getAttribute("minValue")))
            output.value = --outputValue;
    }
}

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
    if (chooseAmount !== undefined)
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

function updateProductCount(productId, element, amount) {
    if (!isNaN(Number(productId)) && !isNaN(Number(amount))) {

        fetch('/Basket/Update', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ productId: productId, amount: amount })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    localStorage.setItem(`product-${productId}-amount`, amount);
                    let newPrices = Array.from(document.getElementsByClassName("new-price"));
                    let oldPrices = Array.from(document.getElementsByClassName("price"));
                    let combinePrice = oldPrices.concat(newPrices);
                    let summa = 0.0;
                    
                    for (let i of combinePrice) {
                        let amount = parseFloat(i.parentNode.parentNode.getElementsByTagName("OUTPUT")[0].value);
                        let price = i.innerHTML.split(' грн')[0].replace(',', '.');
                        /*let z = parseFloat(price[0], ',');
                        let x = amount;
                        b = summa + (price[0]);*/

                        summa += parseFloat(price) * amount;
                    }
                    summa = parseFloat(summa.toFixed(2));
                    document.getElementsByClassName("summa")[0].innerHTML = summa.toString().replace('.', ',');
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}

function deleteProduct(productId) {
    if (!isNaN(Number(productId))) {
        fetch('/Basket/Delete', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ productId: productId })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    localStorage.removeItem(`product-${productId}-in-basket`);
                    localStorage.removeItem(`product-${productId}-amount`);
                    location.reload();
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}