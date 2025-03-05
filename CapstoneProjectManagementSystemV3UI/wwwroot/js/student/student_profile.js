
const editProfileBtn = document.querySelector('#editProfileBtn');
const genderMale = document.querySelector('#male');
const genderFemale = document.querySelector('#female');
const genderOther = document.querySelector('#other');
const button = document.querySelector('.button')
const btnCancel = document.querySelector('.btnCancel');
const textarea = document.querySelector('.formBasicInfo__bio textarea');
const nameBasicInfo = document.querySelector('#nameBasicInfo');
const wantobegrouped_yes = document.querySelector('#wantobegrouped_yes');
const wantobegrouped_no = document.querySelector('#wantobegrouped_no');
const phoneContactInfo = document.querySelector('#phoneContactInfo');
//const emailContactInfo = document.querySelector('#emailContactInfo');
const faceBookContactInfo = document.querySelector('#faceBookContactInfo');
const expectRoleBasicInfo = document.querySelector('#expectRoleBasicInfo');
//const professionalfilter = document.querySelector("#professionalfilter");
const specialtyfilter = document.querySelector("#specialtyfilter");
const genderButtonSection = document.querySelector('.genderButtonSection');
const genderTextSection = document.querySelector('.genderTextSection');


$(document).ready(function () {
    AjaxCall('/StudentProfile/CheckProfile', 'POST').done(function (response) {
        if (response == true) {
            Swal.fire({
                icon: 'warning',
                title: `<p class="popupTitle">Please complete your information <br/>( <span style="color: red;">Phone Number</span>, <span style="color: red;">Alternative Email</span> and <span style="color: red;">Specialty</span> are required )</p>`
            })
            genderMale.disabled = false;
            genderFemale.disabled = false;
            genderOther.disabled = false;
            nameBasicInfo.disabled = false;
            wantobegrouped_yes.disabled = false;
            wantobegrouped_no.disabled = false;
            phoneContactInfo.disabled = false;
            //emailContactInfo.disabled = false;
            faceBookContactInfo.disabled = false;
            //professionalfilter.disabled = false;
            if (specialtyfilter)
                specialtyfilter.disabled = false;
            expectRoleBasicInfo.disabled = false;
            button.style.display = 'flex';
            textarea.disabled = false;
            editProfileBtn.style.display = 'none';
            genderButtonSection.style.display = 'block';
            genderTextSection.style.display = 'none';
        } else {
            genderMale.disabled = true;
            genderFemale.disabled = true;
            genderOther.disabled = true;
            nameBasicInfo.disabled = true;
            wantobegrouped_yes.disabled = true;
            wantobegrouped_no.disabled = true;
            phoneContactInfo.disabled = true;
            //emailContactInfo.disabled = true;
            faceBookContactInfo.disabled = true;
            //professionalfilter.disabled = true;
            if (specialtyfilter)
                specialtyfilter.disabled = true;
            expectRoleBasicInfo.disabled = true
            button.style.display = 'none';
            textarea.disabled = true;
            editProfileBtn.style.display = 'block';
            genderButtonSection.style.display = 'none';
            genderTextSection.style.display = 'block';
        }
    })
})

editProfileBtn.addEventListener('click', () => {
    genderMale.disabled = false;
    genderFemale.disabled = false;
    genderOther.disabled = false;
    nameBasicInfo.disabled = false;
    wantobegrouped_yes.disabled = false;
    wantobegrouped_no.disabled = false;
    phoneContactInfo.disabled = false;
    //emailContactInfo.disabled = false;
    if (specialtyfilter)
        specialtyfilter.disabled = false;
    faceBookContactInfo.disabled = false;
    expectRoleBasicInfo.disabled = false;
    button.style.display = 'flex';
    textarea.disabled = false;
    editProfileBtn.style.display = 'none';
})

btnCancel.addEventListener('click', () => {
    window.location.href = '/StudentProfile/Index'

})

