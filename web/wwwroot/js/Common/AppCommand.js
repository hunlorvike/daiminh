$(document).on('click', '.command-modal-edit', async function (e) {
    e.preventDefault();
    e.stopPropagation();

    const $button = $(this), $container = $button.closest('.screen'),
        $formContainer = $container.find('.form-edit-container');

    const MODAL_ID = $button.data('bs-target'), TARGET_URL = $button.attr('formaction');

    $button.prop('disabled', true);

    try {
        let response = await $.get(TARGET_URL, {contentType: 'application/json; charset=UTF-8'});
        $formContainer.html(response);

        if (MODAL_ID && $(MODAL_ID).length) {
            let modalInstance = bootstrap.Modal.getOrCreateInstance(MODAL_ID);
            modalInstance.show();
        } else {
            console.error(`Modal with ID ${MODAL_ID} not found.`);
        }
    } catch (error) {
        console.error('Error loading modal content:', error);
    } finally {
        setTimeout(() => {
            $button.prop('disabled', false);
        }, 500);
    }
});


$(document).on('click', '.command-modal-close', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const $button = $(this), $modal = $button.closest('.modal'),
        $formContainer = $modal.closest('.form-edit-container');

    if ($modal.length) {
        const modalInstance = bootstrap.Modal.getInstance($modal[0]);
        if (modalInstance) {
            $modal.on('hidden.bs.modal', function () {
                $formContainer.html('');
            });

            modalInstance.hide();
        }
    }
});