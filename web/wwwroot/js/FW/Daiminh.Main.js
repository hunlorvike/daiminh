/**
 * Daiminh.Main Module
 * Application initialization and bootstrap
 * @requires Daiminh
 * @requires Daiminh.DataTable
 * @requires Daiminh.Form
 * @requires Daiminh.Modal
 */
Daiminh.Main = (($, Daiminh) => {
    'use strict';

    // Reference to core configuration
    const Config = Daiminh.Config;

    // Store initialized components
    const _instances = {
        dataTables: {}
    };

    /**
     * Initialize application components
     */
    const init = () => {
        // Initialize DataTables
        _initDataTables();

        // Initialize form handlers
        Daiminh.Form.init();

        // Initialize modal handlers
        Daiminh.Modal.init();

        console.log('Application initialized successfully');
    };

    /**
     * Initialize DataTables based on configuration
     */
    const _initDataTables = () => {
        // Initialize main data table
        _instances.dataTables.main = Daiminh.DataTable.initialize(Config.selectors.dataTable);

        /**
         * To initialize additional tables, uncomment and modify:
         *
         * const customTable = Daiminh.DataTable.initialize('#custom-table', {
         *     pageLength: 25,
         *     ordering: false,
         *     // Other custom options
         * });
         * _instances.dataTables.custom = customTable;
         */
    };

    /**
     * Get initialized component instances
     * @returns {Object} Component instances
     */
    const getInstances = () => {
        return _instances;
    };

    /**
     * Public API
     */
    return {
        init: init, getInstances: getInstances
    };

})(jQuery, Daiminh);

// Initialize application when document is ready
jQuery(document).ready(function () {
    Daiminh.Main.init();
});