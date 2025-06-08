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

