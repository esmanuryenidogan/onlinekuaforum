var topnav           = $('.topnav');
var header           = $('header');
var topText          = $('.top-text');
var headerInfo       = $('.header-info');
var topnavHeight     = topnav.height();
var headerHeight     = header.height();
var headerInfoHeight = $('.header-info').outerHeight();
var windowWidth = $(window).width();
var windowHeight = $(window).height();

$(window).load(function() {

    'use strict';
    setTimeout(function() {
        $('.loader-overlay').addClass('loaded');
        $('body > section').animate({
            opacity: 1,
        }, 400);
        progressBar();
    }, 500);

});

$(window).scroll(function() {

    'use strict';
    var scrollPos = $(window).scrollTop();
    scrollPos > 220 ? $('.sticky-section').addClass('nav-bg') : $('.sticky-section').removeClass('nav-bg');
    if($('.header-info').length){
        if(scrollPos < headerInfoHeight) headerInfo.css('margin-top', - scrollPos);
        else  headerInfo.css('margin-top', - headerInfoHeight);
    }
    navbarScroll(topnav, topnavHeight);
    fadeText(this);
    handleFixedColumn();

});


$(window).resize(function() {

    portfolioIsotope();

});

/* ----------------------------- 
Backgroung slider
----------------------------- */
$(window).ready(function() {

    'use strict';
    
    handleNavigation();
    navbarScroll(topnav, topnavHeight);
    rotateText();
    imageZoom();
    var scrollPos = $(window).scrollTop();
    if(scrollPos < headerInfoHeight) headerInfo.css('margin-top', - scrollPos);
    else  headerInfo.css('margin-top', - headerInfoHeight);

    /* Full Height */
    $('.height-full').each(function(){
        var contentHeight = $(this).height();
        if(contentHeight < windowHeight) {
            if($('html').hasClass('border-page')) {
                $(this).height(windowHeight - 40);
            }
            else {
                $(this).height(windowHeight);
            }      
        }
    });

    /* Page Transition Fade Out */
    $('.topnav li a').on('click', function(e){
        if($(this).attr("target", "_blank").length) return;
        e.preventDefault();
        var targetLink = $(this).attr('href');
        if(targetLink == '#' || targetLink == '') return;
        if($('html').hasClass('one-page')) return;
        $('body').fadeOut(350, function(){
            window.location.href = targetLink;
        });
    });

    if($.fn.appear){
        $('.animated').appear({force_process: true});
    }

    $('.animated').on('appear', function(event, $all_appeared_elements) {
       var element = $(this),
            animation = element.data('animation'),
            animationDelay = element.data('animation-delay');
        if (animationDelay) {
            setTimeout(function() {
                element.addClass(animation + " visible");
            }, animationDelay);
        } else {
            element.addClass(animation + " visible");
        }
    });

    $('.next-section').on('click', function(e){
        e.preventDefault();
        var scrollToElement = $(this).parent().next();
        if($(this).parent().parent().is('section')) scrollToElement = $(this).parent().parent().next();
        if($(this).parent().parent().is('header')) scrollToElement = $(this).parent().parent().next();
        if($(this).parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().parent().parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().next();
        if($(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().is('header')) scrollToElement = $(this).parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().next();
        $('html, body').animate({
            scrollTop: scrollToElement.offset().top - 60
        }, 1000);
    });

    $('.go-section-two').on('click', function(e){
        e.preventDefault();
        $('html, body').animate({
            scrollTop: $('#section-two').offset().top
        }, 1000);
    });

    $('.go-team').on('click', function(e){
        e.preventDefault();
        $('html, body').animate({
            scrollTop: $('#team').offset().top
        }, 1000);
    });
    
    /* Background Slider */
    if($('.bg-slider').length && $.fn.backstretch){
        $('.bg-slider').height(windowHeight);

        if($('.bg-slider').attr('data-images')) {

            var bgImages = [];
            bgImages = $('.bg-slider').attr('data-images').split(',');
            $('.bg-slider').backstretch(
                bgImages,
            {
                fade: 600,
                duration: 4000
            });
            
        }
        else{

            $('.bg-slider').backstretch([
                '../../common-files/images/bg/bg-6.jpg',
                '../../common-files/images/bg/bg-2.jpg',
                '../../common-files/images/bg/bg-3.jpg'],
            {
                fade: 600,
                duration: 4000
            });
        }
    }

    /* Video Background */
    if($(".video-player").length){
        $(".video-player").each(function(){
            $(this).mb_YTPlayer();
        });
    }

    $('#play-video').click(function(e){
        e.preventDefault();
        if ($(this).hasClass("line-icon-control-play") ) {
            $('.video-player').playYTP();
        } else {
            $('.video-player').pauseYTP();
        }
        $(this).toggleClass("line-icon-control-play line-icon-control-pause");
        return false;
    });

    /* Background Slider */
    if($('.bg-image').length && $.fn.backstretch){
        $('.bg-image').height(windowHeight);
        $('.bg-image').backstretch(["images/bg/bg-2.jpg"]);
    }

    /* Scroll into viewPort Animation */
    if($.fn.appear){
        $('.animated').appear(function() {
            var element = $(this),
                animation = element.data('animation'),
                animationDelay = element.data('animation-delay');
            if (animationDelay) {
                setTimeout(function() {
                    element.addClass(animation + " visible");
                }, animationDelay);
            } else {
                element.addClass(animation + " visible");
            }
        });
    }

    $('.toggle-aside').on('click', function(e){
        e.preventDefault();
        setTimeout(function(){
            $('#aside').addClass('open');
        },100);
    });

    document.addEventListener('click', function(ev) {
        if($('#aside').hasClass('open')){
            var ecommerceSidebar = document.getElementById('aside');
            var target = ev.target;
            if( target !== ecommerceSidebar) {
                if($('#aside').hasClass('open')){
                    $('#aside').removeClass('open');
                }
            }
        }
    });

    /* Fitvids init */
    if($('.video-content').length){
        $('.video-content').fitVids();
    }
        
    /* Testimonial Slider */
    if($('.testimonials-slider').length){
        $('.testimonials-slider').bxSlider({
            pagerCustom: '#testimonials-pager',
            pager: true,
            touchEnabled: true,
            controls: false
        });
    }

    /* IE 9 Placeholder fix */
    $('[placeholder]').focus(function() {
        var input = $(this);
        if (input.val() == input.attr('placeholder')) {
            input.val('');
            input.removeClass('placeholder');
        }
    }).blur(function() {
        var input = $(this);
        if (input.val() == '' || input.val() == input.attr('placeholder')) {
            input.addClass('placeholder');
            input.val(input.attr('placeholder'));
        }
    }).blur();


    if($.fn.select2){
        $('select:not(.select-picker)').each(function(){
            $(this).select2({
                placeholder: $(this).data('placeholder') ?  $(this).data('placeholder') : '',
                allowClear: $(this).data('allowclear') ? $(this).data('allowclear') : true,
                minimumInputLength: $(this).data('minimumInputLength') ? $(this).data('minimumInputLength') : -1,
                minimumResultsForSearch: $(this).data('search') ? 1 : -1,
                dropdownCssClass: $(this).data('style') ? 'form-white' : ''
            });
        });
    }

    if($('.select-picker').length && $.fn.selectpicker){
        $('.select-picker').selectpicker();
    }

    /* Image Carousel */
    $('[data-plugin-carousel]:not(.manual), .owl-carousel:not(.manual)').each(function() {
        var $this = $(this), opts = null,
        pluginOptions = $this.data('plugin-options'),
        defaults = {
            "autoPlay": 3000,
            "items": 1,
            "singleItem": true,
            "itemsDesktop" : [1199,5],
            "itemsDesktopSmall" : [980,4],
            "itemsTablet": [768,3],
            "itemsMobile" : [479,2],
        };

        opts = $.extend( {}, defaults, pluginOptions );
        $this.owlCarousel(opts); 
    });

    /* Video */
    if($(".video").length && $.fn.fitVids){
        $(".video").fitVids();
    }

    /* Blog Masonry */
    if($(".blog-masonry").length && $.fn.masonry){
        $('.blog-masonry').masonry();
    }

    /* Background Image */
    var backgroundImage = $("header, section");
    backgroundImage.each(function(){
        if ($(this).attr("data-img")){
            if($(this).parent().hasClass('demo-page')){
                $(this).css("background-image", "url(common-files/images/" + $(this).data("img") + ")");
            }else{
                $(this).css("background-image", "url(../../common-files/images/" + $(this).data("img") + ")");
            }
            
        }
    });

    /* Parallax */
    $('.parallax').each(function(){
        var parallaxSpeed = $(this).data('speed') ? $(this).data('speed') : 0.1;
        $(this).parallax("50%", parallaxSpeed);
    });

    /**** To Search ****/
    $('.toggle-search').on('click', function(e){
        e.preventDefault();
        $('.form-search').toggleClass('search-open');
    });
    $('.search-close a').on('click', function(e){
        e.preventDefault();
        $('.form-search').removeClass('search-open');
    });

    handleiCheck();
    showTooltip();
    scrollTop();


 }); 


// Handles custom checkboxes & radios using jQuery iCheck plugin
function handleiCheck() {
    if (!$().iCheck)  return;
    $(':checkbox:not(.js-switch, .switch-input, .switch-iphone, .onoffswitch-checkbox, .ios-checkbox), :radio').each(function() {
        var checkboxClass = $(this).attr('data-checkbox') ? $(this).attr('data-checkbox') : 'icheckbox_minimal-grey';
        var radioClass = $(this).attr('data-radio') ? $(this).attr('data-radio') : 'iradio_minimal-grey';
        if (checkboxClass.indexOf('_line') > -1 || radioClass.indexOf('_line') > -1) {
            $(this).iCheck({
                checkboxClass: checkboxClass,
                radioClass: radioClass,
                insert: '<div class="icheck_line-icon"></div>' + $(this).attr("data-label")
            });
        } else {
            $(this).iCheck({
                checkboxClass: checkboxClass,
                radioClass: radioClass
            });
        }
    });
}

/* Image Zoom */
function imageZoom(){
    if($('.portfolioContainer').length && $.fn.magnificPopup){

        $('.portfolioContainer').magnificPopup({
            
            type: 'image',
            delegate: '.magnific',
           gallery:{enabled:true} 
        });
    }
    else {
        if($('.magnific').length && $.fn.magnificPopup){

            $('.magnific').magnificPopup({
                type: 'image',
                closeOnContentClick: true,
                closeBtnInside: false,
                fixedContentPos: true,
                mainClass: 'mfp-no-margins mfp-with-zoom', // class to remove default margin from left and right side
                image: {
                verticalFit: true
                },
                zoom: {
                enabled: true,
                duration: 300 // don't foget to change the duration also in CSS
                }
            });
        }

    }
    
}

/* Handle Aside Fixed */
function handleFixedColumn(){
    var topScroll = $(document).scrollTop();
    if($('.portfolio-single').length){
        var portfolioGalleryHeight = $('.portfolio-single .col-md-8').height();
        var portfolioTextHeight = $('.portfolio-single .col-md-4').height();
        var differenceHeight = portfolioGalleryHeight - portfolioTextHeight;
        if(topScroll < differenceHeight) {
            $('.fixed-column').css('margin-top', topScroll);
        }
    }
    else{
        $('.fixed-column').css('margin-top', topScroll);
    }
}

/* Fade Text / Element on Scroll */
function fadeText() {
    var topScroll = $(document).scrollTop();
    if ($('.fade-text').length && topScroll <= headerHeight){
        $('.fade-text').css('opacity', (1 - topScroll/headerHeight * 1));
    }
}

/* Text Rotation */
function rotateText(){
    $('[data-plugin-rotate-text]:not(.manual), .rotate-text:not(.manual)').each(function() {
        var $this = $(this), opts = null,
        pluginOptions = $this.data('plugin-options'),
        defaults = {
            "animation": "dissolve",  // Options are dissolve (default), fade, flip, flipUp, flipCube, flipCubeUp and spin.
            "separator": ",",  // If you don't want commas to be the separator, you can define a new separator (|, &, * etc.) by yourself using this field.
            "speed": 3500  // How many milliseconds until the next word show.
        };
        opts = $.extend( {}, defaults, pluginOptions );
        $this.textrotator(opts); 
    });
}

/* Progress Bar */
function progressBar(){
    var progressBar = $(".progress-bar");
    setTimeout(function(){
        progressBar.each(function(indx){
        $(this).css("width", $(this).attr("aria-valuenow") + "%");
        $(this).find('span').css("opacity", 1);
    });
    },1000);
}

/* Show Tooltip */
function showTooltip(){
    if ($('[data-rel="tooltip"]').length && $.fn.tooltip) {
        $('[data-rel="tooltip"]').tooltip();
    }
}

/* Scroll to top button */
function scrollTop() {
    if ($(this).scrollTop() > 100) $('.scrollup').fadeIn();
    else $('.scrollup').fadeOut();
    $(window).scroll(function() {
        if ($(this).scrollTop() > 100) $('.scrollup').fadeIn();
        else $('.scrollup').fadeOut();
    });
    $('.scrollup').click(function() {
        $("html, body").animate({
            scrollTop: 0
        }, 1000);
        return false;
    });
}


/**** NAVIGATION ****/
function handleNavigation(){
    /* Navbar fix */
    $('.navbar-nav .dropdown a').on('mouseenter', function(){
        $('.navbar-nav .dropdown a').removeClass('open');
    });
    /* One Page Main navigation */
    if($('html').hasClass('one-page')){
        $('.nav').onePageNav({
            currentClass: 'current',
            scrollSpeed: 1000,
            easing: 'easeInOutQuint',
            filter: ':not(.toggle-search)'
        });
    }
}

/* Navbar Height / Background on Scroll */
function navbarScroll(topnav, topnavHeight) {
    var topScroll = $(window).scrollTop();
    if (topnav.length > 0) {
        if(topScroll >= topnavHeight) {
            topnav.removeClass('topnav-top');
            if(!topnav.hasClass('bg-black') && !topnav.hasClass('bg-white')) topnav.removeClass('transparent');
        } else {
            topnav.addClass('topnav-top');
            if(!topnav.hasClass('bg-black') && !topnav.hasClass('bg-white')) topnav.addClass('transparent');
        }
    }
};

/* Portfolio Isotope */
function portfolioIsotope(){
    if($('.portfolioContainer').length){
        var $container = $('.portfolioContainer');
  
        $container.isotope();
       
        $('.portfolioFilter a').click(function(){
            $('.portfolioFilter .current').removeClass('current');
            $(this).addClass('current');
            var selector = $(this).attr('data-filter');
            $container.isotope({
                filter: selector,
                transitionDuration: "0.7s"
             });
             return false;
        }); 
    }
}

$(window).bind("load", function() {
    portfolioIsotope();
   
});
