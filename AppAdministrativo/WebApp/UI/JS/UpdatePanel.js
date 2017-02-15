var PruebaSL = function () {
    var Inscripcion = function () {
        $('#DescuentoI').noUiSlider({
            start: 50,
            step: 5,
            range: {
                min: 0,
                max: 100
            }
        });

        $('#DescuentoI').noUiSlider_pips({
            mode: 'values',
            values: [20, 80],
            density: 5
        });

        $('#DescuentoI').on('set', function (event, value) {
            if (value < 20) {
                $(this).val(20);
            } else if (value > 80) {
                $(this).val(80);
            }
            $('#valIn').text($(this).val() + '%');
        });

    }
    var Beca = function () {
        $('#DescuentoB').noUiSlider({
            start: 50,
            step: 5,
            range: {
                min: 0,
                max: 100
            }
        });

        $('#DescuentoB').noUiSlider_pips({
            mode: 'values',
            values: [20, 80],
            density: 5
        });

        $('#DescuentoB').on('set', function (event, value) {
            if (value < 20) {
                $(this).val(20);
            } else if (value > 80) {
                $(this).val(80);
            }
            $('#valBe').text($(this).val() + '%');
            
        });
        jQuery('#pulsate-regular').pulsate({
            color: "#bf1c56"
        });
        jQuery("#dvCargosEf").pulsate({
            color: "#bf1c56"
        });
    } 

    return {
        init: function () {
            Inscripcion();
            Beca();
        }

    };
}();

var UIBlockUI = function () {

    var handleSample1 = function () {
        $('#btnGuardar').click(function () {
            Metronic.blockUI({
                target: '#Panel',
                animate: true
            });
            
            window.setTimeout(function () {
                Metronic.unblockUI('#Panel');
            }, 2000);
        });
    }    

    return {
        init: function () {

            handleSample1();
        }

    };

}();


var ComponentsPickers = function () {

    var handleDatePickers = function () {
        var Now = new Date();
        var años = Now.getFullYear() - 18;
        var mes = Now.getMonth() + 1;
        var Fecha = Now.getDate() + '-' +mes + '-' + años;
        
        if (jQuery().datepicker) {
            $('.date-picker').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                language: 'es'
            });
            $(".date-picker").datepicker("setDate", Fecha);
            $('#spnA').text(calcular_edad(Fecha));
            //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }
        $('#txtFNacimiento').change(function () {
            var cumple = $('#txtFNacimiento').val();
            var a = calcular_edad(cumple);
            $('#spnA').text(a);
        });
        

        /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
    }
    /*----------Funcion para obtener la edad------------*/
    function calcular_edad(fecha) {
        var fechaActual = new Date()
        var diaActual = fechaActual.getDate();
        var mmActual = fechaActual.getMonth()+1;
        var yyyyActual = fechaActual.getFullYear();
        FechaNac = fecha.split("-");
        var diaCumple = FechaNac[0];
        var mmCumple = FechaNac[1];
        var yyyyCumple = FechaNac[2];

        var edad = yyyyActual - yyyyCumple;
        var meses = mmActual - mmCumple;

        if (meses < 0) {
            edad = edad - 1;
            meses = 12 + meses;
        }
    
        return edad+' Años,'+meses+' meses';
    }

    
    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
        }
    };

}();