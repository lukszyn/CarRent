$(document).ready(function () {

	$(".collapse.show").each(function () {
		$(this).prev(".card-header").addClass("highlight");
	});

	$(".card-header .btn").click(function () {
		$(".card-header").not($(this).parents()).removeClass("highlight");
		$(this).parents(".card-header").toggleClass("highlight");
	});
});