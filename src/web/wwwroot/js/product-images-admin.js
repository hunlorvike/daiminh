const ProductImageManager = {
    containerSelector: '#product-images-container',
    itemSelector: '.product-image-item',
    templateSelector: '#product-image-template',
    addButtonSelector: '#add-product-image-btn',
    removeButtonSelector: '.remove-product-image-btn',
    selectMediaButtonSelector: '.select-media-button',
    mainImageCheckboxSelector: '.main-image-checkbox',
    imageOrderInputSelector: '.image-order-index',
    noImagesPlaceholderSelector: '#no-images-placeholder',

    init() {
        this.attachEvents();
        this.refresh();
    },

    attachEvents() {
        $(document)
            .off('click', this.addButtonSelector)
            .on('click', this.addButtonSelector, this.handleAddImage.bind(this))
            .off('click', this.removeButtonSelector)
            .on('click', this.removeButtonSelector, this.handleRemoveImage.bind(this))
            .off('click', `${this.containerSelector} ${this.selectMediaButtonSelector}`)
            .on('click', `${this.containerSelector} ${this.selectMediaButtonSelector}`, this.handleSelectMedia.bind(this))
            .off('change', `${this.containerSelector} ${this.mainImageCheckboxSelector}`)
            .on('change', `${this.containerSelector} ${this.mainImageCheckboxSelector}`, this.handleMainImageChange.bind(this));
    },

    handleAddImage(event) {
        event.preventDefault();
        event.stopPropagation();

        const $container = $(this.containerSelector);
        const $template = $(this.templateSelector);

        if (!$template.length) {
            console.error('Product image template not found.');
            return;
        }

        const currentItems = $container.find(this.itemSelector + ':visible');
        const newIndex = currentItems.length;

        const $newItem = $template.clone();
        $newItem.removeAttr('id');
        $newItem.removeClass('d-none').show();

        const newItemHtml = $newItem.prop('outerHTML').replace(/__INDEX__/g, newIndex);
        const $finalItem = $(newItemHtml);

        if (newIndex === 0) {
            $finalItem.find('.main-image-checkbox').prop('checked', true).val('true');
        } else {
            $finalItem.find('.main-image-checkbox').val('false');
        }

        $container.append($finalItem);

        $(this.noImagesPlaceholderSelector).hide();

        this.refresh();

        console.log(`Added new image item with index: ${newIndex}`);
    },

    handleRemoveImage(event) {
        event.preventDefault();
        event.stopPropagation();

        const $item = $(event.currentTarget).closest(this.itemSelector);

        if ($item.length === 0) {
            console.error('Could not find image item to remove');
            return;
        }

        const $mainCheckbox = $item.find(this.mainImageCheckboxSelector);
        const wasMainImage = $mainCheckbox.is(':checked');

        $item.remove();

        if (wasMainImage) {
            const $firstRemaining = $(this.containerSelector).find(this.itemSelector + ':visible').first();
            if ($firstRemaining.length) {
                $firstRemaining.find(this.mainImageCheckboxSelector).prop('checked', true).prev('input[type="hidden"]').val('true');
            }
        }

        const remainingItems = $(this.containerSelector).find(this.itemSelector + ':visible');
        if (remainingItems.length === 0) {
            $(this.noImagesPlaceholderSelector).show();
        }

        this.refresh();

        console.log('Removed image item');
    },

    handleSelectMedia(event) {
        event.preventDefault();

        const $button = $(event.currentTarget);
        const $item = $button.closest(this.itemSelector);
        const $targetInput = $item.find('.media-url-input');
        const $previewImg = $item.find('.product-image-preview');

        if (typeof window.openMediaModal === 'function') {
            window.openMediaModal($targetInput, selectedFile => {
                if (selectedFile && selectedFile.url) {
                    $targetInput.val(selectedFile.url).trigger('change');
                    console.log($previewImg)
                    $previewImg.attr('src', selectedFile.url);
                }
            });
        } else {
            console.error('openMediaModal function is not defined.');
        }
    },

    handleMainImageChange(event) {
        const $checkbox = $(event.currentTarget);

        if ($checkbox.is(':checked')) {
            $(this.containerSelector)
                .find(this.mainImageCheckboxSelector)
                .not($checkbox)
                .prop('checked', false);
            $checkbox.val('true');
        } else {
            const $checkedBoxes = $(this.containerSelector).find(this.mainImageCheckboxSelector + ':checked');
            if ($checkedBoxes.length === 0) {
                const $firstCheckbox = $(this.containerSelector).find(this.itemSelector + ':visible').first().find(this.mainImageCheckboxSelector);
                if ($firstCheckbox.length) {
                    $firstCheckbox.prop('checked', true);
                }
            }
            $checkbox.val('false');
        }
    },

    refresh() {
        this.updateOrderIndexes();
        this.ensureSingleMainImage();
    },

    updateOrderIndexes() {
        const $visibleItems = $(this.containerSelector).find(this.itemSelector + ':visible');

        $visibleItems.each((index, element) => {
            const $item = $(element);

            $item.find(this.imageOrderInputSelector).val(index);

            $item.find('.image-number').text(index + 1);

            $item.find('input, select, textarea').each(function () {
                const $input = $(this);
                const name = $input.attr('name');
                if (name && name.includes('Images[')) {
                    const newName = name.replace(/Images\[\d+\]/g, `Images[${index}]`);
                    $input.attr('name', newName);
                }
            });
        });
    },

    ensureSingleMainImage() {
        const $visibleItems = $(this.containerSelector).find(this.itemSelector + ':visible');

        if ($visibleItems.length === 0) {
            return;
        }

        const $mainCheckboxes = $visibleItems.find(this.mainImageCheckboxSelector + ':checked');

        if ($mainCheckboxes.length > 1) {
            $mainCheckboxes.not(':first').prop('checked', false);
        } else if ($mainCheckboxes.length === 0) {
            $visibleItems.first().find(this.mainImageCheckboxSelector).prop('checked', true);
        }
    },

    getImageCount() {
        return $(this.containerSelector).find(this.itemSelector + ':visible').length;
    },

    validateImages() {
        const $visibleItems = $(this.containerSelector).find(this.itemSelector + ':visible');
        let isValid = true;

        $visibleItems.each(function () {
            const $item = $(this);
            const imageUrl = $item.find('.media-url-input').val();

            if (!imageUrl || imageUrl.trim() === '') {
                isValid = false;
                $item.addClass('border-danger');
            } else {
                $item.removeClass('border-danger');
            }
        });

        return isValid;
    }
};

$(function () {
    ProductImageManager.init();
});