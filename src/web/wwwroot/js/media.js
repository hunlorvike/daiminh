const mediaConfig = {
    dropzoneElementId: "modal-dropzone",
    uploadUrl: "/Admin/Media/UploadFile",
    acceptedFiles: "image/*,video/*,application/pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx",
    maxFileSize: 100,
    antiForgeryTokenKey: "antiForgeryToken"
};

const MediaManager = {
    $modal: null,
    $modalBodyContent: null,
    $modalSelectFileBtn: null,
    $modalSelectedFileInfo: null,
    $modalSelectedFileName: null,
    $loadedContentContainer: null,
    $modalFiles: null,
    $modalMediaTypeFilter: null,
    $modalUploadArea: null,
    $modalLoadingIndicator: null,
    $modalEmptyIndicator: null,
    antiForgeryToken: "",
    selectFileCallback: null,
    currentInputTarget: null,
    myDropzone: null,

    openMediaModal: function (targetInput, callback = null, initialMediaTypeFilter = null) {
        this.currentInputTarget = $(targetInput);
        this.selectFileCallback = callback;

        if (initialMediaTypeFilter && this.$modalMediaTypeFilter) {
            this.$modalMediaTypeFilter.val(initialMediaTypeFilter);
        }

        this.$modal.modal("show");
    },

    init: function () {
        this.$modal = $("#selectMediaModal");
        if (this.$modal.length === 0) return;

        this.$modalBodyContent = $("#select-media-modal-body-content");
        this.$modalSelectFileBtn = $("#modal-select-file-btn");
        this.$modalSelectedFileInfo = $("#modal-selected-file-info");
        this.$modalSelectedFileName = $("#selected-file-name");

        this.$modal.on("show.bs.modal", this.handleModalShow.bind(this));
        this.$modal.on("hidden.bs.modal", this.handleModalHide.bind(this));
        this.$modalSelectFileBtn.on("click", this.handleModalSelectFile.bind(this));
    },

    initDropzone: function () {
        const $dropzoneElement = $("#" + mediaConfig.dropzoneElementId);
        if ($dropzoneElement.length === 0 || ($dropzoneElement.hasClass("dz-clickable") && this.myDropzone)) return;

        this.antiForgeryToken = this.$loadedContentContainer.data(mediaConfig.antiForgeryTokenKey) || "";

        Dropzone.autoDiscover = false;
        this.myDropzone = new Dropzone("#" + mediaConfig.dropzoneElementId, {
            url: mediaConfig.uploadUrl,
            paramName: "file",
            maxFilesize: mediaConfig.maxFileSize,
            acceptedFiles: mediaConfig.acceptedFiles,
            addRemoveLinks: false,
            dictDefaultMessage: "Kéo thả tập tin vào đây hoặc bấm để chọn",
            headers: { "RequestVerificationToken": this.antiForgeryToken }
        });

        this.myDropzone.on("success", (file, response) => {
            if (response.success) {
                this.renderFileItem(response.file, true);
                this.$modalEmptyIndicator.hide();
                toastr.success(`Đã tải lên "${response.file.originalFileName}"`);
            } else {
                toastr.error(`Tải lên "${file.name}" thất bại: ${response.message}`);
            }
        });

        this.myDropzone.on("error", (file, message, xhr) => {
            const errorMessage = xhr?.responseJSON?.message || message;
            toastr.error(`Tải lên "${file.name}" lỗi: ${errorMessage}`);
        });
    },

    handleModalShow: function (event) {
        const relatedTarget = $(event.relatedTarget);
        this.currentInputTarget = relatedTarget.closest(".input-group").find('input[type="text"], input[type="url"]');
        this.$modalSelectFileBtn.prop("disabled", true);
        this.$modalSelectedFileInfo.hide();
        const currentFilterValue = this.$modalMediaTypeFilter ? this.$modalMediaTypeFilter.val() : null;
        this.loadContent(currentFilterValue);
    },

    handleModalHide: function () {
        this.selectFileCallback = null;
        this.currentInputTarget = null;
        this.selectedFile = null;
        this.$modalBodyContent.html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải...</p></div>');
        if (this.myDropzone) {
            this.myDropzone.destroy();
            this.myDropzone = null;
        }
        this.$loadedContentContainer = null;
        this.$modalFiles = null;
        this.$modalMediaTypeFilter = null;
        this.$modalUploadArea = null;
        this.$modalLoadingIndicator = null;
        this.$modalEmptyIndicator = null;
    },

    handleModalSelectFile: function () {
        if (this.selectedFile && this.currentInputTarget?.length > 0) {
            this.currentInputTarget.val(this.selectedFile.url).trigger("change");
            const $previewArea = this.currentInputTarget.closest(".form-group").find(".media-preview-area");
            if ($previewArea.length > 0) {
                $previewArea.html(`<img src="${this.selectedFile.url}" style="max-width: 100px; height: auto;" class="img-thumbnail" />`);
            }
            this.$modal.modal("hide");
            if (typeof this.selectFileCallback === "function") {
                this.selectFileCallback(this.selectedFile);
            }
        } else {
            toastr.warning("Vui lòng chọn một tập tin.");
        }
    },

    loadContent: function (mediaTypeFilter = null) {
        this.$modalBodyContent.find("#media-browser-loading-indicator").show();
        this.$modalBodyContent.find("#media-browser-content").hide();

        $.ajax({
            url: "/Admin/Media/SelectMediaModalContent",
            type: "GET",
            data: { mediaTypeFilter },
            success: (html) => {
                this.$modalBodyContent.html(html);
                this.$loadedContentContainer = this.$modalBodyContent.find("#media-browser-content");
                this.$modalFiles = this.$loadedContentContainer.find("#media-browser-files");
                this.$modalMediaTypeFilter = this.$loadedContentContainer.find("#modal-media-type-filter");
                this.$modalUploadArea = this.$loadedContentContainer.find("#modal-upload-area");
                this.$modalLoadingIndicator = this.$loadedContentContainer.find("#media-browser-loading-indicator");
                this.$modalEmptyIndicator = this.$loadedContentContainer.find("#media-browser-empty-indicator");
                this.attachDynamicContentEvents();
                this.antiForgeryToken = this.$loadedContentContainer.data(mediaConfig.antiForgeryTokenKey);
                this.$modalLoadingIndicator.hide();
                this.$loadedContentContainer.show();
                this.$modalUploadArea.hide();
            },
            error: () => {
                this.$modalBodyContent.html('<div class="alert alert-danger">Không thể tải nội dung media.</div>');
                this.$modalBodyContent.find("#media-browser-loading-indicator").hide();
            }
        });
    },

    attachDynamicContentEvents: function () {
        this.$modalBodyContent.off("click", ".media-file-item").on("click", ".media-file-item", this.handleFileClick.bind(this));
        this.$modalBodyContent.off("change", "#modal-media-type-filter").on("change", "#modal-media-type-filter", this.handleFilterChange.bind(this));
        this.$modalBodyContent.off("click", "#toggle-upload-area").on("click", "#toggle-upload-area", this.handleToggleUploadArea.bind(this));
        this.$modalBodyContent.off("click", ".delete-file-btn").on("click", ".delete-file-btn", this.handleDeleteFileClick.bind(this));
    },

    handleFileClick: function (event) {
        event.preventDefault();
        const $target = $(event.currentTarget).closest(".media-file-item");
        this.$modalFiles.find(".media-file-item").removeClass("selected border border-primary");
        $target.addClass("selected border border-primary");
        this.$modalSelectFileBtn.prop("disabled", false);
        this.$modalSelectedFileInfo.show();
        this.$modalSelectedFileName.text($target.data("file-name"));
        this.selectedFile = {
            url: $target.data("file-url"),
            altText: $target.data("file-alt"),
            id: $target.data("file-id"),
            type: $target.data("file-type")
        };
    },

    handleFilterChange: function (event) {
        const mediaType = $(event.currentTarget).val();
        this.loadContent(mediaType);
    },

    handleToggleUploadArea: function (event) {
        event.preventDefault();
        this.$modalUploadArea = this.$loadedContentContainer.find("#modal-upload-area");
        if (this.$modalUploadArea.length > 0) {
            this.$modalUploadArea.slideToggle();
            if (!this.myDropzone) {
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
            this.executeDeleteFile(fileId, fileName, deleteUrl, $target);
        }
    },

    executeDeleteFile: function (fileId, fileName, deleteUrl, $buttonElement) {
        const originalButtonContent = $buttonElement.html();
        $buttonElement.prop("disabled", true).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

        $.ajax({
            url: deleteUrl,
            type: "POST",
            data: { id: fileId },
            headers: { "RequestVerificationToken": this.antiForgeryToken },
            success: (response) => {
                if (response.success) {
                    $buttonElement.closest(".media-file-item").remove();
                    this.checkEmptyState();
                    toastr.success(response.message);
                } else {
                    toastr.error(`Xóa tập tin "${fileName}" thất bại: ${response.message}`);
                }
            },
            error: () => {
                toastr.error("Đã xảy ra lỗi khi xóa tập tin.");
            },
            complete: () => {
                $buttonElement.prop("disabled", false).html(originalButtonContent);
            }
        });
    },

    renderFileItem: function (fileVM, isInModal = false) {
        const $filesContainer = isInModal ? this.$modalFiles : $("#media-file-grid");
        const mediaTypeString = fileVM.mediaTypeDisplayName || "Khác";
        const formattedFileSize = fileVM.formattedFileSize || `${(fileVM.fileSize / 1024).toFixed(1)} KB`;

        const itemHtml = `
            <div class="media-file-item cursor-pointer"
                 data-file-id="${fileVM.id}"
                 data-file-url="${fileVM.publicUrl}"
                 data-file-alt="${fileVM.altText}"
                 data-file-name="${fileVM.originalFileName}"
                 data-file-type="${mediaTypeString}"
                 data-bs-toggle="tooltip" data-bs-placement="bottom" title="${fileVM.originalFileName} (${formattedFileSize})">
               <div class="card card-sm">
                   <div class="card-img-top img-responsive img-responsive-1by1
                         ${fileVM.mediaType !== 0 ? 'text-muted bg-light d-flex align-items-center justify-content-center fs-1' : ''}"
                        style="${fileVM.mediaType === 0 ? `background-image: url('${fileVM.publicUrl}');` : ''}">
                       ${fileVM.mediaType !== 0 ? `
                       <i class="ti
                           ${fileVM.mediaType === 1 ? 'ti-video' : ''}
                           ${fileVM.mediaType === 2 ? 'ti-file-text' : ''}
                           ${fileVM.mediaType === 3 ? 'ti-file' : ''}"></i>` : ''}
                   </div>
                   <div class="card-body p-2 text-center">
                       <div class="text-truncate small mb-1">${fileVM.originalFileName}</div>
                       <div class="text-muted small">${formattedFileSize}</div>
                       ${fileVM.mediaType === 1 && fileVM.duration ? `<div class="text-muted small">${fileVM.duration} min</div>` : ''}
                   </div>
                   ${!isInModal ? `
                     <div class="card-footer text-center p-1">
                         <div class="btn-list flex-nowrap justify-content-center">
                             <button class="btn btn-sm btn-outline-danger delete-file-btn" data-id="${fileVM.id}" data-name="${fileVM.originalFileName}" title="Xóa" data-delete-url="/Admin/Media/DeleteFile">
                                  <i class="ti ti-trash"></i>
                             </button>
                         </div>
                     </div>` : ''}
               </div>
           </div>`;
        $filesContainer.prepend(itemHtml);
    },

    checkEmptyState: function () {
        this.$modalFiles = this.$loadedContentContainer.find("#media-browser-files");
        const fileCount = this.$modalFiles.find(".media-file-item").length;
        this.$modalEmptyIndicator = this.$loadedContentContainer.find("#media-browser-empty-indicator");
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
