/* ==================================
            toggle password
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
           navigation
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
            $(".nav-scroll .navbar-brand img").attr("src", "/Content/Front/img/pre-login/logo.png");

        } else {

            // Hide White nav
            $(".nav-scroll").removeClass("white-nav-top");

            // Show logo
            $(".nav-scroll .navbar-brand img").attr("src", "/Content/Front/img/pre-login/top-logo.png");

        }
    }
});

/* ==================================
            mobile menu 
====================================*/
$(function () {

    // Show Mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });

    // Hide Mobile nav
    $("#mobile-nav-close-btn, #mobile-nav ").click(function () {
        $("#mobile-nav").css("height", "0");
    });


});

//$(function () {
//    $("#paid").click(function () {

//    }

//});

/* ==================================
           faq
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

/* ==================================
           Add notes
====================================*/

$(document).ready(function () {
    $("#add-paid input[name='IsPaid']").change(function () {
        if ($("#paid").is(":checked")) {
            $("#price").removeAttr("disabled");
            $("#price").focus();
            $("#note-preview").attr("required", "required");
        } else {
            $("#price").val(0);
            $("#price").attr("disabled", "disabled");
            $("#note-preview").removeAttr("required");
        }
    });
});

$(document).ready(function () {
    $("#edit-paid input[name='IsPaid']").change(function () {
        if ($("#paid").is(":checked")) {
            $("#price").removeAttr("disabled");
            $("#price").focus();
        } else {
            $("#price").val(0);
            $("#price").attr("disabled", "disabled");
        }
    });
});

/* ==================================
           search notes
====================================*/

$(document).ready(function () {

    $("#searchfilter #type").change(function () {
        this.form.submit();
    });
    $("#searchfilter #search").change(function () {
        this.form.submit();
    });
    $("#searchfilter #category").change(function () {
        this.form.submit();
    });
    $("#searchfilter #university").change(function () {
        this.form.submit();
    });
    $("#searchfilter #course").change(function () {
        this.form.submit();
    });
    $("#searchfilter #country").change(function () {
        this.form.submit();
    });
    $("#searchfilter #rating").change(function () {
        this.form.submit();
    });

});