$('body').on('click', '.showHidePW', function () {
    if ($('#passalternativeMailInput').attr('type') == 'password') {
        $('#passalternativeMailInput').prop('type', 'text');

        $('.showHidePW').removeClass('fa-eye-slash',);
        $('.showHidePW').addClass('fa-eye');
    } else {
        $('#passalternativeMailInput').prop('type', 'password');
        $('.showHidePW').removeClass('fa-eye',);
        $('.showHidePW').addClass('fa-eye-slash');
    }
})
$('body').on('click', '.showHideConfirmPW', function () {
    if ($('#confirmpassalternativeMailInput').attr('type') == 'password') {
        $('#confirmpassalternativeMailInput').prop('type', 'text');
        $('.showHideConfirmPW').removeClass('fa-eye-slash',);
        $('.showHideConfirmPW').addClass('fa-eye');
    } else {
        $('#confirmpassalternativeMailInput').prop('type', 'password');
        $('.showHideConfirmPW').removeClass('fa-eye',);
        $('.showHideConfirmPW').addClass('fa-eye-slash');
    }
})
$('body').on('click', '.showHideOldPW', function () {
    if ($('#oldPasswordAlternativeMail').attr('type') == 'password') {
        $('#oldPasswordAlternativeMail').prop('type', 'text');
        $('.showHideOldPW').removeClass('fa-eye-slash',);
        $('.showHideOldPW').addClass('fa-eye');
    } else {
        $('#oldPasswordAlternativeMail').prop('type', 'password');
        $('.showHideOldPW').removeClass('fa-eye',);
        $('.showHideOldPW').addClass('fa-eye-slash');
    }
})
$('body').on('click', '.showHideNewPW', function () {
    if ($('#newpassalternativeMailInput').attr('type') == 'password') {
        $('#newpassalternativeMailInput').prop('type', 'text');
        $('.showHideNewPW').removeClass('fa-eye-slash',);
        $('.showHideNewPW').addClass('fa-eye');
    } else {
        $('#newpassalternativeMailInput').prop('type', 'password');
        $('.showHideNewPW').removeClass('fa-eye',);
        $('.showHideNewPW').addClass('fa-eye-slash');
    }
})
$('body').on('click', '.showHideConfirmNewPW', function () {
    if ($('#confirmnewpassalternativeMailInput').attr('type') == 'password') {
        $('#confirmnewpassalternativeMailInput').prop('type', 'text');
        $('.showHideConfirmNewPW').removeClass('fa-eye-slash',);
        $('.showHideConfirmNewPW').addClass('fa-eye');
    } else {
        $('#confirmnewpassalternativeMailInput').prop('type', 'password');
        $('.showHideConfirmNewPW').removeClass('fa-eye',);
        $('.showHideConfirmNewPW').addClass('fa-eye-slash');
    }
})

//var myFunction = function () {
//    var professionId = $('#professionalfilter').val();
//    $('#specialtyfilter').html('');
//    if (professionId == 0) {
//        $('#specialtyfilter').append('<option value="0">Specialty</option>')
//    } else {
//        AjaxCall('/StudentProfile/GetSpecialtyByProfessionId?professionId=' + professionId, 'POST')
//            .done(function (response) {
//                AjaxCall('/StudentProfile/GetSpecialtyIdOfStudent', 'POST').done(function (specialtyId) {
//                    var specialties = '';
//                    for (var i = 0; i < response.length; i++) {
//                        if (specialtyId == response[i].specialtyID)
//                            specialties += ' <option value="' + response[i].specialtyID + '" selected>' + response[i].specialtyFullName + '</option>';
//                        else
//                            specialties += ' <option value="' + response[i].specialtyID + '">' + response[i].specialtyFullName + '</option>';
//                    }
//                    $('#specialtyfilter').append(specialties);
//                }).fail(function (error) {
//                    alert(error.StatusText);
//                });
//            }).fail(function (error) {
//                alert(error.StatusText);
//            });
//    }
//}
// Lấy tham chiếu tới các phần tử HTML
//const selectOption1 = document.getElementById('specialtyfilter');
//const selectedValue1 = document.getElementById('selectedValue1');

