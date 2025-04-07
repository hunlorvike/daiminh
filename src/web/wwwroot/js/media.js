$(document).ready(function () {
    const selectModal = document.getElementById('selectMediaModal');
    const selectGrid = $('#select-media-grid');
    const selectLoading = $('#select-media-loading-indicator');
    const selectEmpty = $('#select-media-empty-indicator');
    const selectBreadcrumbs = $('#select-media-breadcrumbs');
    let selectCurrentFolderId = null;

    // --- Function to open the selection modal ---
    // This function should be callable from other parts of your application
    // 'callback' is a function that will receive the selected file data
    // 'filterType' is optional (e.g., MediaType.Image)
    window.openMediaSelectionModal = function (callback, filterType = null) {
        window.mediaSelectionCallback = callback; // Store callback globally (or better, use event listeners/promises)
        // Load root items when modal opens, potentially applying filter
        loadSelectMediaItems(null, filterType);
        var modalInstance = bootstrap.Modal.getOrCreateInstance(selectModal);
        modalInstance.show();
    };

    // --- Function to load items into the SELECT modal ---
    function loadSelectMediaItems(folderId, mediaTypeFilter = null) {
        selectCurrentFolderId = folderId;
        selectLoading.show();
        selectEmpty.hide();
        selectGrid.empty();

        let apiUrl = `/api/admin/media/items?folderId=${folderId || ''}`;
        if (mediaTypeFilter) {
            apiUrl += `&mediaType=${mediaTypeFilter}`; // Append mediaType query param
        }

        $.ajax({
            url: apiUrl,
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                // Update Breadcrumbs
                selectBreadcrumbs.empty();
                result.breadcrumbs.forEach(function (crumb, index) {
                    // Use data-bs-target to keep modal open on navigation
                    if (index === result.breadcrumbs.length - 1) {
                        selectBreadcrumbs.append(`<li class="breadcrumb-item active" aria-current="page">${crumb.name}</li>`);
                    } else {
                        selectBreadcrumbs.append(`<li class="breadcrumb-item"><a href="#" class="select-folder-link" data-folder-id="${crumb.id || ''}">${crumb.name}</a></li>`);
                    }
                });

                // Update Grid Items
                if (result.items && result.items.length > 0) {
                    result.items.forEach(renderSelectMediaItem); // Use specific render function if needed
                    selectEmpty.hide();
                } else {
                    selectEmpty.show();
                }
            },
            error: function () {
                selectEmpty.show().text('Lỗi tải dữ liệu.');
            },
            complete: function () {
                selectLoading.hide();
            }
        });
    }

    // --- Function to render item in SELECT modal ---
    // (Similar to main render, but click behavior is different)
    function renderSelectMediaItem(item) {
        const thumbnailUrl = item.thumbnailUrl || '';
        // Slightly simplified render for selection
        const itemHtml = `
				 <div class="media-item ${item.isFolder ? 'select-folder-item' : 'select-file-item'}"
					  data-id="${item.id}" data-name="${item.name}" data-path="${item.filePath || ''}"
					  data-url="${thumbnailUrl}" data-alt="${item.altText || ''}" data-mime="${item.mimeType || ''}"
					  data-size="${item.fileSize || 0}" data-type="${item.mediaType || ''}">

					 <div class="media-item-thumbnail">
						${item.isFolder
                ? `<i class="icon-display ${item.displayIconClass}"></i>`
                : thumbnailUrl
                    ? `<img src="${thumbnailUrl}" alt="${item.altText || item.name}" loading="lazy" onerror="this.onerror=null; this.src='/img/placeholder.svg';">`
                    : `<i class="icon-display ${item.displayIconClass}"></i>`
            }
					</div>
					<div class="media-item-name" title="${item.name}">${item.name}</div>
				</div>
			`;
        selectGrid.append(itemHtml);
    }


    // --- Event Handlers for SELECT modal ---
    selectGrid.on('click', '.media-item', function (e) {
        const item = $(this);
        if (item.hasClass('select-folder-item')) {
            const folderId = item.data('id');
            loadSelectMediaItems(folderId); // Navigate within modal
        } else if (item.hasClass('select-file-item')) {
            // --- File Click Logic (Execute Callback) ---
            if (typeof window.mediaSelectionCallback === 'function') {
                const fileData = {
                    id: item.data('id'),
                    path: item.data('path'),
                    url: item.data('url'),
                    name: item.data('name'),
                    alt: item.data('alt'),
                    mime: item.data('mime'),
                    size: item.data('size'),
                    type: item.data('type')
                };
                window.mediaSelectionCallback(fileData); // Pass data back
                // Close the modal after selection
                var modalInstance = bootstrap.Modal.getInstance(selectModal);
                if (modalInstance) modalInstance.hide();
            } else {
                console.warn("Media selection callback not defined.");
            }
        }
    });

    // Breadcrumb navigation within SELECT modal
    selectBreadcrumbs.on('click', 'a.select-folder-link', function (e) {
        e.preventDefault();
        const folderId = $(this).data('folder-id') === '' ? null : $(this).data('folder-id');
        loadSelectMediaItems(folderId);
    });

    // Clear callback when modal is hidden
    $(selectModal).on('hidden.bs.modal', function () {
        window.mediaSelectionCallback = null; // Clean up global callback
        // Optional: Reset grid/breadcrumbs if desired
        // selectGrid.empty();
        // selectBreadcrumbs.empty();
    });

});