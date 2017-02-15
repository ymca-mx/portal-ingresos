$(document).ready(function () {
    $('#BecArchivo').bind('change', function () {
        var file = $('#FileBec');
        var tex = $('#txtBeca').html();
        if (this.files.length > 0) {
            $('#txtBeca').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileBec span span').text('Cambiar');
        }
        else {
            $('#txtBeca').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileBec span span').text('Seleccionar Archivo...');
        }
    });
    $('#FileBec a').click(function () {
        var file = $('#FileBec');
        $('#txtBeca').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileBec span span').text('Seleccionar Archivo...');
    });

    $('#InsArchivo').bind('change',function () {
        var file = $('#FileIns');
        var tex = $('#txtIns').html();
        if (this.files.length > 0) {
            $('#txtIns').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileIns span span').text('Cambiar');
        }
        else {
            $('#txtIns').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileIns span span').text('Seleccionar Archivo...');
        }
    });
    $('#FileIns a').click(function () {
        var file = $('#FileIns');
        $('#txtIns').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileIns span span').text('Seleccionar Archivo...');
    });

    $('#ExamenArchivo').bind('change',function () {
        var file = $('#FileExa');
        var tex = $('#txtExa').html();
        if (this.files.length > 0) {
            $('#txtExa').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileExa span span').text('Cambiar');
        }
        else {
            $('#txtExa').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileExa span span').text('Seleccionar Archivo...');
        }
    });
    $('#FileExa a').click(function () {
        var file = $('#FileExa');
        $('#txtExa').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileExa span span').text('Seleccionar Archivo...');
    });
    function RecortarNombre(name) {
        var cadena;
        if (name.length > 15) {
            cadena = name.substring(0, 8);
            cadena += name.substring(name.length - 4, name.length);
            return cadena;
        } else {
            return name;
        }
    }
});


//// Hola soy una prueba