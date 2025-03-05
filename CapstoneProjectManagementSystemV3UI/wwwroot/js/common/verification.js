const btnVerify = document.getElementById("verify"),
    inputNumOTP = document.getElementById("inputNumOTP");

inputNumOTP.addEventListener('input', () => {
    if (inputNumOTP.value.length != 6) {
        btnVerify.disabled = true;
    } else {
        btnVerify.disabled = false;
    }
});
