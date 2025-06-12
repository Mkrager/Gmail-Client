const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registerlink = document.querySelector('.register-link');
const btnPopup = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerlink.addEventListener('click', () => wrapper.classList.add('active'));
loginlink.addEventListener('click', () => wrapper.classList.remove('active'));
btnPopup.addEventListener('click', () => wrapper.classList.add('active-popup'));
iconClose.addEventListener('click', () => wrapper.classList.remove('active-popup'));

window.onload = function () {
    if (window.loginErrorMessage && window.loginErrorMessage.length > 0) {
        document.querySelector('.wrapper')?.classList.add('active-popup');
        const errorElem = document.querySelector('.error-text');
        if (errorElem) {
            errorElem.textContent = window.loginErrorMessage;
            errorElem.style.display = 'block';
        }
    }
}
