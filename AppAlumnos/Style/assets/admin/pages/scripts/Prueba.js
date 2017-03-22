var Prueba = function () {
    var demo6 = function () {
        $('#soft').noUiSlider({
            start: 50,
            range: {
                min: 0,
                max: 100
            }
        });

        $('#soft').noUiSlider_pips({
            mode: 'values',
            values: [20, 80],
            density: 4
        });

        $('#soft').on('set', function (event, value) {
            if (value < 20) {
                $(this).val(20);
            } else if (value > 80) {
                $(this).val(80);
            }
            $('#txtPorcentaje').val($(this).val());
        });
         $("#soft-btn").click(function()
        {
            $('#txtPorcentaje').val($(this).val());
        });

    }

    return {
        //main function to initiate the module
        init: function () {
            demo6();
        }

    };
} ();