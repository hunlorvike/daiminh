const mediaConfig = {
    dropzoneElementId: "modal-dropzone",
    uploadUrl: "/Admin/Media/UploadFile",
    acceptedFiles: "image/*,video/*,application/pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.csv",
    maxFileSize: 100,
    antiForgeryTokenKey: "antiForgeryToken",
    modalFilesContainerSelector: "#media-browser-files",
    modalMediaTypeFilterSelector: "#modal-media-type-filter",
    modalSearchTermSelector: "#modal-media-search-term"
};

const MediaManager = {
    $modal: null,
    $modalBodyContent: null,
    $modalSelectFileBtn: null,
    $modalSelectedFileInfo: null,
    $modalSelectedFileName: null,
    $loadedContentContainer: null,
    $modalFilesContainer: null,
    $modalMediaTypeFilter: null,
    $modalSearchTermInput: null,
    $modalUploadArea: null,
    $toggleUploadAreaBtn: null,
    $modalLoadingIndicator: null,
    $modalEmptyIndicator: null,
    antiForgeryToken: "",
    selectFileCallback: null,
    currentInputTarget: null,
    selectedFile: null,
    myDropzone: null,

    openMediaModal: function (targetInput, callback = null, initialMediaTypeFilter = null) {
        this.currentInputTarget = $(targetInput);
        this.selectFileCallback = callback;

        if (this.$modalMediaTypeFilter && initialMediaTypeFilter !== null) {
            this.$modalMediaTypeFilter.val(initialMediaTypeFilter).trigger('change');
        } else {
            this.loadContent();
        }
        this.$modal.modal("show");
    },

    init: function () {
        this.$modal = $("#selectMediaModal");
        if (this.$modal.length === 0) return;

        this.$modalBodyContent = this.$modal.find("#select-media-modal-body-content");
        this.$modalSelectFileBtn = this.$modal.find("#modal-select-file-btn");
        this.$modalSelectedFileInfo = this.$modal.find("#modal-selected-file-info");
        this.$modalSelectedFileName = this.$modal.find("#selected-file-name");

        this.$modal.on("show.bs.modal", this.handleModalShow.bind(this));
        this.$modal.on("hidden.bs.modal", this.handleModalHide.bind(this));
        this.$modalSelectFileBtn.on("click", this.handleModalSelectFile.bind(this));

        $('#media-file-grid').on('click', '.delete-file-btn', this.handleDeleteFileClick.bind(this));
        const mainPageToken = $('[name=__RequestVerificationToken]').val();
        if (mainPageToken && !this.antiForgeryToken) {
            this.antiForgeryToken = mainPageToken;
        }
    },

    initDropzone: function () {
        const $dropzoneElement = this.$loadedContentContainer.find("#" + mediaConfig.dropzoneElementId);
        if ($dropzoneElement.length === 0 || ($dropzoneElement.hasClass("dz-clickable") && this.myDropzone)) {
            return;
        }

        this.antiForgeryToken = this.$loadedContentContainer.data(mediaConfig.antiForgeryTokenKey) || this.antiForgeryToken;

        Dropzone.autoDiscover = false;
        this.myDropzone = new Dropzone("#" + mediaConfig.dropzoneElementId, {
            url: mediaConfig.uploadUrl,
            paramName: "file",
            maxFilesize: mediaConfig.maxFileSize,
            acceptedFiles: mediaConfig.acceptedFiles,
            addRemoveLinks: false,
            dictDefaultMessage: $dropzoneElement.find('.dz-message').html(),
            headers: { "RequestVerificationToken": this.antiForgeryToken },
            previewsContainer: false,
        });

        this.myDropzone.on("sending", (file, xhr, formData) => {
        });

        this.myDropzone.on("success", (file, response) => {
            this.myDropzone.removeFile(file);
            if (response.success && response.file) {
                this.renderFileItem(response.file, true);
                this.checkEmptyState();
                toastr.success(response.message || `Đã tải lên "${response.file.originalFileName}"`);
            } else {
                toastr.error(response.message || `Tải lên "${file.name}" thất bại.`);
            }
        });

        this.myDropzone.on("error", (file, message, xhr) => {
            this.myDropzone.removeFile(file);
            const errorMessage = xhr?.responseJSON?.message || (typeof message === 'string' ? message : "Lỗi không xác định.");
            toastr.error(`Tải lên "${file.name}" lỗi: ${errorMessage}`);
        });
    },

    handleModalShow: function (event) {
        const relatedTarget = $(event.relatedTarget);
        if (relatedTarget && relatedTarget.length > 0) {
            const $inputGroup = relatedTarget.closest('.input-group');
            if ($inputGroup.length > 0) {
                this.currentInputTarget = $inputGroup.find('input.media-url-input');
            } else {
                const targetData = relatedTarget.data('target-input');
                if (targetData) {
                    this.currentInputTarget = $(targetData);
                }
            }
        }

        this.$modalSelectFileBtn.prop("disabled", true);
        this.$modalSelectedFileInfo.hide();
        this.selectedFile = null;

        const initialFilterType = relatedTarget.data('media-type-filter');
        this.loadContent(initialFilterType);
    },

    handleModalHide: function () {
        this.selectFileCallback = null;
        this.currentInputTarget = null;
        this.selectedFile = null;
        this.$modalBodyContent.html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải...</p></div>'); // Reset modal body
        if (this.myDropzone) {
            this.myDropzone.destroy();
            this.myDropzone = null;
        }
        this.$modalBodyContent.off("click", ".media-file-item")
            .off("change", mediaConfig.modalMediaTypeFilterSelector)
            .off("input", mediaConfig.modalSearchTermSelector)
            .off("click", "#toggle-upload-area")
            .off("click", ".delete-file-btn");

        this.$loadedContentContainer = null;
        this.$modalFilesContainer = null;
        this.$modalMediaTypeFilter = null;
        this.$modalSearchTermInput = null;
        this.$modalUploadArea = null;
        this.$toggleUploadAreaBtn = null;
        this.$modalLoadingIndicator = null;
        this.$modalEmptyIndicator = null;
    },

    handleModalSelectFile: function () {
        if (this.selectedFile && this.currentInputTarget && this.currentInputTarget.length > 0) {
            this.currentInputTarget.val(this.selectedFile.url).trigger("change");

            if (typeof this.selectFileCallback === "function") {
                this.selectFileCallback(this.selectedFile);
            }
            this.$modal.modal("hide");
        } else {
            toastr.warning("Vui lòng chọn một tập tin.");
        }
    },

    loadContent: function (mediaType = null, searchTerm = null) {
        this.$modalBodyContent.html('<div class="text-center p-5" id="media-browser-loading-indicator-initial"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải thư viện...</p></div>');

        $.ajax({
            url: "/Admin/Media/SelectMediaModalContent",
            type: "GET",
            data: { MediaType: mediaType, SearchTerm: searchTerm },
            success: (html) => {
                this.$modalBodyContent.html(html);
                this.assignModalElements();
                this.attachDynamicContentEvents();
                if (this.$loadedContentContainer && this.$loadedContentContainer.find("#" + mediaConfig.dropzoneElementId).length > 0) {
                    // Dropzone will be initialized by handleToggleUploadArea if button is clicked
                } else {
                    // console.warn("Dropzone element not found in loaded modal content.");
                }
                this.checkEmptyState();
                if (this.$modalMediaTypeFilter && typeof TomSelect !== 'undefined') {
                    new TomSelect(this.$modalMediaTypeFilter[0], {
                        copyClassesToDropdown: false,
                        dropdownParent: 'body',
                        controlInput: '<input>',
                    });
                }
            },
            error: () => {
                this.$modalBodyContent.html('<div class="alert alert-danger m-3">Không thể tải thư viện media. Vui lòng thử lại.</div>');
            }
        });
    },

    assignModalElements: function () {
        this.$loadedContentContainer = this.$modalBodyContent.find("#media-browser-content");
        this.$modalFilesContainer = this.$loadedContentContainer.find(mediaConfig.modalFilesContainerSelector);
        this.$modalMediaTypeFilter = this.$loadedContentContainer.find(mediaConfig.modalMediaTypeFilterSelector);
        this.$modalSearchTermInput = this.$loadedContentContainer.find(mediaConfig.modalSearchTermSelector);
        this.$modalUploadArea = this.$loadedContentContainer.find("#modal-upload-area");
        this.$toggleUploadAreaBtn = this.$loadedContentContainer.find("#toggle-upload-area");
        this.$modalLoadingIndicator = this.$loadedContentContainer.find("#media-browser-loading-indicator");
        this.$modalEmptyIndicator = this.$loadedContentContainer.find("#media-browser-empty-indicator");
        this.antiForgeryToken = this.$loadedContentContainer.data(mediaConfig.antiForgeryTokenKey) || this.antiForgeryToken;
    },

    attachDynamicContentEvents: function () {
        if (!this.$loadedContentContainer) return;

        this.$loadedContentContainer.off("click", ".media-file-item").on("click", ".media-file-item", this.handleFileClick.bind(this));
        if (this.$modalMediaTypeFilter) {
            this.$modalMediaTypeFilter.off("change").on("change", this.handleFilterOrSearchChange.bind(this));
        }
        if (this.$modalSearchTermInput) {
            let searchTimeout;
            this.$modalSearchTermInput.off("input").on("input", () => {
                clearTimeout(searchTimeout);
                searchTimeout = setTimeout(() => {
                    this.handleFilterOrSearchChange();
                }, 500);
            });
        }
        if (this.$toggleUploadAreaBtn) {
            this.$toggleUploadAreaBtn.off("click").on("click", this.handleToggleUploadArea.bind(this));
        }
    },

    handleFileClick: function (event) {
        event.preventDefault();
        const $target = $(event.currentTarget);
        const $fileItemDiv = $target.closest(".media-file-item");


        this.$modalFilesContainer.find(".media-file-item .card").removeClass("border-primary shadow");
        $fileItemDiv.find(".card").addClass("border-primary shadow");

        this.$modalSelectFileBtn.prop("disabled", false);
        this.$modalSelectedFileInfo.show();
        this.$modalSelectedFileName.text($fileItemDiv.data("file-name"));

        this.selectedFile = {
            url: $fileItemDiv.data("file-url"),
            altText: $fileItemDiv.data("file-alt"),
            id: $fileItemDiv.data("file-id"),
            type: $fileItemDiv.data("file-type"),
            name: $fileItemDiv.data("file-name")
        };
    },

    handleFilterOrSearchChange: function () {
        const mediaType = this.$modalMediaTypeFilter ? this.$modalMediaTypeFilter.val() : null;
        const searchTerm = this.$modalSearchTermInput ? this.$modalSearchTermInput.val() : null;

        this.$modalLoadingIndicator.show();
        this.$modalFilesContainer.hide();
        this.$modalEmptyIndicator.hide();

        $.ajax({
            url: "/Admin/Media/GetFiles",
            type: "GET",
            data: { mediaType: mediaType, searchTerm: searchTerm },
            success: (filesVM) => {
                this.$modalFilesContainer.empty();
                if (filesVM && filesVM.length > 0) {
                    filesVM.forEach(fileVM => this.renderFileItem(fileVM, true));
                    this.$modalEmptyIndicator.hide();
                } else {
                    this.$modalEmptyIndicator.show();
                }
            },
            error: () => {
                this.$modalFilesContainer.html('<div class="col-12"><div class="alert alert-danger">Lỗi khi tải danh sách file.</div></div>');
                this.$modalEmptyIndicator.hide();
            },
            complete: () => {
                this.$modalLoadingIndicator.hide();
                this.$modalFilesContainer.show();
                $('[data-bs-toggle="tooltip"]').each(function () {
                    new tabler.Tooltip(this);
                });
            }
        });
    },

    handleToggleUploadArea: function (event) {
        event.preventDefault();
        if (this.$modalUploadArea) {
            this.$modalUploadArea.slideToggle();
            if (this.$modalUploadArea.is(':visible') && !this.myDropzone) {
                this.initDropzone();
            }
        }
    },

    handleDeleteFileClick: function (event) {
        event.preventDefault();
        event.stopPropagation();
        const $target = $(event.currentTarget);
        const fileId = $target.data("id");
        const fileName = $target.data("name");
        const deleteUrl = $target.data("delete-url");

        if (!deleteUrl || !fileId) {
            toastr.error("Không thể xóa tập tin (thiếu thông tin).");
            return;
        }

        if (confirm(`Bạn có chắc chắn muốn xóa tập tin "${fileName}"? Hành động này không thể hoàn tác.`)) {
            this.executeDeleteFile(fileId, fileName, deleteUrl, $target.closest('.col'));
        }
    },

    executeDeleteFile: function (fileId, fileName, deleteUrl, $itemContainer) {
        const $buttonElement = $itemContainer.find('.delete-file-btn');
        const originalButtonContent = $buttonElement.html();
        $buttonElement.prop("disabled", true).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

        const currentToken = this.antiForgeryToken || $('[name=__RequestVerificationToken]').val();

        $.ajax({
            url: deleteUrl,
            type: "POST",
            data: { id: fileId },
            headers: { "RequestVerificationToken": currentToken },
            success: (response) => {
                if (response.success) {
                    $itemContainer.remove();
                    this.checkEmptyState();
                    toastr.success(response.message || `Đã xóa "${fileName}"`);
                } else {
                    toastr.error(response.message || `Xóa "${fileName}" thất bại.`);
                }
            },
            error: (xhr, status, error) => {
                toastr.error(`Lỗi khi xóa tập tin: ${error || 'Lỗi không xác định'}`);
            },
            complete: () => {
                $buttonElement.prop("disabled", false).html(originalButtonContent);
            }
        });
    },

    renderFileItem: function (fileVM, isInModal = false) {
        const $filesContainer = isInModal ? this.$modalFilesContainer : $("#media-file-grid");
        if (!$filesContainer || $filesContainer.length === 0) {
            return;
        }

        const mediaTypeString = fileVM.mediaTypeDisplayName || "Khác";
        const formattedFileSize = fileVM.formattedFileSize || `${(fileVM.fileSize / 1024).toFixed(1)} KB`;
        const presignedUrl = fileVM.presignedUrl || '/img/placeholder.svg';

        let iconHtml = '';
        if (fileVM.mediaType !== 0) {
            let iconClass = 'ti-file';
            if (fileVM.mediaType === 1) iconClass = 'ti-video';
            else if (fileVM.mediaType === 2) iconClass = 'ti-file-text';
            iconHtml = `<i class="ti ${iconClass} fs-1 text-muted"></i>`;
        }

        const itemHtml = `
            <div class="col">
                <div class="card card-sm h-100 media-file-item cursor-pointer"
                     data-file-id="${fileVM.id}"
                     data-file-url="${presignedUrl}"
                     data-file-alt="${fileVM.altText || ''}"
                     data-file-name="${fileVM.originalFileName}"
                     data-file-type="${fileVM.mediaType.toString().toLowerCase()}"
                     data-bs-toggle="tooltip" data-bs-placement="top" title="${fileVM.originalFileName} (${formattedFileSize})">
                    
                    <div class="card-img-top img-responsive img-responsive-1x1 d-flex align-items-center justify-content-center 
                                ${fileVM.mediaType !== 0 ? 'bg-light-subtle' : ''}" 
                         style="${fileVM.mediaType === 0 ? `background-image: url('${presignedUrl}'); background-size: cover; background-position: center;` : ''}">
                        ${iconHtml}
                    </div>

                    <div class="card-body p-2 text-center">
                        <div class="text-truncate small fw-medium mb-1" title="${fileVM.originalFileName}">${fileVM.originalFileName}</div>
                        <div class="text-muted small">${formattedFileSize}</div>
                        ${fileVM.mediaType === 1 && fileVM.duration ? `<div class="text-muted small">${fileVM.duration} giây</div>` : ''}
                    </div>
                    
                    ${!isInModal ? `
                     <div class="card-footer p-2">
                         <div class="btn-list justify-content-center flex-nowrap">
                              <a href="/Admin/Media/Edit/${fileVM.id}" class="btn btn-sm btn-ghost-secondary" title="Sửa thông tin">
                                 <i class="ti ti-pencil"></i>
                             </a>
                             <button class="btn btn-sm btn-ghost-danger delete-file-btn" 
                                     data-id="${fileVM.id}" 
                                     data-name="${fileVM.originalFileName}" 
                                     title="Xóa" 
                                     data-delete-url="/Admin/Media/DeleteFile">
                                  <i class="ti ti-trash"></i>
                             </button>
                         </div>
                     </div>` : ''}
                </div>
            </div>`;

        $filesContainer.prepend(itemHtml);
        if (!isInModal) {
            $filesContainer.find('[data-bs-toggle="tooltip"]').first().each(function () {
                new tabler.Tooltip(this);
            });
        }
    },

    checkEmptyState: function () {
        if (!this.$modalFilesContainer || !this.$modalEmptyIndicator) return;

        const fileCount = this.$modalFilesContainer.find(".media-file-item").length;
        if (fileCount === 0) {
            this.$modalEmptyIndicator.show();
        } else {
            this.$modalEmptyIndicator.hide();
        }
    }
};

window.openMediaModal = MediaManager.openMediaModal.bind(MediaManager);

$(function () {
    MediaManager.init();
});

$(document).ready(function () {
    if ($('#media-file-grid').length) {
        $('#media-file-grid [data-bs-toggle="tooltip"]').each(function () {
            new tabler.Tooltip(this);
        });
    }
});