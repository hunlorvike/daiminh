/**
 * Manages account-specific functionality
 * @requires Daiminh.Core
 * @requires Daiminh.DataTable
 * @requires Daiminh.Form
 * @requires Daiminh.Modal
 */
jQuery(document).ready(function ($) {
    'use strict';

    // Get the existing DataTable instance if available
    const tableSelector = "#table";
    let dataTable;

    if ($.fn.dataTable.isDataTable(tableSelector)) {
        // If already initialized, just get the instance
        dataTable = $(tableSelector).DataTable();
    } else {
        // Initialize with custom configuration
        dataTable = Daiminh.DataTable.initialize(tableSelector, {
            columnDefs: [{targets: 0, orderable: true, searchable: true},
                {targets: 1, orderable: true, searchable: true},
                {targets: 2, orderable: true, searchable: true},
                {targets: 3, orderable: true, searchable: true},
                {targets: 4, orderable: false, searchable: false}],
        });
    }
});