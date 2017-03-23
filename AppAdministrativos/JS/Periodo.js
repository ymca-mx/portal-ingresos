var Periodo = function () {
    var cargarPeriodo = function () {
        var PeriodoAlcorriente = null;
        var Pariodo = null;
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPeriodos",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                var sig = null, act = null, sig2 = null;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.PeriodoId +" "+ this.Anio);
                    
                    $("#slcPeriodo").append(option);
                });
                Pariodo = datos[0].PeriodoId + " " + datos[0].Anio;
                $("#slcPeriodo").val(Pariodo);
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
            }
        });
    }

    function Calcular(fInicial, fFinal) {
        var MesI = fInicial.split("/");
        var Anio = parseInt(MesI[2], 10);
        var AnioA = new Date();
        AnioA = AnioA.getFullYear();

        if (Anio == AnioA) {
            MesI = parseInt(MesI[1], 10);
            var MesF = fFinal.split("/");
            MesF = parseInt(MesF[1], 10);
            var MesAct = new Date();
            MesAct = MesAct.getMonth() + 1;
            if (MesAct >= MesI && MesAct <= MesF) {
                return MesAct;
            }
            return null;
        }
        return null
    }

    return {
        //main function to initiate the module
        init: function () {

            cargarPeriodo();
        }

    };
}();