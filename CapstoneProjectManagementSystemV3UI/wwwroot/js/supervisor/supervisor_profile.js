
const editProfileBtn = document.querySelector('#editProfileBtn');
const genderMale = document.querySelector('#male');
const genderFemale = document.querySelector('#female');
const genderOther = document.querySelector('#other');
const button = document.querySelector('.button')
const btnCancel = document.querySelector('.btnCancel');
const textarea = document.querySelector('.formBasicInfo__bio textarea');
const nameBasicInfo = document.querySelector('#nameBasicInfo');
const phoneContactInfo = document.querySelector('#phoneContactInfo');
const personalEmailInfo = document.querySelector('#personalEmailContactInfo');
const emailContactInfo = document.querySelector('#emailContactInfo');
const maxgroup = document.querySelector('#maxGroupBasicInfo');
const displayinline = document.querySelectorAll('.displayinline');
const checkbox_phoneNumber = document.querySelector('#checkbox-phoneNumber');
const checkbox_personalEmail = document.querySelector('#checkbox-personalEmail');
const checkbox_selfDescription = document.querySelector('#checkbox-selfDescription');
const profession_check = document.getElementsByClassName('Profession_Check');
$(document).ready(function () {
    AjaxCall('/ProfileSelfSupervisor/CheckSupervisorProfile', 'POST').done(function (response) {
        if (response == true) {
            Swal.fire({
                icon: 'warning',
                title: '<p class="popupTitle">Please complete your information <br/>( Field with <span style="color: red;">*</span> is required )</p>'
            })
            genderMale.disabled = false;
            genderFemale.disabled = false;
            genderOther.disabled = false;
            nameBasicInfo.disabled = false;
            phoneContactInfo.disabled = false;
            personalEmailInfo.disabled = false;
            checkbox_phoneNumber.disabled = false;
            checkbox_personalEmail.disabled = false;
            checkbox_selfDescription.disabled = false;
            //emailContactInfo.disabled = false;
            //maxGroupBasicInfo.disabled = false;
            //for (var i = 0; i < profession_check.length; i++) {
            //    profession_check[0].disabled = false
            //}

            button.style.display = 'flex';
            textarea.disabled = false;
            editProfileBtn.style.display = 'none';
        } else {
            genderMale.disabled = true;
            genderFemale.disabled = true;
            genderOther.disabled = true;
            nameBasicInfo.disabled = true;
            phoneContactInfo.disabled = true;
            personalEmailInfo.disabled = true;
            checkbox_phoneNumber.disabled = true;
            checkbox_personalEmail.disabled = true;
            checkbox_selfDescription.disabled = true;
            //for (var i = 0; i < profession_check.length; i++) {
            //    profession_check[i].disabled = true
            //}
            //emailContactInfo.disabled = true;
            //maxGroupBasicInfo.disabled = true;
            button.style.display = 'none';
            textarea.disabled = true;
            editProfileBtn.style.display = 'block';
        }
    })
})

editProfileBtn.addEventListener('click', () => {
    genderMale.disabled = false;
    genderFemale.disabled = false;
    genderOther.disabled = false;
    nameBasicInfo.disabled = false;
    phoneContactInfo.disabled = false;
    personalEmailInfo.disabled = false;
    //emailContactInfo.disabled = false;
    //for (var i = 0; i < profession_check.length; i++) {
    //    profession_check[i].disabled = false
    //}
    //maxGroupBasicInfo.disabled = false;
    button.style.display = 'flex';
    textarea.disabled = false;
    editProfileBtn.style.display = 'none';
    checkbox_phoneNumber.disabled = false;

    if (emailContactInfo.value.length == 0) {
        checkbox_personalEmail.disabled = true;
    } else {
        checkbox_personalEmail.disabled = false;
    }
    checkbox_selfDescription.disabled = false;
})

btnCancel.addEventListener('click', () => {
    window.location.href = '/ProfileSelfSupervisor/ViewSelfProfileSupervisor'
})


