$(document).ready(function () {
    $("#table").DataTable({
        dom: '<"card-header d-flex justify-content-between align-items-center"<"col-md-6"l><"col-md-6"f>>' +
            '<"table-responsive"t>' +
            '<"card-footer d-flex justify-content-between align-items-center"ip>',
        paging: true,
        searching: true,
        searchDelay: 500,
        info: true,
        lengthChange: true,
        ordering: true,
        pageLength: 10,
        lengthMenu: [[8, 25, 50, 100], [8, 25, 50, 100]],
        columnDefs: [{targets: 0, orderable: true, searchable: true},
            {targets: 1, orderable: true, searchable: true},
            {targets: 2, orderable: true, searchable: true},
            {targets: 3, orderable: true, searchable: true},
            {targets: 4, orderable: false, searchable: false}],
        language: {
            search: "",
            searchPlaceholder: "Tìm kiếm...",
            lengthMenu: `<span class="text-secondary">Hiển thị</span>
                <select class="form-select form-select-sm d-inline-block w-50">
                    <option value="10">10</option>
                    <option value="15">15</option>
                    <option value="20">20</option>
                </select>`,
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
            infoEmpty: "Hiển thị 0 đến 0 của 0 mục",
            zeroRecords: "Không có dữ liệu nào khớp",
            infoFiltered: "(lọc từ tổng số _MAX_ mục)",
            paginate: {
                first: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M11 7l-5 5l5 5" /><path d="M17 7l-5 5l5 5" /></svg>',
                previous: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M15 6l-6 6l6 6" /></svg>',
                next: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M9 6l6 6l-6 6" /></svg>',
                last: '<svg xmlns="http://www.w3.org/2000/svg" class="icon m-0" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M7 7l5 5l-5 5" /><path d="M13 7l5 5l-5 5" /></svg>'
            }
        },
        initComplete: updatePaginationClasses,
        drawCallback: updatePaginationClasses
    });

    function updatePaginationClasses() {
        const $dtSearch = $('.dt-search'), $pagingNav = $('.dt-paging nav'), $pagingButton = $('.dt-paging-button');
        const $dtLength = $('.dt-length');


        $dtSearch.addClass('d-flex align-items-center justify-content-end')
            .find('input').addClass('form-control form-control-sm w-50');

        $dtLength.find('label').addClass('w-50');
        $pagingNav.addClass('pagination justify-content-center m-0');
        $pagingButton.addClass('page-link')
            .filter('.current').addClass('active')
            .wrap('<li class="page-item"></li>');

        $pagingButton.filter('.disabled').parent().addClass('disabled');
        $pagingButton.filter('.current').parent().addClass('active');
    }
});
