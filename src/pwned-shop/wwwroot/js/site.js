window.onload = function () {
    let addCartButtonList = document.getElementsByClassName("addcart-button");

    for (let i = 0; i < addCartButtonList.length; i++)
        addCartButtonList[i].addEventListener("click", onClick);
}

function onClick(event) {
    let elem = event.currentTarget;

    addToCart(elem);
}

// trigger by pressing Add to Cart button on Product page
function addToCart(elem) {
    let cartBadge = document.getElementById("lblCartCount");
    let productId = elem.getAttribute("product-id");

    let xhr = new XMLHttpRequest();

    xhr.open("POST", `/Cart/AddToCart?productId=${productId}`);
    //xhr.setRequestHeader("Content-Type", "multipart/form-data");

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                // for debugging

                console.log("Add to cart status: " + data.success);

                if (data.success) {
                    cartBadge.innerHTML = data.cartCount;
                }
            }
        }
    }

    // send cart update to server

    //xhr.send(`productId: "${productId}"`);
    xhr.send();
}

function updateCart(elem) {
    let productId = elem.getAttribute("product-id");
    let qty = elem.value;
    let cartBadge = document.getElementById("lblCartCount");
    let cartSubTotal = document.getElementById(`cart-subtotal-${productId}`);

    // for debugging

    console.log(qty);

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/UpdateCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                // for debugging

                console.log("Cart update status: " + data.success);
                if (data.success) {
                    cartBadge.innerHTML = data.cartCount;
                    cartSubTotal.innerHTML = "$" + data.subTotal;
                }
            }
        }
    }

    // send cart update to server
    xhr.send(JSON.stringify({
        productId: parseInt(productId),
        qty: parseInt(qty)
    }));
}