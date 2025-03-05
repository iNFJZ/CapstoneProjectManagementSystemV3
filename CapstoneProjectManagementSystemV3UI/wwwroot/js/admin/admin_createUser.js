//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: type,
//        contentType: "application/json; charset=utf-8",
//    });
//}

//create staff
/*$('#btn-CreateStaff').on('click', function () {*/
function createStaff() {
    var mail = $('#fptMail').val().trim();
    var pattern = /^[a-zA-Z0-9._%+-]+@fpt.edu.vn/i;

    if (mail.length > 100) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>FPT Email must less than 100 characters</p>",
            timer: 1500,
            showConfirmButton: false
        })
    }
    else if (!mail.match(pattern)) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Wrong format FPT Email</p>",
            timer: 1500,
            showConfirmButton: false
        })
    }
    else {
        var user = {};
        user.UserID = user.FptEmail = mail;
        user.FullName = $('#name').val().trim();
        user.Gender = $('input[name="gender"]:checked').val();

        AjaxCall('/CreateUser/CreateStaff', JSON.stringify(user), 'POST').done(function (respone) {
            if (respone == 1) {
                if ($('#sessionRole').val() == 5) { // role admin
                    Swal.fire({
                        icon: 'success',
                        title: "<p class='popupTitle'>Create staff sucessfully</p>",
                        text: 'Go to list page?',
                        showDenyButton: true,
                        confirmButtonText: 'Yes',
                        denyButtonText: `No`
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location = "/ListUser/Index";
                        } else {
                            window.location = "/CreateUser/Index";
                        }
                    });
                } else { //role staff
                    Swal.fire({
                        icon: 'success',
                        title: "<p class='popupTitle'>Create staff sucessfully</p>",
                        timer: 1500,
                        showConfirmButton: false
                    }).then((result) => {
                        window.location = "/CreateUser/Index";
                    });
                }
            } else if (respone == 0) {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>FPTEmail does exist</p>",
                    timer: 1500,
                    showConfirmButton: false
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something went wrong</p>",
                    timer: 1500,
                    showConfirmButton: false
                })
            }
        }).fail(function (error) {
            //alert(error.StatusText);
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Something went wrong</p>",
                timer: 1500,
                showConfirmButton: false
            })
        });
    }
}
//});

$('#btn-CreateUser').on('click', function () {
    var role = $('#select-role').val();
    if (role === "staff") {
        createStaff();
    } else if (role === "departmentLeader") {

        var fptMail = $('#fptMail').val().trim();
        var feMail = $('#feMail').val().trim();
        var patternFPTMail = /^[a-zA-Z0-9._%+-]+@fpt.edu.vn/i;
        var patternFEMail = /^[a-zA-Z0-9._%+-]+@fe.edu.vn/i;
        var fullName = $('#name').val().trim();

        if (fptMail.length > 100) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>FPT Email must less than 100 characters</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (feMail.length > 100) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>FE Email must less than 100 characters</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (fullName.length > 50) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Full name must be less than 50</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (!fptMail.match(patternFPTMail)) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Wrong format FPT Email</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (feMail.trim().length != 0 && !feMail.match(patternFEMail)) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Wrong format FE Email</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else {
            var user = {};
            user.UserID = user.FptEmail = fptMail;
            user.FullName = fullName;
            user.Gender = $('input[name="gender"]:checked').val();

            var profession = {};
            profession.ProfessionID = $('#profession').find(":selected").val();

            var settingObj = {};
            settingObj.PhoneNumber = false;
            settingObj.PersonalEmail = false;
            settingObj.SelfDescription = false;
            var settingStringJson = JSON.stringify(settingObj);

            var supervisor = {};
            supervisor.user = user;
            if (feMail.trim().length != 0) {
                supervisor.FeEduEmail = feMail;
            } else {
                supervisor.FeEduEmail = "";
            }
            supervisor.FieldSetting = settingStringJson;

            AjaxCall('/CreateUser/CreateSupervisorLeader', JSON.stringify(supervisor), 'POST').done(function (respone) {
                if (respone == 2) {
                    Swal.fire({
                        icon: 'success',
                        title: "<p class='popupTitle'>Create department leader sucessfully</p>",
                        text: 'Go to list page?',
                        showDenyButton: true,
                        confirmButtonText: 'Yes',
                        denyButtonText: `No`
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if ($('#sessionRole').val() == 5)
                                window.location = "/ListUser/Index";
                            else
                                window.location = "/ManageDevhead/Index";
                        } else {
                            window.location = "/CreateUser/Index";
                        }
                    });
                }
                else if (respone == 1) {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>FE Email does exist</p>",
                        timer: 1500,
                        showConfirmButton: false
                    })
                }
                else if (respone == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>FPT Email does exist</p>",
                        timer: 1500,
                        showConfirmButton: false
                    })
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: "<p class='popupTitle'>Something went wrong</p>",
                        timer: 1500,
                        showConfirmButton: false
                    })
                }
            }).fail(function (error) {
                //alert(error.StatusText);
                Swal.fire({
                    icon: 'error',
                    title: "<p class='popupTitle'>Something went wrong</p>",
                    timer: 1500,
                    showConfirmButton: false
                })
            });
        }
    }
});