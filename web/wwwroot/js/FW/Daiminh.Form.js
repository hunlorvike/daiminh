/**
 * Daiminh.Form Module
 * Handles form submissions and validation
 * @requires Daiminh.Core
 */
Daiminh.Form = (($, Daiminh) => {
    'use strict';

    // Import from core module
    const Config = Daiminh.Config;
    const Utils = Daiminh.Utils;

    /**
     * Process form submission using AJAX
     * @param {jQuery} $form - The form to submit
     * @returns {Promise} AJAX promise
     */
    const _submitForm = async ($form) => {
        const url = $form.attr('action');
        const method = $form.attr('method') || 'POST';
        const $container = $form.closest(Config.selectors.formContainer);
        const $submitBtn = $form.find(Config.selectors.submitButton);

        // Disable submit button during submission
        Utils.disableButton($submitBtn);

        try {
            // Make the AJAX request
            const response = await Utils.ajax({
                url: url,
                method: method,
                data: $form.serialize(),
                dataType: 'html'
            }, null, null, 'Form submission error');

            // Handle response based on validation status
            if (Utils.hasValidationErrors(response)) {
                _updateFormContent($container, response);
            } else {
                window.location.reload();
            }

            return response;
        } catch (error) {
            // Error handling already done in Utils.ajax
            throw error;
        } finally {
            // Re-enable the submit button
            $submitBtn.prop('disabled', false);
        }
    };

    /**
     * Update form content with response HTML
     * @param {jQuery} $container - The form container
     * @param {string} html - HTML content to update with
     */
    const _updateFormContent = ($container, html) => {
        $container.find('form').html($(html).find('form').html());
    };

    /**
     * Public API
     */
    return {
        /**
         * Initialize form handlers
         */
        init: () => {
            // Handle modal form submissions
            $(document).on('click', `.modal ${Config.selectors.submitButton}`, function (e) {
                e.preventDefault();
                e.stopPropagation();

                const $form = $(this).closest('form');
                _submitForm($form);
            });
        },

        /**
         * Manually submit a form
         * @param {jQuery|string} form - Form element or selector
         * @returns {Promise} AJAX promise
         */
        submit: (form) => {
            const $form = typeof form === 'string' ? $(form) : form;
            return _submitForm($form);
        },

        /**
         * Validate a form without submitting
         * @param {jQuery|string} form - Form element or selector
         * @returns {boolean} True if form is valid
         */
        validate: (form) => {
            const $form = typeof form === 'string' ? $(form) : form;

            // Check HTML5 validation if available
            if ($form[0] && typeof $form[0].checkValidity === 'function') {
                return $form[0].checkValidity();
            }

            // Fallback: check for visible validation error messages
            return $form.find('.field-validation-error:visible').length === 0;
        }
    };

})(jQuery, Daiminh);