/**
 * Daiminh Module
 * Core application functionality and shared utilities
 */
const Daiminh = (($) => {
    'use strict';

    /**
     * Configuration settings
     * Centralized application configuration
     */
    const Config = {
        selectors: {
            // DataTable selectors
            dataTable: '#table',
            dtSearch: '.dt-search',
            dtLength: '.dt-length',
            dtColumnTitle: '.dt-column-title',
            pagingNav: '.dt-paging nav',
            pagingButton: '.dt-paging-button',

            // Form selectors
            formContainer: '.dm-form',
            submitButton: '.command-submit',

            // Modal selectors
            modalDetail: '.command-modal-detail',
            modalCreate: '.command-modal-create',
            modalEdit: '.command-modal-edit',
            modalDelete: '.command-modal-delete',
            modalClose: '.command-modal-close',

            // Container selectors
            detailContainer: '.form-details-container',
            createContainer: '.form-create-container',
            editContainer: '.form-edit-container',
            deleteContainer: '.form-delete-container'
        },

        // Default options for various components
        defaults: {
            // DataTable default options
            dataTable: {
                paging: true,
                searching: true,
                searchDelay: 500,
                info: true,
                lengthChange: true,
                ordering: true,
                pageLength: 10,
                lengthMenu: [[8, 25, 50, 100], [8, 25, 50, 100]],

                // DataTable DOM structure
                dom: '<"card-header d-flex justify-content-between align-items-center"<"col-md-6"l><"col-md-6"f>>' + '<"table-responsive"t>' + '<"card-footer d-flex justify-content-between align-items-center"ip>',

                // DataTable language settings
                language: {
                    search: "",
                    searchPlaceholder: "Tìm kiếm...",
                    lengthMenu: `<span class="text-secondary">Hiển thị</span>
                        <select class="form-select form-select-sm d-inline-block w-50">
                            <option value="10">10</option>
                            <option value="15">15</option>
                            <option value="20">20</option>
                        </select>`,
                    info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
                    infoEmpty: "Hiển thị 0 đến 0 của 0 mục",
                    zeroRecords: "Không có dữ liệu nào khớp",
                    infoFiltered: "(lọc từ tổng số _MAX_ mục)",
                    paginate: {
                        first: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M11 7l-5 5l5 5" /><path d="M17 7l-5 5l5 5" /></svg>',
                        previous: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M15 6l-6 6l6 6" /></svg>',
                        next: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M9 6l6 6l-6 6" /></svg>',
                        last: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M7 7l5 5l-5 5" /><path d="M13 7l5 5l-5 5" /></svg>'
                    }
                }
            },

            // Modal defaults
            modal: {
                backdrop: true, disableTimeout: 500
            }
        }
    };

    /**
     * Utility functions
     * Reusable helpers for common tasks
     */
    const Utils = {
        /**
         * Temporarily disable a button to prevent double-clicks
         * @param {jQuery} $button - Button element to disable
         * @param {number} timeout - Milliseconds to keep disabled
         */
        disableButton: ($button, timeout = Config.defaults.modal.disableTimeout) => {
            $button.prop('disabled', true);
            setTimeout(() => $button.prop('disabled', false), timeout);
        },

        /**
         * Make an AJAX request with standardized error handling
         * @param {Object} options - jQuery AJAX options
         * @param {Function} onSuccess - Success callback
         * @param {Function} onError - Error callback
         * @param {string} errorMessage - User-friendly error message
         */
        ajax: async (options, onSuccess, onError, errorMessage = 'An error occurred') => {
            try {
                const response = await $.ajax(options);
                if (onSuccess && typeof onSuccess === 'function') {
                    onSuccess(response);
                }
                return response;
            } catch (error) {
                console.error(errorMessage, error);
                if (onError && typeof onError === 'function') {
                    onError(error);
                }
                throw error;
            }
        },

        /**
         * Check if a response contains form validation errors
         * @param {string} response - HTML response to check
         * @returns {boolean} True if validation errors exist
         */
        hasValidationErrors: (response) => {
            return response.includes('field-validation-error');
        },

        /**
         * Generate a URL-friendly slug from a string
         * @param {string} text - The input text
         * @returns {string} The generated slug
         */
        generateSlug: (text) => {
            return text
                .toLowerCase()
                .trim()
                .normalize("NFD")
                .replace(/[\u0300-\u036f]/g, "")
                .replace(/đ/g, "d")
                .replace(/[^a-z0-9\s-]/g, "")
                .replace(/\s+/g, "-")
                .replace(/-+/g, "-");
        }
    };

    // Return public API
    return {
        Config: Config, Utils: Utils
    };

})(jQuery);