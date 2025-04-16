$(document).ready(function () {
	$('.plusOne').click(function () {
		var productId = $(this).data('product-id');
		$.ajax({
			url: '@Url.Action("AddOne", "Cart")',
			type: 'POST',
			data: { id: productId },
			success: function (response) {
				if (response.success) {
					// alert(response.message);
					$("#" + productId).html(`${response.quantity}`);
					$("#totalPrice").html(`Total price = ${(response.total).toFixed(2)}`)
					$("#in" + productId).val(response.quantity);
				}
			},
			error: function () {
				alert('An error occurred while increasing the product in the cart.');
			}
		});
	});

	$('.minusOne').click(function () {
		var productId = $(this).data('product-id');

		let value = parseInt($("#spanCount").html());

		$.ajax({
			url: '@Url.Action("DecreaseOne", "Cart")',
			type: 'POST',
			data: { id: productId },
			success: function (response) {
				if (response.success) {
					// alert(response.message);
					if (response.quantity >= 1) {
						$("#" + productId).html(`${response.quantity}`);
						$("#in" + productId).val(response.quantity);
					} else {
						$("#row" + productId).remove();

						$("#spanCount").html(`${value - 1}`); //To decrease the counter beside the cart
					}
					$("#totalPrice").html(`Total price = ${(response.total).toFixed(2)}`)
				}
			},
			error: function () {
				alert('An error occurred while increasing the product in the cart.');
			}
		});
	});

	$('.delete').click(function () {
		var productId = $(this).data('product-id');

		let value = parseInt($("#spanCount").html());

		$.ajax({
			url: '@Url.Action("DeleteProduct", "Cart")',
			type: 'POST',
			data: { id: productId },
			success: function (response) {
				if (response.success) {
					// alert(response.message);
					if (response.isDeleted) {
						$("#row" + productId).remove();

						$("#spanCount").html(`${value - 1}`); //To decrease the counter beside the cart
					}
					$("#totalPrice").html(`Total price = ${(response.total).toFixed(2)}`)
				}
			},
			error: function () {
				alert('An error occurred while deleting the product in the cart.');
			}
		});
	});
});