function subsribeEventShowModal(subscribeButton) {
    var id = $(subscribeButton).data('id');
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    $('#myModal').modal('show');

}

function redirectToDetails(eventID) {
    console.log('Stisno details.');
    console.log(eventID);
    window.location.href = "../Events/Details/" + eventID;
}