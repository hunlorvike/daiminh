/**
 * Manages account-specific functionality
 * @requires Daiminh
 * @requires Daiminh.DataTable
 * @requires Daiminh.Form
 * @requires Daiminh.Modal
 */
jQuery(document).ready(function ($) {
    'use strict';

    $(document).on('input', '#Name', function (e) {
        e.preventDefault();
        e.stopPropagation();

        $('#Slug').val(Daiminh.Utils.generateSlug($(this).val()));
    })
});