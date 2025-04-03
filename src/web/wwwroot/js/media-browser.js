/**
 * Mở cửa sổ chọn media
 * @param {string} callbackFunction - Tên hàm callback sẽ được gọi khi người dùng chọn file
 * @param {string} fileType - Loại file cần lọc (image, video, document, v.v.)
 * @param {number} folderId - ID của thư mục mặc định (nếu có)
 */
function openMediaBrowser(callbackFunction, fileType, folderId) {
    // Xây dựng URL cho cửa sổ chọn media
    let url = '/Admin/Media/Browser?callback=' + encodeURIComponent(callbackFunction);

    // Thêm tham số fileType nếu có
    if (fileType) {
        url += '&fileType=' + encodeURIComponent(fileType);
    }

    // Thêm tham số folderId nếu có
    if (folderId) {
        url += '&folderId=' + encodeURIComponent(folderId);
    }

    // Mở cửa sổ mới với kích thước phù hợp
    const width = 1000;
    const height = 700;
    const left = (window.innerWidth - width) / 2;
    const top = (window.innerHeight - height) / 2;

    window.open(
        url,
        'MediaBrowser',
        `width=${width},height=${height},top=${top},left=${left},resizable=yes,scrollbars=yes,status=no,location=no,toolbar=no`
    );
}

/**
 * Thiết lập lắng nghe sự kiện postMessage từ cửa sổ chọn media
 * (Phương pháp thay thế nếu callback không hoạt động)
 * @param {function} callback - Hàm xử lý khi nhận được dữ liệu
 */
function setupMediaBrowserListener(callback) {
    window.addEventListener('message', function (event) {
        // Kiểm tra nguồn tin nhắn để đảm bảo an toàn
        // Trong môi trường production, bạn nên kiểm tra origin
        if (event.data && event.data.messageType === 'fileSelected') {
            callback(event.data.data);
        }
    });
}