//selectedValue1.textContent = selectOption1.options[selectOption1.response[i].specialtyID].textContent;



//$('#professionalfilter')
//    .ready(myFunction)
//    .change(myFunction);

$('.btnSave').on('click', function () {
    // object user
    var user = {};
    user.gender = $('input[name="genderUser"]:checked').val();
    user.fullName = $('#nameBasicInfo').val();

    //var profession = {};
    //profession.professionId = $('#professionalfilter').val();

    var specialty = {};
    specialty.specialtyId = specialtyfilter ? $('#specialtyfilter').val() : $("#specIdOfStudentInput").val();

    //object student
    var student = {};
    student.WantToBeGrouped = $('input[name="wantobegrouped"]:checked').val() == "1" ? true : false;
    student.PhoneNumber = $('#phoneContactInfo').val();
    //student.EmailAddress = $('#emailContactInfo').val();
    student.LinkFacebook = $('#faceBookContactInfo').val();
    student.selfDescription = $('#bioInfo').val();
    student.expectedRoleInGroup = $('#expectRoleBasicInfo').val();
    student.user = user;
    //student.profession = profession;
    student.specialty = specialty;

    console.log(student.user.gender);
    AjaxCall('/StudentProfile/EditMyProfile', JSON.stringify(student), 'POST').done(function (respone) {
        if (respone == 1)
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>Profile was saved sucessfully</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                window.location = "/StudentProfile/Index";
            });
        else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Profile was saved fail</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                window.location = "/StudentProfile/Index";
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
    var validPhone = /^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$/;
    $('#phoneContactInfo').blur(function () {
        if (!validPhone.test($('#phoneContactInfo').val())) {
            $('#errorphoneContactInfo').html('Input the correct Vietnamese phone number');
        } else if ($('#phoneContactInfo').val().length > 10) {
            $('#errorphoneContactInfo').html('Input less than 10 numbers');
        } else {
            $('#errorphoneContactInfo').html('');
        }
    })

    //$('#emailContactInfo').blur(function () {
    //    if ($('#emailContactInfo').val().length > 200) {
    //        $('#erroremailContactInfo').html('Input less than 200 characters');
    //    } else {
    //        $('#erroremailContactInfo').html('');
    //    }
    //})

    $('#faceBookContactInfo').blur(function () {
        if ($('#faceBookContactInfo').val().length > 200) {
            $('#errorfaceBookContactInfo').html('Input less than 200 characters');
        } else {
            $('#errorfaceBookContactInfo').html('');
        }
    })

    $('#nameBasicInfo').blur(function () {
        if ($('#nameBasicInfo').val().length > 50) {
            $('#errornameBasicInfo').html('Input less than 50 characters');
        } else {
            $('#errornameBasicInfo').html('');
        }
    })

    //$('#curriculumBasicInfo').blur(function () {
    //    if ($('#curriculumBasicInfo').val().replace(/\s/g, "").length <= 0) {
    //        $('#errorcurriculumBasicInfo').html('This is required');
    //    } else if ($('#curriculumBasicInfo').val().length > 50) {
    //        $('#errorcurriculumBasicInfo').html('Input less than 50 characters');
    //    } else {
    //        $('#errorcurriculumBasicInfo').html('');
    //    }
    //})

    //$('#professionalfilter').blur(function () {
    //    if ($('#professionalfilter').val() == 0) {
    //        $('#errorProfessional').html('This field is required');
    //    } else {
    //        $('#errorProfessional').html('');
    //    }
    //});

    $('#specialtyfilter').blur(function () {
        if ($('#specialtyfilter').val() == 0) {
            $('#errorSpecialty').html('This field is required');
        } else {
            $('#errorSpecialty').html('');
        }
    });
    $('#expectRoleBasicInfo').blur(function () {
        if ($('#expectRoleBasicInfo').val().length > 50) {
            $('#errorexpectRoleBasicInfo').html('Input less than 50 characters');
        } else {
            $('#errorexpectRoleBasicInfo').html('');
        }
    })

    $('#bioInfo').blur(function () {
        if ($('#bioInfo').val().length > 3000) {
            $('#errorbioInfo').html('Input less than 3000 characters');
        } else {
            $('#errorbioInfo').html('');
        }
    })

    $('body').on('blur keyup change', '#phoneContactInfo, #faceBookContactInfo, #nameBasicInfo, #specialtyfilter, #expectRoleBasicInfo, #bioInfo', function () {
        if (!validPhone.test($('#phoneContactInfo').val()) || $('#phoneContactInfo').val().length > 10
            || $('#faceBookContactInfo').val().length > 200
            || $('#nameBasicInfo').val().length > 50 || $('#expectRoleBasicInfo').val().length > 50
            || $('#bioInfo').val().length > 3000
            || $('#specialtyfilter').val() == 0) {
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
/////////personal email////////
//form add email
$('body').on('click', '#btnAddAlternativeForm', function () {
    $('.alternativeMail__popup').toggle('hide-form');
});
$('body').on('click', '#cancelBtn__alternativeMail', function () {
    $('.alternativeMail__popup').toggle('hide-form');
});
$('.alternativeMail__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.alternativeMail__popup').toggle('hide-form');
    }
});

// form verify email
var personalEmail
$('body').on('click', '#continueBtn__alternativeMail', function () {
    $('#continueBtn__alternativeMail').attr('disabled', true);
    $('#continueBtn__alternativeMail').css('cursor', 'not-allowed');
    $('#continueBtn__alternativeMail').css('opacity', '0.5');
    personalEmail = $('#alternativeEmailInput').val();
    $('#emailPersonal').html(personalEmail);
    AjaxCall('/StudentProfile/AddOtp', JSON.stringify(personalEmail), 'POST').done(function (response) {
        if (response == 1) {
            $('.alternativeMail__popup').toggle('hide-form');
            $('.verifyAlternativeMail__popup').toggle('hide-form');
            $('#continueBtn__alternativeMail').removeAttr('disabled');
            $('#continueBtn__alternativeMail').css('cursor', 'pointer');
            $('#continueBtn__alternativeMail').css('opacity', '1');
        }
        if (response == 0) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                $('#continueBtn__alternativeMail').removeAttr('disabled');
                $('#continueBtn__alternativeMail').css('cursor', 'pointer');
                $('#continueBtn__alternativeMail').css('opacity', '1');
            });
        }
        if (response == 2) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>This email already exists in the system</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                $('#continueBtn__alternativeMail').removeAttr('disabled');
                $('#continueBtn__alternativeMail').css('cursor', 'pointer');
                $('#continueBtn__alternativeMail').css('opacity', '1');
            })
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
            timer: 1000,
            showConfirmButton: false
        });
    });
});
$('body').on('click', '#cancelBtn__verifyCode', function () {
    $('.verifyAlternativeMail__popup').toggle('hide-form');
});
$('.verifyAlternativeMail__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.verifyAlternativeMail__popup').toggle('hide-form');
    }
});

