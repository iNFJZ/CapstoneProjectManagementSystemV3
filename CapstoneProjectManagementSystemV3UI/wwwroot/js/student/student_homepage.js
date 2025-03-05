const a = document.querySelectorAll(".searchby a");
for (var i = 0, length = a.length; i < length; i++) {
    a[i].onclick = function () {
        var b = document.querySelector(".searchby .active");
        if (b) b.classList.remove("active");
        this.classList.add('active');
    };
}
/*drop down for each item*/
const showSearchGroup = document.querySelectorAll('.showSearchGroup');
showSearchGroup.forEach(showSearch => {
    const groupIdea = showSearch.querySelector('.groupIdea');
    const hideContent = showSearch.querySelector('.hide-content');

    groupIdea.addEventListener('click', () => {
        hideContent.classList.toggle('showHidenContent');
    })
});

$('#closeLinkUpdateProfile').click(function () {
    $(".notificationText").remove();
});
/*search*/
$('#btnSearch').click(function () {
    $('.pagingType').val("none");
    $('.recordNumber').val("1");
});

$("#searchInput").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $('.pagingType').val("none");
        $('.recordNumber').val("1");
        $('.main').submit();
    }
});

/*previous page*/
$('#previousBtn').click(function () {
    if (!($('.recordNumber').val() === "1")) {
        $('.pagingType').val("previous");
        $('.main').submit();
    }
});
/*next page*/
$('#nextbtn').click(function () {
    var startNum = parseInt($('.recordNumber').val());
    var numberOfRecordsPerPage = parseInt($('.numberOfRecordsPerPage').val());
    var countResult = parseInt($('.countResult').val());
    if ((startNum + numberOfRecordsPerPage - 1) < countResult) {
        $('.pagingType').val("next");
        $('.main').submit();
    }
});

const requestGroupBtns = document.querySelectorAll('.requestGroup');

requestGroupBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        btn.classList.toggle('icon-active');
    })

})

