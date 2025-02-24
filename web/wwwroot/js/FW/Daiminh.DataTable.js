/**
 * Daiminh.DataTable Module
 * Manages and initializes DataTables with standardized styling and behavior
 * @requires Daiminh
 */
Daiminh.DataTable = (($, Daiminh) => {
    'use strict';

    // Reference to core configuration
    const Config = Daiminh.Config;

    /**
     * Updates DataTable styling to match Bootstrap design
     * @param {Object} table - DataTable instance
     */
    const _applyBootstrapStyling = () => {
        const selectors = Config.selectors;

        // Style search input
        $(selectors.dtSearch)
            .addClass('d-flex align-items-center justify-content-end')
            .find('input')
            .addClass('form-control form-control-sm w-50');

        // Style length dropdown
        $(selectors.dtLength)
            .find('label')
            .addClass('w-50');

        // Style pagination
        $(selectors.pagingNav)
            .addClass('pagination justify-content-center m-0');

        // Add Bootstrap classes to pagination buttons
        const $pagingButtons = $(selectors.pagingButton);
        $pagingButtons.addClass('page-link');

        // Ensure buttons are properly wrapped in page-item elements
        $pagingButtons.each(function () {
            const $button = $(this);

            // Only wrap if not already wrapped
            if (!$button.parent().hasClass('page-item')) {
                $button.wrap('<li class="page-item"></li>');
            }

            // Add active/disabled classes
            if ($button.hasClass('current')) {
                $button.parent().addClass('active');
            }

            if ($button.hasClass('disabled')) {
                $button.parent().addClass('disabled');
            }
        });
    };

    /**
     * Public API
     */
    return {
        /**
         * Initialize a DataTable with standard configuration and custom options
         * @param {string|jQuery} selector - The table selector or element
         * @param {Object} customOptions - Optional configuration to override defaults
         * @returns {Object} DataTable instance
         */
        initialize: (selector, customOptions = {}) => {
            // Merge default options with custom options
            const options = {
                ...Config.defaults.dataTable,
                initComplete: _applyBootstrapStyling,
                drawCallback: _applyBootstrapStyling,
                ...customOptions
            };

            // Initialize DataTable with combined options
            return $(selector).DataTable(options);
        },

        /**
         * Refresh a DataTable with new data
         * @param {Object} table - DataTable instance
         */
        refresh: (table) => {
            if (table && typeof table.ajax === 'function') {
                table.ajax.reload();
            } else if (table) {
                table.draw();
            }
        },

        /**
         * Destroy a DataTable instance
         * @param {Object} table - DataTable instance
         */
        destroy: (table) => {
            if (table && typeof table.destroy === 'function') {
                table.destroy();
            }
        }
    };

})(jQuery, Daiminh);