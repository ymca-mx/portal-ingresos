$(document).ready(function () {
    function DisableBack() {
        window.history.forward();
    }
    DisableBack();
    window.onload = DisableBack;
    window.onpageshow = function (evt) {
        if (evt.persisted) DisableBack();
    }

    window.onunload = function () { void (0); };
});