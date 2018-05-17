var MesP = [];
$('#txtDescuentoBec').keyup(function () {
    var Maximo = $('#txtDescuentoBec').data();
    if (Maximo.valMax < $('#txtDescuentoBec').val()) { $('#txtDescuentoBec').val(Maximo.valMax); return false; }
    var monto = CalcularDescuento($('#txtcuotaCol').text().replace('$', ''), $('#txtDescuentoBec').val());
    $('#txtPagarCol').text('$' + String(monto));
    RecalculaTabla($('#txtDescuentoBec').val());
});
$('#txtDescuentoBec').knob({
    change: function (val) {
        var Maximo = $('#txtDescuentoBec').data();
        if (Maximo.valMax < val) { $('#txtDescuentoBec').val(Maximo.valMax); return false; }
        var monto = CalcularDescuento($('#txtcuotaCol').text().replace('$', ''), val);
        $('#txtPagarCol').text('$' + String(monto));
        RecalculaTabla(val);
    }
});
$('#txtDescuentoCred').keyup(function () {
    var Maximo = $('#txtDescuentoCred').data();
    if (Maximo < $('#txtDescuentoCred').val()) { $('#txtDescuentoCred').val(Maximo.valMax); return false; }
    var monto = CalcularDescuento($('#txtcuotaCred').text().replace('$', ''), $('#txtDescuentoCred').val());
    $('#txtPagarCred').text('$' + String(monto));
});
$('#txtDescuentoCred').knob({
    change: function (val) {
        var Maximo = $('#txtDescuentoCred').data();
        if (Maximo.valMax < val) { $('#txtDescuentoCred').val(Maximo.valMax); return false; }
        var monto = CalcularDescuento($('#txtcuotaCred').text().replace('$', ''), val);
        $('#txtPagarCred').text('$' + String(monto));
    }
});
$('#txtDescuentoIns').keyup(function () {
    var Maximo = $('#txtDescuentoIns').data();
    if (Maximo.valMax < $('#txtDescuentoIns').val()) { $('#txtDescuentoIns').val(Maximo.valMax); return false; }
    $('#txtPagarIn').text('$' +
        String(CalcularDescuento($('#txtcuotaIn').text().replace('$', ''), $('#txtDescuentoIns').val())));
});
$('#txtDescuentoIns').knob({
    change: function (val) {
        var Maximo = $('#txtDescuentoIns').data();
        if (Maximo.valMax < val) { $('#txtDescuentoIns').val(Maximo.valMax); return false; }
        $('#txtPagarIn').text('$' +
       String(CalcularDescuento($('#txtcuotaIn').text().replace('$', ''), val)));
    }
});
$('#txtDescuentoExa').keyup(function () {
    var Maximo = $('#txtDescuentoExa').data();
    if (Maximo.valMax < $('#txtDescuentoExa').val()) { $('#txtDescuentoExa').val(Maximo.valMax); return false; }
    $('#txtPagarExa').text('$' +
        String(CalcularDescuento($('#txtcuotaExa').text().replace('$', ''), $('#txtDescuentoExa').val())));
});
$('#txtDescuentoExa').knob({
    change: function (val) {
        var Maximo = $('#txtDescuentoExa').data();
        if (Maximo.valMax < val) { $('#txtDescuentoExa').val(Maximo.valMax); return false; }
        $('#txtPagarExa').text('$' +
       String(CalcularDescuento($('#txtcuotaExa').text().replace('$', ''), val)));
    }
});
function CalcularDescuento(Monto, Descuento) {
    var Redondeado;
    Monto = parseFloat(Monto);
    Descuento = (Monto * (Descuento / 100))
    Redondeado = Monto - Descuento;
    Redondeado = Math.round(Redondeado);
    return Redondeado;
}
function RecalculaTabla(monto) {
    if (MesP.length === 0) {
        for (i = 0; i < 4; i++) {
           MesP[i]= $('#mes' + i).text().replace('$','');
        }
    }
    var filx;
    var descu;
    for (i = 0; i < 4; i++) {
        if (MesP[i] !== '0.00') {
            filx = $('#mes' + i);
            descu = CalcularDescuento(MesP[i], monto);
            $(filx).text('$' + String(descu));
        }
    }
}
