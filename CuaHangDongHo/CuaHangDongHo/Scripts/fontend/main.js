/*price range*/

 $('#sl2').slider();

	var RGBChange = function() {
	  $('#RGB').css('background', 'rgb('+r.getValue()+','+g.getValue()+','+b.getValue()+')')
	};	
		
/*scroll to top*/

$(document).ready(function(){
	$(function () {
		$.scrollUp({
	        scrollName: 'scrollUp', // Element ID
	        scrollDistance: 300, // Distance from top/bottom before showing element (px)
	        scrollFrom: 'top', // 'top' or 'bottom'
	        scrollSpeed: 300, // Speed back to top (ms)
	        easingType: 'linear', // Scroll to top easing (see http://easings.net/)
	        animation: 'fade', // Fade, slide, none
	        animationSpeed: 200, // Animation in speed (ms)
	        scrollTrigger: false, // Set a custom triggering element. Can be an HTML string or jQuery object
					//scrollTarget: false, // Set a custom target element for scrolling to the top
	        scrollText: '<i class="fa fa-angle-up"></i>', // Text for element, can contain HTML
	        scrollTitle: false, // Set a custom <a> title if required.
	        scrollImg: false, // Set true to use image
	        activeOverlay: false, // Set CSS color to display scrollUp active point, e.g '#00FFFF'
	        zIndex: 2147483647 // Z-Index for the overlay
		});
	});

	// set disabled khi khong chon quan/huyen
	let districstId = $("#DistricstId").val()
	if (districstId == undefined || districstId == null || districstId == "") {
		$("#DistricstId").attr("disabled", true)

		// set disabled khi khong chon xa
		let communeId = $("#CommuneId").val()
		if (communeId == undefined || communeId == null || communeId == "") {
			$("#CommuneId").attr("disabled", true)
		}
	}

	setShowHideInput()

	$("#ProvinceId").change(function () {
		getDistricts(this.value)
	});

	$("#DistricstId").change(function () {
		getCommunes(this.value)
	});
});


function ShowDisplayLoading() {
	$('#loading').fadeIn(1000);
}

function HideDisplayLoading() {
	$('#loading').fadeOut(1000);
}

// add 1 san pham
function AddToCart(id) {
	var data;
	if ($("#quantity").val() != undefined && $("#quantity").val() != null && $("#quantity").val() > 0) {
		data = {id: id }
	}
	ShowDisplayLoading();
	$.ajax({
		method: "POST",
		url: "/Cart/AddToCart",
		data: { id: id }
	})
		.done(function (data) {
			if (data.err == 1) {
				window.location.href = "/Auth/Login"
			}
			else if (data.err == 0) {
				$('span#lblCartCount').text(data.msg)
            }
			console.log(data)
			HideDisplayLoading();
		});
}

// add nhieu san pham
function AddToCarts(id) {
	ShowDisplayLoading();
	$.ajax({
		method: "POST",
		url: "/Cart/AddToCart",
		data: { id: id, quantity: $("#quantity").val() }
	})
		.done(function (data) {
			if (data.err == 1) {
				window.location.href = "/Auth/Login"
			}
			else if (data.err == 0) {
				$('span#lblCartCount').text(data.msg)
            }
			HideDisplayLoading();
		});
}

// delay function
function delay(callback, ms) {
	var timer = 0;
	return function () {
		var context = this, args = arguments;
		clearTimeout(timer);
		timer = setTimeout(function () {
			callback.apply(context, args);
		}, ms || 0);
	};
}


function getDistricts(ProvinceId) {
	$.ajax({
		method: "POST",
		url: "/AjaxCommon/AjaxGetDistricts",
		data: { str: ProvinceId }
	})
		.done(function (data) {
			if (data.length > 0) {
				$("#DistricstId").attr("disabled", false)
				$('#DistricstId').empty(); //remove all child nodes

				$.each(data, function (index, item) {
					let newOption = $('<option value="' + item.Id + '">' + item.Name + '</option>');
					$('#DistricstId').append(newOption);
				});
			}
			else {
				setDistrictsDefault()
			}
			setCommunesDefault()
			setShowHideInput()
		});
}

function getCommunes(DistrictId) {
	$.ajax({
		method: "POST",
		url: "/AjaxCommon/AjaxGetCommunes",
		data: { str: DistrictId }
	})
		.done(function (data) {
			if (data.length > 0) {
				$("#CommuneId").attr("disabled", false)
				$('#CommuneId').empty(); //remove all child nodes

				$.each(data, function (index, item) {
					let newOption = $('<option value="' + item.Id + '">' + item.Name + '</option>');
					$('#CommuneId').append(newOption);
				});
			}
			else {
				setCommunesDefault()
			}
			setShowHideInput()
		});
}

function setDistrictsDefault() {
	$("#DistricstId").attr("disabled", true)
	$('#DistricstId').empty();
	let newOption = $('<option value="">Quận/Huyện</option>');
	$('#DistricstId').append(newOption);
}

function setCommunesDefault() {
	$("#CommuneId").attr("disabled", true)
	$('#CommuneId').empty();
	let newOption = $('<option value="">Xã/Phường</option>');
	$('#CommuneId').append(newOption);
}

function setShowHideInput() {
	$("#DetailAddress").attr("disabled", $("#CommuneId").is(':disabled'))
}