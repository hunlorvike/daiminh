/**
 * daiminhjs - A lightweight JavaScript library for ASP.NET Core MVC applications
 * @version 2.0.0
 * @requires jQuery, Bootstrap, DataTables
 */
const jqueryDaiminh = (($, bootstrap) => {
    'use strict';

    const Config = {
        selectors: {
            dataTable: '#table',
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
        }, defaults: {
            dataTable: {paging: true, searching: true, serverSide: false, pageLength: 10},
            modal: {backdrop: true, disableTimeout: 500},
            notification: {duration: 3000, position: 'top-right'},
            logging: false,
            i18n: {defaultLocale: 'en', fallbackLocale: 'en'},
        }, update: function (newConfig) {
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

    const Utils = {
        disableButton: ($button, timeout = Config.defaults.modal.disableTimeout) => {
            $button.prop('disabled', true);
            setTimeout(() => $button.prop('disabled', false), timeout);
        },
        generateSlug: (text) => text.toLowerCase().trim().replace(/\s+/g, "-").replace(/[^a-z0-9-]/g, ""),
        log: (message, ...args) => Config.defaults.logging && console.log(`[daiminhjs] ${message}`, ...args),
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
        formatNumber: (number, options = {}) => {
            const defaults = {
                locale: 'en-US', style: 'decimal', minimumFractionDigits: 0, maximumFractionDigits: 2
            };
            const settings = {...defaults, ...options};
            return new Intl.NumberFormat(settings.locale, settings).format(number);
        },
        copyToClipboard: (text) => {
            navigator.clipboard.writeText(text)
                .then(() => Notification.show('success', I18n.t('clipboard.copied')))
                .catch(() => Notification.show('error', I18n.t('clipboard.failed')));
        }
    };

    const I18n = {
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
                }, validation: {
                    required: 'This field is required',
                    email: 'Please enter a valid email address',
                    minLength: 'Please enter at least {0} characters',
                    maxLength: 'Please enter no more than {0} characters',
                    pattern: 'Please enter a valid value',
                    url: 'Please enter a valid URL',
                    numeric: 'Please enter a numeric value',
                    integer: 'Please enter an integer value',
                    min: 'Please enter a value greater than or equal to {0}',
                    max: 'Please enter a value less than or equal to {0}'
                }, notification: {
                    success: 'Success', error: 'Error', warning: 'Warning', info: 'Information'
                }, clipboard: {
                    copied: 'Copied to clipboard', failed: 'Failed to copy to clipboard'
                }, errors: {
                    ajax: 'An error occurred while processing your request',
                    validation: 'Please correct the errors in the form',
                    network: 'Network error. Please check your connection',
                    unknown: 'An unknown error occurred'
                }, dataTable: {
                    info: 'Showing _START_ to _END_ of _TOTAL_ entries',
                    infoEmpty: 'Showing 0 to 0 of 0 entries',
                    infoFiltered: '(filtered from _MAX_ total entries)',
                    lengthMenu: `<span class="text-secondary">Show</span>
                        <select class="form-select form-select-sm d-inline-block w-50">
                            <option value="10">10</option>
                            <option value="15">15</option>
                            <option value="20">20</option>
                        </select>`,
                    first: 'First',
                    last: 'Last',
                    next: 'Next',
                    previous: 'Previous'
                }, form: {
                    submitSuccess: 'Form submitted successfully', submitError: 'Form submission failed'
                }, modal: {
                    loadError: 'Error loading {type} modal'
                }, router: {
                    navigationFailed: 'Navigation failed'
                }
            }
        }, currentLocale: Config.defaults.i18n.defaultLocale,

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

        setLocale: function (locale) {
            if (this.translations[locale]) {
                this.currentLocale = locale;
            } else {
                Utils.log(`Locale ${locale} not found, using ${this.currentLocale}`);
            }
            return this;
        },

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

    const DataTable = {
        initialize: (selector, customOptions = {}) => {
            const options = {
                ...Config.defaults.dataTable,

                dom: '<"card-header d-flex justify-content-between align-items-center"<"col-md-6"l><"col-md-6"f>>' + '<"table-responsive"t>' + '<"card-footer d-flex justify-content-between align-items-center"ip>',

                language: {
                    search: "",
                    searchPlaceholder: I18n.t('common.search'),
                    emptyTable: I18n.t('common.noData'),
                    info: I18n.t('dataTable.info', {start: '_START_', end: '_END_', total: '_TOTAL_'}),
                    infoEmpty: I18n.t('dataTable.infoEmpty'),
                    infoFiltered: I18n.t('dataTable.infoFiltered', {max: '_MAX_'}),
                    lengthMenu: I18n.t('dataTable.lengthMenu', {menu: '_MENU_'}),
                    loadingRecords: I18n.t('common.loading'),
                    processing: I18n.t('common.loading'),
                    zeroRecords: I18n.t('common.noData'),
                    paginate: {
                        first: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M11 7l-5 5l5 5" /><path d="M17 7l-5 5l5 5" /></svg>',
                        previous: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M15 6l-6 6l6 6" /></svg>',
                        next: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M9 6l6 6l-6 6" /></svg>',
                        last: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M7 7l5 5l-5 5" /><path d="M13 7l5 5l-5 5" /></svg>'
                    }
                }, initComplete: (settings, json) => {
                    DataTable._applyBootstrapStyling();
                    if (customOptions.initComplete) {
                        customOptions.initComplete(settings, json);
                    }
                }, drawCallback: (settings) => {
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

        _applyBootstrapStyling: () => {

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

        refresh: (table) => {
            if (!table) return;
            if (table.ajax) {
                table.ajax.reload();
            } else {
                table.draw();
            }
        },

        destroy: (table) => {
            if (table?.destroy) {
                table.destroy();
            }
        },

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

        _exportCsv: (data, headers, filename) => {
            const csvContent = [headers.join(','), ...data.map(row => Object.values(row).join(','))].join('\n');

            const blob = new Blob([csvContent], {type: 'text/csv;charset=utf-8;'});
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${filename}.csv`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            return csvContent;
        },

        _exportJson: (data, headers, filename) => {
            const jsonData = data.map(row => {
                const rowObj = {};
                headers.forEach((header, index) => {
                    rowObj[header] = row[index];
                });
                return rowObj;
            });

            const jsonContent = JSON.stringify(jsonData, null, 2);
            const blob = new Blob([jsonContent], {type: 'application/json;charset=utf-8;'});
            const url = URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${filename}.json`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            return jsonContent;
        },

        _exportExcel: (data, headers, filename) => {
            return DataTable._exportCsv(data, headers, filename);
        }
    };

    const Form = {
        init: () => {
            $(document).on('click', `${Config.selectors.submitButton}`, (e) => {
                e.preventDefault();
                const $form = $(e.target).closest('form');
                Form.submit($form);
            });

        },

        submit: async ($form) => {
            console.log($form)
            const url = $form.attr('action');
            const method = $form.attr('method') || 'POST';
            const $submitBtn = $form.find(Config.selectors.submitButton);

            Utils.disableButton($submitBtn);

            const hasFiles = $form.find('input[type="file"]').length > 0;
            let response;

            if (hasFiles) {
                const formData = new FormData($form[0]);
                // TODO: thực hiện logic submit FormData
            } else {
                $.ajax({
                    url: url,
                    type: method,
                    data: Form.serializeObject($form),
                    success: function (response, textStatus, jqXHR) {
                        if (response.redirectUrl) {
                            window.location.href = response.redirectUrl;
                        }
                    },
                    error: function (response, textStatus, jqXHR) {
                        const errors = response.responseJSON.errors;
                        console.log(errors);
                        $form.find('span[data-valmsg-for]').text('');
                        if (errors) {
                            Object.keys(errors).forEach(key => {
                                const $errorSpan = $form.find(`span[data-valmsg-for="${key}"]`);
                                if ($errorSpan.length) {
                                    $errorSpan.text(errors[key][0]);
                                    $errorSpan.removeClass('field-validation-valid').addClass('field-validation-error');
                                } else {
                                    console.warn(`No error span found for field: ${key}`);
                                }
                            });
                        }
                    }
                });
            }
        },

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

    const Modal = {
        instances: {},

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

        _setupModalAction: ({buttonSelector, containerSelector, modalType}) => {
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
                        Notification.show('error', I18n.t('modal.loadError', {type: modalType}));
                    }
                });
            });
        },


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

        createDynamic: (options = {}) => {
            const defaults = {
                id: 'dynamicModal' + Date.now(), title: '', body: '', size: '', // '', 'modal-sm', 'modal-lg', 'modal-xl'
                buttons: [{
                    text: I18n.t('common.close'), class: 'btn-secondary', dismiss: true
                }], onShow: null, onHide: null
            };

            const settings = {...defaults, ...options};

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

    const Notification = {
        show: (type, message, options = {}) => {
            const defaults = {
                duration: Config.defaults.notification.duration,
                position: Config.defaults.notification.position,
                title: I18n.t(`notification.${type}`),
                icon: true,
                dismissible: true,
                onClose: null
            };

            const settings = {...defaults, ...options};

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

        dismiss: (id) => {
            const $notification = $(`#${id}`);
            if ($notification.length) {
                const toast = bootstrap.Toast.getOrCreateInstance($notification[0]);
                toast.hide();

            }
        },

        dismissAll: () => {
            $(Config.selectors.notificationContainer).find('.toast').each(function () {
                const toast = bootstrap.Toast.getOrCreateInstance(this);
                toast.hide();
            });

        }
    };

    const Storage = {
        set: (key, value, options = {}) => {
            const defaults = {
                useSession: false, expiry: null, namespace: 'daiminhjs'
            };

            const settings = {...defaults, ...options};
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            const item = {
                value, expiry: settings.expiry ? Date.now() + settings.expiry : null
            };

            storage.setItem(namespacedKey, JSON.stringify(item));

            return value;
        },

        get: (key, options = {}) => {
            const defaults = {
                useSession: false, defaultValue: null, namespace: 'daiminhjs'
            };

            const settings = {...defaults, ...options};
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            const data = storage.getItem(namespacedKey);

            if (!data) {
                return settings.defaultValue;
            }

            try {
                const item = JSON.parse(data);

                if (item.expiry && item.expiry < Date.now()) {
                    Storage.remove(key, {useSession: settings.useSession, namespace: settings.namespace});
                    return settings.defaultValue;
                }

                return item.value;
            } catch (e) {
                return data;
            }
        },

        remove: (key, options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = {...defaults, ...options};
            const storage = settings.useSession ? sessionStorage : localStorage;
            const namespacedKey = `${settings.namespace}.${key}`;

            storage.removeItem(namespacedKey);

            return true;
        },

        clear: (options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = {...defaults, ...options};
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

        getAll: (options = {}) => {
            const defaults = {
                useSession: false, namespace: 'daiminhjs'
            };

            const settings = {...defaults, ...options};
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

        size: (options = {}) => {
            return Object.keys(Storage.getAll(options)).length;
        }
    };

    const Main = {
        instances: {
            dataTables: {}, forms: {}, modals: {}
        },

        init: (customConfig = {}) => {
            if (Object.keys(customConfig).length) {
                Config.update(customConfig);
            }

            Main.instances.dataTables.main = DataTable.initialize(Config.selectors.dataTable);
            Form.init();
            Modal.init();

            Utils.log('daiminhjs v2.0.0 initialized');

            return Main;
        },

        getInstances: () => Main.instances,

        getInstance: (type, id) => {
            return Main.instances[type]?.[id] || null;
        }
    };

    return {
        Config,
        Utils,
        DataTable,
        Form,
        Modal,
        Notification,
        Storage,
        Main,
        I18n,
        init: Main.init,
        getInstances: Main.getInstances,
    };
})(jQuery, bootstrap);

jQuery(document).ready(() => jqueryDaiminh.init());

console.log("daiminhjs v2.0.0 loaded");
