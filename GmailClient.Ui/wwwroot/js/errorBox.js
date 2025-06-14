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