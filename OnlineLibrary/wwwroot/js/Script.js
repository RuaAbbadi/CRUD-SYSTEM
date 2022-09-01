let colors = ['red', 'green', 'blue', 'yellow', 'orange'];

for (let i = 0; i < colors.length; i++) {
    $(".color-options li").eq(i).css("backgroundColor", colors[i]);
}


$('.color-options li').click(function () {
    let currentColor = $(this).css("backgroundColor");
    $('.change').css("color", currentColor);
})

$(".color-box").click(function () {
    let currentWidth = $(".color-options").outerWidth();
    console.log(currentWidth);
    if ($(".color-box").css("left") == "0px") {
        $('.color-box').animate({ "left": -currentWidth }, 5000);
    } else {
        $('.color-box').animate({ "left": 0 }, 5000);
    }
})







