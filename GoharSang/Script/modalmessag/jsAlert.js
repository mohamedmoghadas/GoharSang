function AlertInAjax(TextIn, content, Class) {
    var str = '<div class="modal-dialog" role="document"><div class="modal-header alert ' + Class + '"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title" id="myModalLabel">' + TextIn + '</h4></div></div>';
    content.innerHTML = str;
}

