/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Date Picker
5. Init Google Map


******************************/

var map;
var infowindow;
var marker = new Array();
var old_id = 0;
var infoWindowArray = new Array();
var infowindow_array = new Array();

$(document).ready(function () {
    "use strict";

    /* 

	1. Vars and Inits

	*/

    var header = $('.header');

    setHeader();

    $(window).on('resize', function () {
        setHeader();

        setTimeout(function () {
            $(window).trigger('resize.px.parallax');
        }, 375);
    });

    $(document).on('scroll', function () {
        setHeader();
    });

    initMenu();
    initGoogleMap();

    /* 

	2. Set Header

	*/

    function setHeader() {
        if ($(window).scrollTop() > 91) {
            header.addClass('scrolled');
        }
        else {
            header.removeClass('scrolled');
        }
    }

    /* 

	3. Init Menu

	*/

    function initMenu() {
        if ($('.menu').length) {
            var menu = $('.menu');
            var hamburger = $('.hamburger');
            var close = $('.menu_close');

            hamburger.on('click', function () {
                menu.toggleClass('active');
            });

            close.on('click', function () {
                menu.toggleClass('active');
            });
        }
    }

    /*

	5. Init Google Map

	*/
    

    function initGoogleMap() {
        var myLatlng = new google.maps.LatLng(12.2465969, 109.1959485);
        var mapOptions =
    	{
    	    center: myLatlng,
    	    zoom: 15,
    	    mapTypeId: google.maps.MapTypeId.ROADMAP,
    	    draggable: true,
    	    scrollwheel: false,
    	    zoomControl: true,
    	    zoomControlOptions:
			{
			    position: google.maps.ControlPosition.RIGHT_CENTER
			},
    	    mapTypeControl: false,
    	    scaleControl: false,
    	    streetViewControl: false,
    	    rotateControl: false,
    	    fullscreenControl: true,
    	    styles:
			[
			  {
			      "featureType": "road.highway",
			      "elementType": "geometry.fill",
			      "stylers": [
                    {
                        "color": "#ffeba1"
                    }
			      ]
			  }
			]
    	}

        // Initialize a map with options
        map = new google.maps.Map(document.getElementById('map'), mapOptions);

        // Re-center map after window resize
        google.maps.event.addDomListener(window, 'resize', function () {
            setTimeout(function () {
                google.maps.event.trigger(map, "resize");
                map.setCenter(myLatlng);
            }, 1400);
        });

        var arrLatLng = new google.maps.LatLng(12.2465969, 109.1959485);
        infoWindowArray[7895] = '<div class="map_description"><div class="map_title">Luxury Nha Trang Hotel</div><div>24 Trần Phú, Lộc Thọ, Tp. Nha Trang, Khánh Hòa</div></div>';
        loadMarker(arrLatLng, infoWindowArray[7895], 7895);

        moveToMaker(7895);
    }

    function loadMarker(myLocation, myInfoWindow, id) {
        marker[id] = new google.maps.Marker({ position: myLocation, map: map, visible: true });
        var popup = myInfoWindow;
        infowindow_array[id] = new google.maps.InfoWindow({ content: popup });
        google.maps.event.addListener(marker[id], 'mouseover', function () {
            if (id == old_id)
                return;
            if (old_id > 0)
                infowindow_array[old_id].close();
            infowindow_array[id].open(map, marker[id]);
            old_id = id;
        });
        google.maps.event.addListener(infowindow_array[id], 'closeclick', function () {
            old_id = 0;
        });
    }
    function moveToMaker(id) {
        var location = marker[id].position;
        map.setCenter(location);
        if (old_id > 0)
            infowindow_array[old_id].close();
        infowindow_array[id].open(map, marker[id]);
        old_id = id;
    }

});