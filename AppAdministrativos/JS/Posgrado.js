$('#chkUniSi').click(function () {
    var chekNo = $('#chkUniNo')[0];
    var txt = $('#txtUniMotivo')[0];
    if(chekNo.checked==true)
    {
        chekNo.attr("checked", false);
        txt.disabled = false;
    } else {
        txt.disabled = false;
    }
});

$('#chkUniNo').click(function () {
    var chekSi = $('#chkUniSi')[0];
    var txt= $('#txtUniMotivo')[0];
    if (chekSi.checked == true) {
        chekSi.attr("checked", false);
        txt.disabled = true;
        txt.value = "";
    } else {
        txt.disabled = true;
    }
});