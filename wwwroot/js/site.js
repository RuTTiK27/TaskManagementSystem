// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const sessionTimeout = 15 * 60 * 1000;
const warningTime = 14 * 60 * 1000;

//_Layout.cshtml
setTimeout(() => {
    if (confirm("Your session is about to expire. Would you like to extend it?")) {
        fetch('/Home/ExtendSession', { method: 'POST' })
            .then(response => {
                if (response.ok) {
                    alert("Your session has been extended!");
                } else {
                    alert('Unable to extend the session. Please login again');
                }
            });
    }
}, warningTime);

//For toast message
if (document.getElementById('showToast')!=null) {
    if (document.getElementById('showToast').value == "Yes") {
        var toastLiveExample = document.getElementById('liveToast')
        var toast = new bootstrap.Toast(toastLiveExample)
        toast.show()
    }
}
//For show attachment
function showAttachments(taskId) {
    const $attachmentBody = $("#attachments-body");
    $attachmentBody.html('<tr><td colspan="4">Loading...</td></tr>');

    $.ajax({
        url: '/Task/GetAttachments',
        type: 'GET',
        data: { taskId: taskId },
        success: function (response) {
            $attachmentBody.empty();

            if (response.success) {
                $.each(response.data, function (index, attachment) {
                    const row = `
                                <tr>
                                    <td>${attachment.attachmentId}</td>
                                    <td>${attachment.fileName}</td>
                                    <td>${attachment.fileType}</td>
                                    <td><a href="${attachment.filePath}" target="_blank">Download</a></td>
                                </tr>
                            `;
                    $attachmentBody.append(row);
                });
            } else {
                $attachmentBody.html(`<tr><td colspan="4">${response.message || "No attachments found."}</td></tr>`)
            }
        },
        error: function () {
            $attachmentBody.html('<tr><td colspan="4">Error loading attachments</td></tr>')
        }
    });
}

//AddTask.cshtml
//For view selected files
function previewFiles() {
    const preview = document.getElementById('filePreview');
    const input = document.getElementById('Attachments');
    const files = input.files;

    preview.innerHTML = '';
    preview.classList.remove("d-none")
    if (files.length > 0) {
        const list = document.createElement('ul');
        list.classList.add('list-unstyled');

        for (let i = 0; i < files.length; i++) {
            const listItem = document.createElement('li');
            listItem.textContent = files[i].name;
            list.appendChild(listItem);
        }
        preview.append("These files will be uploaded. If you have selected the wrong file(s), please select them again carefully.");
        preview.appendChild(list);
    } else {
        preview.textContent = 'No files selected';
    }
}

//VerifyUser.cshtml
//For disable resend button
function disableResendButton() {
    $.ajax({
        type: 'POST',
        url: '@Url.Action("ResendVerification")', /* Home/Add */
        data: {},
        dataType: 'text', /* dataType:'json', */
        success: function (response, status, xhr) {
            console.log("Email resent successfully", response);
        },
        error: function (req, status, error) {
            console.log("Error occured while sending email", error);
        }
    });

    const resendButton = document.getElementById('resend-btn');
    const timerMsg = document.getElementById('timer-msg');
    const timerSpan = document.getElementById('timer');

    // Disable the button
    resendButton.disabled = true;

    // Show the timer message
    timerMsg.style.display = 'block';

    // Timer countdown (30 seconds)
    let secondsLeft = 30;
    const interval = setInterval(() => {
        secondsLeft--;
        timerSpan.textContent = secondsLeft;

        if (secondsLeft <= 0) {
            clearInterval(interval);
            // Enable the button after the countdown
            resendButton.disabled = false;
            timerMsg.style.display = 'none';
        }
    }, 1000);
}

document.querySelectorAll('.open-modal').forEach(button => {
    button.addEventListener('click', function () {
        const entity = this.getAttribute('data-entity'); 
        const entityId = this.getAttribute('data-id');
        const entityName = this.getAttribute('data-filename');
        showModal(entity, entityId, entityName)
    });
});

function showModal(entity,entityId, entityName) {
    var myModal = new bootstrap.Modal(document.getElementById('exampleModal'), {
        keyboard: false
    });
    myModal.show();
    if (entity == "attachment") {
        document.getElementById('entityId').href = `/Task/DeleteAttatchment/?AttachmentId=${entityId}`;
        document.getElementById('entityName').innerText = entityName
        document.getElementById('entityLabel').innerText = "Are you sure you want to delete attachment?";
    } else if (entity == "task") {
        document.getElementById('entityId').href = `/Task/DeleteTask/?taskId=${entityId}`;
        document.getElementById('entityName').innerText = entityName
        document.getElementById('entityLabel').innerText = "Are you sure you want to delete this task?";
    }
    
}
