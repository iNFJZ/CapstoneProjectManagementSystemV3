const menuMember = document.querySelectorAll('.menuMember');
menuMember.forEach(dropdown => {
    const showMenuBtn = dropdown.querySelector('.showMenu');
    const dropdownMenu = dropdown.querySelector('.menuMember__dropdown');

    showMenuBtn.addEventListener('click', () => {
        dropdownMenu.classList.toggle('show--menuMember__dropdown');
    });

    document.addEventListener('click', function handleClickOutsideBox(event) {

        if (!showMenuBtn.contains(event.target)) {
            dropdownMenu.classList.remove('show--menuMember__dropdown');
        }
    });
})

const informmemRequest = document.querySelectorAll('.informmemRequest');
informmemRequest.forEach(inform => {
    const basicInformMemRequest = inform.querySelector('.basicInformMemRequest');
    const desAboutYourSelf = inform.querySelector('.desAboutYourSelf');

    basicInformMemRequest.addEventListener('click', () => {
        desAboutYourSelf.classList.toggle('show--desAboutYourSelf');
    })
})

$(document).ready(function () {
    $('.submitFormRequest').click(function () {
        AjaxCall('/GroupDetails/CheckIfStudentAlreadyHaveGroup', JSON.stringify(null), "POST").done(function (response) {
            if (response == true) {
                $('.showDeniedRequestForm').toggle('hide-form');
            } else if (response == false) {
                $('.showForm').toggle('hide-form');
            };
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
    $('.discardBtn').click(function () {
        $('.showForm').toggle('hide-form');
    });
    $('.showForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showForm').toggle('hide-form');
        }
    });
    $('.okBtnCancelDeniedRequest').click(function () {
        $('.showDeniedRequestForm').toggle('hide-form');
    });
    $('.showDeniedRequestForm').click(function (e) {
        if (e.target === e.currentTarget) {
            $('.showDeniedRequestForm').toggle('hide-form');
        }
    });


    $('.addToFavorites').on("click", function () {
        var groupId = this.id;
        AjaxCall('/StudentHome/AddToFavorites', JSON.stringify(groupId), "POST").done(function (response) {
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Group added to your Bookmarks</p>',
                    timer: 1000,
                    showConfirmButton: false
                });
                $('.addToFavorites').toggle('hide-btn');
                $('.removeFromFavorites').toggle('hide-btn');
            } else if (response == false) {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">This group is already in your Bookmarks!</p>',
                    timer: 1000,
                    showConfirmButton: false
                });
            };           
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
    $('.removeFromFavorites').on("click", function () {
        var groupId = this.id;
        AjaxCall('/StudentHome/RemoveFromFavorites', JSON.stringify(groupId), "POST").done(function (response) {
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    title: '<p class="popupTitle">Remove Successfully!</p>',
                    timer: 1000,
                    showConfirmButton: false
                });
                $('.removeFromFavorites').toggle('hide-btn');
                $('.addToFavorites').toggle('hide-btn');
            } else if (response == false) {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">Error!</p>',
                    timer: 1000,
                    showConfirmButton: false
                });
            };
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

