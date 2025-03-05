//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: type,
//        contentType: "application/json; charset=utf-8",
//    });
//}

$('#btn-cancel').on('click', function () {
    window.location = "/ManageDevhead/Index";
});

$('#btn-update').on('click', function () {
    var feMail = $('#feMail').val().trim();
    var patternFEMail = /^[a-zA-Z0-9._%+-]+@fe.edu.vn/i;
    var name = $('#fullName').val().trim();

    if (feMail.length != 0 && !feMail.match(patternFEMail)) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Wrong format FE Email</p>",
            timer: 1500,
            showConfirmButton: false
        })
    } else if (feMail.length > 100) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>FE Email must less than 100 characters</p>",
            timer: 1500,
            showConfirmButton: false
        })
    }
    else if (name.length == 0) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Full name is required</p>",
            timer: 1500,
            showConfirmButton: false
        })
    }
    else if (name.length > 50) {
        Swal.fire({
            icon: 'error',
            title: "<p class='popupTitle'>Full name must less than 50 characters</p>",
            timer: 1500,
            showConfirmButton: false
        })
    }
    else {
        var array_profession = [];
        $('.select-profession').each(function () {
            var value = $(this).val();
            array_profession.push(value);
        });

        var checkZeroValue = true;
        for (var i = 0; i < array_profession.length; i++) {
            if (array_profession[i] != 0) {
                checkZeroValue = false;
                break;
            }
        }

        var array_maxgroup = [];
        $('.maxgroups').each(function () {
            var value = $(this).val();
            array_maxgroup.push(value);
        });

        var checkNegativeMaxGroup = false;
        for (var i = 0; i < array_maxgroup.length; i++) {
            if (array_maxgroup[i] < 0) {
                checkNegativeMaxGroup = true;
                break;
            }
        }

        if (array_profession.length == 0 || checkZeroValue) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Choose a profession to update!</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else if (checkNegativeMaxGroup) {
            Swal.fire({
                icon: 'error',
                title: "<p class='popupTitle'>Max group must >= 0!</p>",
                timer: 1500,
                showConfirmButton: false
            })
        }
        else {

            var array_devhead = [];
            $('.checkbox-devhead').each(function () {
                var value = $(this).is(":checked") ? true : false;
                array_devhead.push(value);
            });

            var user = {};
            user.UserID = $('#userId').val();
            user.FullName = name;
            user.Gender = $('input[name="gender"]:checked').val();

            var supervisorProfesisons = [];
            for (var i = 0; i < array_profession.length; i++) {
                if (array_profession[i] != 0) {
                    var supervisorProfesison = {};
                    var profession = {};
                    profession.ProfessionID = array_profession[i];
                    supervisorProfesison.Profession = profession;

                    var isDevHead = array_devhead[i];
                    supervisorProfesison.IsDevHead = isDevHead;

                    var maxGroup = array_maxgroup[i];
                    supervisorProfesison.MaxGroup = maxGroup;

                    supervisorProfesisons[i] = supervisorProfesison;
                }
            }

            if (!array_devhead.includes(true)) {
                Swal.fire({
                    icon: 'warning',
                    title: "<p class='popupTitle'>This account will become a normal supervisor</p>",
                    test: "Are you sure?",
                    showDenyButton: true,
                    confirmButtonText: 'Yes',
                    denyButtonText: `No`
                }).then((result) => {
                    if (result.isConfirmed) {
                        var supervisor = {};
                        supervisor.User = user;
                        supervisor.FeEduEmail = feMail;
                        supervisor.IsActive = $('input[name="active"]:checked').val() == "yes" ? true : false;
                        supervisor.SupervisorProfessions = supervisorProfesisons;

                        AjaxCall('/ManageDevhead/UpdateDevhead', JSON.stringify(supervisor), 'POST').done(function (respone) {
                            if (respone == 1) {
                                Swal.fire({
                                    icon: 'success',
                                    title: "<p class='popupTitle'>Update Department Leader sucessfully</p>",
                                    timer: 1500,
                                    showConfirmButton: false
                                }).then((result) => {
                                    window.location = "/ManageDevhead/Index";
                                });
                            } else if (respone == 2) {
                                Swal.fire({
                                    icon: 'error',
                                    title: "<p class='popupTitle'>FE Email does exist</p>",
                                    timer: 1500,
                                    showConfirmButton: false
                                })
                            }
                            else {
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
                        })
                    }
                });
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: "<p class='popupTitle'>Are you sure?</p>",
                    showDenyButton: true,
                    confirmButtonText: 'Yes',
                    denyButtonText: `No`
                }).then((result) => {
                    if (result.isConfirmed) {
                        var supervisor = {};
                        supervisor.User = user;
                        supervisor.FeEduEmail = feMail;
                        supervisor.IsActive = $('input[name="active"]:checked').val() == "yes" ? true : false;
                        supervisor.SupervisorProfessions = supervisorProfesisons;

                        AjaxCall('/ManageDevhead/UpdateDevhead', JSON.stringify(supervisor), 'POST').done(function (respone) {
                            if (respone == 1) {
                                Swal.fire({
                                    icon: 'success',
                                    title: "<p class='popupTitle'>Update Department Leader sucessfully</p>",
                                    timer: 1500,
                                    showConfirmButton: false
                                }).then((result) => {
                                    window.location = "/ManageDevhead/Index";
                                });
                            }
                            else if (respone == 2) {
                                Swal.fire({
                                    icon: 'error',
                                    title: "<p class='popupTitle'>FE Email does exist</p>",
                                    timer: 1500,
                                    showConfirmButton: false
                                })
                            }
                            else {
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
                        })
                    }
                });
            }
        }
    }
});