$(document).ready(function () {
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
})

// preview image
function previewImg(input, idPreview) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function (e) {
			$(idPreview).attr('src', e.target.result);
		}

		reader.readAsDataURL(input.files[0]); // convert to base64 string
	}
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


function FormatMoneyVND(money, selectorResult) {
	$.ajax({
		method: "POST",
		url: "/AjaxCommon/FormatMoneyVND",
		data: { money: money }
	})
		.done(function (data) {
			console.log(data);
			if (data.result != "") {
				$(selectorResult).text(data.result)
			}
		});
}

function FormatNumber(number, selectorResult) {
	$.ajax({
		method: "POST",
		url: "/AjaxCommon/FormatNumber",
		data: { number: number }
	})
		.done(function (data) {
			console.log(data);
			if (data.result != "") {
				$(selectorResult).text(data.result)
			}
		});
}


function CreateSlug(text, selectorResult) {
	$.ajax({
		method: "POST",
		url: "/Admin/Categories/AjaxCreateSlugName",
		data: { str: text }
	})
		.done(function (data) {
			if (data.result != "") {
				$(selectorResult).val(data.result)
			}
		});
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
