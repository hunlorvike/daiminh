﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function scrollToTop() {
    console.log(window.pageYOffset);
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}