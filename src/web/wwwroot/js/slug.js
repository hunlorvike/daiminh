const SlugManager = {
    generateVietnameseSlug: function (sourceString) {
        if (!sourceString) {
            return '';
        }

        let slug = sourceString.toString();
        slug = slug.toLowerCase();
        slug = slug.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
        slug = slug.replace(/đ/g, 'd');
        slug = slug.replace(/\s+/g, '-');
        slug = slug.replace(/[^\w-]+/g, '');
        slug = slug.replace(/-{2,}/g, '-');
        slug = slug.replace(/^-+/, '');
        slug = slug.replace(/-+$/, '');
        return slug;
    },

    init: function (options) {
        const nameElement = typeof options.nameInput === 'string' ? $(options.nameInput) : $(options.nameInput);
        const slugElement = typeof options.slugInput === 'string' ? $(options.slugInput) : $(options.slugInput);
        const buttonElement = options.generateButton
            ? (typeof options.generateButton === 'string' ? $(options.generateButton) : $(options.generateButton))
            : null;

        if (!nameElement.length || !slugElement.length) {
            console.warn('SlugManager: nameInput and slugInput are required.');
            return null;
        }

        let userModifiedSlug = false;

        const handleSlugInput = function () {
            userModifiedSlug = true;
        };

        const handleNameInput = function () {
            if (!userModifiedSlug) {
                slugElement.val(SlugManager.generateVietnameseSlug(nameElement.val()));
            }
        };

        const handleGenerateButton = function () {
            userModifiedSlug = false;
            slugElement.val(SlugManager.generateVietnameseSlug(nameElement.val()));
        };

        slugElement.on('input', handleSlugInput);
        nameElement.on('input', handleNameInput);
        if (buttonElement) {
            buttonElement.on('click', handleGenerateButton);
        }

        return {
            generateSlug: function () {
                userModifiedSlug = false;
                slugElement.val(SlugManager.generateVietnameseSlug(nameElement.val()));
            },
            reset: function () {
                userModifiedSlug = false;
                slugElement.val('');
            },
            destroy: function () {
                slugElement.off('input', handleSlugInput);
                nameElement.off('input', handleNameInput);
                if (buttonElement) {
                    buttonElement.off('click', handleGenerateButton);
                }
            }
        };
    }
};

// Usage in main script
/*$(document).ready(function () {
    // Example 1: Using IDs
    const slugManager = SlugManager.init({
        nameInput: '#Name',
        slugInput: '#Slug',
        generateButton: '#generateSlugButton'
    });

    // Example 2: Using DOM elements directly
    const nameInput = $('#Name');
    const slugInput = $('#Slug');
    const generateButton = $('#generateSlugButton');
    const slugManager = SlugManager.init({
        nameInput: nameInput,
        slugInput: slugInput,
        generateButton: generateButton
    });
});
*/