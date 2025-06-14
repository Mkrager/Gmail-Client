const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registerlink = document.querySelector('.register-link');
const btnPopup = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerlink.addEventListener('click', () => wrapper.classList.add('active'));
loginlink.addEventListener('click', () => wrapper.classList.remove('active'));
btnPopup.addEventListener('click', () => wrapper.classList.add('active-popup'));
iconClose.addEventListener('click', () => wrapper.classList.remove('active-popup'));

document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const data = {
        loginRequest: {
            email: document.getElementById("LoginEmail").value,
            password: document.getElementById("LoginPassword").value
        }
    };

    console.log(data);

    const response = await fetch("/Authentication/Login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    const result = await response.json();

    if (result.success) {
        window.location.href = result.redirectUrl;
    } else {
        document.getElementById("LoginError").textContent = result.error;
        openLoginModal();
    }
});

document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const data = {
        registrationRequest: {
            email: document.getElementById("RegisterEmail").value,
            userName: document.getElementById("RegisterUserName").value,
            firstName: document.getElementById("RegisterFirstName").value,
            lastName: document.getElementById("RegisterLastName").value,
            password: document.getElementById("RegisterPassword").value
        }
    };

    console.log(data);

    const response = await fetch("/Authentication/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    const result = await response.json();

    if (result.success) {
        window.location.href = result.redirectUrl;
    } else {
        document.getElementById("RegisterError").textContent = result.error;
        openLoginModal();
    }
})