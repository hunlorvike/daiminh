/**
 * Manages account-specific functionality
 * @requires Daiminh.Core
 * @requires Daiminh.DataTable
 * @requires Daiminh.Form
 * @requires Daiminh.Modal
 */
jQuery(document).ready(function ($) {
    'use strict';

    const tableSelector = "#table";
    let dataTable;

    if ($.fn.dataTable.isDataTable(tableSelector)) {
        dataTable = $(tableSelector).DataTable();
    } else {
        dataTable = Daiminh.DataTable.initialize(tableSelector, {
            columnDefs: [{targets: 0, orderable: true, searchable: true},
                {targets: 1, orderable: true, searchable: true},
                {targets: 2, orderable: true, searchable: true},
                {targets: 3, orderable: false, searchable: false}],

        });
    }
});