const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registerlink = document.querySelector('.register-link');
const btnPopup = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerlink.addEventListener('click', () => {
    wrapper.classList.add('active');
});

loginlink.addEventListener('click', () => {
    wrapper.classList.remove('active');
});

btnPopup.addEventListener('click', () => {
    wrapper.classList.add('active-popup');
});

iconClose.addEventListener('click', () => {
    wrapper.classList.remove('active-popup');
});

async function signInWithGoogle() {
    //const token = localStorage.getItem("access_token");

    const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNa3JhZ2VyIiwianRpIjoiMjlmYWMyNmQtYTc5Mi00N2E0LThlNjMtYTQ0NzM5ZjNkYzFhIiwiZW1haWwiOiJzbWFnYS5tYXhAZ21haWwuY29tIiwidWlkIjoiMTU1MDFlYTYtNjI5OS00YzZiLThiODAtMDNhOWYyMmRiNmU0IiwiRW5hYmxlZFR3b0ZhY3RvckF1dGgiOiJGYWxzZSIsImV4cCI6MTc1MTkyMjIwMywiaXNzIjoiR21haWxDbGllbnRJZGVudGl0eSIsImF1ZCI6IkdtYWlsQ2xpZW50SWRlbnRpdHlVc2VyIn0.SoZNqSQxtquDNMshYFhJ7v6OffmC0uTG0SONkp2WofE';

    const res = await fetch("https://localhost:7075/api/googleoauth/generate-google-state", {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    const { googleUrl } = await res.json();
    window.open(googleUrl, "GoogleLogin", "width=500,height=600");
}

function escapeHtml(str) {
    const div = document.createElement('div');
    div.appendChild(document.createTextNode(str));
    return div.innerHTML;
}

async function getNext10Messages(nextPageToken) {
    if (nextPageToken) {
        document.getElementById('btnLoadMore').style.display = 'inline-block';
    }    

    if (!nextPageToken) return;

    const response = await fetch(`/Dashboard/GetMessagesPage?pageToken=${nextPageToken}`);
    if (!response.ok) {
        alert('Error loading more messages.');
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
            <td class="${letters.isSent ? 'status-sent' : 'status-inbox'}">${letters.IsSent ? 'Sent' : 'Inbox'}</td>
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

function openSendEmailModal() {
    window.location.href = '/Dashboard/SendLetter';
}


let emailEditor;
let sendEmailEditor;

function initEditors() {
    ClassicEditor
        .create(document.querySelector('#emailContent'))
        .then(editor => {
            emailEditor = editor;
            emailEditor.isReadOnly = true;
        })
        .catch(error => {
            console.error('Failed to initialize email viewer editor:', error);
        });

    ClassicEditor
        .create(document.querySelector('#sendEmailContent'))
        .then(editor => {
            sendEmailEditor = editor;
        })
        .catch(error => {
            console.error('Failed to initialize email sender editor:', error);
        });
}

function openModal(body) {
    document.getElementById('emailModal').style.display = 'flex';

    if (emailEditor) {
        const decodedBody = JSON.parse(body);
        emailEditor.setData(decodedBody);
    } else {
        document.getElementById('emailContent').value = body;
    }
}

function closeModal() {
    document.getElementById('emailModal').style.display = 'none';
}

function openSendEmailModal() {
    document.getElementById('sendEmailModal').style.display = 'flex';
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
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ to, subject, body })
    })
        .then(response => {
            if (response.ok) {
                alert('Email sent successfully!');
                closeSendEmailModal();
                document.getElementById('sendEmailForm').reset();
                sendEmailEditor.setData('');
            } else {
                alert('Failed to send email.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error sending email.');
        });

    return false;
}

document.addEventListener('DOMContentLoaded', function () {
    initEditors();

    const sendForm = document.getElementById('sendEmailForm');
    if (sendForm) {
        sendForm.addEventListener('submit', sendEmail);
    }
})