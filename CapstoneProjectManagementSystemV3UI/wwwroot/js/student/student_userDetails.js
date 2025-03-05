const editProfileBtn = document.querySelector('#editProfileBtn');
const genderMale = document.querySelector('#male');
const genderFemale = document.querySelector('#female');
const genderOther = document.querySelector('#other');
const button = document.querySelector('.button')
const btnCancel = document.querySelector('.btnCancel');
const btnSave = document.querySelector('.btnSave');
const textarea = document.querySelector('.formBasicInfo__bio textarea');
const nameBasicInfo = document.querySelector('#nameBasicInfo');
const curriculumBasicInfo = document.querySelector('#curriculumBasicInfo');
const professionalBasicInfo = document.querySelector('#professionalBasicInfo');
const phoneContactInfo = document.querySelector('#phoneContactInfo');
const emailContactInfo = document.querySelector('#emailContactInfo');
const faceBookContactInfo = document.querySelector('#faceBookContactInfo');
const inputMailpopup = document.querySelector('.inputMailpopup');
const inputVerifypopup = document.querySelector('.inputVerifypopup');
const inputSetPasspopup = document.querySelector('.inputSetPasspopup');
const continueBtnMail = document.querySelector('#continueBtn__alternativeMail');
const continueBtnVerify = document.querySelector('#continueBtn__verifyCode');
const alternativeEmailInput = document.querySelector('#alternativeEmailInput');
const verifyCodeInput = document.querySelector('#verifyCodeInput');
const pwShowPWHide = document.querySelectorAll(".showHidePW");
const pwShowConfirmPWHide = document.querySelectorAll(".showHideConfirmPW");
const passAlternavtive = document.querySelector("#passalternativeMailInput");
const confirmpassAlternavtive = document.querySelector("#confirmpassalternativeMailInput");
const submitBtn = document.querySelector("#submitBtn");


$(document).ready(function () {
    $('#btnAddAlternativeForm').click(function () {
        $('.alternativeMail__popup').toggle('hide-form');
    });
    $('.cancelBtn').click(function () {
        $('.alternativeMail__popup').toggle('hide-form');
    });
    $('.alternativeMail__popup').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.alternativeMail__popup').toggle('hide-form');
        }
    });
});

editProfileBtn.addEventListener('click', () => {
    genderMale.disabled = false;
    genderFemale.disabled = false;
    genderOther.disabled = false;
    nameBasicInfo.disabled = false;
    curriculumBasicInfo.disabled = false;
    professionalBasicInfo.disabled = false;
    phoneContactInfo.disabled = false;
    emailContactInfo.disabled = false;
    faceBookContactInfo.disabled = false;
    button.style.display = 'flex';
    textarea.disabled = false;
    editProfileBtn.style.display = 'none';
})

btnCancel.addEventListener('click', () => {
    genderMale.disabled = true;
    genderFemale.disabled = true;
    genderOther.disabled = true;
    nameBasicInfo.disabled = true;
    curriculumBasicInfo.disabled = true;
    professionalBasicInfo.disabled = true;
    phoneContactInfo.disabled = true;
    emailContactInfo.disabled = true;
    faceBookContactInfo.disabled = true;
    button.style.display = 'none';
    textarea.disabled = true;
    editProfileBtn.style.display = 'block';
})

alternativeEmailInput.addEventListener('input', () => {
    if (alternativeEmailInput.value.length != 0) {
        continueBtnMail.disabled = false;
    } else {
        continueBtnMail.disabled = true;
    }
})

verifyCodeInput.addEventListener('input', () => {
    if (verifyCodeInput.value.length != 0) {
        continueBtnVerify.disabled = false;
    } else {
        continueBtnVerify.disabled = true;
    }
})

continueBtnMail.addEventListener('click', () => {
    inputMailpopup.classList.add('hide__popup');
    inputVerifypopup.classList.remove('hide__popup');
})

continueBtnVerify.addEventListener('click', () => {
    inputMailpopup.classList.add('hide__popup');
    inputVerifypopup.classList.add('hide__popup');
    inputSetPasspopup.classList.remove('hide__popup');
})

pwShowPWHide.forEach(eyeIcon => {
    eyeIcon.addEventListener("click", () => {
        if (passAlternavtive.type === "password") {
            passAlternavtive.type = "text";
            pwShowPWHide.forEach(icon => {
                icon.classList.replace("fa-eye-slash", "fa-eye");
            })
        } else {
            passAlternavtive.type = "password"
            pwShowPWHide.forEach(icon => {
                icon.classList.replace("fa-eye", "fa-eye-slash");
            })
        }
    })
})

pwShowConfirmPWHide.forEach(eyeIcon => {
    eyeIcon.addEventListener("click", () => {
        if (confirmpassAlternavtive.type === "password") {
            confirmpassAlternavtive.type = "text";
            pwShowConfirmPWHide.forEach(icon => {
                icon.classList.replace("fa-eye-slash", "fa-eye");
            })
        } else {
            confirmpassAlternavtive.type = "password"
            pwShowConfirmPWHide.forEach(icon => {
                icon.classList.replace("fa-eye", "fa-eye-slash");
            })
        }
    })
})

