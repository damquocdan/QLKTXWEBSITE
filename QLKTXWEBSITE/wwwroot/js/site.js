// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var btn_thong_bao = document.getElementById("button_thong_bao");
var noi_dung = document.getElementById("noi_dung");

var btn_user = document.getElementById("button_user");
var menu_user = document.getElementById("menu_user");

var display = document.querySelectorAll('*');

//Lặp qua mỗi phần tử và gán trình lắng nghe sự kiện click cho từng phần tử
//display.forEach(function (element) {
//    element.addEventListener("click", function () {
//        menu_user.style.display = "none";
//        noi_dung.style.display = "none";
//    });
//});

btn_thong_bao.addEventListener("click", function () {
    // Kiểm tra lớp .hienthi có hiển thị hay không
    if (noi_dung.style.display === "none" || noi_dung.style.display === "") {
        noi_dung.style.display = "block";
        menu_user.style.display = "none";// Hiển thị nếu đang ẩn
    } else {
        noi_dung.style.display = "none"; // Ẩn nếu đang hiển thị
    }
});

btn_user.addEventListener("click", function () {
    // Kiểm tra lớp .hienthi có hiển thị hay không
    if (menu_user.style.display === "none" || menu_user.style.display === "") {
        menu_user.style.display = "block";
        noi_dung.style.display = "none"; // Hiển thị nếu đang ẩn
    } else {
        menu_user.style.display = "none"; // Ẩn nếu đang hiển thị
    }
});

/*contact*/
