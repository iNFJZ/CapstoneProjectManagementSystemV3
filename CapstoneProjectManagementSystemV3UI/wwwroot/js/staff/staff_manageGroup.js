const lackofMember = document.querySelector('.lackofMember');
const fullMember = document.querySelector('.fullMember');
const mainlackOfMem = document.querySelector('.manageGroup__left--mainlackOfMem');
const mainFullMem = document.querySelector('.manageGroup__left--mainFullMem');
const stuNotInGroup = document.querySelectorAll('.memberNotInGroup');
//checkbox show GroupName to student
$('#checkbox').click(function () {
    AjaxCall('/ManageGroup/ChangeShowGroupNameStatus', JSON.stringify(null), "POST").done(function (response) {
        if (response != null) {
            Swal.fire({
                icon: 'success',
                title: '<p class="popupTitle">Change Status Successfully!</p>'
            })
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
});
//export to excel
$('.btnExport').click(function () {
    $('.showExportForm').toggle('hide-form');
});
$('.noBtnExport').click(function () {
    $('.showExportForm').toggle('hide-form');
});
$('.showExportForm').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.showExportForm').toggle('hide-form');
    }
});
$('.exportAllGroup').click(function () {
    var semesterId = $('.semesterDDL').val();
    $('#inputExport1').val(semesterId);
    $('#formExport1').submit();
});
$('.exportAllStudent').click(function () {
    var semesterId = $('.semesterDDL').val();
    $('#inputExport2').val(semesterId);
    $('#formExport2').submit();
});

$('.exportUngroupedStudent').click(function () {
    var semesterId = $('.semesterDDL').val();
    $('#inputExportUngroupedStudents').val(semesterId);
    $('#formExportUngropedStudent').submit();
});

$('.exportRegisteredGroupStudent').click(function () {
    $('#formExportRegisteredGroupStudent').submit();
});

$('.exportUnsubmittedGroupStudent').click(function () {
    $('#formExportUnsubmittedGroupStudent').submit();
});


//import excel file
$('.btnImport').click(function () {
    $('.showImportForm').toggle('hide-form');
});
$('.noBtnImport').click(function () {
    $('.showImportForm').toggle('hide-form');
});
$('.showImportForm').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.showImportForm').toggle('hide-form');
    }
});
$('.getTemplate1').click(function () {
    $('#formExport3').submit();
});
$('.getTemplate2').click(function () {
    $('#formExport4').submit();
});
$('#btnImportStudentList').click(function () {
    if (document.getElementById("fileStudentList").files.length === 0) {
        Swal.fire({
            icon: 'error',
            title: '<p class="popupTitle">You have not selected any files yet.</p>'
        })
    } else {
        $('#formImportStudentList').submit();
    }
});
$('#btnImportCheckCondition').click(function () {
    if (document.getElementById("fileCheckCondition").files.length === 0) {
        Swal.fire({
            icon: 'error',
            title: '<p class="popupTitle">You have not selected any files yet.</p>'
        })
    } else {
        $('#btnImportCheckCondition').attr('disabled', 'disabled');
        $('#btnImportCheckCondition').css('cursor', 'not-allowed');
        $('#btnImportCheckCondition').css('opacity', '0.5');
        $('#formImportCheckCondition').submit();
    }
});

$('.searchBtnUser').click(function () {
    var a, filter, txtValue;
    filter = document.getElementById("searchStudent").value.toUpperCase();
    for (i = 0; i < stuNotInGroup.length; i++) {
        a = stuNotInGroup[i].getElementsByTagName("a")[0];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            stuNotInGroup[i].style.display = "";
        } else {
            stuNotInGroup[i].style.display = "none";
        }
    }
});

fullMember.addEventListener('click', () => {
    mainlackOfMem.classList.add('hide-form');
    mainFullMem.classList.remove('hide-form');
    fullMember.classList.add('activeBtn');
    lackofMember.classList.remove('activeBtn');
})

lackofMember.addEventListener('click', () => {
    mainlackOfMem.classList.remove('hide-form');
    mainFullMem.classList.add('hide-form');
    fullMember.classList.remove('activeBtn');
    lackofMember.classList.add('activeBtn');
})

/*grouping popup*/
$('.btnGrouping').click(function () {

    $('.showGroupingForm').toggle('hide-form');
});
$('.noBtnGrouping').click(function () {
    $('.showGroupingForm').toggle('hide-form');
});
$('.showGroupingForm').click(function (e) {
    if (e.target === e.currentTarget) {
        $('.showGroupingForm').toggle('hide-form');
    }
});
$('.yesBtnGrouping').click(function () {
    $('.yesBtnGrouping').attr('disabled', 'disabled');
    $('.yesBtnGrouping').css('cursor', 'not-allowed');
    $('.yesBtnGrouping').css('opacity', '0.5');
    var specialtyId = $('.passSpecialtyId').attr('id');
    AjaxCall('/ManageGroup/Grouping', JSON.stringify(specialtyId), "POST").done(function (response) {
        if (response != null) {
            $('.finishGroupingText').html("" + response + " groups have been created");
            $('.showFinishGroupingForm').toggle('hide-form');
        }
    }).fail(function (error) {
        alert(error.StatusText);
    });
});
/*after grouping popup*/
$('.yesBtnFinishGrouping').click(function () {
    $('.searchBtn').click();
});

// Ajax for Flter Search
$(function () {

    $('#professionalfilter').on("change", function () {
        var profession = $('#professionalfilter').val();
        AjaxCall('/ManageGroup/GetCorrespondingSpecialty', JSON.stringify(profession), "POST").done(function (response) {
            $('#specialtyfilter').html('');
            var options = '';
            options += '<option value="0">Specialty</option>';
            if (response != null) {
                for (var i = 0; i < response.length; i++) {
                    var specialty = response[i]
                    options += '<option value="' + specialty.specialtyID + '">' + specialty.specialtyFullName + '</option>';
                }
            }
            $('#specialtyfilter').append(options);
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });

});
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}