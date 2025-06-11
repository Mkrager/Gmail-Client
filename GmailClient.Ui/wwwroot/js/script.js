const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registerlink = document.querySelector('.register-link');
const btnPopup = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerlink.addEventListener('click', () => wrapper.classList.add('active'));
loginlink.addEventListener('click', () => wrapper.classList.remove('active'));
btnPopup.addEventListener('click', () => wrapper.classList.add('active-popup'));
iconClose.addEventListener('click', () => wrapper.classList.remove('active-popup'));

async function signInWithGoogle() {
    const res = await fetch("/GoogleAuth/Login", {
        method: "POST",
    });

    const { googleUrl } = await res.json();
    window.open(googleUrl, "GoogleLogin", "width=500,height=600");
}


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

function sendEmail(event) {
    event.preventDefault();

    const to = document.getElementById('emailTo').value;
    const subject = document.getElementById('emailSubject').value;
    const body = sendEmailEditor.getData();

    fetch('/dashboard/send', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ to, subject, body })
    })
        .then(response => {
            if (response.ok) {
                alert('Success');
                closeSendEmailModal();
                document.getElementById('sendEmailForm').reset();
                sendEmailEditor.setData('');
            } else {
                alert('Error');
            }
        })
        .catch(error => {
            alert('Error');
        });

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

window.addEventListener('DOMContentLoaded', (event) => {
    const errorBox = document.getElementById('errorBox');
    if (errorBox) {
        setTimeout(() => {
            errorBox.style.transition = "opacity 0.5s ease";
            errorBox.style.opacity = "0";
            setTimeout(() => errorBox.remove(), 500);
        }, 3000);
    }
});