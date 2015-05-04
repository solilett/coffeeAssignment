





function redirect() {
    location.href = '~/Account/Manage';
    alert("cick");
}


$('.items').each(function () {
    $(this).mouseover(function () {
        $(this).css('opacity', '0.8')
    });
    $(this).mouseout(function () {
        $(this).css('opacity', '1');
    });

});


