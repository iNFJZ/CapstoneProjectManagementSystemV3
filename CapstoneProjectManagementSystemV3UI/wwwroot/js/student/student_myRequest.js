const requestMain = document.querySelector('.requestMain');
const invitedMain = document.querySelector('.invitedMain');
const myRequestBtn = document.querySelector('.myRequestbtn');
const groupInvitedbtn = document.querySelector('.groupInvitedbtn');

const informGroupRequestAccepted = document.querySelector('.accp');
const informGroupRequestPending = document.querySelector('.pend');
const informGroupRequestRejected = document.querySelector('.reje');
const filterStatus = document.querySelector('.filterStatus');

myRequestBtn.addEventListener('click', () => {
    requestMain.classList.remove('hide--list');
    invitedMain.classList.add('hide--list');
    myRequestBtn.classList.add('active');
    groupInvitedbtn.classList.remove('active');
    filterStatus.classList.remove('hide-form');
})
groupInvitedbtn.addEventListener('click', () => {
    requestMain.classList.add('hide--list');
    invitedMain.classList.remove('hide--list');
    myRequestBtn.classList.remove('active');
    groupInvitedbtn.classList.add('active');
    filterStatus.classList.add('hide-form');
})


const dropdownStatus = document.querySelectorAll('.dropdownStatus');
const statusChoose = document.querySelector('.statusChoose');
const arr = document.querySelector('.arr');
const options = document.querySelectorAll('.statusChoose li');

dropdownStatus.forEach(dropdown => {
    const selected = dropdown.querySelector('.selected');
    dropdown.addEventListener('click', () => {
        arr.classList.toggle('active-arr');
        statusChoose.classList.toggle('show--status');
    });

    options.forEach(option => {
        option.addEventListener('click', () => {
            var status = option.innerText;
            if (status == "All Status") {
                informGroupRequestAccepted.classList.remove('hide-form');
                informGroupRequestPending.classList.remove('hide-form');
                informGroupRequestRejected.classList.remove('hide-form');
            } else if (status == "Pending") {
                informGroupRequestAccepted.classList.add('hide-form');
                informGroupRequestPending.classList.remove('hide-form');
                informGroupRequestRejected.classList.add('hide-form');
            } else if (status == "Accepted") {
                informGroupRequestAccepted.classList.remove('hide-form');
                informGroupRequestPending.classList.add('hide-form');
                informGroupRequestRejected.classList.add('hide-form');
            } else if (status == "Rejected") {
                informGroupRequestAccepted.classList.add('hide-form');
                informGroupRequestPending.classList.add('hide-form');
                informGroupRequestRejected.classList.remove('hide-form');
            }
            selected.innerText = option.innerText;
            arr.classList.remove('active-arr');
            statusChoose.classList.remove('show--status');
        })
    })

    document.addEventListener('click', function handleClickOutsideBox(event) {

        if (!dropdown.contains(event.target)) {
            arr.classList.remove('active-arr');
            statusChoose.classList.remove('show--status');
        }
    });

}) 

/*confirm popup for join button*/
$(document).ready(function () {
    var groupId;
    $('.acceptBtn').click(function () {
        groupId = this.id;
        AjaxCall('/MyRequest/CheckIfUserAlreadyHaveGroup', JSON.stringify(), "POST").done(function (response) {
            if (response == true) {
                //case already in group
                $('.showNotify2Form').toggle('hide-Notify2form');
            } else {
                //case not 
                $('.showJoinForm').toggle('hide-Joinform');
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
        
    });
    $('.noBtnJoin').click(function () {
        $('.showJoinForm').toggle('hide-Joinform');
    });
    $('.showJoinForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showJoinForm').toggle('hide-Joinform');
        }
    });
    $('.yesBtnJoin').click(function () {

        AjaxCall('/MyRequest/CheckIfGroupIsFull', JSON.stringify(groupId), "POST").done(function (response) {
            if (response == true) {
                //case full of member
                $('.showJoinForm').toggle('hide-Joinform');
                $('.showNotifyForm').toggle('hide-Notifyform');
            } else {
                //case not full
                var form = document.getElementById("form " + groupId);
                form.submit();
            }
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
});

/*confirm popup for cancel button*/
$(document).ready(function () {
    var groupId;
    $('.cancelBtn').click(function () {
        groupId = this.id;
        $('.showCancelForm').toggle('hide-Cancelform');
    });
    $('.noBtnCancel').click(function () {
        $('.showCancelForm').toggle('hide-Cancelform');
    });
    $('.showCancelForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showCancelForm').toggle('hide-Cancelform');
        }
    });
    $('.yesBtnCancel').click(function () {
        var form = document.getElementById("form2 " + groupId);
        form.submit();
    });
});

/*confirm popup for delete button*/
$(document).ready(function () {
    var groupId;
    $('.rejectBtn').click(function () {
        groupId = this.id;
        $('.showDeleteForm').toggle('hide-Deleteform');
    });
    $('.noBtnDelete').click(function () {
        $('.showDeleteForm').toggle('hide-Deleteform');
    });
    $('.showDeleteForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showDeleteForm').toggle('hide-Deleteform');
        }
    });
    $('.yesBtnDelete').click(function () {
        var form = document.getElementById("form2 " + groupId);
        form.submit();
    });
});
/*notify that goup is full of members*/
$(document).ready(function () {
    $('.showNotifyForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showNotifyForm').toggle('hide-Notifyform');
        }
    });
    $('.okBtn').click(function () {
        $('.showNotifyForm').toggle('hide-Notifyform');
    });
});
/*notify that user already have group*/
$(document).ready(function () {
    $('.showNotify2Form').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showNotify2Form').toggle('hide-Notify2form');
        }
    });
    $('.okBtn2').click(function () {
        $('.showNotify2Form').toggle('hide-Notify2form');
    });
});


// Ajax for Check if request is full of members or not
//function AjaxCall(url, data, type) {
//    return $.ajax({
//        url: url,
//        dataType: "json",
//        data: data,
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//    });
//}