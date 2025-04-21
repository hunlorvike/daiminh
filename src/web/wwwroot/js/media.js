var mediaManager = {
    $modal: null,
    $modalBodyContent: null,
    $modalBreadcrumbs: null,
    $modalFolders: null,
    $modalFiles: null,
    $modalMediaTypeFilter: null,
    $modalUploadArea: null,
    $modalSelectFileBtn: null,
    $modalSelectedFileInfo: null,
    $modalSelectedFileName: null,
    $modalLoadingIndicator: null,
    $modalEmptyIndicator: null,

    currentFolderId: null,
    antiForgeryToken: '',
    selectFileCallback: null,

    // --- Initialization ---
    init: function () {
        this.$modal = $('#selectMediaModal');
        if (this.$modal.length === 0) {
            console.error("Media modal element #selectMediaModal not found.");
            return;
        }

        this.$modalBodyContent = $('#select-media-modal-body-content');
        this.$modalSelectFileBtn = $('#modal-select-file-btn');
        this.$modalSelectedFileInfo = $('#modal-selected-file-info');
        this.$modalSelectedFileName = $('#selected-file-name');

        // Event listeners for modal show/hide
        this.$modal.on('show.bs.modal', this.handleModalShow.bind(this));
        this.$modal.on('hidden.bs.modal', this.handleModalHide.bind(this));

        // Event listener for the Select button in the modal footer
        this.$modalSelectFileBtn.on('click', this.handleModalSelectFile.bind(this));

        // Event delegation for folder clicks (inside the dynamically loaded content)
        this.$modalBodyContent.on('click', '.media-folder-link', this.handleFolderClick.bind(this));

        // Event delegation for breadcrumb clicks
        this.$modalBodyContent.on('click', '#modal-media-breadcrumbs a', this.handleBreadcrumbClick.bind(this));

        // Event delegation for file selection clicks
        this.$modalBodyContent.on('click', '.media-file-item', this.handleFileClick.bind(this));

        // Event delegation for filter change
        this.$modalBodyContent.on('change', '#modal-media-type-filter', this.handleFilterChange.bind(this));

        // Event delegation for Create Folder button
        this.$modalBodyContent.on('click', '#create-folder-btn', this.handleCreateFolderClick.bind(this));

        // Event delegation for Toggle Upload Area button
        this.$modalBodyContent.on('click', '#toggle-upload-area', this.handleToggleUploadArea.bind(this));

        // Event delegation for Delete File button
        this.$modalBodyContent.on('click', '.delete-file-btn', this.handleDeleteFileClick.bind(this));

        // Event delegation for Delete Folder button
        this.$modalBodyContent.on('click', '.delete-folder-btn', this.handleDeleteFolderClick.bind(this));


        this.initDropzone(); 
    },

    initDropzone: function () {
        // Ensure Dropzone is only initialized once and target element exists
        const dropzoneElementId = 'modal-dropzone';

        // Check if Dropzone is already attached
        if ($('#' + dropzoneElementId).hasClass('dz-clickable')) {
            console.log('Dropzone already initialized.');
            return;
        }

        // Ensure the element exists before initializing
        if ($('#' + dropzoneElementId).length === 0) {
            console.warn('Dropzone element not found. Skipping Dropzone init.');
            return;
        }

        // Get anti-forgery token from the loaded partial view data
        this.antiForgeryToken = $('#media-browser-content').data('anti-forgery-token');
        if (!this.antiForgeryToken) {
            console.error("Anti-forgery token not found for Dropzone.");
            // Optionally disable upload functionality
        }

        Dropzone.autoDiscover = false; // Prevent automatic initialization
        this.myDropzone = new Dropzone("#" + dropzoneElementId, {
            url: "/Admin/Media/UploadFile", // Your upload endpoint
            paramName: "file", // The name of the file parameter
            maxFilesize: 100, // MB
            acceptedFiles: "image/*,video/*,application/pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx", // Allowed file types
            addRemoveLinks: true,
            dictDefaultMessage: 'Kéo thả tập tin vào đây hoặc bấm để chọn',
            dictRemoveFile: 'Xóa',
            dictCancelUpload: 'Hủy tải lên',
            dictUploadCanceled: 'Đã hủy tải lên.',
            dictInvalidFileType: 'Không thể tải lên tập tin loại này.',
            dictFileTooBig: 'Tập tin quá lớn ({{filesize}}MB). Kích thước tối đa: {{maxFilesize}}MB.',
            headers: {
                'RequestVerificationToken': this.antiForgeryToken
            },
            params: function () {
                return {
                    folderId: mediaManager.currentFolderId
                };
            }
        });

        this.myDropzone.on("success", function (file, response) {
            console.log(response)
            // response should be your UploadResponseViewModel JSON
            if (response.success) {
                console.log("File uploaded successfully:", response.file);
                // Append the new file item to the grid
                mediaManager.renderFileItem(response.file, true); // Render in modal context
                // Remove the preview element created by Dropzone
                file.previewElement.remove();
                // Show success toast?
                toastr.success(`Đã tải lên "${response.file.originalFileName}"`);

                // Hide empty indicator if visible
                $('#media-browser-empty-indicator').hide();


            } else {
                console.error("Upload failed:", response.message);
                // Show error message on the file preview
                $(file.previewElement).addClass('dz-error').find('.dz-error-message').text(response.message);
                // Show error toast?
                toastr.error(`Tải lên "${file.name}" thất bại: ${response.message}`);
            }
        });

        this.myDropzone.on("error", function (file, message, xhr) {
            console.error("Upload error for file", file.name, message);
            // Display the error message received from the server or default Dropzone message
            const errorMessage = xhr && xhr.responseJSON && xhr.responseJSON.message ? xhr.responseJSON.message : message;
            $(file.previewElement).addClass('dz-error').find('.dz-error-message span').text(errorMessage);
            // Show error toast?
            toastr.error(`Tải lên "${file.name}" lỗi: ${errorMessage}`);
        });

        this.myDropzone.on("removedfile", function (file) {
            // If you need to delete the file from the server immediately upon removing it from preview
            // You would need to implement this AJAX call here.
            console.log("Removed file preview:", file.name);
        });

        this.myDropzone.on("sending", function (file, xhr, formData) {
            // Add extra data or modify headers before sending
            // formData.append("altText", "my alt text"); // Example
            // formData.append("description", "my description"); // Example
        });
    },


    // --- Event Handlers ---

    // Called when any button/element triggers the modal
    handleModalShow: function (event) {
        console.log("Media modal shown.");
        // You might get the target element here to know which input field needs the selected URL
        const relatedTarget = $(event.relatedTarget); // The element that triggered the modal
        // Store a reference to the input element that needs the URL
        // For example, if the button is next to an input: relatedTarget.prev('input')
        // Or if using data attributes: relatedTarget.data('target-input')
        this.currentInputTarget = relatedTarget.closest('.input-group').find('input[type="text"], input[type="url"]');

        // Reset modal state
        this.currentFolderId = null; // Start at root for the modal
        this.$modalSelectFileBtn.prop('disabled', true); // Disable select button initially
        this.$modalSelectedFileInfo.hide(); // Hide selected file info area

        // Load initial content for the root folder (or last visited folder)
        this.loadFolderContent(this.currentFolderId);
    },

    // Called when the modal is hidden
    handleModalHide: function () {
        console.log("Media modal hidden.");
        // Clear any temporary state
        this.currentFolderId = null;
        this.selectFileCallback = null;
        this.currentInputTarget = null;
        // Optional: Clear modal content to ensure fresh load next time
        this.$modalBodyContent.html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải...</p></div>');

        // Destroy Dropzone instance if needed to prevent duplicates
        if (this.myDropzone) {
            this.myDropzone.destroy();
            this.myDropzone = null; // Clear the reference
        }
    },

    // Handle click on a folder item
    handleFolderClick: function (event) {
        event.preventDefault();
        const $target = $(event.currentTarget).closest('.media-folder-item'); // Find the folder item element
        const folderId = $target.data('folder-id');
        console.log("Navigating into folder:", folderId);
        this.currentFolderId = folderId > 0 ? folderId : null; // Use null for root (ID 0)
        this.loadFolderContent(this.currentFolderId); // Load content of the clicked folder
    },

    // Handle click on a breadcrumb link
    handleBreadcrumbClick: function (event) {
        event.preventDefault();
        const $target = $(event.currentTarget);
        const folderId = $target.data('folder-id'); // This will be empty string for root
        console.log("Navigating via breadcrumb to folder:", folderId);
        this.currentFolderId = folderId === '' ? null : folderId; // Use null for root
        this.loadFolderContent(this.currentFolderId); // Load content
    },

    // Handle click on a file item (selection)
    handleFileClick: function (event) {
        event.preventDefault();
        const $target = $(event.currentTarget).closest('.media-file-item'); // Find the file item element

        // Remove 'selected' class from previously selected item
        this.$modalBodyContent.find('.media-file-item').removeClass('selected border border-primary');

        // Add 'selected' class to the clicked item
        $target.addClass('selected border border-primary');

        // Enable the Select button and show file info
        this.$modalSelectFileBtn.prop('disabled', false);
        this.$modalSelectedFileInfo.show();
        this.$modalSelectedFileName.text($target.data('file-name'));

        // Store the selected file data temporarily
        this.selectedFile = {
            url: $target.data('file-url'),
            altText: $target.data('file-alt'),
            // Add other data you might need
        };

        // Update hidden inputs (optional, can rely on this.selectedFile)
        $('#selected-media-url-modal-input').val(this.selectedFile.url);
        $('#selected-media-alt-text-modal-input').val(this.selectedFile.altText);
    },

    // Handle click on the "Chọn" button in the modal footer
    handleModalSelectFile: function () {
        if (this.selectedFile && this.currentInputTarget && this.currentInputTarget.length > 0) {
            // Put the selected file URL into the target input field
            this.currentInputTarget.val(this.selectedFile.url).trigger('change'); // Trigger change event if needed

            // If you had separate alt text inputs, update them here too
            // this.currentInputTarget.closest('.form-group').find('input[data-alt-text-for="..."]').val(this.selectedFile.altText);

            console.log("Selected file:", this.selectedFile.url, "Target input:", this.currentInputTarget);

            // Hide the modal
            this.$modal.modal('hide');

            // Execute the callback if provided
            // This allows the page that opened the modal to react
            if (typeof this.selectFileCallback === 'function') {
                this.selectFileCallback(this.selectedFile);
            }

        } else {
            console.warn("No file selected or target input not found.");
        }
    },

    // Handle change on the media type filter dropdown
    handleFilterChange: function () {
        const mediaType = $('#modal-media-type-filter').val();
        console.log("Filtering by media type:", mediaType);
        // Reload files in the current folder with the new filter
        this.loadFolderContent(this.currentFolderId, mediaType);
    },

    // Handle click on Create Folder button
    handleCreateFolderClick: function (event) {
        event.preventDefault();
        const parentId = $(event.currentTarget).data('parent-folder-id'); // Get parent ID from button data

        // Prompt user for folder name
        const folderName = prompt("Nhập tên thư mục mới:");
        if (folderName === null || folderName.trim() === "") {
            console.log("Folder creation cancelled or empty name.");
            return; // User cancelled or entered empty name
        }

        // AJAX call to create folder
        $.ajax({
            url: '/Admin/Media/CreateFolder', // Your create folder endpoint
            type: 'POST',
            contentType: 'application/json', // Send data as JSON
            data: JSON.stringify({ name: folderName.trim(), parentId: parentId > 0 ? parentId : null }), // Send parentId (use null for root)
            headers: {
                'RequestVerificationToken': this.antiForgeryToken // Include token
            },
            success: function (response) {
                if (response.success) {
                    console.log("Folder created:", response.folder);
                    toastr.success(response.message);
                    // Re-load current folder content to show the new folder
                    mediaManager.loadFolderContent(mediaManager.currentFolderId);
                } else {
                    console.error("Failed to create folder:", response.message, response.errors);
                    const errorMessage = response.errors ? response.errors.join('<br>') : response.message;
                    toastr.error(`Tạo thư mục thất bại: ${errorMessage}`);
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX error creating folder:", status, error, xhr.responseText);
                toastr.error('Đã xảy ra lỗi khi tạo thư mục.');
            }
        });
    },

    // Handle click on Toggle Upload Area button
    handleToggleUploadArea: function (event) {
        event.preventDefault();
        this.$modalUploadArea = $('#modal-upload-area'); // Ensure element is found after content load
        this.$modalUploadArea.slideToggle(); // Toggle visibility
        // Initialize Dropzone here if not already done
        if (!this.myDropzone) {
            this.initDropzone();
        }
    },

    // Handle click on Delete File button (requires confirmation modal or similar)
    handleDeleteFileClick: function (event) {
        event.preventDefault();
        event.stopPropagation(); // Prevent triggering file selection
        const $target = $(event.currentTarget);
        const fileId = $target.data('id');
        const fileName = $target.data('name');
        const deleteUrl = $target.data('delete-url'); // Get URL from button data

        if (confirm(`Bạn có chắc chắn muốn xóa tập tin "${fileName}"?`)) { // Use simple confirm for now
            $.ajax({
                url: deleteUrl,
                type: 'POST',
                data: { id: fileId }, // Send ID as form data
                headers: {
                    'RequestVerificationToken': this.antiForgeryToken // Include token
                },
                success: function (response) {
                    if (response.success) {
                        console.log("File deleted:", fileId);
                        toastr.success(response.message);
                        // Remove the file item from the DOM
                        $target.closest('.media-file-item').remove();
                        // Check if folder is now empty and update indicator
                        mediaManager.checkEmptyState();
                    } else {
                        console.error("Failed to delete file:", response.message);
                        toastr.error(`Xóa tập tin "${fileName}" thất bại: ${response.message}`);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX error deleting file:", status, error, xhr.responseText);
                    toastr.error('Đã xảy ra lỗi khi xóa tập tin.');
                }
            });
        }
    },

    // Handle click on Delete Folder button (requires confirmation and check for empty)
    handleDeleteFolderClick: function (event) {
        event.preventDefault();
        event.stopPropagation(); // Prevent triggering folder navigation
        const $target = $(event.currentTarget);
        const folderId = $target.data('id');
        const folderName = $target.data('name');
        const deleteUrl = $target.data('delete-url'); // Get URL from button data

        if (confirm(`Bạn có chắc chắn muốn xóa thư mục "${folderName}"? Thư mục phải rỗng để có thể xóa.`)) { // Use simple confirm
            $.ajax({
                url: deleteUrl,
                type: 'POST',
                data: { id: folderId }, // Send ID as form data
                headers: {
                    'RequestVerificationToken': this.antiForgeryToken // Include token
                },
                success: function (response) {
                    if (response.success) {
                        console.log("Folder deleted:", folderId);
                        toastr.success(response.message);
                        // Remove the folder item from the DOM
                        $target.closest('.media-folder-item').remove();
                        // Check if parent folder is now empty and update indicator (more complex, might need full reload)
                        // For simplicity, let's just remove the item. Checking parent emptiness requires fetching parent state.
                    } else {
                        console.error("Failed to delete folder:", response.message);
                        toastr.error(`Xóa thư mục "${folderName}" thất bại: ${response.message}`);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX error deleting folder:", status, error, xhr.responseText);
                    toastr.error('Đã xảy ra lỗi khi xóa thư mục.');
                }
            });
        }
    },


    // --- Data Loading Functions ---

    // Load content (folders and files) for a given folder ID
    loadFolderContent: function (folderId, mediaTypeFilter = null) {
        console.log("Loading content for folder:", folderId, "Filter:", mediaTypeFilter);

        // Show loading indicator
        this.$modalBodyContent.find('#media-browser-loading-indicator').show();
        this.$modalBodyContent.find('#media-browser-folders, #media-browser-files, #media-browser-empty-indicator').hide();
        this.$modalSelectFileBtn.prop('disabled', true); // Disable select button during load
        this.$modalSelectedFileInfo.hide(); // Hide selected info

        // Fetch content via AJAX Partial View render
        $.ajax({
            url: '/Admin/Media/SelectMediaModalContent', // Endpoint to render the partial
            type: 'GET',
            data: { folderId: folderId, mediaTypeFilter: mediaTypeFilter }, // Pass folder ID and filter
            success: function (html) {
                // Replace the current modal body content with the loaded HTML
                mediaManager.$modalBodyContent.html(html);

                // Update cached element references AFTER loading new content
                mediaManager.$modalBreadcrumbs = mediaManager.$modalBodyContent.find('#modal-media-breadcrumbs');
                mediaManager.$modalFolders = mediaManager.$modalBodyContent.find('#media-browser-folders');
                mediaManager.$modalFiles = mediaManager.$modalBodyContent.find('#media-browser-files');
                mediaManager.$modalMediaTypeFilter = mediaManager.$modalBodyContent.find('#modal-media-type-filter');
                mediaManager.$modalUploadArea = mediaManager.$modalBodyContent.find('#modal-upload-area');
                mediaManager.$modalLoadingIndicator = mediaManager.$modalBodyContent.find('#media-browser-loading-indicator');
                mediaManager.$modalEmptyIndicator = mediaManager.$modalBodyContent.find('#media-browser-empty-indicator');

                // Update current folder ID state from the data attribute on the loaded content div
                mediaManager.currentFolderId = mediaManager.$modalBodyContent.find('#media-browser-content').data('current-folder-id');
                mediaManager.antiForgeryToken = mediaManager.$modalBodyContent.find('#media-browser-content').data('anti-forgery-token');


                // Hide loading indicator, show content
                mediaManager.$modalLoadingIndicator.hide();
                mediaManager.$modalFolders.show();
                mediaManager.$modalFiles.show();


                // Initialize tooltips for new elements
                mediaManager.initTooltips(mediaManager.$modalBodyContent); // Assuming you have a global tooltip init function

                // Initialize Dropzone instance specific to the newly loaded upload area
                // Only if the upload area is part of the loaded content
                if (mediaManager.$modalUploadArea.length > 0) {
                    // Re-initialize Dropzone for the new element if needed
                    // Or initialize only when the upload area is toggled visible
                    // mediaManager.initDropzone(); // Example: init here if area is always visible
                }


                // Check and update empty state indicator
                mediaManager.checkEmptyState();


            },
            error: function (xhr, status, error) {
                console.error("AJAX error loading media content:", status, error, xhr.responseText);
                mediaManager.$modalBodyContent.html('<div class="alert alert-danger">Không thể tải nội dung media.</div>');
                // Hide loading
                mediaManager.$modalBodyContent.find('#media-browser-loading-indicator').hide();
            }
        });
    },

    // Helper function to render a single folder item HTML (if not using server-side partials for lists)
    renderFolderItem: function (folderVM, isInModal = false) {
        // You would build the HTML string here based on folderVM properties
        // For simplicity, we rely on the server-side partial _MediaFolderItem.cshtml being rendered by SelectMediaModalContent action.
        console.warn("renderFolderItem not implemented. Relying on server-rendered partials.");
    },

    // Helper function to render a single file item HTML (if not using server-side partials for lists, or for adding new uploads)
    renderFileItem: function (fileVM, isInModal = false) {
        // Use a simplified template or call AJAX to get the partial HTML for a single item
        const $filesContainer = isInModal ? this.$modalFiles : $('#media-file-grid'); // Target container

        // Build HTML string based on fileVM, matching _MediaFileItem.cshtml structure
        let itemHtml = `
         <div class="media-file-item cursor-pointer"
              data-file-id="${fileVM.id}"
              data-file-url="${fileVM.publicUrl}"
              data-file-alt="${fileVM.altText}"
              data-file-name="${fileVM.originalFileName}"
              data-file-type="${fileVM.mediaType.toLowerCase()}"
              data-bs-toggle="tooltip" data-bs-placement="bottom" title="${fileVM.originalFileName} (${fileVM.formattedFileSize})">
             <div class="card card-sm">
                 <div class="card-img-top img-responsive img-responsive-1by1 ${fileVM.mediaType.toLowerCase() !== 'image' ? 'text-muted bg-light d-flex align-items-center justify-content-center fs-1' : ''}"
                      ${fileVM.mediaType.toLowerCase() === 'image' ? `style="background-image: url(${fileVM.publicUrl});"` : ''} >
                      ${fileVM.mediaType.toLowerCase() !== 'image' ? `<i class="ti ${fileVM.mediaType.toLowerCase() === 'video' ? 'ti-video' : (fileVM.mediaType.toLowerCase() === 'document' ? 'ti-file-text' : 'ti-file')}"></i>` : ''}
                 </div>
                 <div class="card-body p-2 text-center">
                     <div class="text-truncate small mb-1">${fileVM.originalFileName}</div>
                     <div class="text-muted small">${fileVM.formattedFileSize}</div>
                      ${fileVM.mediaType.toLowerCase() === 'video' && fileVM.duration ? `<div class="text-muted small">${fileVM.duration} min</div>` : ''}
                 </div>
                  ${!isInModal ? `
                   <div class="card-footer text-center p-1">
                       <div class="btn-list flex-nowrap justify-content-center">
                           <button class="btn btn-sm btn-outline-danger delete-file-btn" data-id="${fileVM.id}" data-name="${fileVM.originalFileName}" title="Xóa" data-delete-url="/Admin/Media/DeleteFile"><i class="ti ti-trash"></i></button>
                       </div>
                   </div>
                  ` : ''}
             </div>
         </div>
         `;

        $filesContainer.prepend(itemHtml); // Add new file at the beginning
        this.initTooltips($filesContainer); // Re-initialize tooltips for the new item
    },

    // Check if folder/file lists are empty and update the empty indicator
    checkEmptyState: function () {
        const folderCount = this.$modalBodyContent.find('.media-folder-item').length;
        const fileCount = this.$modalBodyContent.find('.media-file-item').length;

        if (folderCount === 0 && fileCount === 0) {
            this.$modalEmptyIndicator.show();
        } else {
            this.$modalEmptyIndicator.hide();
        }
    },

    // Helper to initialize tooltips (assuming Tabler or Bootstrap tooltips)
    initTooltips: function ($container) {
        $container.find('[data-bs-toggle="tooltip"]').tooltip();
    }

    // Add other helper functions as needed (e.g., update counts after delete)
};

// Initialize the media manager when the document is ready
$(document).ready(function () {
    mediaManager.init();
});

// Function to be called from other pages to open the media modal
// targetInput: The jQuery or DOM element of the input field that needs the selected URL
// callback: An optional function to run after a file is selected and modal is closed (receives selected file data)
// initialFolderId: Optional ID of the folder to open initially
// initialMediaTypeFilter: Optional MediaType to filter by initially
function openMediaModal(targetInput, callback = null, initialFolderId = null, initialMediaTypeFilter = null) {
    mediaManager.currentInputTarget = $(targetInput);
    mediaManager.selectFileCallback = callback;
    // Pass initial state if needed, though handleModalShow currently defaults to root
    // You would modify handleModalShow or add another load function to take initial state
    // For now, call loadFolderContent explicitly if needed after modal shows
    mediaManager.$modal.modal('show');

    // After modal is shown and content loaded, you might want to load content for initialFolderId
    // This can be done in the show.bs.modal handler or here after a slight delay
    // mediaManager.loadFolderContent(initialFolderId, initialMediaTypeFilter); // Example if handler doesn't do it
}