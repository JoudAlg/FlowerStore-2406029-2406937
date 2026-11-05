
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
    const storeImage = document.getElementById("storeImage");
    const changePictureBtn = document.getElementById("changePictureBtn");

    if (storeImage && changePictureBtn) {

        changePictureBtn.addEventListener("click", function () {

            if (storeImage.getAttribute("src") === "flower.jpg") {
                storeImage.src = "flower2.png";
                changePictureBtn.textContent = "Original Picture 🌸";
            } else {
                storeImage.src = "flower.jpg";
                changePictureBtn.textContent = "Change Picture 🌸";
            }

        });
    }

    // PART 3: Dynamic CSS and Product Cards
    const productCards = document.querySelectorAll(".product-card");

    productCards.forEach(function (card) {

        card.addEventListener("mouseenter", function () {
            card.style.transform = "scale(1.05)";
            card.style.boxShadow = "0 8px 20px rgba(214,51,132,0.3)";
        });

        card.addEventListener("mouseleave", function () {
            card.style.transform = "scale(1)";
            card.style.boxShadow = "0 2px 5px rgba(0,0,0,0.1)";
        });
    });

    // PART 4: Add to Cart
    const cartButtons = document.querySelectorAll(".product-card button");

    for (let i = 0; i < cartButtons.length; i++) {

        cartButtons[i].addEventListener("click", function () {

            const card = this.closest(".product-card");
            const productName = card.querySelector("h3").textContent;

            alert(productName + " added to cart! 🌸");

            this.textContent = "Added ✓";
            this.style.backgroundColor = "#4CAF50";
        });
    }

    // PART 5: Login Form Validation
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {

        loginForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;

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

            const name = document.getElementById("fullname").value.trim();
            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;
            const confirmPassword = document.getElementById("confirm-password").value;

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
                alert("Registration form validated successfully! 🌸 (Demo only)");
                registerForm.reset();
            }
        });
    }

});

// PART 7: JavaScript Function for Email Validation
function isValidEmail(email) {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}
