

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
                storeImage.src = "/images/flower2.png";
                changePictureBtn.textContent = "Original Picture 🌸";
                isOriginalPicture = false;
            }
            else {
                storeImage.src = "/images/flower.jpg";
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

    // PART 4: Shopping Cart buttons
    // (The cart itself is now saved on the server in the Session by CartController)
    const cartForms = document.querySelectorAll(".add-to-cart-form");

    for (let i = 0; i < cartForms.length; i++) {

        cartForms[i].addEventListener("submit", function () {
            const button = this.querySelector("button");

            // Show feedback while the page sends the product to the server
            button.textContent = "Adding... 🌸";
            button.style.backgroundColor = "#4CAF50";
        });
    }

    // Ask before clearing the whole cart
    const clearCartForm = document.getElementById("clearCartForm");

    if (clearCartForm) {
        clearCartForm.addEventListener("submit", function (event) {
            if (!confirm("Are you sure you want to clear your shopping cart?")) {
                event.preventDefault();
            }
        });
    }

    // Client-side validation for the Add / Edit product form
    const productForm = document.getElementById("productForm");

    if (productForm) {
        productForm.addEventListener("submit", function (event) {
            const name = document.getElementById("Name").value.trim();
            const price = Number(document.getElementById("Price").value);

            if (name.length < 2) {
                alert("Please enter a product name (at least 2 characters).");
                event.preventDefault();
            }
            else if (!Number.isFinite(price) || price < 1 || price > 10000) {
                alert("Price must be between 1 and 10,000 SAR.");
                event.preventDefault();
            }
        });
    }


    // PART 5: Login Form Validation
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", function (event) {

            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;

            if (email === "" || password === "") {
                event.preventDefault();
                alert("Please fill in all fields.");
            }
            else if (!isValidEmail(email)) {
                event.preventDefault();
                alert("Please enter a valid email address.");
            }
            else if (password.length < 6) {
                event.preventDefault();
                alert("Password must be at least 6 characters.");
            }
            else {
                // Valid login: allow form submission to AccountController.
            }

        });
    }

    // PART 6: Register Form Validation
    const registerForm = document.getElementById("registerForm");

    if (registerForm) {

        registerForm.addEventListener("submit", function (event) {

            if (!registerForm.checkValidity()) {
                event.preventDefault();
                return;
            }


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

                event.preventDefault();

                alert("Please fill in all fields.");
            }
            else if (name.length < 2) {
                event.preventDefault();
                alert("Please enter your full name.");
            }

            else if (!isValidEmail(email)) {
                event.preventDefault();
                alert("Please enter a valid email address.");
            }
            else if (password.length < 8) {
                event.preventDefault();
                alert("Password must be at least 8 characters.");
            }



else if (password !== confirmPassword) {
    event.preventDefault();
    alert("Passwords do not match!");
}


            else {
                // Valid registration: allow the form to submit to the server.
            }

        });
    }

});

// PART 7: Email Validation Function
function isValidEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

