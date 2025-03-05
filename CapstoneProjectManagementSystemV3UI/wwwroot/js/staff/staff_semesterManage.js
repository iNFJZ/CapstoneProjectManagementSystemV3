var bodyMailEditor;
var bodyMailValue = $("#bodyMailValue").val();
function initTextEditor() {
    tinymce.init({
        selector: 'textarea',
        height: 300,
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media paste code help wordcount'
        ],
        toolbar: 'undo redo | formatselect | bold italic backcolor | \
                    alignleft aligncenter alignright alignjustify | \
                    bullist numlist outdent indent | removeformat | help',
        setup: function (editor) {
            editor.on('init', function () {

            });

            editor.on('input', function () {
                var content = editor.getContent({ format: 'text' });
                if (bodyMailValue == null || bodyMailValue.length == 0) {
                    if ($('#newSemesterName').val().replace(/\s/g, "").length <= 0 || $('#newSemesterName').val().length > 50
                        || $('#newSemesterCode').val().replace(/\s/g, "").length <= 0 || $('#newSemesterCode').val().length > 20
                        || $('#newSemesterStartTime').val().replace(/\s/g, "").length <= 0 || $('#newSemesterEndTime').val().replace(/\s/g, "").length <= 0
                        || $('#deadlineChangeTopic').val().replace(/\s/g, "").length <= 0 || content.replace(/\s/g, "").length <= 0 || $('#subjectMailTemplate').val().replace(/\s/g, "").length <= 0) {
                        $('#nextBtn').attr('disabled', 'disabled');
                        $('#nextBtn').css('cursor', 'not-allowed');
                        $('#nextBtn').css('opacity', '0.5');
                    } else {
                        $('#nextBtn').removeAttr('disabled', 'disabled');
                        $('#nextBtn').css('cursor', 'pointer');
                        $('#nextBtn').css('opacity', '1');
                    }
                } else {
                    if ($('#showSemesterInputAbbreviation').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputAbbreviation').val().length > 50
                        || $('#showSemesterInputCode').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputCode').val().length > 20
                        || $('#showSemesterInputStart').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputEnd').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputDeadlineChangeIdea').val().replace(/\s/g, "").length <= 0
                        || $('#showSemesterInputDeadlineRegisterGroup').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputSubjectMailTemplate').val().replace(/\s/g, "").length <= 0 || content.replace(/\s/g, "").length <= 0) {
                        $('#saveEditSemester').attr('disabled', 'disabled');
                        $('#saveEditSemester').css('cursor', 'not-allowed');
                        $('#saveEditSemester').css('opacity', '0.5');
                    } else {
                        $('#saveEditSemester').removeAttr('disabled', 'disabled');
                        $('#saveEditSemester').css('cursor', 'pointer');
                        $('#saveEditSemester').css('opacity', '1');
                    }
                }
            });
        }
    }).then(function (editors) {
        bodyMailEditor = editors[0];
        console.log(bodyMailEditor);
        if (bodyMailValue == null || bodyMailValue.length == 0) {
            return;
        }
        bodyMailEditor.setContent(bodyMailValue);
        bodyMailEditor.setMode('readonly');
    });
}

