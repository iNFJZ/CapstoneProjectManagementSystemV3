$(document).ready(function () {
    var proCount = document.getElementById("passToJs1").value;
    var speCount = document.getElementById("passToJs2").value;
    console.log(proCount);
    console.log(speCount);

    //$('.pro' + ' input').prop('disabled', false)

    $('#addPro').click(function () {

        $('#createPro').append('<div class="create">\
        <button class="editPro hide-btn" id="editPro_' + proCount + '"><i class="fa-solid fa-pen-to-square"></i>Edit</button>\
        <button class="savePro hide-btn" id="savePro_' + proCount + '"><i class="fa-solid fa-floppy-disk"></i>Save</button>\
        <button class="deletePro" id="deletePro_' + proCount + '"><i class="fa-solid fa-trash"></i>Delete</button>\
        <div class="pro" id="pro_' + proCount + '">\
        <div class="pro__left">\
        <p class="title">Profession</p>\
        <div class="pro__left--info">\
            <div class="info--left">\
                <input type="hidden" value="0" id="proId_'+ proCount + '" />\
                <p class="smallTitle">Abbreviation</p>\
                <input class="introInput" type="text" placeholder="Ex: SE,..." id="proAbbre_'+ proCount + '">\
            </div>\
            <div class="info--right">\
                <p class="smallTitle">Fullname</p>\
                <input type="text" class="introInput" placeholder="Ex: Software Engineering,..." id="proFull_'+ proCount + '">\
            </div>\
        </div>\
    </div>\
    <div class="pro__right">\
        <p class="title">Specialty</p>\
        <div class="pro__right--info" id="showSpe_'+ proCount + '">\
            <div class="infoInput" id="infoInput_'+ speCount + '">\
                <input type="hidden" value="0" id="speId_'+ speCount + '" />\
                <div class="info--abbrev">\
                    <p class="smallTitle">Abbreviation</p>\
                    <input type="text" class="introInput" placeholder="Ex: IS,..." id="speAbbre_'+ speCount + '">\
                </div>\
                <div class="info--fullname">\
                    <p class="smallTitle">Fullname</p>\
                    <input type="text" class="introInput" placeholder="Ex: Information System,..." id="speFull_'+ speCount + '">\
                </div>\
                <div class="info--maxMember">\
                    <p class="smallTitle">Max Member</p>\
                    <input type="number" class="introInput " placeholder="Ex: 5,..." id="speMaxMem_'+ speCount + '">\
                </div>\
                <div class="info--maxMember">\
                    <p class="smallTitle">Code</p>\
                    <input type="text" class="introInput" placeholder="Ex: 	SWP490,..." id="speCode_'+ speCount + '">\
                </div>\
                <button class="deleteSpe" id="deleteSpe_'+ speCount + '"><i class="fa-solid fa-trash"></i></button>\
            </div>\
        </div>\
        <button class="addSpe" id="'+ proCount + '"><i class="fa-solid fa-plus"></i>Add Specialty</button>\
    </div>\
    </div>\
    </div>'
        )
        proCount++;
        speCount++;
    })

    $(document).on('click', 'button.addSpe', function () {
        var speId = this.id;
        $('#showSpe_' + speId + '').append('<div class="infoInput" id="infoInput_' + speCount + '">\
        <input type="hidden" value="0" id="speId_'+ speCount + '" />\
        <div class="info--abbrev">\
            <p class="smallTitle">Abbreviation</p>\
            <input class="introInput" type="text" placeholder="Ex: IS,..." id="speAbbre_'+ speCount + '">\
        </div>\
        <div class="info--fullname">\
            <p class="smallTitle">Fullname</p>\
            <input class="introInput" type="text" placeholder="Ex: Information System,..." id="speFull_'+ speCount + '">\
        </div>\
        <div class="info--maxMember">\
            <p class="smallTitle">Max Member</p>\
            <input class="introInput" type="number" placeholder="Ex: 5,..." id="speMaxMem_'+ speCount + '">\
        </div>\
        <div class="info--maxMember">\
            <p class="smallTitle">Code</p>\
            <input type="text" class="introInput" placeholder="Ex: 	SWP490,..." id="speCode_'+ speCount + '">\
        </div>\
        <button class="deleteSpe" style="margin-bottom: 0px" id="deleteSpe_'+ speCount + '"><i class="fa-solid fa-trash"></i></button>\
    </div>');
        speCount++;
    })

    $(document).on('click', 'button.saveAll', function () {
        var checkBlank = false;
        const input = document.querySelectorAll("input");
        for (let i = 0; i < input.length; i++) {
            if (input[i].value.length == 0) {
                checkBlank = true;
                break;
            }
        }
        if (checkBlank) {
            Swal.fire({
                icon: 'error',
                title: '<p class="popupTitle">All fields cannot be blank!</p>'
            })
        } else {
            const saveBtn = document.querySelectorAll(".savePro");
            for (let i = 0; i < saveBtn.length; i++) {
                saveBtn[i].click();
            }
        }
        /*$('.savePro').click();*/
    })

    //$(document).on('click', 'button.editPro', function () {
    //    var btnId = this.id.substring(8);
    //    $('#showSpe_' + btnId + '').children('.infoInput').children('.deleteSpe').toggle('hide-btn');
    //    $('#savePro_' + btnId + '').toggle('hide-form');
    //    $('#editPro_' + btnId + '').toggle('hide-form');
    //    $('#' + btnId + '').toggle('hide-btn');
    //    $('#pro_' + btnId + ' input').prop('disabled', false)
    //})

    $(document).on('click', 'button.savePro', function () {
        var btnId = this.id.substring(8);
        //$('#showSpe_' + btnId + '').children('.infoInput').children('.deleteSpe').toggle('hide-btn');
        //$('#savePro_' + btnId + '').toggle('hide-form');
        //$('#editPro_' + btnId + '').toggle('hide-form');
        //$('#' + btnId + '').toggle('hide-btn');
        //$('#pro_' + btnId + ' input').prop('disabled', true)

        /*save to db*/
        var profession = {};
        profession.ProfessionID = $('#proId_' + btnId + '').val();
        profession.ProfessionAbbreviation = $('#proAbbre_' + btnId + '').val();
        profession.ProfessionFullName = $('#proFull_' + btnId + '').val();

        var Specialties = [];
        $('#showSpe_' + btnId + '').children('.infoInput').each(function () {
            var speId = this.id.substring(10);
            var specialty = {};
            specialty.SpecialtyID = $('#speId_' + speId + '').val();
            specialty.SpecialtyAbbreviation = $('#speAbbre_' + speId + '').val();
            specialty.SpecialtyFullName = $('#speFull_' + speId + '').val();
            specialty.MaxMember = $('#speMaxMem_' + speId + '').val();
            specialty.CodeOfGroupName = $('#speCode_' + speId + '').val();
            Specialties.push(specialty);
        });

        profession.Specialties = Specialties;

        console.log(profession);

        AjaxCall('/ManageMajor/UpdateMajorV2', JSON.stringify(profession), "POST").done(function (response) {
            window.location.href = '/SemesterManage/Index';
        }).fail(function (error) {
            alert(error.StatusText);
        });
    })


    $(document).on('click', 'button.deleteSpe', function () {
        var speId = this.id.substring(10);
        var specId = $('#speId_' + speId + '').val();
        var selt = this;
        Swal.fire({
            title: 'Confirmation',
            text: 'Are you sure you want to delete the speciality?',
            icon: 'warning',
            position: 'fixed',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Loading',
                    position: 'fixed',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                        var startTime = performance.now();
                        $.ajax({
                            url: '/ManageMajor/RemoveSpecialty',
                            data: JSON.stringify(specId),
                            dataType: "json",
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                var endTime = performance.now();
                                var duration = endTime - startTime;
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Loading completed',
                                    html: 'Time taken: ' + duration.toFixed(2) + ' milliseconds',
                                    position: 'fixed',
                                    showConfirmButton: false,
                                    timer: 1500
                                }).then(function () {
                                    $(selt).parent().remove();
                                });
                            },
                            error: function (xhr, status, error) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: 'An error occurred!',
                                    footer: error,
                                    position: 'fixed'
                                });
                            }
                        });
                    }
                });
            }
        });
    });

    $(document).on('click', 'button.deletePro', function () {
        var stt = this.id.substring(10);
        var sproId = $('#proId_' + stt + '').val();
        var selt = this;
        Swal.fire({
            title: 'Confirmation',
            text: 'Are you sure you want to delete the profession?',
            icon: 'warning',
            position: 'fixed',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Loading',
                    position: 'fixed',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                        var startTime = performance.now();
                        $.ajax({
                            url: '/ManageMajor/RemoveProfession',
                            data: JSON.stringify(sproId),
                            dataType: "json",
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                var endTime = performance.now();
                                var duration = endTime - startTime;
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Loading completed',
                                    html: 'Time taken: ' + duration.toFixed(2) + ' milliseconds',
                                    position: 'fixed',
                                    showConfirmButton: false,
                                    timer: 1500
                                }).then(function () {
                                    $(selt).parent().remove();
                                });
                            },
                            error: function (xhr, status, error) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: 'An error occurred!',
                                    footer: error,
                                    position: 'fixed'
                                });
                            }
                        });
                    }
                });
            }
        });
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