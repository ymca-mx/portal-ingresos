$(function () {
    var ArchivosFn = {
        init() {
            $('#FileBec .close').hide();
            $('#FileIns .close').hide();
            $('#FileExa .close').hide();

            $('#BecArchivo').bind('change', function () {
                ArchivosFn.bind_Change('FileBec', 'txtBeca',this);
            });
            $('#FileBec a').click(function () {
                ArchivosFn.Click_a('FileBec', 'txtBeca');
            });

            $('#InsArchivo').bind('change', function () {
                ArchivosFn.bind_Change('FileIns', 'txtIns', this);
            });
            $('#FileIns a').click(function () {
                ArchivosFn.Click_a('FileIns', 'txtIns');
            });

            $('#ExamenArchivo').bind('change', function () {
                ArchivosFn.bind_Change('FileExa', 'txtExa', this);
            });
            $('#FileExa a').click(function () {
                ArchivosFn.Click_a('FileExa', 'txtExa');
            });
        },
        bind_Change(FileId, txtId, thisfile) {

            var file = $('#' + FileId);
            var tex = $('#' + txtId).html();
            if (thisfile.files.length > 0) {
                $('#' + txtId).text(ArchivosFn.RecortarNombre(thisfile.files[0].name));
                file.addClass('fileinput-exists').removeClass('fileinput-new');
                $('#' + FileId + ' span span').text('Cambiar');
                $('#' + FileId + ' .close').show();
            }
            else {
                $('#' + txtId).text('');
                file.removeClass('fileinput-exists').addClass('fileinput-new');
                $('#' + FileId + ' span span').text('Seleccionar Archivo...');
                $('#' + FileId + ' .close').hide();
            }
        },
        Click_a(FileId, txtId) {

            var file = $('#' + FileId);
            $('#' + txtId).text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            file[0] = null;
            $('#' + FileId + ' span span').text('Seleccionar Archivo...');
            $('#' + FileId + ' .close').hide();
        },
        RecortarNombre(name) {
            var cadena;
            if (name.length > 15) {
                cadena = name.substring(0, 8);
                cadena += name.substring(name.length - 4, name.length);
                return cadena;
            } else {
                return name;
            }
        }
    };

    ArchivosFn.init();
});