$(document).ready(function () {

    initTextEditor();

    $(".backToSemesterMain").click(function () {
        location.reload(true);
    });

    $(".editBtn").click(function () {
        $(".backToSemesterMain").toggle('showBtn');
    });
    $('#nextBtn').attr('disabled', 'disabled');
    $('#nextBtn').css('cursor', 'not-allowed');
    $('#nextBtn').css('opacity', '0.5');

    $('#saveEditSemester').attr('disabled', 'disabled');
    $('#saveEditSemester').css('cursor', 'not-allowed');
    $('#saveEditSemester').css('opacity', '0.5');

    $('.starSemester').click(function () {
        $('.createNewSemester').toggle('hide-form');
    });
    $('.cancelBtn').click(function () {
        $('.createNewSemester').toggle('hide-form');
    });
    $('.createNewSemester').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.createNewSemester').toggle('hide-form');
        }
    });

    $('#newSemesterName').blur(function () {
        if ($('#newSemesterName').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterName').html('This field is required');
        } else if ($('#newSemesterName').val().length > 50) {
            $('#errornewSemesterName').html('Input less than 50 characters');
        } else {
            $('#errornewSemesterName').html('');
        }
    })

    $('#newSemesterCode').blur(function () {
        if ($('#newSemesterCode').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterCode').html('This field is required');
        } else if ($('#newSemesterCode').val().length > 20) {
            $('#errornewSemesterCode').html('Input less than 20 characters');
        } else {
            $('#errornewSemesterCode').html('');
        }
    })

    $('#newSemesterStartTime').blur(function () {
        if ($('#newSemesterStartTime').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterStartTime').html('This field is required');
        } else {
            $('#errornewSemesterStartTime').html('');
            validateEndDateStartSemester();
        }
    })

    $('#newSemesterEndTime').blur(function () {
        if ($('#newSemesterEndTime').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterEndTime').html('This field is required');
        } else {
            $('#errornewSemesterEndTime').html('');
            validateEndDateStartSemester();
        }
    })


    function validateEndDateStartSemester() {
        var startDate = new Date($('#newSemesterStartTime').val());
        var endDate = new Date($('#newSemesterEndTime').val());

        if (endDate < startDate) {
            $('#errornewSemesterEndTime').html('End Date must be greater than Start Date');
            $('#nextBtn').attr('disabled', 'disabled');
            $('#nextBtn').css('cursor', 'not-allowed');
            $('#nextBtn').css('opacity', '0.5');


        } else {
            $('#errornewSemesterEndTime').html('');
            $('#nextBtn').removeAttr('disabled', 'disabled');
            $('#nextBtn').css('cursor', 'pointer');
            $('#nextBtn').css('opacity', '1');
        }
    }

    $('#deadlineChangeTopic').blur(function () {
        if ($('#deadlineChangeTopic').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterDeadlineChangeTopic').html('This field is required');
        } else {
            $('#errornewSemesterDeadlineChangeTopicc').html('');
        }
    })

    $('#deadlineRegisterGroup').blur(function () {
        if ($('#deadlineRegisterGroup').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterDeadlineRegisterGroup').html('This field is required');
        } else {
            $('#errornewSemesterDeadlineRegisterGroup').html('');
        }
    })

    $('#subjectMailTemplate').blur(function () {
        if ($('#subjectMailTemplate').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterSubjectMailTemplate').html('This field is required');
        } else {
            $('#errornewSemesterSubjectMailTemplate').html('');
        }
    })

    $('#bodyMailTemplate').blur(function () {
        if ($('#bodyMailTemplate').val().replace(/\s/g, "").length <= 0) {
            $('#errornewSemesterBodyMailTemplate').html('This field is required');
        } else {
            $('#errornewSemesterBodyMailTemplate').html('');
        }
    })

    $('body').on('blur keyup change', '#newSemesterName, #newSemesterCode, #newSemesterStartTime, #newSemesterEndTime , #deadlineRegisterGroup , #deadlineChangeTopic , #subjectMailTemplate  ', function () {
        if ($('#newSemesterName').val().replace(/\s/g, "").length <= 0 || $('#newSemesterName').val().length > 50
            || $('#newSemesterCode').val().replace(/\s/g, "").length <= 0 || $('#newSemesterCode').val().length > 20
            || $('#newSemesterStartTime').val().replace(/\s/g, "").length <= 0 || $('#newSemesterEndTime').val().replace(/\s/g, "").length <= 0
            || $('#deadlineChangeTopic').val().replace(/\s/g, "").length <= 0 || bodyMailEditor.getContent({ format: 'text' }).replace(/\s/g, "").length <= 0 || $('#subjectMailTemplate').val().replace(/\s/g, "").length <= 0) {
            $('#nextBtn').attr('disabled', 'disabled');
            $('#nextBtn').css('cursor', 'not-allowed');
            $('#nextBtn').css('opacity', '0.5');
        } else {
            $('#nextBtn').removeAttr('disabled', 'disabled');
            $('#nextBtn').css('cursor', 'pointer');
            $('#nextBtn').css('opacity', '1');
        }
    })

    $('#showSemesterInputAbbreviation').blur(function () {
        if ($('#showSemesterInputAbbreviation').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputAbbreviation').html('This field is required');
        } else if ($('#showSemesterInputAbbreviation').val().length > 50) {
            $('#errorshowSemesterInputAbbreviation').html('Input less than 50 characters');
        } else {
            $('#errorshowSemesterInputAbbreviation').html('');
        }
    })

    $('#showSemesterInputCode').blur(function () {
        if ($('#showSemesterInputCode').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputCode').html('This field is required');
        } else if ($('#showSemesterInputCode').val().length > 20) {
            $('#errorshowSemesterInputCode').html('Input less than 20 characters');
        } else {
            $('#errorshowSemesterInputCode').html('');
        }
    })

    $('#showSemesterInputStart').blur(function () {
        if ($('#showSemesterInputStart').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputStart').html('This field is required');
        } else {
            $('#errorshowSemesterInputStart').html('');
            validateEndDate();
        }
    })

    $('#showSemesterInputEnd').blur(function () {
        if ($('#showSemesterInputEnd').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputEnd').html('This field is required');
        } else {
            $('#errorshowSemesterInputEnd').html('');
            validateEndDate();
        }
    })

    //validate enddate>startdate
    function validateEndDate() {
        var startDate = new Date($('#showSemesterInputStart').val());
        var endDate = new Date($('#showSemesterInputEnd').val());

        if (endDate < startDate) {
            $('#errorshowSemesterInputEnd').html('End Date must be greater than Start Date');
            $('#saveEditSemester').attr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'not-allowed');
            $('#saveEditSemester').css('opacity', '0.5');


        } else {
            $('#errorshowSemesterInputEnd').html('');
            $('#saveEditSemester').removeAttr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'pointer');
            $('#saveEditSemester').css('opacity', '1');
        }
    }

    $('#showSemesterInputDeadlineChangeIdea').blur(function () {
        if ($('#showSemesterInputDeadlineChangeIdea').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputDeadlineChangeIdea').html('This field is required');
        } else {
            $('#errorshowSemesterInputDeadlineChangeIdea').html('');
        }
    })

    $('#showSemesterInputSubjectMailTemplate').blur(function () {
        if ($('#showSemesterInputSubjectMailTemplate').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputSubjectMailTemplate').html('This field is required');
        } else {
            $('#errorshowSemesterInputSubjectMailTemplate').html('');
        }
    })

    $('#showSemesterInputBodyMailTemplate').blur(function () {
        if ($('#showSemesterInputBodyMailTemplate').val().replace(/\s/g, "").length <= 0) {
            $('#errorshowSemesterInputBodyMailTemplate').html('This field is required');
        } else {
            $('#errorshowSemesterInputBodyMailTemplate').html('');
        }
    })


    $('body').on('blur keyup change', '#showSemesterInputAbbreviation, #showSemesterInputCode , #showSemesterInputStart, #showSemesterInputEnd , #showSemesterInputDeadlineChangeIdea , #showSemesterInputDeadlineRegisterGroup , #showSemesterInputSubjectMailTemplate  ', function () {
        if ($('#showSemesterInputAbbreviation').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputAbbreviation').val().length > 50
            || $('#showSemesterInputCode').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputCode').val().length > 20
            || $('#showSemesterInputStart').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputEnd').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputDeadlineChangeIdea').val().replace(/\s/g, "").length <= 0
            || $('#showSemesterInputDeadlineRegisterGroup').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputSubjectMailTemplate').val().replace(/\s/g, "").length <= 0 || bodyMailEditor.getContent({ format: 'text' }).replace(/\s/g, "").length <= 0) {
            $('#saveEditSemester').attr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'not-allowed');
            $('#saveEditSemester').css('opacity', '0.5');
        } else {
            $('#saveEditSemester').removeAttr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'pointer');
            $('#saveEditSemester').css('opacity', '1');
        }
    })

    $('#showSemesterInputConfirmationOfDevheadNeeded').on('click', function () {
        if ($('#showSemesterInputAbbreviation').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputAbbreviation').val().length > 50
            || $('#showSemesterInputCode').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputCode').val().length > 20
            || $('#showSemesterInputStart').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputEnd').val().replace(/\s/g, "").length <= 0 || $('#showSemesterInputDeadlineChangeIdea').val().replace(/\s/g, "").length <= 0
            || $('#showSemesterInputDeadlineRegisterGroup').val().replace(/\s/g, "").length <= 0) {
            $('#saveEditSemester').attr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'not-allowed');
            $('#saveEditSemester').css('opacity', '0.5');
        } else {
            $('#saveEditSemester').removeAttr('disabled', 'disabled');
            $('#saveEditSemester').css('cursor', 'pointer');
            $('#saveEditSemester').css('opacity', '1');
        }
    })


});