// form set password
$('body').on('click', '#continueBtn__verifyCode', function () {
    var otp = $('#verifyCodeInput').val();
    AjaxCall('/StudentProfile/VerifyEmailByOTP?otp=' + otp + '&&personalEmail=' + personalEmail, 'POST').done(function (response) {
        if (response == 1) {
            $('.verifyAlternativeMail__popup').toggle('hide-form');
            $('.setPasswordForAlternativeMail__popup').toggle('hide-form');
        } else if (response == 0) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Incorrect OTP</p>",
                timer: 1000,
                showConfirmButton: false
            });
        } else if (response == 2) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Expired otp</p>",
                timer: 1000,
                showConfirmButton: false
            });

        } else if (response == 3) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            });
        } else if (response == 4) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>OTP must have 6 digits</p>",
                timer: 1000,
                showConfirmButton: false
            });
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
});
$('body').on('click', '#cancelBtn__setPassword', function () {
    location.reload();
});
$('.setPasswordForAlternativeMail__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.setPasswordForAlternativeMail__popup').toggle('hide-form');
    }
});

//submit form
$('body').on('click', '#submitBtn__setPassword', function () {
    var password = $('#passalternativeMailInput').val();
    var confirmPassword = $('#confirmpassalternativeMailInput').val();
    AjaxCall('/StudentProfile/SetPasswordFofAccount?password=' + password + '&&confirmPassword=' + confirmPassword, 'POST').done(function (respone) {
        if (respone == 1)
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>Password has been set successfully</p>",
                timer: 1000,
                showConfirmButton: false
            }).then(function () {
                location.reload(true);
            });
        else if (respone == 2) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>New password or confirm new password not empty</p>",
                timer: 1000,
                showConfirmButton: false
            })
        } else if (respone == 3) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>New password and confirm new password is not match</p>",
                timer: 1000,
                showConfirmButton: false
            })
        } else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            })
        }
    }).fail(function (error) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>>Something is wrong! Try again later</p>",
            timer: 1000,
            showConfirmButton: false
        })
    });
});


