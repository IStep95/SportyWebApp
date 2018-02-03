function subsribeEventShowModal(elem) {
    var id = $(elem).data('id');
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    $('#myModal').modal('show');

}