const saveBtn = document.querySelector('.saveBtn');
const editBtn = document.querySelector('.editBtn');
const showSemesterInputAbbreviation = document.querySelector('#showSemesterInputAbbreviation');
const showSemesterInputStar = document.querySelector('#showSemesterInputStart');
const showSemesterInputEnd = document.querySelector('#showSemesterInputEnd');
const showSemesterInputCode = document.querySelector('#showSemesterInputCode');
const showSemesterInputDeadlineChangeIdea = document.querySelector("#showSemesterInputDeadlineChangeIdea");
const showSemesterInputDeadlineRegisterGroup = document.querySelector("#showSemesterInputDeadlineRegisterGroup");
const showSemesterInputConfirmationOfDevHeadNeeded = document.querySelector("#showSemesterInputConfirmationOfDevheadNeeded");
if (editBtn != null) {
    editBtn.addEventListener('click', () => {
        showSemesterInputAbbreviation.disabled = false;
        showSemesterInputStar.disabled = false;
        showSemesterInputEnd.disabled = false;
        showSemesterInputCode.disabled = false;
        showSemesterInputDeadlineChangeIdea.disabled = false;
        showSemesterInputConfirmationOfDevHeadNeeded.disabled = false;
        showSemesterInputDeadlineRegisterGroup.disabled = false;
        $('#showSemesterInputSubjectMailTemplate').prop("disabled", false);
        if (bodyMailEditor) {
            bodyMailEditor.setMode("design");
        }
        editBtn.classList.toggle('hide-btn');
        saveBtn.classList.toggle('hide-btn');
    })
}

