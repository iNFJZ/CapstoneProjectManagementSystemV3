/*document.addEventListener("DOMContentLoaded", function () {
    var modal = document.getElementById("myModal");
    var closeBtn = document.getElementsByClassName("close")[0];

    closeBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });

    window.addEventListener("click", function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    });
});*/

/*document.addEventListener("DOMContentLoaded", function () {
    var viewNews = document.getElementById("viewNews");
    var pageIndex = parseInt(viewNews.dataset.pageIndex);
    var isFirstPage = pageIndex === 1;
    if (isFirstPage) {
        var modal = document.getElementById("myModal");
        modal.style.display = "block";
    }
});*/


//setURL
function setUrlParam(key, value) {
    const urlObject = new URL(window.location.href);
    urlObject.searchParams.set(key, value);
    return urlObject.toString();
}

//Paging
function pagination(page, totalPage, gaps) {
    let pagingResult = "";
    let pageIndex = page;

    if (totalPage <= 1) {
        return;
    }
    pagingResult += `<a href="${setUrlParam("page", 1)}"  style="margin-right: 10px; padding: 10px; color: #7470E9;background-color: rgba(116, 112, 233, 0.2);
                                                            border-radius: 8px; text-decoration: none;">First</a> `;

    for (let i = pageIndex - gaps; i < pageIndex; i++) {
        if (i > 0)
            pagingResult += `<a href="${setUrlParam("page", i)}" style="margin-right: 10px; padding: 10px; color: #7470E9;background-color: rgba(116, 112, 233, 0.2);
                                                            border-radius: 8px; text-decoration: none;">${i}</a>`;
    }

    pagingResult += `<a style="margin-right: 10px; padding: 10px; color: #7470E9; font-weight: bold; background-color: rgba(116, 112, 233, 0.2);
                                                            border-radius: 8px; text-decoration: none;">${pageIndex}</a>`;

    for (let i = pageIndex + 1; i <= pageIndex + gaps; i++) {
        if (i <= totalPage)
            pagingResult += `<a href="${setUrlParam("page", i)}" style="margin-right: 10px; padding: 10px; color: #7470E9;background-color: rgba(116, 112, 233, 0.2);
                                                            border-radius: 8px; text-decoration: none;">${i}</a>`;
    }
    pagingResult += ` <a href="${setUrlParam("page", totalPage)}" style="margin-right: 10px; padding: 10px; color: #7470E9;background-color: rgba(116, 112, 233, 0.2);
                                                            border-radius: 8px; text-decoration: none;">Last</a>`;

    document.getElementById("pagination").innerHTML = `${pagingResult}`;
}