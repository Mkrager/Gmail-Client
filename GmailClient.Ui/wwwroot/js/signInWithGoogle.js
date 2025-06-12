async function signInWithGoogle() {
    const res = await fetch("/GoogleAuth/Login", {
        method: "POST",
    });

    const { googleUrl } = await res.json();
    window.open(googleUrl, "GoogleLogin", "width=500,height=600");
}