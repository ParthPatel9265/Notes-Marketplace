
/* ==================================
            Toggle Password
==================================== */
$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $($(this).attr("toggle"));
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});

/* ==================================
            Navigation
====================================*/

/* Show & Hide White Navigation */
$(function () {

    // show/hide nav on page load
    showHideNav();

    $(window).scroll(function () {

        showHideNav();

    });

    function showHideNav() {

        if ($(window).scrollTop() > 50) {

            //Show White nav
            $(".nav-scroll").addClass("white-nav-top");

            // Show dark logo
            $(".nav-scroll .navbar-brand img").attr("src", "img/pre-login/logo.png");

        } else {

            // Hide White nav
            $(".nav-scroll").removeClass("white-nav-top");

            // Show logo
            $(".nav-scroll .navbar-brand img").attr("src", "img/pre-login/top-logo.png");

        }
    }
});


/* ==================================
            Mobile Menu
====================================*/
$(function () {

    // Show Mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });

    // Hide Mobile nav
    $("#mobile-nav-close-btn").click(function () {
        $("#mobile-nav").css("height", "0");
    });


});
/* ==================================
            Accodion
====================================*/
$(document).ready(function () {

    for (let i = 1; i <= 7; i++) {
        $(".showdata" + i).click(function () {
            $(".ans" + i).show();
            $(".que" + i).hide();
        });
        $(".hidedata" + i).click(function () {
            $(".ans" + i).hide();
            $(".que" + i).show();
        });
    }
});






