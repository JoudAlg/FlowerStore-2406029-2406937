
 // Flower & Gift Store - JavaScript

document.addEventListener("DOMContentLoaded", function () {

    // PART 1: Dynamic Text
    const welcomeTitle = document.querySelector(".hero h1");

    if (welcomeTitle) {
        const messages = [
            "Welcome to Flower & Gift Store 🌸",
            "Make Every Moment Special 💐",
            "Fresh Flowers, Happy Hearts 💗"
        ];

        let index = 0;

        setInterval(function () {
            index = (index + 1) % messages.length;
            welcomeTitle.textContent = messages[index];
        }, 3000);
    }

    // PART 2: Dynamic Picture
    const storeImage = document.querySelector(
        ".hero-image-container img"
    );

    if (storeImage) {

        const changePictureBtn = document.createElement("button");

        changePictureBtn.textContent = "Change Picture 🌸";
        changePictureBtn.className = "btn";
        changePictureBtn.type = "button";

        const buttonContainer = document.createElement("div");
        buttonContainer.style.textAlign = "center";
        buttonContainer.style.marginTop = "15px";

        buttonContainer.appendChild(changePictureBtn);
        storeImage.parentElement.appendChild(buttonContainer);

        let isOriginalPicture = true;

        changePictureBtn.addEventListener("click", function () {

            if (isOriginalPicture) {
                storeImage.src = "flower2.png";
                changePictureBtn.textContent = "Original Picture 🌸";
                isOriginalPicture = false;
            }
            else {
                storeImage.src = "flower.jpg";
                changePictureBtn.textContent = "Change Picture 🌸";
                isOriginalPicture = true;
            }

        });
    }

    // PART 3: Dynamic CSS and Product Cards
    const productCards = document.querySelectorAll(".product-card");

    productCards.forEach(function (card) {

        card.addEventListener("mouseenter", function () {
            card.style.transform = "scale(1.05)";
            card.style.boxShadow =
                "0 8px 20px rgba(214,51,132,0.3)";
            card.style.transition = "0.3s";
        });

        card.addEventListener("mouseleave", function () {
            card.style.transform = "scale(1)";
            card.style.boxShadow =
                "0 2px 5px rgba(0,0,0,0.1)";
        });

    });

    // PART 4: Shopping Cart

    // Get saved cart items
    function getCart() {
        try {
            const savedCart = JSON.parse(
                localStorage.getItem("flowerCart") || "[]"
            );

            return Array.isArray(savedCart) ? savedCart : [];
        }
        catch (error) {
            return [];
        }
    }

    // Save cart items
    function saveCart(cart) {
        localStorage.setItem(
            "flowerCart",
            JSON.stringify(cart)
        );
    }

    // Add products to cart
    const cartButtons = document.querySelectorAll(
        ".product-card button"
    );

    for (let i = 0; i < cartButtons.length; i++) {

        cartButtons[i].addEventListener("click", function () {

            const card = this.closest(".product-card");

            const productName = card.dataset.name ||
                card.querySelector("h3").textContent;

            const priceText = card.dataset.price ||
                card.querySelector("p").textContent;

            const productPrice = Number(
                String(priceText).replace(/[^\d.]/g, "")
            );

            if (!Number.isFinite(productPrice) ||
                productPrice <= 0) {
                alert("Invalid product price.");
                return;
            }

            const cart = getCart();

            cart.push({
                name: productName,
                price: productPrice
            });

            saveCart(cart);

            alert(productName + " added to cart! 🌸");

            this.textContent = "Added ✓";
            this.style.backgroundColor = "#4CAF50";
        });
    }

    // Display shopping cart
    const cartItems = document.getElementById("cart-items");
    const cartTotal = document.getElementById("cart-total");

    if (cartItems && cartTotal) {

        function displayCart() {
            const cart = getCart();
            let total = 0;

            cartItems.innerHTML = "";

            if (cart.length === 0) {
                const emptyItem = document.createElement("li");

                emptyItem.textContent = "Your cart is empty.";
                cartItems.appendChild(emptyItem);
            }
            else {
                cart.forEach(function (product) {

                    total += Number(product.price);

                    const item = document.createElement("li");
                    item.className = "cart-item";

                    item.textContent =
                        product.name + " - " +
                        product.price + " SAR";

                    cartItems.appendChild(item);
                });
            }

            cartTotal.textContent = total;
        }

        displayCart();

        // Clear shopping cart
        const clearButton = document.getElementById(
            "clearCartBtn"
        );

        if (clearButton) {

            clearButton.addEventListener("click", function () {

                saveCart([]);
                displayCart();

                alert("Shopping cart cleared successfully!");
            });
        }
    }

    // PART 5: Login Form Validation
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {

        loginForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const email = document.getElementById("email")
                .value.trim();

            const password = document.getElementById("password")
                .value;

            if (email === "" || password === "") {
                alert("Please fill in all fields.");
            }
            else if (!isValidEmail(email)) {
                alert("Please enter a valid email address.");
            }
            else if (password.length < 6) {
                alert("Password must be at least 6 characters.");
            }
            else {
                alert("Login form validated successfully! (Demo only)");
            }
        });
    }

    // PART 6: Register Form Validation
    const registerForm = document.getElementById("registerForm");

    if (registerForm) {

        registerForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const name = document.getElementById("fullname")
                .value.trim();

            const email = document.getElementById("email")
                .value.trim();

            const password = document.getElementById("password")
                .value;

            const confirmPassword = document.getElementById(
                "confirm-password"
            ).value;

            if (name === "" || email === "" ||
                password === "" || confirmPassword === "") {

                alert("Please fill in all fields.");
            }
            else if (name.length < 2) {
                alert("Please enter your full name.");
            }
            else if (!isValidEmail(email)) {
                alert("Please enter a valid email address.");
            }
            else if (password.length < 6) {
                alert("Password must be at least 6 characters.");
            }
            else if (password !== confirmPassword) {
                alert("Passwords do not match!");
            }
            else {
                alert(
                    "Registration form validated successfully! 🌸 (Demo only)"
                );

                registerForm.reset();
            }
        });
    }

});

// PART 7: Email Validation Function
function isValidEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

