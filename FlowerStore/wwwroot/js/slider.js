// Flower Store - Home Page Image Slider
// Shows one slide at a time, changes automatically every 4 seconds,
// and can be controlled with the arrows and the dots.

document.addEventListener("DOMContentLoaded", function () {

    const slider = document.getElementById("homeSlider");
    if (!slider) {
        return; // this page has no slider
    }

    const slides = slider.querySelectorAll(".slide");
    const dotsContainer = slider.querySelector(".slider-dots");
    const prevButton = slider.querySelector(".prev");
    const nextButton = slider.querySelector(".next");

    let currentIndex = 0;
    let timer = null;

    // Create one dot for each slide (loop)
    for (let i = 0; i < slides.length; i++) {
        const dot = document.createElement("button");
        dot.type = "button";
        dot.className = "slider-dot";
        dot.setAttribute("aria-label", "Go to slide " + (i + 1));

        dot.addEventListener("click", function () {
            showSlide(i);
            restartTimer();
        });

        dotsContainer.appendChild(dot);
    }

    const dots = dotsContainer.querySelectorAll(".slider-dot");

    // Show the slide with the given number
    function showSlide(index) {
        // Go around: after the last slide comes the first one
        if (index >= slides.length) {
            index = 0;
        }
        else if (index < 0) {
            index = slides.length - 1;
        }

        slides.forEach(function (slide, i) {
            slide.classList.toggle("active", i === index);
            dots[i].classList.toggle("active", i === index);
        });

        currentIndex = index;
    }

    function nextSlide() {
        showSlide(currentIndex + 1);
    }

    function previousSlide() {
        showSlide(currentIndex - 1);
    }

    // Automatic change every 4 seconds
    function startTimer() {
        timer = setInterval(nextSlide, 4000);
    }

    function restartTimer() {
        clearInterval(timer);
        startTimer();
    }

    // Events
    nextButton.addEventListener("click", function () {
        nextSlide();
        restartTimer();
    });

    prevButton.addEventListener("click", function () {
        previousSlide();
        restartTimer();
    });

    // Pause while the mouse is over the slider
    slider.addEventListener("mouseenter", function () {
        clearInterval(timer);
    });

    slider.addEventListener("mouseleave", startTimer);

    // Keyboard arrows also work
    document.addEventListener("keydown", function (event) {
        if (event.key === "ArrowRight") {
            nextSlide();
            restartTimer();
        }
        else if (event.key === "ArrowLeft") {
            previousSlide();
            restartTimer();
        }
    });

    showSlide(0);
    startTimer();
});