//form change email
$('body').on('click', '#btnChangeAlternativeForm', function () {
    $('#emailPersonal').html($('#personalEmail').html());
    $('#btnChangeAlternativeForm').attr('disabled', true);
    $('#btnChangeAlternativeForm').css('cursor', 'not-allowed');
    $('#btnChangeAlternativeForm').css('opacity', '0.5');
    AjaxCall('/StudentProfile/SendOtpVerify', 'POST').done(function (response) {
        if (response == true) {
            $('.verifyChangeAlternativeMail__popup').toggle('hide-form');
            $('#btnChangeAlternativeForm').removeAttr('disabled');
            $('#btnChangeAlternativeForm').css('cursor', 'pointer');
            $('#btnChangeAlternativeForm').css('opacity', '1');
        }
        else {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>"
            })
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
})
$('body').on('click', '#cancelBtn__changeEmail', function () {
    $('.verifyChangeAlternativeMail__popup').toggle('hide-form');
})
$('.verifyChangeAlternativeMail__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.verifyChangeAlternativeMail__popup').toggle('hide-form');
    }
});


$('body').on('click', '#continueBtn__changeEmail', function () {
    var otp = $('#verifyOldEmail_CodeInput').val();
    AjaxCall('/StudentProfile/VerifyEmailByOTP?otp=' + otp, 'POST').done(function (response) {
        if (response == 1) {
            $('.alternativeMail__popup').toggle('hide-form');
            $('.verifyChangeAlternativeMail__popup').toggle('hide-form');
        } else if (response == 0) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Incorrect OTP</p>",
                timer: 1000,
                showConfirmButton: false
            });
        } else if (response == 2) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Expired otp</p>",
                timer: 1000,
                showConfirmButton: false
            });

        } else if (response == 3) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            });
        } else if (response == 4) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>OTP must have 6 digits</p>",
                timer: 1000,
                showConfirmButton: false
            });
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
})

//form input old password
$('body').on('click', '#btnchangePasswordAlternativeForm', function () {
    $('.changePassword__popup').toggle('hide-form');
});
$('body').on('click', '#continueBtn_InputOldPassword', function () {
    var oldPassword = $('#oldPasswordAlternativeMail').val();
    var personalEmail = $('#personalEmail').html();
    AjaxCall('/StudentProfile/CheckInputOldPassword?personalEmail=' + personalEmail + '&&oldPassword=' + oldPassword, 'POST')
        .done(function (response) {
            if (response == true) {
                $('.changePassword__popup').toggle('hide-form');
                $('.setPassword__popup').toggle('hide-form');
            } else if (response == false) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Old Password incorrect</p>",
                    timer: 1000,
                    showConfirmButton: false
                });
            } else if (response == 0) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                    timer: 1000,
                    showConfirmButton: false
                });
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
})
$('body').on('click', '#cancelBtn_InputOldPassword', function () {
    $('.changePassword__popup').toggle('hide-form');
})
$('.changePassword__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.changePassword__popup').toggle('hide-form');
    }
});


