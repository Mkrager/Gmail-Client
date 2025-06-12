function escapeHtml(str) {
    const div = document.createElement('div');
    div.appendChild(document.createTextNode(str));
    return div.innerHTML;
}

function openModal(body) {
    document.getElementById('emailModal').style.display = 'flex';

    try {
        const decodedBody = JSON.parse(body);

        if (!window.emailEditor) {
            ClassicEditor
                .create(document.querySelector('#emailContent'))
                .then(editor => {
                    window.emailEditor = editor;
                    editor.setData(decodedBody);
                })
                .catch(error => {
                    console.error("Error:", error);
                    document.getElementById('emailContent').value = decodedBody;
                });
        } else {
            window.emailEditor.setData(decodedBody);
        }
    } catch (err) {
        console.error("Error:", err);
        document.getElementById('emailContent').value = body;
    }
}
function closeModal() {
    document.getElementById('emailModal').style.display = 'none';
}
function openSendEmailModal() {
    document.getElementById("sendEmailModal").style.display = "flex";

    if (!window.sendEmailEditor) {
        ClassicEditor
            .create(document.querySelector("#sendEmailContent"))
            .then(editor => {
                window.sendEmailEditor = editor;
            })
            .catch(error => {
                console.error(error);
            });
    }
}

function closeSendEmailModal() {
    document.getElementById('sendEmailModal').style.display = 'none';
}

async function sendEmail(event) {
    event.preventDefault();

    const to = document.getElementById('emailTo').value;
    const subject = document.getElementById('emailSubject').value;
    const body = sendEmailEditor.getData();

    console.log(body);

    if (!body) {
        alert('Body requried');
        return false;
    }

    try {
        await fetch('/dashboard/send', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ to, subject, body })
        });

        closeSendEmailModal();
    } catch (error) {
        alert("Unexpected error: " + error.message);
        console.error("Send email failed:", error);
    }

    return false;
}

async function getNext10Messages(nextPageToken) {
    if (nextPageToken === null) {
        document.getElementById('btnLoadMore').style.display = 'none';
    }

    if (!nextPageToken) return;

    const response = await fetch(`/Dashboard/GetMessagesPage?pageToken=${nextPageToken}`);
    if (!response.ok) {
        alert('Error');
        return;
    }

    const data = await response.json();
    const tbody = document.querySelector('table tbody');

    data.messages.forEach(letters => {
        const tr = document.createElement('tr');
        const jsonBody = JSON.stringify(letters.body);
        const safeHtmlDataBody = escapeHtml(jsonBody);

        tr.innerHTML = `
            <td>${escapeHtml(letters.subject)}</td>
            <td>${escapeHtml(letters.from)}</td>
            <td>${new Date(letters.date).toLocaleDateString('uk-UA')}</td>
            <td class="${letters.isSent ? 'status-sent' : 'status-inbox'}">
                ${letters.isSent ? 'Sent' : 'Inbox'}
            </td>
            <td>
                <button class="btn-view"
                        data-body='${safeHtmlDataBody}'
                        onclick="openModal(this.dataset.body)">
                    View
                </button>
            </td>
        `;

        tbody.appendChild(tr);
    });

    const loadMoreBtn = document.getElementById('btnLoadMore');
    if (data.nextPageToken) {
        loadMoreBtn.style.display = 'inline-block';
        loadMoreBtn.onclick = () => getNext10Messages(data.nextPageToken);
    } else {
        loadMoreBtn.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', function () {
    initEditors();

    const sendForm = document.getElementById('sendEmailForm');
    if (sendForm) {
        sendForm.addEventListener('submit', sendEmail);
    }
});

async function saveDraft() {
    const to = document.getElementById('emailTo').value;
    const subject = document.getElementById('emailSubject').value;
    const body = window.sendEmailEditor.getData();

    if (!body) {
        alert('Body requried');
        return false;
    }

    const draftData = {
        to: to,
        subject: subject,
        body: body
    };

    try {
        
        await fetch('/Draft/SaveDraft', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(draftData)
        });

    } catch (error) {
        alert('Error saving draft: ' + error.message);
    }
}

function openSendEmailModalWithDraft(draft) {
    document.getElementById('draftId').value = draft.draftId;
    document.getElementById('draftReminder').style.display = 'none';
    document.getElementById('draftEmailTo').value = draft.to || '';
    document.getElementById('draftEmailSubject').value = draft.subject || '';
    document.getElementById('sendDraftContent').value = draft.body || '';

    openSendDraftModal();
}

function openSendDraftModal() {
    document.getElementById("sendDraftModal").style.display = "flex";

    if (!window.sendEmailEditor) {
        ClassicEditor
            .create(document.querySelector("#sendDraftContent"))
            .then(editor => {
                window.sendEmailEditor = editor;
            })
            .catch(error => {
                console.error(error);
            });
    }
}

async function sendDraft(event) {
    event.preventDefault();

    const to = document.getElementById('draftEmailTo').value;
    const subject = document.getElementById('draftEmailSubject').value;
    const body = sendEmailEditor.getData();
    const draftId = document.getElementById('draftId').value;

    console.log(draftId);

    console.log(body);

    if (!body) {
        alert('Body requried');
        return false;
    }

    try {
        const response = await fetch('/dashboard/send', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ to, subject, body })
        });

        if (response.ok) {
            await fetch(`/draft/deleteDraft?draftId=${encodeURIComponent(draftId)}`, {
                method: 'DELETE'
            });
        }

        closeSendDraftModal();
    } catch (error) {
        alert("Unexpected error: " + error.message);
        console.error("Send email failed:", error);
    }

    return false;
}


function closeSendDraftModal() {
    document.getElementById('sendDraftModal').style.display = 'none';
}

function closeAllertBox() {
    document.getElementById('alert-box').style.display = 'none';
}

async function updateDraft() {
    const draftId = document.getElementById('draftId').value;
    const to = document.getElementById('draftEmailTo').value;
    const subject = document.getElementById('draftEmailSubject').value;
    const body = window.sendEmailEditor.getData();

    if (!body) {
        alert('Body requried');
        return false;
    }

    try {
        const response = await fetch('/Draft/UpdateDraft', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ to, subject, body, draftId })
        });


    } catch (error) {
        alert('Error saving draft: ' + error.message);
    }
}


window.addEventListener('DOMContentLoaded', async () => {
    try {
        const response = await fetch('/Draft/CheckLastDraft');
        if (response.ok) {
            const html = await response.text();
            if (html) {
                const draftDiv = document.getElementById('draftReminder');
                if (draftDiv) {
                    draftDiv.innerHTML = html;
                } else {
                    console.warn('Element #draftReminder not found.');
                }
            }
        } else {
            console.error('Draft fetch failed:', response.status);
        }
    } catch (error) {
        console.error('Error loading draft reminder:', error);
    }
});