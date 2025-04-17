function getTooltipHeader() {
    return `
               <div class="d-flex align-items-center mb-3">
                 <img class="d-inline-block" style="width:50px; height:50px; border-radius:4px;" 
                      src="/assets/img/HumHum_logo.jpg"
                      alt="HumHum Logo" />
                 <span class="fs-3 fw-bold ms-2" style="background: linear-gradient(90deg, #F17228 0%, #FFB30E 100%); 
                      -webkit-background-clip: text; background-clip: text; -webkit-text-fill-color: transparent;">
                   HumHum
                 </span>
               </div>`;
}


function startTour() {
    introJs()
        .setOptions({
            steps: [
                {
                    element: document.getElementById("restaurant-tour"),
                    intro: `${getTooltipHeader()}
                   <strong>Welcome to Hum Hum!</strong><br>Discover delicious meals delivered fast to your doorstep. Our hero section showcases what we offer and how easy it is to get started.`,
                    position: "bottom",
                },
                {
                    element: document.getElementById("search-tour"),
                    intro: `${getTooltipHeader()}
                   <strong>Welcome to Hum Hum!</strong><br>Discover delicious meals delivered fast to your doorstep. Our hero section showcases what we offer and how easy it is to get started.`,
                    position: "bottom",
                },
                {
                    element: document.getElementById("popular-items"),
                    intro: `${getTooltipHeader()}
                   <strong>Welcome to Hum Hum!</strong><br>Discover delicious meals delivered fast to your doorstep. Our hero section showcases what we offer and how easy it is to get started.`,
                    position: "bottom",
                },
                {
                    element: document.getElementById("testimonial"),
                    intro:
                        "<strong>Why Choose Hum Hum?</strong><br>We highlight three core benefits that make us the preferred food delivery choice: Fast Delivery, Fresh Food, and 24/7 Support.",
                    position: "bottom",
                },
                {
                    element: document.getElementById("logout-tour"),
                    intro: `${getTooltipHeader()}
                         <strong>Welcome to Hum Hum!</strong><br>Discover delicious meals delivered fast to your doorstep. Our hero section showcases what we offer and how easy it is to get started.`,
                    position: "bottom",
                },
            ],
            nextLabel: 'Next <i class="fas fa-chevron-right"></i>',
            prevLabel: '<i class="fas fa-chevron-left"></i> Back',
            skipLabel: '<i class="btn-close" style="color: #ffffff;"></i>',
            doneLabel: 'Finish <i class="fas fa-utensils"></i>',
            tooltipClass: 'customTooltip',
            highlightClass: 'customHighlight',
            exitOnEsc: true,
            exitOnOverlayClick: false,
            showStepNumbers: false,
            showBullets: true,
            showProgress: true,
            keyboardNavigation: true,
            scrollToElement: true,
            overlayOpacity: 0.8,
            position: 'auto',
            tooltipClass: 'customTooltip',
            isActive: true,
        })
        .oncomplete(function () {
            // Mark tour as completed when user finishes it
            localStorage.setItem('humhumTourCompleted', 'true');
        })
        .onexit(function () {
            // Mark tour as completed if user exits it (even if not finished)
            localStorage.setItem('humhumTourCompleted', 'true');
        })
        .start();
}

window.onload = function () {
    // Check if the user has completed the tour before
    const tourCompleted = localStorage.getItem('humhumTourCompleted');
    // Only start the tour if it hasn't been completed before
    if (tourCompleted !== 'true') {
        // Start tour after a slight delay to allow page to fully load
        setTimeout(startTour, 1000);
    }
};