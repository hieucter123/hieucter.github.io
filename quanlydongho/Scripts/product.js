$(document).ready(function () {
    // Lấy danh mục cho 'Nam' khi di chuột vào "Đồng hồ Nam"
    $('#navbarDropdown1').on('mouseenter', function () {
        loadCategories('Nam');
    });

    // Lấy danh mục cho 'Nữ' khi di chuột vào "Đồng hồ Nữ"
    $('#navbarDropdown2').on('mouseenter', function () {
        loadCategories('Nữ');
    });

    // Hàm tải danh mục thông qua AJAX
    function loadCategories(gender) {
        $.ajax({
            url: '@Url.Action("GetCategories", "SanPhams")',
            type: 'GET',
            success: function (data) {
                var dropdownMenu = gender === 'Nam' ? '#dropdownCategories1' : '#dropdownCategories2';
                $(dropdownMenu).empty();  // Xóa các mục cũ

                data.forEach(function (category) {
                    $(dropdownMenu).append(
                        `<li><a class="dropdown-item" href="#" onclick="loadProducts(${category.MaDanhMuc}, '${gender}')">${category.TenDanhMuc}</a></li>`
                    );
                });
            }
        });
    }

    // Hàm tải sản phẩm theo danh mục và giới tính
    window.loadProducts = function (categoryId, gender) {
        $.ajax({
            url: '@Url.Action("GetProductsByCategoryAndGender", "SanPhams")',
            type: 'GET',
            data: { categoryId: categoryId, gender: gender },
            success: function (data) {
                $('#productListContainer').html(data);  // Cập nhật container danh sách sản phẩm
            }
        });
    };
});
