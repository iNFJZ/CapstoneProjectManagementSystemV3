
var myFunction = function () {
    var professionId = $('#professionalfilter').val();
    $('#specialtyfilter').html('');
    if (professionId == 0) {
        $('#specialtyfilter').append('<option value="0">Specialty</option>')
    } else {
        AjaxCall('/StudentProfile/GetSpecialtyByProfessionId?professionId=' + professionId, 'POST')
            .done(function (response) {
                AjaxCall('/StudentProfile/GetSpecialtyIdOfStudent', 'POST').done(function (specialtyId) {
                    var specialties = '';
                    for (var i = 0; i < response.length; i++) {
                        if (specialtyId == response[i].specialtyID)
                            specialties += ' <option value="' + response[i].specialtyID + '" selected>' + response[i].specialtyFullName + '</option>';
                        else
                            specialties += ' <option value="' + response[i].specialtyID + '">' + response[i].specialtyFullName + '</option>';
                    }
                    $('#specialtyfilter').append(specialties);
                }).fail(function (error) {
                    alert(error.StatusText);
                });
            }).fail(function (error) {
                alert(error.StatusText);
            });
    }
}

$('#professionalfilter')
    .ready(myFunction)
    .change(myFunction);
