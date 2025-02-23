/**
 * Daiminh.Modal Module
 * Manages modal dialogs for create, edit, and delete operations
 * @requires Daiminh
 */
Daiminh.Modal = (($, Daiminh) => {
    'use strict';

    // Import from core module
    const Config = Daiminh.Config;
    const Utils = Daiminh.Utils;

    /**
     * Shows a Bootstrap modal by ID
     * @param {string} modalId - ID of the modal to show
     */
    const _showModal = (modalId) => {
        const $modal = $(modalId);

        if (!$modal.length) {
            console.error(`Modal ${modalId} not found`);
            return null;
        }

        // Get or create modal instance with configured options
        const instance = bootstrap.Modal.getOrCreateInstance($modal[0], {
            backdrop: Config.defaults.modal.backdrop
        });

        // Show the modal
        instance.show();
        return instance;
    };

    /**
     * Configure a modal action button handler
     * @param {Object} config - Configuration object
     * @param {string} config.buttonSelector - Selector for action button
     * @param {string} config.containerSelector - Selector for content container
     * @param {string} config.modalType - Type of modal (detail, create, edit, delete)
     */
    const _setupModalAction = (config) => {
        const {
            buttonSelector, containerSelector, modalType
        } = config;

        // Attach click handler to action button
        $(document).on('click', buttonSelector, async function (e) {
            e.preventDefault();
            e.stopPropagation();

            const $button = $(this);
            const $screen = $button.closest('.screen');
            const $container = $screen.find(containerSelector);

            // Get modal target and action URL
            const modalId = $button.data('bs-target');
            const targetUrl = $button.attr('formaction') || $button.attr('href');

            // Prevent double-clicks
            Utils.disableButton($button);

            try {
                // Load modal content via AJAX
                const response = await Utils.ajax({
                    url: targetUrl, method: 'GET', contentType: 'application/json; charset=UTF-8'
                }, null, null, `Error loading ${modalType} modal`);

                // Update container with loaded content and show modal
                $container.html(response);
                _showModal(modalId);
            } catch (error) {
                // Error handling already done in Utils.ajax
            }
        });
    };

    /**
     * Public API
     */
    return {
        /**
         * Initialize all modal handlers
         */
        init: () => {
            // Initialize modal action handlers
            Daiminh.Modal.initDetailModal();
            Daiminh.Modal.initCreateModal();
            Daiminh.Modal.initEditModal();
            Daiminh.Modal.initDeleteModal();
            Daiminh.Modal.initCloseHandler();
        },


        /**
         * Initialize detail modal handler
         */
        initDetailModal: () => {
            _setupModalAction({
                buttonSelector: Config.selectors.modalDetail,
                containerSelector: Config.selectors.detailContainer,
                modalType: 'detail'
            });
        },

        /**
         * Initialize create modal handler
         */
        initCreateModal: () => {
            _setupModalAction({
                buttonSelector: Config.selectors.modalCreate,
                containerSelector: Config.selectors.createContainer,
                modalType: 'create'
            });
        },

        /**
         * Initialize edit modal handler
         */
        initEditModal: () => {
            _setupModalAction({
                buttonSelector: Config.selectors.modalEdit,
                containerSelector: Config.selectors.editContainer,
                modalType: 'edit'
            });
        },

        /**
         * Initialize delete modal handler
         */
        initDeleteModal: () => {
            _setupModalAction({
                buttonSelector: Config.selectors.modalDelete,
                containerSelector: Config.selectors.deleteContainer,
                modalType: 'delete'
            });
        },

        /**
         * Initialize modal close handler
         */
        initCloseHandler: () => {
            // Handle modal close button clicks
            $(document).on('click', Config.selectors.modalClose, function (e) {
                e.preventDefault();
                e.stopPropagation();

                const $modal = $(this).closest('.modal');
                const $formContainer = $modal.closest(Config.selectors.formContainer);
                const instance = bootstrap.Modal.getInstance($modal[0]);

                if (instance) {
                    instance.hide();
                    $formContainer.html('');
                }
            });
        },

        /**
         * Show a modal programmatically
         * @param {string} modalId - ID of the modal to show
         */
        show: (modalId) => _showModal(modalId),

        /**
         * Hide a modal programmatically
         * @param {string} modalId - ID of the modal to hide
         */
        hide: (modalId) => {
            const $modal = $(modalId);

            if (!$modal.length) {
                console.error(`Modal ${modalId} not found`);
                return;
            }

            const instance = bootstrap.Modal.getInstance($modal[0]);

            if (instance) {
                instance.hide();
            }
        }
    };

})(jQuery, Daiminh);