function showAttachments(TaskId) {
    $.ajax({
        type: 'GET',
        url: 'Task/GetAttachments/' + TaskId,
        success: function (response, status, xhr) {

        },
        error: function (req, status, error) {
            alert(error);
        }
    })
}
