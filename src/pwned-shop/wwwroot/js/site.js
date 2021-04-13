window.onload = function () {
    let addCartButtonList = document.getElementsByClassName("addcart-button");

    for (let i = 0; i < addCartButtonList.length; i++)
        addCartButtonList[i].addEventListener("click", onClick);
}

function onClick(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("id");

    sendCartUpdate(productId, 1);
}

function sendCartUpdate(productId, qty) {
    let xhr = new XMLHttpRequest();
    let cartBadge = document.getElementById("lblCartCount");

    xhr.open("POST", "/Cart/UpdateCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                // for debugging
                console.log("Operation Status: " + data.success);
                cartBadge.innerHTML = data.cartCount;
            }
        }
    }

    // send cart update to server
    xhr.send(JSON.stringify({
        productId: parseInt(productId),
        qty: qty
    }));
}