$('body').on('click', '.saveBtn', function () {
    var semester = {};
    semester.semesterId = $('#semesterId').html();
    semester.semesterName = $('#showSemesterInputAbbreviation').val();
    semester.semesterCode = $('#showSemesterInputCode').val();
    semester.startTime = $('#showSemesterInputStart').val();
    semester.endTime = $('#showSemesterInputEnd').val();
    semester.deadlineChangeIdea = $("#showSemesterInputDeadlineChangeIdea").val();
    semester.deadlineRegisterGroup = $("#showSemesterInputDeadlineRegisterGroup").val();
    semester.isConfirmationOfDevHeadNeeded = $("#showSemesterInputConfirmationOfDevheadNeeded").prop("checked");
    semester.subjectMailTemplate = $('#showSemesterInputSubjectMailTemplate').val();
    semester.bodyMailTemplate = bodyMailEditor.getContent();

    AjaxCall('/SemesterManage/UpdateCurrentSemester', JSON.stringify(semester), 'POST').done(function (response) {
        if (response >= 1) {
            window.location.href = '/SemesterManage/Index';
        } else
            alert('Update semester error');
    }).fail(function (error) {
        alert(error.StatusText);
    });
});

$('body').on('click', '.submitBtn', function () {

    var newSemester = {};
    newSemester.semesterName = $('#newSemesterName').val();
    newSemester.semesterCode = $('#newSemesterCode').val();
    newSemester.startTime = $('#newSemesterStartTime').val();
    newSemester.endTime = $('#newSemesterEndTime').val();
    newSemester.deadlineChangeIdea = $("#deadlineChangeTopic").val();
    newSemester.deadlineRegisterGroup = $("#deadlineRegisterGroup").val();
    newSemester.isConfirmationOfDevHeadNeeded = $("#isConfirmationOfDevHeadNeeded").prop("checked");
    newSemester.subjectMailTemplate = $('#subjectMailTemplate').val();
    newSemester.bodyMailTemplate = bodyMailEditor.getContent();
    AjaxCall('/SemesterManage/AddNewSemester', JSON.stringify(newSemester), 'POST').done(function (response) {
        if (response >= 1) {
            window.location.href = '/SemesterManage/SetupMajor';
        } else {
            alert('Add new semester error');
        }
    }).fail(function (error) {
        alert(errortatusText);
    });
})

//$('body').on('click', '#closesemesterbtn', function () {
//    $('<div></div>').dialog({
//        title: 'confirmation',
//        modal: true,
//        resizable: false,
//        buttons: {
//            'yes': function () {
//                $(this).dialog('close');
//                ajaxcall('/semestermanage/closesemestercurrent', 'post').done(function (response) {
//                    if (response == 1) {
//                        window.location.href = '/semestermanage/index';
//                    } else {
//                        alert('add new semester error');
//                    }
//                }).fail(function (error) {
//                    alert(error.statustext);
//                });
//            },
//            'no': function () {
//                $(this).dialog('close');
//            }
//        },
//        open: function () {
//            $(this).html('are you sure you want to close the semester nhi?');
//        }
//    });
//});


//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}
