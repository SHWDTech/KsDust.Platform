$(function() {
    $('.accroding-menu-item').on('click', function (event) {
        if (!$(event.target).is('.panel-heading')) return;
        $('.accroding-menu-item > .panel-body').height(0);
        $(event.target).parent().find('.panel-body').height($('.accroding-menu').height() - $('.accroding-menu-item').length * 24.6 - 1);
    });
    $($('.accroding-menu-item')[0]).find('.panel-heading').click();
});