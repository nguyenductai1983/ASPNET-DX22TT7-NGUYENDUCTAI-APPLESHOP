$(document).ready(function () {
    // Script xem trước ảnh khi chọn file mới
    $("#ImageFile").on("change", function () {
        // Kiểm tra xem người dùng có chọn file không
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                // Hiển thị ảnh đã chọn
                $("#imagePreview")
                    .attr('src', e.target.result)
                    .show();
            };
            // Đọc file đã chọn
            reader.readAsDataURL(this.files[0]);
        }
    });
});