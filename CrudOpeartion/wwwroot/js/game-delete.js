$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);

        $.ajax({
            url: `/Games/Delete/${btn.data('id')}`,
            method: 'DELETE',

            success: function () {
                alert('Success');
                btn.parents('tr').fadeOut();
            },

            error: function () {
                alert('Error');
            }
        });
    });
});