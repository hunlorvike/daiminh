/**
 * daiminhjs - Một thư viện JavaScript nhẹ dành cho ứng dụng ASP.NET Core MVC
 * @version 2.0.0
 * @requires jQuery, Bootstrap, DataTables
 */
const jqueryDaiminh = (($, bootstrap) => {
    'use strict';

    /**
     * Cấu hình mặc định cho thư viện
     * @type {Object}
     */
    const Config = {
        /**
         * Các bộ chọn CSS được sử dụng trong thư viện
         * @type {Object}
         */
        selectors: {
            dataTable: '#table',
            dtMenu: '.dt-input',
            dtSearch: '.dt-search',
            dtLength: '.dt-length',
            pagingNav: '.dt-paging nav',
            pagingButton: '.dt-paging-button',
            formContainer: '.dm-form',
            submitButton: '.command-submit',
            modalDetail: '.command-modal-detail',
            modalCreate: '.command-modal-create',
            modalEdit: '.command-modal-edit',
            modalDelete: '.command-modal-delete',
            modalClose: '.command-modal-close',
            detailContainer: '.form-details-container',
            createContainer: '.form-create-container',
            editContainer: '.form-edit-container',
            deleteContainer: '.form-delete-container',
            notificationContainer: '.notification-container'
        },
        /**
         * Các giá trị mặc định cho các thành phần
         * @type {Object}
         */
        defaults: {
            dataTable: { paging: true, searching: true, serverSide: false, pageLength: 10 },
            modal: { backdrop: true, disableTimeout: 500 },
            notification: { duration: 3000, position: 'top-right' },
            logging: false,
            i18n: { defaultLocale: 'en', fallbackLocale: 'en' },
        }, /**
         * Cập nhật cấu hình với các tùy chọn mới
         * @param {Object} newConfig - Cấu hình mới cần được áp dụng
         * @returns {Object} Cấu hình đã được cập nhật
         */
        update: function (newConfig) {
            const mergeDeep = (target, source) => {
                for (const key in source) {
                    if (source[key] instanceof Object && key in target) {
                        mergeDeep(target[key], source[key]);
                    } else {
                        target[key] = source[key];
                    }
                }
                return target;
            };

            return mergeDeep(this, newConfig);
        }
    };

    /**
     * Các tiện ích hỗ trợ cho thư viện
     * @type {Object}
     */
    const Utils = {
        /**
         * Vô hiệu hóa nút trong một khoảng thời gian
         * @param {jQuery} $button - Đối tượng jQuery của nút cần vô hiệu hóa
         * @param {number} [timeout=Config.defaults.modal.disableTimeout] - Thời gian vô hiệu hóa (milliseconds)
         * @returns {void}
         */
        disableButton: ($button, timeout = Config.defaults.modal.disableTimeout) => {
            $button.prop('disabled', true);
            setTimeout(() => $button.prop('disabled', false), timeout);
        },

        /**
         * Tạo slug từ văn bản
         * @param {string} text - Văn bản cần chuyển đổi thành slug
         * @returns {string} Chuỗi slug đã được tạo
         * @example
         * Utils.generateSlug("Tiếng Việt"); // "tieng-viet"
         */
        generateSlug: (text) => text.toLowerCase().trim().replace(/\s+/g, "-").replace(/[^a-z0-9-]/g, ""),

        /**
         * Ghi log nếu chế độ ghi log được bật
         * @param {string} message - Tin nhắn cần ghi log
         * @param {...*} args - Các tham số bổ sung cho log
         * @returns {void}
         */
        log: (message, ...args) => Config.defaults.logging && console.log(`[daiminhjs] ${message}`, ...args),

        /**
         * Trì hoãn thực thi hàm để ngăn chặn gọi quá nhiều lần
         * @param {Function} func - Hàm cần trì hoãn
         * @param {number} wait - Thời gian chờ (milliseconds)
         * @returns {Function} Hàm đã được xử lý trì hoãn
         */
        debounce: (func, wait) => {
            let timeout;
            return function executedFunction(...args) {
                const later = () => {
                    clearTimeout(timeout);
                    func(...args);
                };
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
            };
        },

        /**
         * Giới hạn tần suất gọi hàm
         * @param {Function} func - Hàm cần giới hạn
         * @param {number} limit - Thời gian giới hạn (milliseconds)
         * @returns {Function} Hàm đã được xử lý giới hạn
         */
        throttle: (func, limit) => {
            let inThrottle;
            return function executedFunction(...args) {
                if (!inThrottle) {
                    func(...args);
                    inThrottle = true;
                    setTimeout(() => inThrottle = false, limit);
                }
            };
        },

        /**
         * Định dạng ngày tháng theo mẫu chỉ định
         * @param {Date|string} date - Đối tượng Date hoặc chuỗi ngày tháng
         * @param {string} [format='YYYY-MM-DD'] - Định dạng mong muốn (YYYY: năm, MM: tháng, DD: ngày, HH: giờ, mm: phút, ss: giây)
         * @returns {string} Chuỗi ngày tháng đã định dạng
         * @example
         * Utils.formatDate(new Date(2023, 0, 15), "DD/MM/YYYY"); // "15/01/2023"
         */
        formatDate: (date, format = 'YYYY-MM-DD') => {
            const d = new Date(date);
            const year = d.getFullYear();
            const month = String(d.getMonth() + 1).padStart(2, '0');
            const day = String(d.getDate()).padStart(2, '0');
            const hours = String(d.getHours()).padStart(2, '0');
            const minutes = String(d.getMinutes()).padStart(2, '0');
            const seconds = String(d.getSeconds()).padStart(2, '0');

            return format
                .replace('YYYY', year)
                .replace('MM', month)
                .replace('DD', day)
                .replace('HH', hours)
                .replace('mm', minutes)
                .replace('ss', seconds);
        },

        /**
         * Định dạng số theo địa phương và các tùy chọn khác
         * @param {number} number - Số cần định dạng
         * @param {Object} [options={}] - Các tùy chọn định dạng
         * @param {string} [options.locale='en-US'] - Mã địa phương
         * @param {string} [options.style='decimal'] - Kiểu định dạng (decimal, currency, percent)
         * @param {number} [options.minimumFractionDigits=0] - Số chữ số thập phân tối thiểu
         * @param {number} [options.maximumFractionDigits=2] - Số chữ số thập phân tối đa
         * @returns {string} Chuỗi số đã định dạng
         * @example
         * Utils.formatNumber(1234.56, {locale: 'vi-VN', style: 'currency', currency: 'VND'}); // "1.234,56 ₫"
         */
        formatNumber: (number, options = {}) => {
            const defaults = {
                locale: 'en-US', style: 'decimal', minimumFractionDigits: 0, maximumFractionDigits: 2
            };
            const settings = { ...defaults, ...options };
            return new Intl.NumberFormat(settings.locale, settings).format(number);
        },

        /**
         * Sao chép văn bản vào clipboard
         * @param {string} text - Văn bản cần sao chép
         * @returns {void}
         */
        copyToClipboard: (text) => {
            navigator.clipboard.writeText(text)
                .then(() => Notification.show('success', I18n.t('clipboard.copied')))
                .catch(() => Notification.show('error', I18n.t('clipboard.failed')));
        }
    };

    /**
     * Quản lý đa ngôn ngữ
     * @type {Object}
     */
    const I18n = {
        /**
         * Dữ liệu dịch được lưu trữ theo ngôn ngữ
         * @type {Object}
         */
        translations: {
            en: {
                common: {
                    save: 'Save',
                    cancel: 'Cancel',
                    delete: 'Delete',
                    edit: 'Edit',
                    create: 'Create',
                    details: 'Details',
                    close: 'Close',
                    search: 'Search',
                    loading: 'Loading...',
                    noData: 'No data available'
                },
                dataTable: {
                    lengthMenu: 'Show _MENU_ entries',
                    info: 'Showing _START_ to _END_ of _TOTAL_ entries',
                    infoEmpty: 'Showing 0 to 0 of 0 entries',
                    infoFiltered: '(filtered from _MAX_ total entries)',
                    paginate: {
                        first: 'First',
                        previous: 'Previous',
                        next: 'Next',
                        last: 'Last'
                    },
                    processing: 'Processing...',
                    zeroRecords: 'No matching records found',
                    emptyTable: 'No data available in table'
                },
                notification: {
                    success: 'Success',
                    error: 'Error',
                    warning: 'Warning',
                    info: 'Information'
                },
                modal: {
                    loadError: 'Failed to load {type} data'
                },
                clipboard: {
                    copied: 'Copied to clipboard',
                    failed: 'Failed to copy to clipboard'
                },
                validation: {
                    required: 'This field is required',
                    email: 'Please enter a valid email address',
                    minLength: 'Please enter at least {min} characters',
                    maxLength: 'Please enter no more than {max} characters',
                    pattern: 'Please enter a valid format',
                    number: 'Please enter a valid number',
                    min: 'Please enter a value greater than or equal to {min}',
                    max: 'Please enter a value less than or equal to {max}'
                },
                form: {
                    submitting: 'Submitting...',
                    submitted: 'Form submitted successfully',
                    error: 'There was an error submitting the form'
                },
                pagination: {
                    previous: 'Previous',
                    next: 'Next',
                    showing: 'Showing',
                    to: 'to',
                    of: 'of',
                    results: 'results'
                },
                theme: {
                    light: 'Light Mode',
                    dark: 'Dark Mode',
                    toggle: 'Toggle Theme'
                }
            },
            vi: {
                common: {
                    save: 'Lưu',
                    cancel: 'Hủy',
                    delete: 'Xóa',
                    edit: 'Sửa',
                    create: 'Tạo mới',
                    details: 'Chi tiết',
                    close: 'Đóng',
                    search: 'Tìm kiếm',
                    loading: 'Đang tải...',
                    noData: 'Không có dữ liệu'
                },
                dataTable: {
                    lengthMenu: 'Hiển thị _MENU_ mục',
                    info: 'Hiển thị _START_ đến _END_ của _TOTAL_ mục',
                    infoEmpty: 'Hiển thị 0 đến 0 của 0 mục',
                    infoFiltered: '(lọc từ _MAX_ mục)',
                    paginate: {
                        first: 'Đầu tiên',
                        previous: 'Trước',
                        next: 'Tiếp',
                        last: 'Cuối cùng'
                    },
                    processing: 'Đang xử lý...',
                    zeroRecords: 'Không tìm thấy kết quả phù hợp',
                    emptyTable: 'Không có dữ liệu trong bảng'
                },
                notification: {
                    success: 'Thành công',
                    error: 'Lỗi',
                    warning: 'Cảnh báo',
                    info: 'Thông tin'
                },
                modal: {
                    loadError: 'Không thể tải dữ liệu {type}'
                },
                clipboard: {
                    copied: 'Đã sao chép vào clipboard',
                    failed: 'Không thể sao chép vào clipboard'
                },
                validation: {
                    required: 'Trường này là bắt buộc',
                    email: 'Vui lòng nhập địa chỉ email hợp lệ',
                    minLength: 'Vui lòng nhập ít nhất {min} ký tự',
                    maxLength: 'Vui lòng nhập không quá {max} ký tự',
                    pattern: 'Vui lòng nhập đúng định dạng',
                    number: 'Vui lòng nhập số hợp lệ',
                    min: 'Vui lòng nhập giá trị lớn hơn hoặc bằng {min}',
                    max: 'Vui lòng nhập giá trị nhỏ hơn hoặc bằng {max}'
                },
                form: {
                    submitting: 'Đang gửi...',
                    submitted: 'Biểu mẫu đã được gửi thành công',
                    error: 'Đã xảy ra lỗi khi gửi biểu mẫu'
                },
                pagination: {
                    previous: 'Trước',
                    next: 'Tiếp',
                    showing: 'Hiển thị',
                    to: 'đến',
                    of: 'của',
                    results: 'kết quả'
                },
                theme: {
                    light: 'Chế độ sáng',
                    dark: 'Chế độ tối',
                    toggle: 'Chuyển đổi chế độ'
                }
            }
        }, /**
         * Ngôn ngữ hiện tại đang sử dụng
         * @type {string}
         */
        currentLocale: Config.defaults.i18n.defaultLocale,

        /**
         * Thêm dữ liệu dịch mới cho một ngôn ngữ
         * @param {string} locale - Mã ngôn ngữ (ví dụ: 'vi', 'en')
         * @param {Object} translations - Đối tượng chứa dữ liệu dịch
         * @returns {Object} Đối tượng I18n (để hỗ trợ gọi móc xích)
         * @example
         * I18n.addTranslations('vi', {
         *   common: {
         *     save: 'Lưu',
         *     cancel: 'Hủy'
         *   }
         * });
         */
        addTranslations: function (locale, translations) {
            if (!this.translations[locale]) {
                this.translations[locale] = {};
            }

            const mergeDeep = (target, source) => {
                for (const key in source) {
                    if (source[key] instanceof Object && key in target) {
                        mergeDeep(target[key], source[key]);
                    } else {
                        target[key] = source[key];
                    }
                }
                return target;
            };

            mergeDeep(this.translations[locale], translations);
            return this;
        },

        /**
         * Thiết lập ngôn ngữ hiện tại
         * @param {string} locale - Mã ngôn ngữ cần thiết lập
         * @returns {Object} Đối tượng I18n (để hỗ trợ gọi móc xích)
         * @example
         * I18n.setLocale('vi');
         */
        setLocale: function (locale) {
            if (this.translations[locale]) {
                this.currentLocale = locale;
            } else {
                Utils.log(`Locale ${locale} not found, using ${this.currentLocale}`);
            }
            return this;
        },

        /**
         * Lấy chuỗi dịch theo khóa
         * @param {string} key - Khóa cần lấy (định dạng: 'section.subsection.key')
         * @param {Object} [replacements={}] - Các tham số thay thế trong chuỗi
         * @returns {string} Chuỗi đã dịch
         * @example
         * // Giả sử dữ liệu dịch: { common: { hello: 'Xin chào {name}' } }
         * I18n.t('common.hello', {name: 'John'}); // "Xin chào John"
         */
        t: function (key, replacements = {}) {
            const getNestedValue = (obj, path) => {
                return path.split('.').reduce((prev, curr) => {
                    return prev ? prev[curr] : null;
                }, obj);
            };

            let translation = getNestedValue(this.translations[this.currentLocale], key);

            if (!translation && this.currentLocale !== Config.defaults.i18n.fallbackLocale) {
                translation = getNestedValue(this.translations[Config.defaults.i18n.fallbackLocale], key);
            }

            if (!translation) {
                return key;
            }

            let result = translation;
            for (const [placeholder, value] of Object.entries(replacements)) {
                result = result.replace(new RegExp(`{${placeholder}}`, 'g'), value);
            }

            return result;
        }
    };

    /**
     * Quản lý và tương tác với DataTables
     * @type {Object}
     */
    const DataTable = {
        /**
         * Khởi tạo bảng dữ liệu với DataTables
         * @param {string} selector - Bộ chọn CSS để tìm bảng
         * @param {Object} [customOptions={}] - Tùy chọn tùy chỉnh cho DataTables
         * @returns {Object} Đối tượng DataTable đã khởi tạo
         * @example
         * const table = DataTable.initialize('#my-table', {
         *   ordering: false,
         *   pageLength: 20
         * });
         */
        initialize: (selector, customOptions = {}) => {
            const options = {
                ...Config.defaults.dataTable,
                dom: '<"card-header d-flex justify-content-between align-items-center"<"col-md-6"l><"col-md-6"f>>' + '<"table-responsive"t>' + '<"card-footer d-flex justify-content-between align-items-center"ip>',
                language: {
                    search: "",
                    searchPlaceholder: I18n.t('common.search'),
                    emptyTable: I18n.t('common.noData'),
                    info: I18n.t('dataTable.info', { start: '_START_', end: '_END_', total: '_TOTAL_' }),
                    infoEmpty: I18n.t('dataTable.infoEmpty'),
                    infoFiltered: I18n.t('dataTable.infoFiltered', { max: '_MAX_' }),
                    lengthMenu: I18n.t('dataTable.lengthMenu', { menu: '_MENU_' }),
                    loadingRecords: I18n.t('common.loading'),
                    processing: I18n.t('common.loading'),
                    zeroRecords: I18n.t('common.noData'),
                    paginate: {
                        first: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M11 7l-5 5l5 5" /><path d="M17 7l-5 5l5 5" /></svg>',
                        previous: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M15 6l-6 6l6 6" /></svg>',
                        next: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M9 6l6 6l-6 6" /></svg>',
                        last: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M7 7l5 5l-5 5" /><path d="M13 7l5 5l-5 5" /></svg>'
                    }
                },
                initComplete: (settings, json) => {
                    DataTable._applyBootstrapStyling();
                    if (customOptions.initComplete) {
                        customOptions.initComplete(settings, json);
                    }
                },
                drawCallback: (settings) => {
                    DataTable._applyBootstrapStyling();
                    if (customOptions.drawCallback) {
                        customOptions.drawCallback(settings);
                    }
                }, ...customOptions
            };

            const table = $(selector).DataTable(options);

            const id = $(selector).attr('id') || 'dataTable' + Object.keys(Main.instances.dataTables).length;
            Main.instances.dataTables[id] = table;

            return table;
        },

        /**
         * Áp dụng các kiểu Bootstrap cho các phần tử DataTables
         * @private
         * @returns {void}
         */
        _applyBootstrapStyling: () => {
            $(Config.selectors.dtMenu)
                .addClass('form-select form-select-sm d-inline-block w-50')

            $(Config.selectors.dtSearch)
                .addClass('d-flex align-items-center justify-content-end')
                .find('input')
                .addClass('form-control form-control-sm w-50');

            $(Config.selectors.dtLength)
                .find('label')
                .addClass('w-50');

            $(Config.selectors.pagingNav)
                .addClass('pagination justify-content-center m-0');

            const $pagingButtons = $(Config.selectors.pagingButton);
            $pagingButtons.addClass('page-link');

            $pagingButtons.each(function () {
                const $button = $(this);

                if (!$button.parent().hasClass('page-item')) {
                    $button.wrap('<li class="page-item"></li>');
                }

                if ($button.hasClass('current')) {
                    $button.parent().addClass('active');
                }

                if ($button.hasClass('disabled')) {
                    $button.parent().addClass('disabled');
                }
            });
        },

        /**
         * Làm mới dữ liệu bảng
         * @param {Object} table - Đối tượng DataTable cần làm mới
         * @returns {void}
         */
        refresh: (table) => {
            if (!table) return;
            if (table.ajax) {
                table.ajax.reload();
            } else {
                table.draw();
            }
        },

        /**
         * Hủy bảng dữ liệu
         * @param {Object} table - Đối tượng DataTable cần hủy
         * @returns {void}
         */
        destroy: (table) => {
            if (table?.destroy) {
                table.destroy();
            }
        },

        /**
         * Xuất dữ liệu từ bảng sang các định dạng khác nhau
         * @param {Object} table - Đối tượng DataTable cần xuất dữ liệu
         * @param {string} [format='csv'] - Định dạng xuất ('csv', 'json', 'excel')
         * @param {string} [filename='export'] - Tên tệp xuất (không bao gồm phần mở rộng)
         * @returns {string|null} Nội dung đã xuất hoặc null nếu có lỗi
         * @example
         * DataTable.exportData(table, 'json', 'data-export-2023');
         */
        exportData: (table, format = 'csv', filename = 'export') => {
            if (!table) return;

            const data = table.data().toArray();
            const headers = table.columns().header().toArray().map(header => $(header).text());

            switch (format.toLowerCase()) {
                case 'csv':
                    return DataTable._exportCsv(data, headers, filename);
                case 'json':
                    return DataTable._exportJson(data, headers, filename);
                case 'excel':
                    return DataTable._exportExcel(data, headers, filename);
                default:
                    Utils.log(`Unsupported export format: ${format}`);
                    return null;
            }
        },

        /**
         * Xuất dữ liệu sang định dạng CSV
         * @private
         * @param {Array} data - Dữ liệu cần xuất
         * @param {Array} headers - Tiêu đề cột
         * @param {string} filename - Tên tệp xuất
         * @returns {string} Nội dung CSV
         */
        _exportCsv: (data, headers, filename) => {
            const csvContent = [headers.join(','), ...data.map(row => Object.values(row).join(','))].join('\n');

            const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${filename}.csv`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            return csvContent;
        },

        /**
         * Xuất dữ liệu sang định dạng JSON
         * @private
         * @param {Array} data - Dữ liệu cần xuất
         * @param {Array} headers - Tiêu đề cột
         * @param {string} filename - Tên tệp xuất
         * @returns {string} Nội dung JSON
         */
        _exportJson: (data, headers, filename) => {
            const jsonData = data.map(row => {
                const rowObj = {};
                headers.forEach((header, index) => {
                    rowObj[header] = row[index];
                });
                return rowObj;
            });

            const jsonContent = JSON.stringify(jsonData, null, 2);
            const blob = new Blob([jsonContent], { type: 'application/json;charset=utf-8;' });
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${filename}.json`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            return jsonContent;
        },

        /**
         * Xuất dữ liệu sang định dạng Excel
         * @private
         * @param {Array} data - Dữ liệu cần xuất
         * @param {Array} headers - Tiêu đề cột
         * @param {string} filename - Tên tệp xuất
         * @returns {string} Nội dung Excel (hiện tại chỉ là CSV)
         */
        _exportExcel: (data, headers, filename) => {
            return DataTable._exportCsv(data, headers, filename);
        }
    };

    /**
     * Quản lý và xử lý biểu mẫu
     * @type {Object}
     */
    const Form = {
        /**
         * Khởi tạo sự kiện cho biểu mẫu
         * @returns {void}
         */
        init: () => {
            $(document).on('click', `${Config.selectors.submitButton}`, (e) => {
                e.preventDefault();
                const $form = $(e.target).closest('form');
                Form.submit($form);
            });
        },

        /**
         * Gửi biểu mẫu bằng Ajax
         * @param {jQuery} $form - Đối tượng jQuery của biểu mẫu cần gửi
         * @returns {Promise} Promise kết quả gửi biểu mẫu
         * @example
         * $("#myForm").on("submit", function(e) {
         *   e.preventDefault();
         *   Form.submit($(this));
         * });
         */
        submit: async ($form) => {
            console.log($form)
            const url = $form.attr('action');
            const method = $form.attr('method') || 'POST';
            const $submitBtn = $form.find(Config.selectors.submitButton);

            Utils.disableButton($submitBtn);

            const hasFiles = $form.find('input[type="file"]').length > 0 &&
                $form.find('input[type="file"]').get().some(input => input.files.length > 0);

            try {
                let response;

                if (hasFiles) {
                    // Use FormData for file uploads
                    const formData = new FormData($form[0]);

                    response = await $.ajax({
                        url: url,
                        type: method,
                        data: formData,
                        processData: false,  // Don't process the data
                        contentType: false,  // Don't set content type
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
                        }
                    });

                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                } else {
                    // Use regular serialization for non-file forms
                    response = await $.ajax({
                        url: url,
                        type: method,
                        data: Form.serializeObject($form),
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
                        }
                    });

                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                }

                return response;
            } catch (error) {
                const response = error.responseJSON || {};
                const errors = response.errors || {};

                console.log(errors);
                $form.find('span[data-valmsg-for]').text('');

                if (Object.keys(errors).length > 0) {
                    Object.keys(errors).forEach(key => {
                        const $errorSpan = $form.find(`span[data-valmsg-for="${key}"]`);
                        if ($errorSpan.length) {
                            $errorSpan.text(errors[key][0]);
                            $errorSpan.removeClass('field-validation-valid').addClass('field-validation-error');
                        } else {
                            console.warn(`No error span found for field: ${key}`);
                        }
                    });
                } else {
                    // Show generic error if no specific errors returned
                    Notification.show('error', 'Đã xảy ra lỗi khi xử lý yêu cầu.');
                }

                throw error;
            } finally {
                // Re-enable the button after processing
                setTimeout(() => $submitBtn.prop('disabled', false), Config.defaults.modal.disableTimeout);
            }
        },

        /**
         * Chuyển đổi biểu mẫu thành đối tượng JavaScript
         * @param {jQuery} $form - Đối tượng jQuery của biểu mẫu
         * @returns {Object} Đối tượng chứa dữ liệu biểu mẫu
         * @example
         * const formData = Form.serializeObject($("#myForm"));
         * console.log(formData.userName); // Truy cập giá trị trường userName
         */
        serializeObject: ($form) => {
            if (!$form.length) return {};

            const formArray = $form.serializeArray();
            const formObject = {};

            formArray.forEach(item => {
                formObject[item.name] = item.value;
            });

            $form.find('input[type="checkbox"], select[multiple]').each(function () {
                const $input = $(this);
                const name = $input.attr('name');

                if ($input.is('input[type="checkbox"]')) {
                    formObject[name] = $input.prop('checked');
                } else if ($input.is('select[multiple]')) {
                    formObject[name] = $input.val() || [];
                }
            });

            return formObject;
        }
    };

    /**
     * Quản lý và tương tác với cửa sổ modal
     * @type {Object}
     */
    const Modal = {
        /**
         * Lưu trữ các thể hiện modal
         * @type {Object}
         */
        instances: {},

        /**
         * Khởi tạo sự kiện cho modal
         * @returns {void}
         */
        init: () => {
            Modal._setupModalAction({
                buttonSelector: Config.selectors.modalDetail,
                containerSelector: Config.selectors.detailContainer,
                modalType: 'detail'
            });
            Modal._setupModalAction({
                buttonSelector: Config.selectors.modalCreate,
                containerSelector: Config.selectors.createContainer,
                modalType: 'create'
            });
            Modal._setupModalAction({
                buttonSelector: Config.selectors.modalEdit,
                containerSelector: Config.selectors.editContainer,
                modalType: 'edit'
            });
            Modal._setupModalAction({
                buttonSelector: Config.selectors.modalDelete,
                containerSelector: Config.selectors.deleteContainer,
                modalType: 'delete'
            });

            $(document).on('click', Config.selectors.modalClose, (e) => {
                const modalId = $(e.target).closest('.modal').attr('id');
                Modal.hide(modalId);
            });

            $(document).on('shown.bs.modal', '.modal', function () {
                const modalId = $(this).attr('id');
            });

            $(document).on('hidden.bs.modal', '.modal', function () {
                const modalId = $(this).attr('id');
            });

        },

        /**
         * Thiết lập hành động cho các nút modal
         * @private
         * @param {Object} options - Tùy chọn cấu hình
         * @param {string} options.buttonSelector - Bộ chọn CSS cho nút kích hoạt
         * @param {string} options.containerSelector - Bộ chọn CSS cho vùng chứa nội dung
         * @param {string} options.modalType - Loại modal ('detail', 'create', 'edit', 'delete')
         * @returns {void}
         */
        _setupModalAction: ({ buttonSelector, containerSelector, modalType }) => {
            $(document).on('click', buttonSelector, async (e) => {
                e.preventDefault();
                const $button = $(e.target).closest(buttonSelector);
                const modalId = $button.data('bs-target') || $button.attr('href');
                const targetUrl = $button.attr('formaction') || $button.attr('href');

                Utils.disableButton($button);

                $.ajax({
                    url: targetUrl, type: 'GET', beforeSend: function (xhr) {
                        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
                    }, success: function (response) {
                        $(containerSelector).html(response);
                        Modal.show(modalId);
                    }, error: function () {
                        Notification.show('error', I18n.t('modal.loadError', { type: modalType }));
                    }
                });
            });
        },

        /**
         * Hiển thị cửa sổ modal
         * @param {string} modalId - ID của modal cần hiển thị
         * @returns {Object|undefined} Thể hiện của modal hoặc undefined nếu không tìm thấy
         * @example
         * Modal.show('userDetailsModal');
         */
        show: (modalId) => {
            if (!modalId) return;

            const id = modalId.startsWith('#') ? modalId.substring(1) : modalId;
            const $modal = $(`#${id}`);

            if (!$modal.length) {
                Utils.log(`Modal with ID ${id} not found`);
                return;
            }

            const instance = bootstrap.Modal.getOrCreateInstance($modal[0], {
                backdrop: Config.defaults.modal.backdrop, keyboard: true, focus: true
            });

            Modal.instances[id] = instance;
            instance.show();

            return instance;
        },

        /**
         * Ẩn cửa sổ modal
         * @param {string} modalId - ID của modal cần ẩn
         * @returns {boolean} true nếu thành công, false nếu không tìm thấy modal
         * @example
         * Modal.hide('userDetailsModal');
         */
        hide: (modalId) => {
            if (!modalId) return;

            const id = modalId.startsWith('#') ? modalId.substring(1) : modalId;

            const instance = Modal.instances[id] || bootstrap.Modal.getInstance(document.getElementById(id));
            console.log(instance);

            if (instance) {
                const $container = instance._element.closest('.dm-form');
                instance.hide();
                if ($container) $container.innerHTML = '';
                return true;
            }

            return false;
        },

        /**
         * Tạo cửa sổ modal động
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {string} [options.id] - ID của modal
         * @param {string} [options.title=''] - Tiêu đề
         * @param {string} [options.body=''] - Nội dung
         * @param {string} [options.size=''] - Kích thước ('modal-sm', 'modal-lg', 'modal-xl')
         * @param {Array} [options.buttons] - Mảng các nút
         * @param {Function} [options.onShow] - Hàm gọi khi hiển thị
         * @param {Function} [options.onHide] - Hàm gọi khi ẩn
         * @returns {Object} Đối tượng điều khiển modal
         * @example
         * const modal = Modal.createDynamic({
         *   title: 'Thông báo',
         *   body: 'Bạn có chắc chắn muốn xóa mục này?',
         *   buttons: [
         *     { text: 'Hủy', class: 'btn-secondary', dismiss: true },
         *     { text: 'Xóa', class: 'btn-danger', onClick: () => deleteItem(1) }
         *   ]
         * });
         */
        createDynamic: (options = {}) => {
            const defaults = {
                id: 'dynamicModal' + Date.now(), title: '', body: '', size: '', // '', 'modal-sm', 'modal-lg', 'modal-xl'
                buttons: [{
                    text: I18n.t('common.close'), class: 'btn-secondary', dismiss: true
                }], onShow: null, onHide: null
            };

            const settings = { ...defaults, ...options };

            const modalHtml = `
                <div class="modal fade" id="${settings.id}" tabindex="-1" aria-labelledby="${settings.id}Label" aria-hidden="true">
                    <div class="modal-dialog ${settings.size}">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="${settings.id}Label">${settings.title}</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                ${settings.body}
                            </div>
                            <div class="modal-footer">
                                ${settings.buttons.map(btn => `
                                    <button type="button" class="btn ${btn.class}" ${btn.dismiss ? 'data-bs-dismiss="modal"' : ''} ${btn.id ? `id="${btn.id}"` : ''}>${btn.text}</button>
                                `).join('')}
                            </div>
                        </div>
                    </div>
                </div>
            `;

            $(`#${settings.id}`).remove();

            $('body').append(modalHtml);

            const $modal = $(`#${settings.id}`);

            if (settings.onShow) {
                $modal.on('shown.bs.modal', settings.onShow);
            }

            if (settings.onHide) {
                $modal.on('hidden.bs.modal', settings.onHide);
            }

            settings.buttons.forEach((btn, index) => {
                if (btn.onClick) {
                    $modal.find('.modal-footer .btn').eq(index).on('click', btn.onClick);
                }
            });

            Modal.show(settings.id);

            return {
                id: settings.id,
                element: $modal,
                show: () => Modal.show(settings.id),
                hide: () => Modal.hide(settings.id)
            };
        }
    };

    /**
     * Quản lý và hiển thị thông báo
     * @type {Object}
     */
    const Notification = {
        /**
         * Hiển thị thông báo
         * @param {string} type - Loại thông báo ('success', 'error', 'warning', 'info')
         * @param {string} message - Nội dung thông báo
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {number} [options.duration] - Thời gian hiển thị (ms)
         * @param {string} [options.position] - Vị trí ('top-right', 'top-left', 'bottom-right', 'bottom-left')
         * @param {string} [options.title] - Tiêu đề thông báo
         * @param {boolean} [options.icon] - Hiển thị biểu tượng
         * @param {boolean} [options.dismissible] - Cho phép đóng thông báo
         * @param {Function} [options.onClose] - Hàm gọi khi đóng thông báo
         * @returns {string} ID của thông báo
         * @example
         * Notification.show('success', 'Dữ liệu đã được lưu thành công');
         */
        show: (type, message, options = {}) => {
            const defaults = {
                duration: Config.defaults.notification.duration,
                position: Config.defaults.notification.position,
                title: I18n.t(`notification.${type}`),
                icon: true,
                dismissible: true,
                onClose: null
            };

            const settings = { ...defaults, ...options };

            let $container = $(Config.selectors.notificationContainer);
            if (!$container.length) {
                $('body').append(`<div class="notification-container position-fixed ${settings.position}" style="z-index: 9999;"></div>`);
                $container = $(Config.selectors.notificationContainer);
            }

            $container.removeClass('top-0 top-50 bottom-0 start-0 start-50 end-0')
                .addClass(settings.position.split('-').map(pos => pos === 'right' ? 'end-0' : pos === 'left' ? 'start-0' : pos === 'center' ? 'start-50 translate-middle-x' : `${pos}-0`).join(' '));

            const icons = {
                success: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>',
                error: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-exclamation-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4zm.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2z"/></svg>',
                warning: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-exclamation-triangle-fill" viewBox="0 0 16 16"><path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/></svg>',
                info: '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle-fill" viewBox="0 0 16 16"><path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/></svg>'
            };

            const notificationId = `notification-${Date.now()}`;
            const iconHtml = settings.icon ? `<div class="me-3">${icons[type] || icons.info}</div>` : '';
            const titleHtml = settings.title ? `<div class="fw-bold">${settings.title}</div>` : '';

            const $notification = $(`
                <div id="${notificationId}" class="toast show mb-3" role="alert" aria-live="assertive" aria-atomic="true">
                    <div class="toast-header bg-${type === 'error' ? 'danger' : type} text-white">
                        ${iconHtml}
                        ${titleHtml}
                        <div class="ms-auto"></div>
                        ${settings.dismissible ? '<button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>' : ''}
                    </div>
                    <div class="toast-body">
                        ${message}
                    </div>
                </div>
            `);

            $container.prepend($notification);

            if (settings.duration) {
                setTimeout(() => Notification.dismiss(notificationId), settings.duration);
            }

            if (settings.onClose) {
                $notification.on('hidden.bs.toast', settings.onClose);
            }

            return notificationId;
        },

        /**
         * Đóng một thông báo cụ thể
         * @param {string} id - ID của thông báo cần đóng
         * @returns {void}
         * @example
         * const noteId = Notification.show('info', 'Đang xử lý...');
         * // Sau khi hoàn thành
         * Notification.dismiss(noteId);
         */
        dismiss: (id) => {
            const $notification = $(`#${id}`);
            if ($notification.length) {
                const toast = bootstrap.Toast.getOrCreateInstance($notification[0]);
                toast.hide();
            }
        },

        /**
         * Đóng tất cả thông báo đang hiển thị
         * @returns {void}
         * @example
         * Notification.dismissAll();
         */
        dismissAll: () => {
            $(Config.selectors.notificationContainer).find('.toast').each(function () {
                const toast = bootstrap.Toast.getOrCreateInstance(this);
                toast.hide();
            });
        }
    };

    /**
     * Quản lý lưu trữ dữ liệu trên trình duyệt
     * @type {Object}
     */
    const Storage = {
        /**
         * Lưu trữ dữ liệu
         * @param {string} key - Khóa lưu trữ
         * @param {*} value - Giá trị cần lưu trữ
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {boolean} [options.useSession=false] - Sử dụng sessionStorage thay vì localStorage
         * @param {number|null} [options.expiry=null] - Thời gian hết hạn (ms)
         * @param {string} [options.namespace='daiminhjs'] - Không gian tên
         * @returns {*} Giá trị đã lưu trữ
         * @example
         * Storage.set('user', {id: 1, name: 'John'}, {expiry: 3600000}); // Hết hạn sau 1 giờ
         */
        set: (key, value, options = {}) => {
            const defaults = {
                useSession: false, expiry: null, namespace: 'daiminhjs'
            };

            const settings = { ...defaults, ...options };
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            const item = {
                value, expiry: settings.expiry ? Date.now() + settings.expiry : null
            };

            storage.setItem(namespacedKey, JSON.stringify(item));

            return value;
        },

        /**
         * Lấy dữ liệu đã lưu trữ
         * @param {string} key - Khóa lưu trữ
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {boolean} [options.useSession=false] - Sử dụng sessionStorage thay vì localStorage
         * @param {*} [options.defaultValue=null] - Giá trị mặc định nếu không tìm thấy
         * @param {string} [options.namespace='daiminhjs'] - Không gian tên
         * @returns {*} Giá trị đã lưu trữ hoặc giá trị mặc định
         * @example
         * const user = Storage.get('user', {defaultValue: {guest: true}});
         */
        get: (key, options = {}) => {
            const defaults = {
                useSession: false, defaultValue: null, namespace: 'daiminhjs'
            };

            const settings = { ...defaults, ...options };
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            const data = storage.getItem(namespacedKey);

            if (!data) {
                return settings.defaultValue;
            }

            try {
                const item = JSON.parse(data);

                if (item.expiry && item.expiry < Date.now()) {
                    Storage.remove(key, { useSession: settings.useSession, namespace: settings.namespace });
                    return settings.defaultValue;
                }

                return item.value;
            } catch (e) {
                return data;
            }
        },

        /**
         * Xóa dữ liệu đã lưu trữ
         * @param {string} key - Khóa lưu trữ cần xóa
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {boolean} [options.useSession=false] - Sử dụng sessionStorage thay vì localStorage
         * @param {string} [options.namespace='daiminhjs'] - Không gian tên
         * @returns {boolean} true nếu thành công
         * @example
         * Storage.remove('user');
         */
        remove: (key, options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = { ...defaults, ...options };
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            storage.removeItem(namespacedKey);

            return true;
        },

        /**
         * Xóa tất cả dữ liệu lưu trữ
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {boolean} [options.useSession=false] - Sử dụng sessionStorage thay vì localStorage
         * @param {string} [options.namespace='daiminhjs'] - Không gian tên
         * @returns {boolean} true nếu thành công
         * @example
         * Storage.clear(); // Xóa tất cả dữ liệu của thư viện
         */
        clear: (options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = { ...defaults, ...options };
            const storage = settings.useSession ? sessionStorage : localStorage;

            if (settings.namespace === 'daiminhjs') {
                for (let i = storage.length - 1; i >= 0; i--) {
                    const key = storage.key(i);
                    if (key.startsWith(`${settings.namespace}.`)) {
                        storage.removeItem(key);
                    }
                }
            } else {
                storage.clear();
            }

            return true;
        },

        /**
         * Lấy tất cả dữ liệu đã lưu trữ
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @param {boolean} [options.useSession=false] - Sử dụng sessionStorage thay vì localStorage
         * @param {string} [options.namespace='daiminhjs'] - Không gian tên
         * @returns {Object} Đối tượng chứa tất cả dữ liệu
         * @example
         * const allData = Storage.getAll();
         * console.log(allData.user); // Truy cập dữ liệu có khóa 'user'
         */
        getAll: (options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = { ...defaults, ...options };
            const storage = settings.useSession ? sessionStorage : localStorage;
            const result = {};

            for (let i = 0; i < storage.length; i++) {
                const key = storage.key(i);

                if (key.startsWith(`${settings.namespace}.`)) {
                    const shortKey = key.substring(settings.namespace.length + 1);
                    const value = Storage.get(shortKey, settings);

                    if (value !== null) {
                        result[shortKey] = value;
                    }
                }
            }

            return result;
        },

        /**
         * Lấy số lượng khóa đã lưu trữ
         * @param {Object} [options={}] - Tùy chọn cấu hình
         * @returns {number} Số lượng khóa
         * @example
         * const count = Storage.size();
         * console.log(`Có ${count} mục dữ liệu được lưu trữ`);
         */
        size: (options = {}) => {
            return Object.keys(Storage.getAll(options)).length;
        }
    };


    /**
    * Quản lý chế độ hiển thị (Dark/Light Mode)
    * @type {Object}
    */
    const Theme = {
        /**
         * Khóa lưu trữ cho theme
         * @type {string}
         */
        storageKey: "daiminhjsTheme",

        /**
         * Theme mặc định
         * @type {string}
         */
        defaultTheme: "light",

        /**
         * Theme hiện tại
         * @type {string}
         */
        currentTheme: null,


        /**
         * Khởi tạo theme
         * @returns {void}
         */
        init: () => {
            Theme.currentTheme = Theme._getInitialTheme();
            Theme._applyTheme(Theme.currentTheme);
        },

        /**
         * Lấy theme ban đầu từ URL param hoặc localStorage
         * @private
         * @returns {string}
         */
        _getInitialTheme: () => {
            const params = new Proxy(new URLSearchParams(window.location.search), {
                get: (searchParams, prop) => searchParams.get(prop),
            });

            if (!!{
                get: (searchParams, prop) => searchParams.get(prop),
            });

            if (!!params.theme) {
                localStorage.setItem(Theme.storageKey, params.theme);
                return params.theme;
            } else {
                const storedTheme = localStorage.getItem(Theme.storageKey);
                return storedTheme ? storedTheme : Theme.defaultTheme;
            }
        },

        /**
         * Áp dụng theme
         * @private
         * @param {string} theme
         * @returns {void}
         */
        _applyTheme: (theme) => {
            if (theme === 'dark') {
                document.body.setAttribute("data-bs-theme", theme);
            } else {
                document.body.removeAttribute("data-bs-theme");
            }
            localStorage.setItem(Theme.storageKey, theme);
            Theme.currentTheme = theme;
        },

        /**
         * Chuyển đổi theme (dark/light)
         * @returns {void}
         */
        toggle: () => {
            const newTheme = Theme.currentTheme === 'dark' ? 'light' : 'dark';
            Theme._applyTheme(newTheme);
        },

        /**
        * Lấy theme hiện tại.
        * @returns {string} Tên của theme hiện tại ('dark' hoặc 'light').
        */
        getCurrentTheme: () => {
            return Theme.currentTheme;
        }

    };


    /**
     * Điểm nhập chính của thư viện
     * @type {Object}
     */
    const Main = {
        /**
         * Lưu trữ các thể hiện của các thành phần
         * @type {Object}
         */
        instances: {
            dataTables: {}, forms: {}, modals: {}
        },

        /**
         * Khởi tạo thư viện
         * @param {Object} [customConfig={}] - Cấu hình tùy chỉnh
         * @returns {Object} Đối tượng Main (để hỗ trợ gọi móc xích)
         * @example
         * jqueryDaiminh.init({
         *   defaults: {
         *     i18n: {defaultLocale: 'vi'}
         *   }
         * });
         */
        init: (customConfig = {}) => {
            if (Object.keys(customConfig).length) {
                Config.update(customConfig);
            }

            Main.instances.dataTables.main = DataTable.initialize(Config.selectors.dataTable);
            Form.init();
            Modal.init();
            Theme.init();

            Utils.log('daiminhjs v2.0.0 initialized');

            return Main;
        },

        /**
         * Lấy tất cả các thể hiện
         * @returns {Object} Đối tượng chứa tất cả các thể hiện
         * @example
         * const instances = Main.getInstances();
         * console.log(instances.dataTables.main); // Truy cập DataTable chính
         */
        getInstances: () => Main.instances,

        /**
         * Lấy một thể hiện cụ thể
         * @param {string} type - Loại thể hiện ('dataTables', 'forms', 'modals')
         * @param {string} id - ID của thể hiện
         * @returns {Object|null} Thể hiện hoặc null nếu không tìm thấy
         * @example
         * const table = Main.getInstance('dataTables', 'users');
         */
        getInstance: (type, id) => {
            return Main.instances[type]?.[id] || null;
        }
    };



    return {
        Config, Utils, DataTable, Form, Modal, Notification, Storage, Main, I18n, /**
         * Khởi tạo thư viện
         * @param {Object} [customConfig={}] - Cấu hình tùy chỉnh
         * @returns {Object} Đối tượng thư viện
         */
        init: Main.init, /**
         * Lấy tất cả các thể hiện
         * @returns {Object} Đối tượng chứa tất cả các thể hiện
         */
        getInstances: Main.getInstances,
    };
})(jQuery, bootstrap);

jQuery(document).ready(() => jqueryDaiminh.init());

console.log("daiminhjs v2.0.0 loaded");