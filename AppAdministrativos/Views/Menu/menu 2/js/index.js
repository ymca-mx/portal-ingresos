$('[data-sidenav]').sidenav();
var size = false;

var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-36251023-1']);
_gaq.push(['_setDomainName', 'jqueryscript.net']);
_gaq.push(['_trackPageview']);

(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();


$(window).resize(function () {
    if ($(window).width() < 600 && size==false) {
        size = true;
        if ($('.sidenav.show')[0] != undefined){ $("#sidenav-toggle").click();}
        console.log(size);
    } else if ($(window).width() > 800 && size == true) {
        size = false;
        console.log(size);
    }
});

$('li').on('click','a', function()
{
    //alert($(this).attr('href'));
    $("#divDinamico").load("file:///C:/Users/Jesus_Galvan/AppData/Local/Microsoft/Windows/INetCache/Content.Outlook/G3BNM7IR/EnvioReporteVistoBueno.html");
});
