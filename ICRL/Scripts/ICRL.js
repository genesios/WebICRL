function ToggleGridPanel(btn, row) {
    var current = $('#' + row).css('display');
    if (current == 'none') {
        $('#' + row).show();
        $(btn).removeClass('glyphicon-plus')
        $(btn).addClass('glyphicon-minus')
    } else {
        $('#' + row).hide();
        $(btn).removeClass('glyphicon-minus')
        $(btn).addClass('glyphicon-plus')
    }
    return false;
}

function ConfirmarActualizarOnBase() {
    var resultado = confirm('¿Se actualizará los datos desde OnBase, esta seguro ?');
    if (resultado) {
        return true;
    }
    else {
        return false;
    }
}

//ConfirmarFinalizarFlujoOnBase()
function ConfirmarFinalizarFlujoOnBase() {
    var resultado = confirm('¿Se Finalizará el Flujo , esta seguro ?');
    if (resultado) {
        return true;
    }
    else {
        return false;
    }
}

function ConfirmarFinalizarCotizacion() {
    var resultado = confirm('¿Se Finalizará la cotización, esta seguro ?');
    if (resultado) {
        return true;
    }
    else {
        return false;
    }
}
