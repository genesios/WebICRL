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