//fomr set password
$('body').on('click', '#btnSetPasswordAlternativeForm', function () {
    $('.setPasswordForAlternativeMail__popup').toggle('hide-form');
})


// form change new password
$('body').on('click', '#cancelInputNewPassBtn', function () {
    $('.setPassword__popup').toggle('hide-form');
})
$('.setPassword__popup').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.setPassword__popup').toggle('hide-form');
    }
});
$('body').on('click', '#submitNewPassBtn', function () {
    var newPassword = $('#newpassalternativeMailInput').val();
    var confirmNewPassword = $('#confirmnewpassalternativeMailInput').val();
    AjaxCall('/StudentProfile/ChangePassword?newPassword=' + newPassword + '&&confirmNewPassword=' + confirmNewPassword, 'Post')
        .done(function (response) {
            if (response == true) {
                $('.changePassword__popup').html('');
                Swal.fire({
                    icon: 'success',
                    title: "<p class='popupTitle'>Change password was successfully.</p>",
                    timer: 1000,
                    showConfirmButton: false
                }).then(function () {
                    window.location.href = '/StudentProfile/Index';
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Passwords don’t match.</p>",
                    timer: 1000,
                    showConfirmButton: false
                });
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
})

// resend
$('#btnResend1').click(function () {
    AjaxCall('/StudentProfile/SendOtpVerify', JSON.stringify(personalEmail), 'POST').done(function (response) {
        if (response == false) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            })
        } else {
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>New code has been sent to your email</p>",
                timer: 1000,
                showConfirmButton: false
            })
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
})
$('#btnResend2').click(function () {
    AjaxCall('/StudentProfile/SendOtpVerify', 'POST').done(function (response) {
        if (response == false) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something is wrong! Try again later</p>",
                timer: 1000,
                showConfirmButton: false
            })
        } else {
            Swal.fire({
                icon: 'success',
                title: "<p class='popupTitle'>New code has been sent to your email</p>",
                timer: 1000,
                showConfirmButton: false
            })
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
})




//validate block add alternative email ,verify code 
$('#alternativeEmailInput,#verifyCodeInput,#passalternativeMailInput,#confirmpassalternativeMailInput,#verifyOldEmail_CodeInput,#oldPasswordAlternativeMail,#newpassalternativeMailInput,#confirmnewpassalternativeMailInput').keypress(function (e) {
    var key = e.keyCode;
    if (key === 32) {
        e.preventDefault();
    }
});
// validate alternative email
$(document).ready(function () {
    var regexEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
    var regexOTPCode = /^[0-9]{6,6}$/;
    var regextPass = /^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^\w\s])\S{8,32}$/;

    $('#alternativeEmailInput').blur(function () {
        var inputAlternativeEmail = $('#alternativeEmailInput').val();
        if (inputAlternativeEmail.length != 0) {
            if (inputAlternativeEmail.length < 100) {
                if (!regexEmail.test(inputAlternativeEmail)) {
                    $('#errordesAlternativeEmailInput').html('Email is invalid');
                } else {
                    $('#errordesAlternativeEmailInput').html('');
                }
            } else {
                $('#errordesAlternativeEmailInput').html('Input less than 100 characters');
            }
        } else {
            $('#errordesAlternativeEmailInput').html('This field is required');
        }
    })
    $('#alternativeEmailInput').keyup(function () {
        var inputAlternativeEmail = $('#alternativeEmailInput').val();
        if (inputAlternativeEmail.length == 0 || inputAlternativeEmail.length > 100 || !regexEmail.test(inputAlternativeEmail)) {
            $('#continueBtn__alternativeMail').attr('disabled', true);
            $('#continueBtn__alternativeMail').css('cursor', 'not-allowed');
            $('#continueBtn__alternativeMail').css('opacity', '0.5');
        } else {
            $('#continueBtn__alternativeMail').removeAttr('disabled');
            $('#continueBtn__alternativeMail').css('cursor', 'pointer');
            $('#continueBtn__alternativeMail').css('opacity', '1');
        }
    })

    $('#verifyCodeInput').blur(function () {
        var inputVerifyCode = $('#verifyCodeInput').val();
        if (inputVerifyCode.length != 0) {
            if (!regexOTPCode.test(inputVerifyCode)) {
                $('#errorVerifyCodeInput').html('OTP must have 6 digits');
            } else {
                $('#errorVerifyCodeInput').html('');
            }
        } else {
            $('#errorVerifyCodeInput').html('This field is required');
        }
    })
    $('#verifyCodeInput').keyup(function () {
        var inputVerifyCode = $('#verifyCodeInput').val();
        if (!regexOTPCode.test(inputVerifyCode) || inputVerifyCode.length == 0) {
            $('#continueBtn__verifyCode').attr('disabled', true);
            $('#continueBtn__verifyCode').css('cursor', 'not-allowed');
            $('#continueBtn__verifyCode').css('opacity', '0.5');
        } else {
            $('#continueBtn__verifyCode').removeAttr('disabled');
            $('#continueBtn__verifyCode').css('cursor', 'pointer');
            $('#continueBtn__verifyCode').css('opacity', '1');
        }
    })

    $('#passalternativeMailInput').blur(function () {
        var inputPassword = $('#passalternativeMailInput').val();
        if (inputPassword.length != 0) {
            if (regextPass.test(inputPassword)) {
                $('#errorpassalternativeMailInput').html('');
            } else {
                $('#errorpassalternativeMailInput').html('Input 8-32 characters of which at least 1 uppercase letter and 1 special character');
            }
        } else {
            $('#errorpassalternativeMailInput').html('This field is required');
        }
    })

    $('#confirmpassalternativeMailInput').blur(function () {
        var inputPassword = $('#passalternativeMailInput').val()
        var inputConfirmPassword = $('#confirmpassalternativeMailInput').val();
        if (inputConfirmPassword.length != 0) {
            if (inputConfirmPassword === inputPassword) {
                $('#errorconfirmpassalternativeMailInput').html('');
            } else {
                $('#errorconfirmpassalternativeMailInput').html('The confirm password not match with password');
            }
        } else {
            $('#errorconfirmpassalternativeMailInput').html('This field is required');
        }
    })

    $('#passalternativeMailInput,#confirmpassalternativeMailInput').keyup(function () {
        var inputPassword = $('#passalternativeMailInput').val()
        var inputConfirmPassword = $('#confirmpassalternativeMailInput').val();
        if (inputPassword == 0 || inputConfirmPassword == 0 || !regextPass.test(inputPassword) || inputConfirmPassword !== inputPassword) {
            $('#submitBtn__setPassword').attr('disabled', true);
            $('#submitBtn__setPassword').css('cursor', 'not-allowed');
            $('#submitBtn__setPassword').css('opacity', '0.5');
        } else {
            $('#submitBtn__setPassword').removeAttr('disabled');
            $('#submitBtn__setPassword').css('cursor', 'pointer');
            $('#submitBtn__setPassword').css('opacity', '1');
        }
    })


    $('#verifyOldEmail_CodeInput').blur(function () {
        var inputVerifyOldEmail = $('#verifyOldEmail_CodeInput').val();
        if (inputVerifyOldEmail.length != 0) {
            if (!regexOTPCode.test(inputVerifyOldEmail)) {
                $('#errorVerifyOldEmail').html('OTP must have 6 digits');
            } else {
                $('#errorVerifyOldEmail').html('');
            }
        } else {
            $('#errorVerifyOldEmail').html('This field is required');
        }
    })
    $('#verifyOldEmail_CodeInput').keyup(function () {
        var inputVerifyOldEmail = $('#verifyOldEmail_CodeInput').val();
        if (!regexOTPCode.test(inputVerifyOldEmail) || inputVerifyOldEmail.length == 0) {
            $('#continueBtn__changeEmail').attr('disabled', true);
            $('#continueBtn__changeEmail').css('cursor', 'not-allowed');
            $('#continueBtn__changeEmail').css('opacity', '0.5');
        } else {
            $('#continueBtn__changeEmail').removeAttr('disabled');
            $('#continueBtn__changeEmail').css('cursor', 'pointer');
            $('#continueBtn__changeEmail').css('opacity', '1');
        }
    })

    $('#oldPasswordAlternativeMail').blur(function () {
        var inputOldPassword = $('#oldPasswordAlternativeMail').val();
        if (inputOldPassword.length != 0) {
            if (regextPass.test(inputOldPassword)) {
                $('#errorOldPasswordAlternativeMail').html('');
            } else {
                $('#errorOldPasswordAlternativeMail').html('Input 8-32 characters of which at least 1 uppercase letter and 1 special character');
            }
        } else {
            $('#errorOldPasswordAlternativeMail').html('This field is required');
        }
    })

    $('#oldPasswordAlternativeMail').keyup(function () {
        var inputOldPassword = $('#oldPasswordAlternativeMail').val();
        if (inputOldPassword == 0 || !regextPass.test(inputOldPassword)) {
            $('#continueBtn_InputOldPassword').attr('disabled', true);
            $('#continueBtn_InputOldPassword').css('cursor', 'not-allowed');
            $('#continueBtn_InputOldPassword').css('opacity', '0.5');
        } else {
            $('#continueBtn_InputOldPassword').removeAttr('disabled');
            $('#continueBtn_InputOldPassword').css('cursor', 'pointer');
            $('#continueBtn_InputOldPassword').css('opacity', '1');
        }
    })

    $('#newpassalternativeMailInput').blur(function () {
        var inputNewPassword = $('#newpassalternativeMailInput').val();
        if (inputNewPassword.length != 0) {
            if (regextPass.test(inputNewPassword)) {
                $('#errornewpassalternativeMailInput').html('');
            } else {
                $('#errornewpassalternativeMailInput').html('Input 8-32 characters of which at least 1 uppercase letter and 1 special character');
            }
        } else {
            $('#errornewpassalternativeMailInput').html('This field is required');
        }
    })

    $('#confirmnewpassalternativeMailInput').blur(function () {
        var inputNewPassword = $('#newpassalternativeMailInput').val()
        var inputConfirmNewPassword = $('#confirmnewpassalternativeMailInput').val();
        if (inputConfirmNewPassword.length != 0) {
            if (inputConfirmNewPassword === inputNewPassword) {
                $('#errorconfirmnewpassalternativeMailInput').html('');
            } else {
                $('#errorconfirmnewpassalternativeMailInput').html('The confirm password not match with password');
            }
        } else {
            $('#errorconfirmnewpassalternativeMailInput').html('This field is required');
        }
    })

    $('#newpassalternativeMailInput,#confirmnewpassalternativeMailInput').keyup(function () {
        var inputNewPassword = $('#newpassalternativeMailInput').val()
        var inputConfirmNewPassword = $('#confirmnewpassalternativeMailInput').val();
        if (inputNewPassword == 0 || inputConfirmNewPassword == 0 || !regextPass.test(inputNewPassword) || inputConfirmNewPassword !== inputNewPassword) {
            $('#submitNewPassBtn').attr('disabled', true);
            $('#submitNewPassBtn').css('cursor', 'not-allowed');
            $('#submitNewPassBtn').css('opacity', '0.5');
        } else {
            $('#submitNewPassBtn').removeAttr('disabled');
            $('#submitNewPassBtn').css('cursor', 'pointer');
            $('#submitNewPassBtn').css('opacity', '1');
        }
    })
})

