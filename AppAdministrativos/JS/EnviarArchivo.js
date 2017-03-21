$(document).ready(function () {
    $('#btnEnviar').on('click', function () {
        GuardarDocumentos(18,19,20)
    });
    function GuardarDocumentos(Beca, Insc, Exam) {
        var data = new FormData();
        var flIns = $('#BecArchivo'); // FileList object

        flIns = flIns[0].files[0];
        data.append("DocBeca", flIns);
        data.append("DescuentoIdB", Beca);
        flIns = $('#InsArchivo');
        flIns = flIns[0].files[0];
        data.append("DocInscipcion", flIns);
        data.append("DescuentoIdI", Insc);
        flIns = $('#ExamenArchivo');
        flIns = flIns[0].files[0];
        data.append("DocExamen", flIns);
        data.append("DescuentoExam", Exam);

        try{
            var request = new XMLHttpRequest();
            request.open("POST", '../WebServices/WS/Descuentos.asmx/GuardarDocumentos', true);
            request.send(data);
        }
        catch(err){
            console.log(err);
        }
    }
});