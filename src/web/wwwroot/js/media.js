const mediaConfig = {
    modalId: "#selectMediaModal",
    modalBodyContentSelector: "#select-media-modal-body-content",
    modalFilesContainerSelector: "#media-browser-files",
    modalSelectFileBtnSelector: "#modal-select-file-btn",
    modalSelectedFileInfoSelector: "#modal-selected-file-info",
    modalSelectedFileNameSelector: "#selected-file-name",
    modalLoadingIndicatorSelector: "#media-browser-loading-indicator",
    modalEmptyIndicatorSelector: "#media-browser-empty-indicator",
};

const MediaManager = {
    // --- MODAL PROPERTIES ---
    $modal: null,
    $modalBodyContent: null,
    $modalSelectFileBtn: null,
    $modalSelectedFileInfo: null,
    $modalSelectedFileName: null,

    $modalFilesContainer: null,
    $modalLoadingIndicator: null,
    $modalEmptyIndicator: null,

    selectFileCallback: null,
    currentInputTargetModal: null,
    selectedFileModal: null,

    /**
     * Khởi tạo MediaManager.
     */
    init: function () {
        this.$modal = $(mediaConfig.modalId);
        if (this.$modal.length > 0) {
            this.$modalBodyContent = this.$modal.find(mediaConfig.modalBodyContentSelector);
            this.$modalSelectFileBtn = this.$modal.find(mediaConfig.modalSelectFileBtnSelector);
            this.$modalSelectedFileInfo = this.$modal.find(mediaConfig.modalSelectedFileInfoSelector);
            this.$modalSelectedFileName = this.$modal.find(mediaConfig.modalSelectedFileNameSelector);

            this.$modal.on("show.bs.modal", this.handleModalShow.bind(this));
            this.$modal.on("hidden.bs.modal", this.handleModalHide.bind(this));
            this.$modalSelectFileBtn.on("click", this.handleModalConfirmSelection.bind(this));
        }
    },

    /**
     * Mở modal chọn file.
     */
    openMediaModal: function (targetInput, callback = null) {
        this.currentInputTargetModal = $(targetInput);
        this.selectFileCallback = callback;
        this.loadModalContent();
        this.$modal.modal("show");
    },

    handleModalShow: function (event) {
        const relatedTarget = $(event.relatedTarget);
        if (relatedTarget && relatedTarget.length > 0) {
            const $inputGroup = relatedTarget.closest('.input-group');
            if ($inputGroup.length > 0) {
                this.currentInputTargetModal = $inputGroup.find('input.media-url-input, input[type="text"], input[type="url"]');
            } else {
                const targetData = relatedTarget.data('target-input');
                if (targetData) this.currentInputTargetModal = $(targetData);
            }
        }
        this.$modalSelectFileBtn.prop("disabled", true);
        this.$modalSelectedFileInfo.hide();
        this.selectedFileModal = null;
        if (!this.$modalBodyContent.find('#media-browser-content').length || this.$modalFilesContainer === null) {
            this.loadModalContent();
        }
    },

    handleModalHide: function () {
        this.selectFileCallback = null;
        this.currentInputTargetModal = null;
        this.selectedFileModal = null;
        this.$modalBodyContent.html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải...</p></div>');

        this.$modalBodyContent.off("click", ".media-file-item");
        this.$modalFilesContainer = null;
        this.$modalLoadingIndicator = null;
        this.$modalEmptyIndicator = null;
    },

    handleModalConfirmSelection: function () {
        if (this.selectedFileModal && this.currentInputTargetModal && this.currentInputTargetModal.length > 0) {
            this.currentInputTargetModal.val(this.selectedFileModal.url).trigger("change");
            if (typeof this.selectFileCallback === "function") {
                this.selectFileCallback(this.selectedFileModal);
            }
            this.$modal.modal("hide");
        }
    },

    /**
     * Tải nội dung (tất cả các file media) cho modal.
     */
    loadModalContent: function () {
        this.$modalBodyContent.html('<div class="text-center p-5" id="media-browser-loading-initial"><div class="spinner-border text-primary" role="status"></div><p class="mt-2 text-muted">Đang tải thư viện...</p></div>');

        $.ajax({
            url: "/Admin/Media/SelectMediaModalContent",
            type: "GET",
            data: {
            },
            success: (html) => {
                this.$modalBodyContent.html(html);
                this.assignModalDynamicElements();
                this.attachModalDynamicEvents();
                this.checkModalEmptyState();
            },
            error: (xhr, status, error) => {
                console.error("Lỗi tải thư viện media:", status, error);
                this.$modalBodyContent.html('<div class="alert alert-danger m-3">Không thể tải thư viện media. Vui lòng thử lại.</div>');
            },
            complete: () => {
                this.$modalBodyContent.find('#media-browser-loading-initial').remove();
                this.$modalLoadingIndicator?.hide();
                this.$modalFilesContainer?.show();
            }
        });
    },

    assignModalDynamicElements: function () {
        this.$modalFilesContainer = this.$modalBodyContent.find(mediaConfig.modalFilesContainerSelector);
        this.$modalLoadingIndicator = this.$modalBodyContent.find(mediaConfig.modalLoadingIndicatorSelector);
        this.$modalEmptyIndicator = this.$modalBodyContent.find(mediaConfig.modalEmptyIndicatorSelector);
    },

    attachModalDynamicEvents: function () {
        this.$modalBodyContent.off("click", ".media-file-item").on("click", ".media-file-item", this.handleModalFileItemClick.bind(this));
    },

    checkModalEmptyState: function () {
        if (!this.$modalFilesContainer || !this.$modalEmptyIndicator) return;
        const hasItems = this.$modalFilesContainer.find(".media-file-item").length > 0;
        this.$modalEmptyIndicator.toggle(!hasItems);
    },

    handleModalFileItemClick: function (event) {
        event.preventDefault();
        const $fileItemDiv = $(event.currentTarget);

        this.$modalFilesContainer.find(".media-file-item").removeClass("border-2 border-primary shadow-sm");
        $fileItemDiv.addClass("border-2 border-primary shadow-sm");

        this.$modalSelectFileBtn.prop("disabled", false);
        this.$modalSelectedFileInfo.css('display', 'flex');
        this.$modalSelectedFileName.text($fileItemDiv.data("file-name"));

        this.selectedFileModal = {
            url: $fileItemDiv.data("file-url"),
            altText: $fileItemDiv.data("file-alt") || '',
            id: $fileItemDiv.data("file-id"),
            type: $fileItemDiv.data("file-type"),
            name: $fileItemDiv.data("file-name")
        };
    },
};

window.openMediaModal = MediaManager.openMediaModal.bind(MediaManager);
$(function () { MediaManager.init(); });