$('.btnSave').on('click', function () {
    // object user
    var user = {};
    user.Gender = $('input[name="genderUser"]:checked').val();
    user.FullName = $('#nameBasicInfo').val().trim();

    //object student
    var supervisor = {};

    supervisor.PhoneNumber = $('#phoneContactInfo').val().trim();
    /*supervisor.MaxGroup = $('#maxGroupBasicInfo').val();*/
    supervisor.SelfDescription = $('.formBasicInfo__bio textarea').val();
    supervisor.PersonalEmail = $('#personalEmailContactInfo').val().trim();
    supervisor.FeEduEmail = $('#emailContactInfo').val().trim(); //tan them
    supervisor.user = user;

    var settingObj = {};
    if ($('input[name="checkbox-phoneNumber"]').is(':checked'))
        settingObj.PhoneNumber = true;
    else
        settingObj.PhoneNumber = false;

    if ($('input[name="checkbox-personalEmail"]').is(':checked'))
        settingObj.PersonalEmail = true;
    else
        settingObj.PersonalEmail = false;

    if ($('input[name="checkbox-selfDescription"]').is(':checked'))
        settingObj.SelfDescription = true;
    else
        settingObj.SelfDescription = false;

    var settingStringJson = JSON.stringify(settingObj);
    supervisor.FieldSetting = settingStringJson;

    var profesions = [];
    var lstPro = $('.Profession_Check');
    if (lstPro !== undefined) {
        for (var i = 0; i < lstPro.length; i++) {
            if (lstPro[i].checked) {
                profesions.push({ ProfessionID: parseInt(lstPro[i].value) });
            }
        }
        supervisor.Professions = profesions;
    }

    /*console.log(supervisor.user.Gender);*/
    AjaxCall('/ProfileSelfSupervisor/EditProfileSupervisor', JSON.stringify(supervisor), 'POST').done(function (respone) {
        if (respone == 1)
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>Profile was saved sucessfully</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                window.location = "/ProfileSelfSupervisor/ViewSelfProfileSupervisor";
            });
        else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Profile was saved fail</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                window.location = "/ProfileSelfSupervisor/ViewSelfProfileSupervisor";
            });
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
});

//validate Edit Profile
$(document).ready(function () {
    //$('.btnSave').attr('disabled', 'disabled');
    //$('.btnSave').css('cursor', 'not-allowed');
    //$('.btnSave').css('opacity', '0.5');
    $('#nameBasicInfo').blur(function () {
        if ($('#nameBasicInfo').val().trim().length <= 0) {
            $('#errornameBasicInfo').html('Please input your name!');
        } else if ($('#nameBasicInfo').val().trim().length > 50) {
            $('#errornameBasicInfo').html('Input less than 50 characters');
        } else {
            $('#errornameBasicInfo').html('');
        }
    })

    var validPhone = /^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$/;
    $('#phoneContactInfo').blur(function () {
        if (!validPhone.test($('#phoneContactInfo').val().trim())) {
            $('#errorphoneContactInfo').html('Input the correct Vietnamese phone number');
        } else if ($('#phoneContactInfo').val().trim().length > 10) {
            $('#errorphoneContactInfo').html('Input less than 10 numbers');
        } else {
            $('#errorphoneContactInfo').html('');
        }
    });

    var validEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    $('#personalEmailContactInfo').blur(function () {
        if (!validEmail.test($('#personalEmailContactInfo').val().trim())) {
            $('#erroremailPersonContactInfo').html('Email must have a format example@gmail.com');
        } else {
            $('#erroremailPersonContactInfo').html('');
        }
    });


    $('#bioInfo').blur(function () {
        if ($('#bioInfo').val().trim().length > 3000) {
            $('#errorbioInfo').html('Input less than 3000 characters');
        } else {
            $('#errorbioInfo').html('');
        }
    })

    $('body').on('blur keyup change', ' #phoneContactInfo, #nameBasicInfo', function () {
        if (!validPhone.test($('#phoneContactInfo').val().trim()) || $('#phoneContactInfo').val().trim().length > 10
            || $('#nameBasicInfo').val().trim().length > 50 || $('#nameBasicInfo').val().trim().length <= 0) {
            $('.btnSave').attr('disabled', 'disabled');
            $('.btnSave').css('cursor', 'not-allowed');
            $('.btnSave').css('opacity', '0.5');
        } else {
            $('.btnSave').removeAttr('disabled', 'disabled');
            $('.btnSave').css('cursor', 'pointer');
            $('.btnSave').css('opacity', '1');
        }
    });
})

//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}
















