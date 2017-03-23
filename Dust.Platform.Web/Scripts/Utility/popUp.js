(function () {
    var modal = $('#modalPopUp');
    modal.title = $('#modalPopUpLabel');
    modal.body = $('#modalPopUp').find('.modal-body');
    window.popUp = function (options) {
        modal.title.html(options.title);
        modal.body.html(options.body);
        switch (options.size) {
            case 'large':
                modal.find('.modal-dialog').addClass('modal-lg');
            case 'small':
                modal.find('.modal-dialog').addClass('modal-sm');
            default:
        }
        debugger;
        if (options.call != null) {
            options.call();
        }
        modal.modal();
    }

    $('#modalPopUp').on('hidden.bs.modal', function (e) {
        modal.body.empty();
        modal.find('.modal-dialog').removeClass('modal-lg').removeClass('modal-sm');
    });
})()