$('#searchbyIdeaBtn').on("click", function () {
    $('.searchDisplayShow').removeClass("hide-form");
    $('.favoriteDisplayShow').addClass("hide-form");
    /*drop down for each item*/
    const showSearchGroup = document.querySelectorAll('.showSearchGroup');
    showSearchGroup.forEach(showSearch => {
        const groupIdea = showSearch.querySelector('.groupIdea');
        const hideContent = showSearch.querySelector('.hide-content');

        groupIdea.addEventListener('click', () => {
            hideContent.classList.toggle('showHidenContent');
        })
    });
});
$('#searchbyFavoriteBtn').on("click", function () {
    $('.searchDisplayShow').remove("hide-form");
    $('.favoriteDisplayShow').add("hide-form");
    AjaxCall('/StudentHome/GetFavoritesList', JSON.stringify("favorites"), "POST").done(function (response) {
        var item = '';
        if (response.length != 0) {
            item += '<div class="title">\
                    <p> Name</p>\
                    <p>Keywords</p>\
                    <p>Action</p>\
                 </div >';
            for (var i = 0; i < response.length; i++) {
                item += '<div class="showSearchGroup" id="item_' + response[i].groupIdeaID + '">\
                         <div class="groupIdea">\
                                    <div class="name">\
                                        <img src="'+ response[i].avatar + '" alt="Avatar">\
                                        <div class="groupIntro">\
                                            <a href = "/GroupDetails/Index?id='+ response[i].groupIdeaID + '" class="nameProject" > ' + response[i].projectEnglishName + '</a >\
                                            <p class="createdby">Created by: <span>'+ response[i].leaderFullName + '</span></p>\
                                            <p class="dateCreate">'+ response[i].createdAt + '</p>\
                                        </div>\
                                    </div>\
                                    <div class="keywords">';
                for (var j = 0; j < response[i].projectTags.length; j++) {
                    item += '<p class="major">' + response[i].projectTags[j] + '</p>';
                };
                item += '</div>\
                                    <div class="hide-content">\
                                        <div class="introDetailsGroupIdea">\
                                            <div class="introDetailsGroupIdea--left">\
                                                <p>Professional: <span>'+ response[i].professionFullName + '</span></p>\
                                            </div>\
                                            <div class="introDetailsGroupIdea--right">\
                                                <p>Specialty: <span>'+ response[i].specialtyFullName + '</span></p>\
                                            </div>\
                                            <div class="description">\
                                                <p>Description</p>\
                                                <p>\
                                                    '+ response[i].description + '\
                                                </p>\
                                                <div class="showMember">\
                                                    <p>Available slot: <span>'+ response[i].availableSlot + '</span></p>\
                                                </div>\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="action">\
                                    <button type="button" class="removeFromFavorites" id="'+ response[i].groupIdeaID + '">Remove <i class="fa-solid fa-trash"></i></button>\
                                </div>\
                            </div>';
            }
        } else {
            item += '<div class="searchDisplayShow">\
                    <div class="ccsSvg">\
                <svg width = "322" height = "219" viewBox = "0 0 322 219" fill = "none" xmlns = "http://www.w3.org/2000/svg" >\
                            <g clip-path="url(#clip0_1053_7212)">\
                                <path d="M171.767 111.507L164.059 127.609H221.912L214.203 111.507L192.985 99.1929L171.767 111.507Z" fill="#BC9BDD" />\
                                <path d="M182.133 115.028C182.133 111.519 182.531 108.103 183.283 104.824L171.767 111.507L164.059 127.609H183.898C182.75 123.615 182.133 119.394 182.133 115.028V115.028Z" fill="#B08BD7" />\
                                <path d="M214.204 111.507C207.514 110.065 200.333 109.306 192.986 109.306C185.638 109.306 178.458 110.065 171.768 111.507L181.588 91.0001C183.696 86.5984 188.127 83.7974 192.986 83.7974C197.844 83.7974 202.275 86.5984 204.384 91.0003L214.204 111.507Z" fill="#5C586F" />\
                                <path d="M194.493 83.8897C193.997 83.8297 193.494 83.7974 192.986 83.7974C188.127 83.7974 183.696 86.5984 181.588 91.0003L171.768 111.507C175.206 110.766 178.776 110.207 182.43 109.837C183.567 99.8311 187.948 90.8173 194.493 83.8897V83.8897Z" fill="#4F4C5F" />\
                                <path d="M192.985 171.228C219.224 171.228 240.495 160.338 240.495 146.904C240.495 133.47 219.224 122.58 192.985 122.58C166.746 122.58 145.475 133.47 145.475 146.904C145.475 160.338 166.746 171.228 192.985 171.228Z" fill="#4F4C5F" />\
                                <path d="M230.036 177.141C239.138 177.141 246.517 185.251 246.517 195.254C246.517 197.808 246.034 200.238 245.165 202.441C248.411 203.069 250.864 205.939 250.864 209.388C250.864 211.585 249.093 213.367 246.909 213.367H230.036C220.934 213.367 213.555 205.257 213.555 195.254C213.555 185.25 220.934 177.141 230.036 177.141Z" fill="#8BDF9E" />\
                                <path d="M155.934 177.141C146.831 177.141 139.452 185.251 139.452 195.254C139.452 197.808 139.936 200.238 140.804 202.441C137.558 203.069 135.105 205.939 135.105 209.388C135.105 211.585 136.876 213.367 139.061 213.367H155.934C165.036 213.367 172.415 205.257 172.415 195.254C172.415 185.25 165.036 177.141 155.934 177.141V177.141Z" fill="#8BDF9E" />\
                                <path d="M242.261 184.613C242.261 184.097 242.248 183.582 242.229 183.068C239.214 179.427 234.868 177.141 230.036 177.141C220.934 177.141 213.555 185.251 213.555 195.254C213.555 204.719 220.162 212.483 228.582 213.292C229.602 212.419 230.612 211.482 231.612 210.476C238.479 203.567 242.261 194.383 242.261 184.613Z" fill="#73D98B" />\
                                <path d="M173.445 213.367C160.851 210.364 151.482 198.201 151.482 184.613C151.482 179.553 152.564 174.745 154.512 170.412C156.457 166.082 159.265 162.227 162.714 159.068C165.385 156.623 166.897 153.147 166.897 149.513V147.572C166.897 140.324 172.737 134.448 179.939 134.448C187.144 134.448 192.984 140.324 192.984 147.572C192.984 140.324 198.824 134.448 206.029 134.448C209.631 134.448 212.891 135.917 215.25 138.293C217.612 140.666 219.071 143.948 219.071 147.572V149.512C219.071 153.147 220.583 156.622 223.254 159.067C230.152 165.383 234.486 174.49 234.486 184.613C234.486 192.554 231.287 199.741 226.115 204.945C222.436 208.646 217.757 212.119 212.523 213.366L192.984 200.764L173.445 213.367Z" fill="#8BDF9E" />\
                                <path d="M176.504 157.508C175.43 157.508 174.561 156.633 174.561 155.553V152.367C174.561 151.287 175.43 150.412 176.504 150.412C177.578 150.412 178.448 151.287 178.448 152.367V155.553C178.448 156.633 177.577 157.508 176.504 157.508Z" fill="#423E4F" />\
                                <path d="M209.465 157.508C208.391 157.508 207.521 156.633 207.521 155.553V152.367C207.521 151.287 208.391 150.412 209.465 150.412C210.539 150.412 211.409 151.287 211.409 152.367V155.553C211.409 156.633 210.539 157.508 209.465 157.508Z" fill="#423E4F" />\
                                <path d="M192.985 153.503C195.212 153.503 197.329 154.473 198.795 156.165C199.5 156.979 199.416 158.214 198.607 158.923C197.798 159.633 196.57 159.548 195.864 158.734C195.137 157.895 194.088 157.413 192.985 157.413C191.882 157.413 190.832 157.895 190.105 158.734C189.4 159.548 188.172 159.633 187.363 158.923C186.553 158.214 186.47 156.979 187.174 156.165C188.64 154.473 190.758 153.503 192.985 153.503Z" fill="#423E4F" />\
                                <path d="M219.131 195.114C219.131 201.823 216.635 207.945 212.523 212.591C210.399 213.098 208.182 213.366 205.905 213.366H180.063C177.785 213.366 175.569 213.098 173.444 212.591C169.333 207.945 166.836 201.823 166.836 195.114C166.836 180.586 178.543 168.806 192.984 168.806C207.425 168.806 219.131 180.586 219.131 195.114Z" fill="#B9EBC2" />\
                                <path d="M213.792 198.93C219.703 198.93 224.494 203.751 224.494 209.697C224.494 211.724 222.861 213.366 220.847 213.366H206.737C204.723 213.366 203.09 211.723 203.09 209.697C203.09 203.751 207.881 198.93 213.792 198.93V198.93Z" fill="#73D98B" />\
                                <path d="M172.175 198.93C166.264 198.93 161.473 203.751 161.473 209.697C161.473 211.724 163.106 213.366 165.12 213.366H179.229C181.244 213.366 182.877 211.723 182.877 209.697C182.877 203.751 178.085 198.93 172.175 198.93V198.93Z" fill="#73D98B" />\
                                <path d="M161.245 182.417C161.245 162.995 171.273 145.93 186.403 136.175C184.497 135.078 182.292 134.448 179.939 134.448C172.737 134.448 166.897 140.324 166.897 147.572V149.513C166.897 153.147 165.385 156.623 162.714 159.068C159.265 162.227 156.457 166.082 154.512 170.412C152.564 174.745 151.482 179.553 151.482 184.614C151.482 196.945 159.199 208.102 170.039 212.304C164.478 203.704 161.245 193.441 161.245 182.417Z" fill="#73D98B" />\
                                <path d="M164.791 201.909C162.749 203.87 161.475 206.633 161.475 209.697C161.475 211.724 163.108 213.367 165.122 213.367H170.745C168.336 209.824 166.33 205.982 164.791 201.909Z" fill="#55D47B" />\
                                <path d="M214.655 164.309C214.655 165.735 213.5 166.891 212.075 166.891C210.65 166.891 209.494 165.735 209.494 164.309C209.494 162.883 210.65 161.727 212.075 161.727C213.5 161.727 214.655 162.883 214.655 164.309Z" fill="#7470E9" />\
                                <path d="M212.074 159.145L214.588 163.502H209.56L212.074 159.145Z" fill="#7470E9" />\
                            </g>\
                            <path d="M117.443 4.38H104.809C102.393 4.38 100.432 6.34224 100.432 8.76V210.24C100.432 212.658 102.393 214.62 104.809 214.62H117.443C119.859 214.62 121.821 212.658 121.821 210.24V8.76C121.821 6.33787 119.859 4.38 117.443 4.38Z" fill="#C29870" />\
                            <path d="M100.432 87.9154H121.821V121.225H100.432V87.9154Z" fill="#AD8764" />\
                            <path d="M100.432 40.8654H121.821V65.8489H100.432V40.8654Z" fill="#AD8764" />\
                            <path d="M198.525 35.3466L184.985 20.9145C183.917 19.7757 182.424 19.1274 180.861 19.1274H76.7886C73.6674 19.1274 71.1328 21.6591 71.1328 24.7864V53.655C71.1328 56.7779 73.6631 59.3139 76.7886 59.3139H180.861C182.424 59.3139 183.917 58.6657 184.985 57.5269L198.525 43.0948C200.569 40.9136 200.569 37.5234 198.525 35.3466V35.3466Z" fill="#D8A97D" />\
                            <path d="M142.383 72.0204H38.3107C36.7479 72.0204 35.2551 72.6686 34.187 73.8074L20.6472 88.2395C18.6072 90.4164 18.6072 93.8065 20.6472 95.9834L34.187 110.415C35.2551 111.554 36.7479 112.203 38.3107 112.203H142.383C145.505 112.203 148.039 109.671 148.039 106.544V77.675C148.039 74.552 145.505 72.0204 142.383 72.0204Z" fill="#D8A97D" />\
                            <path d="M111.128 45.0877C114.367 45.0877 116.994 42.46 116.994 39.2185C116.994 35.977 114.367 33.3493 111.128 33.3493C107.888 33.3493 105.262 35.977 105.262 39.2185C105.262 42.46 107.888 45.0877 111.128 45.0877Z" fill="#C29870" />\
                            <path d="M111.128 97.9806C114.367 97.9806 116.994 95.3529 116.994 92.1114C116.994 88.8699 114.367 86.2422 111.128 86.2422C107.888 86.2422 105.262 88.8699 105.262 92.1114C105.262 95.3529 107.888 97.9806 111.128 97.9806Z" fill="#C29870" />\
                            <path d="M150.389 204.415C150.389 210.065 145.136 214.62 138.657 214.62H84.8569C78.3781 214.62 73.125 210.065 73.125 204.415C73.125 196.881 79.8096 197.599 82.1822 189.47C83.4736 185.042 88.1445 181.858 93.5245 181.858C100.003 181.858 105.256 186.413 105.256 192.063C105.256 192.808 105.169 193.552 104.95 194.253C105.081 194.253 105.169 194.209 105.256 194.209C107.664 194.209 109.94 194.866 111.823 195.961C112.961 195.26 114.231 194.779 115.588 194.516C115.369 193.727 115.194 192.895 115.194 192.063C115.194 186.413 120.447 181.858 126.925 181.858C132.008 181.858 136.123 184.591 137.821 188.279C142.076 197.507 150.389 195.436 150.389 204.415Z" fill="#5BA658" />\
                            <path d="M150.389 204.415C150.389 210.065 145.136 214.62 138.657 214.62H84.8569C78.3781 214.62 73.125 210.065 73.125 204.415C73.125 203.67 73.2126 202.969 73.3877 202.268C74.5696 206.824 79.2536 210.24 84.8569 210.24H138.657C144.261 210.24 148.945 206.824 150.127 202.225C150.302 202.925 150.389 203.67 150.389 204.415Z" fill="#52954F" />\
                            <g clip-path="url(#clip1_1053_7212)">\
                                <path d="M265.006 208.203L281.528 144.619C281.676 144.05 282.19 143.653 282.777 143.653H291.104C291.692 143.653 292.205 144.05 292.353 144.619L308.876 208.203" fill="#F29C1F" />\
                                <path d="M285.963 143.653H282.779C282.191 143.653 281.678 144.05 281.53 144.619L265.008 208.203H281.781L285.669 144.619C285.704 144.05 285.825 143.653 285.963 143.653Z" fill="#E57E25" />\
                                <path d="M315.328 208.203H258.556C257.843 208.203 257.266 208.781 257.266 209.494V212.076C257.266 212.789 257.843 213.367 258.556 213.367H315.328C316.041 213.367 316.619 212.789 316.619 212.076V209.494C316.619 208.78 316.041 208.203 315.328 208.203Z" fill="#E57E25" />\
                                <path d="M279.2 212.076V209.494C279.2 208.781 279.778 208.203 280.491 208.203H258.556C257.843 208.203 257.266 208.781 257.266 209.494V212.076C257.266 212.789 257.843 213.367 258.556 213.367H280.491C279.778 213.367 279.2 212.789 279.2 212.076Z" fill="#DB7721" />\
                                <path d="M268.361 195.293H305.521L303.509 187.547H270.374L268.361 195.293Z" fill="#E6E7E8" />\
                                <path d="M275.07 169.473H298.812L296.799 161.727H277.083L275.07 169.473Z" fill="#E6E7E8" />\
                                <path d="M275.07 169.473H284.148L284.622 161.727H277.083L275.07 169.473Z" fill="#C7CFE2" />\
                                <path d="M268.361 195.293H282.57L283.043 187.547H270.374L268.361 195.293Z" fill="#C7CFE2" />\
                            </g>\
                            <defs>\
                                <clipPath id="clip0_1053_7212">\
                                    <rect width="128.794" height="129.569" fill="white" transform="translate(128.588 83.7974)" />\
                                </clipPath>\
                                <clipPath id="clip1_1053_7212">\
                                    <rect width="69.6754" height="69.7138" fill="white" transform="translate(252.104 143.653)" />\
                                </clipPath>\
                            </defs>\
                        </svg >\
                <p>No Bookmarks Yet!</p>\
                    </div>\
            </div>';
        }
        $('.favoriteDisplayShow').empty();
        $('.favoriteDisplayShow').append(item);
        /*drop down for each item*/
        const showSearchGroup = document.querySelectorAll('.showSearchGroup');
        showSearchGroup.forEach(showSearch => {
            const groupIdea = showSearch.querySelector('.groupIdea');
            const hideContent = showSearch.querySelector('.hide-content');

            groupIdea.addEventListener('click', () => {
                hideContent.classList.toggle('showHidenContent');
            })
        });
        /*remove bookmarks button*/
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
                    document.getElementById("item_" + groupId).remove();
                } else if (response == false) {
                    Swal.fire({
                        icon: 'error',
                        title: '<p class="popupTitle">Error!</p>',
                        timer: 1000,
                        showConfirmButton: false
                    });
                }
            }).fail(function (error) {
                alert(error.StatusText);
            });
        });
    }).fail(function (error) {
        alert(error.StatusText);
    });
    $('.searchDisplayShow').addClass("hide-form");
    $('.favoriteDisplayShow').removeClass("hide-form");
});

$(function () {
    // Ajax for Flter Search
    $('#professionDDL').on("change", function () {
        var profession = $('#professionDDL').val();
        AjaxCall('/StudentHome/GetCorrespondingSpecialty', JSON.stringify(profession), "POST").done(function (response) {
            $('#specialtyDDL').html('');
            var options = '';
            options += '<option value="0">Specialty</option>';
            if (response != null) {
                for (var i = 0; i < response.length; i++) {
                    var specialty = response[i]
                    options += '<option value="' + specialty.specialtyID + '">' + specialty.specialtyFullName + '</option>';
                }
            }
            $('#specialtyDDL').append(options);
        }).fail(function (error) {
            alert(error.StatusText);
        });
    });
    /*bookmarks button*/
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
            } else if (response == false) {
                Swal.fire({
                    icon: 'error',
                    title: '<p class="popupTitle">This group is already in your Bookmarks!</p>',
                    timer: 1000,
                    showConfirmButton: false
                });
            }
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
