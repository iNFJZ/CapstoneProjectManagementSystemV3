let links = document.querySelectorAll(".links a");
let bodyID = document.querySelector(".homepage").id;

for (let link of links) {
    if (link.dataset.active == bodyID) {
        link.classList.add("active");
    }
}