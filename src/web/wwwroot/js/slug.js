/**
 * 
 * @param {string} sourceString The string to be converted to a slug.   
 * @returns {string} The slugified string.  
 */
function generateVietnameseSlug(sourceString) {
    if (!sourceString) {
        return '';
    };

    let slug = sourceString.toString(); // Ensure it's a string

    // 1. Convert to lowercase  
    slug = slug.toLowerCase();

    // 2. Normalize Unicode (NFD) and remove diacritics (accents)
    // \u0300-\u036f is a range of combining diacritical marks in Unicode.
    slug = slug.normalize('NFD').replace(/[\u0300-\u036f]/g, '');

    // 3. Convert Vietnamese 'đ/Đ' to 'd' 
    slug = slug.replace(/đ/g, 'd');

    // 4. Replace spaces and consecutive spaces with a single hyphen
    slug = slug.replace(/\s+/g, '-');

    // 5. Remove all characters that are not letters, numbers, or hyphens
    slug = slug.replace(/[^\w\-]+/g, '');

    // 6. Replace multiple consecutive hyphens with a single hyphen
    slug = slug.replace(/\-\-+/g, '-');

    // 7. Trim hyphens from the start of the string
    slug = slug.replace(/^-+/, '');

    // 8. Trim hyphens from the end of the string
    slug = slug.replace(/-+$/, '');

    // 9. Return the slug
    return slug;
}