# QuanLyBanDienThoai

## Các link
1. [Link template đồ án sử dụng (ai làm đăng nhập có thể lấy trang đăng nhập sẵn ở đây)](https://bootstrapmade.com/nice-admin-bootstrap-admin-html-template/)
2. [Link đọc document để sử dụng các tính năng của bootstrap 5.2](https://getbootstrap.com/docs/5.2/getting-started/introduction/)

## Các chức năng của đồ án
1. Đăng nhập, đăng xuất, quản lý nhân viên (Thêm, xóa, sửa, xem, tìm kiếm nhân viên, phân quyền) 
2. Sản phẩm (thêm, xem, xóa, sửa, tìm kiếm danh sách sản phẩm, biến thể sản phẩm, sản phẩm thuộc kho nào)
3. Hóa đơn (tạo, xem, xóa, sửa, tìm kiếm hóa đơn, tạo hóa đơn thì thay đổi tồn kho )
4. Khách hàng (tạo, xem, xóa, sửa, tìm kiếm khách hàng)
5. Phiếu nhập (tạo, xem, xóa, sửa, tìm kiếm phiếu nhập, tạo phiếu nhập cũng thay đổi tồn kho)
6. Nhà cung cấp (tạo, xem, xóa, sửa, tìm kiếm nhà cung cấp)
7. Kho (tạo, xem, xóa, sửa, tìm kiếm kho)
8. Thống kê ở dashboard (thống kê số hóa đơn, số khách hàng, số lợi nhuận theo ngày, tháng, năm, liệt kê các sản phẩm bán chạy nhất, xem các đơn hàng đã bán hôm nay)

## Sơ đồ ERD
![ERD diagram](/Content/img/ERD.png)

Code tạo database
```
CREATE DATABASE QuanLyBanDienThoai;

USE QuanLyBanDienThoai

CREATE TABLE ThuongHieu (
  id int NOT NULL Primary key,
  ten varchar(300) NOT NULL,
  hinh_anh varchar(300),
)

CREATE TABLE SanPham (
  id int NOT NULL Primary key,
  ten varchar(300) NOT NULL,
  mo_ta varchar(1000),
  gia_goc int,
  hinh_anh varchar(300),
  id_thuong_hieu int,
  Constraint FK_thuong_hieu Foreign key (id_thuong_hieu) 
  References ThuongHieu(id),
);

CREATE TABLE ThongSoSanPham (
  id int NOT NULL Primary key,
  ram varchar(300),
  dung_luong varchar(300),
  mau_sac varchar(300),
  gia_them int NOT NULL,
)

CREATE TABLE BienTheSanPham (
  id int NOT NULL Primary key,
  hinh_anh varchar(300),
  id_thong_so_san_pham int NOT NULL,
  id_san_pham int NOT NULL,
  Constraint FK_thongSoSanPham Foreign key (id_thong_so_san_pham)
  References ThongSoSanPham(id),
  Constraint FK_sanPham Foreign key (id_san_pham)
  References SanPham(id),
)

CREATE TABLE Kho (
  id int NOT NULL Primary key,
  ten varchar(300),
  dia_chi varchar(300),
)

CREATE TABLE SanPhamTonKho (
  id_bien_the_san_pham int NOT NULL,
  id_kho int NOT NULL,
  so_luong int NOT NULL,
  Constraint FK_Kho Foreign key (id_kho) References Kho(id),
  Constraint FK_BienTheSanPham Foreign key (id_bien_the_san_pham) References BienTheSanPham(id),
  Primary key (id_bien_the_san_pham, id_kho),
)

CREATE TABLE NhaCungCap (
  id int NOT NULL Primary key,
  ten varchar(300),
  sdt varchar(300),
  hinh_anh varchar(300)
)

CREATE TABLE PhieuNhap (
  id int NOT NULL Primary key,
  id_nha_cung_cap int NOT NULL, 
  ngay_tao_phieu Date NOT NULL,
  Constraint FK_NhaCungCap_PhieuNhap Foreign key (id_nha_cung_cap)
  References NhaCungCap(id),
)

CREATE TABLE ChiTietPhieuNhap (
  id_phieu_nhap int NOT NULL,
  id_bien_the_san_pham int NOT NULL,
  so_luong int NOT NULL,
  Constraint FK_PhieuNhap_ChiTietPhieuNhap Foreign key (id_phieu_nhap) References PhieuNhap(id),
  Constraint FK_BienTheSanPham_ChiTietPhieuNhap Foreign key (id_bien_the_san_pham) References BienTheSanPham(id),
  Primary key (id_phieu_nhap, id_bien_the_san_pham)
)


CREATE TABLE Nhanvien (
  id int NOT NULL Primary key,
  ten varchar(300) NOT NULL,
  sdt varchar(300) NOT NULL,
  ngay_sinh Date,
  quyen_han varchar(30) NOT NULL,
  hinh_anh varchar(300),
  usernane varchar(300),
  password varchar(300)
)

CREATE TABLE KhachHang (
  id int NOT NULL Primary key,
  ten varchar(300) NOT NULL,
  sdt varchar(300),
  ngay_sinh Date,
  hinh_anh varchar(300),
)

CREATE TABLE HoaDon (
  id int NOT NULL Primary key,
  id_khach_hang int NOT NULL,
  id_nhan_vien int NOT NULL,
  ngay_tao_phieu Date NOT NULL,
  Constraint FK_HoaDon_KhachHang Foreign key (id_khach_hang) References KhachHang(id),
  Constraint FK_HoaDon_NhanVien Foreign key (id_khach_hang) References Nhanvien(id),
)

CREATE TABLE ChiTietHoaDon (
  id_hoa_don int NOT NULL,
  id_bien_the_san_pham int NOT NULL,
  so_luong int NOT NULL,
  Constraint FK_HoaDon_ChiTietHoaDon Foreign key (id_hoa_don) References HoaDon(id),
  Constraint FK_BienTheSanPham_ChiTietHoaDon Foreign key (id_bien_the_san_pham) References BienTheSanPham(id),
  Primary key (id_hoa_don, id_bien_the_san_pham),
)


INSERT INTO NHANVIEN
VALUES
  (1, 'Ly Tuan Minh', '123', '2015-2-3', 'Ban hang', null, 'lyminh8565', '123456'),
  (2, 'Tran Quoc Nam', '321', '2015-2-3', 'Chu cua hang', null, 'admin', 'admin')

INSERT INTO KhachHang
VALUES
  (1, 'Huynh Ngoc Nam', '123', '2015-2-3', null),
  (2, 'Le Trung Nguyen', '321', '2015-2-3', null)

INSERT INTO HoaDon
VALUES
  (1, 1, 2, GETDATE()),
  (2, 2, 1, GETDATE()),
  (3, 1, 1, GETDATE())


INSERT INTO ThuongHieu(id, ten, hinh_anh)
VALUES
  (1, 'Apple', null),
  (2, 'Samsung', null)

INSERT INTO SanPham (id, ten, mo_ta, gia_goc, hinh_anh, id_thuong_hieu)
VALUES
  (1, 'Iphone 12 pro max', 'Iphone 12 pro max bla bla bla...', 10000000, null, 1),
  (2, 'Iphone 11 pro max', 'Bla bla bla bla bla bla...', 9000000, null, 1)

INSERT INTO ThongSoSanPham (id, ram, dung_luong, mau_sac, gia_them)
VALUES
  (1, '8GB', '128GB', 'Xám', 0),
  (2, '8GB', '128GB', 'Lam', 0)


INSERT INTO BienTheSanPham (id, hinh_anh, id_thong_so_san_pham, id_san_pham)
VALUES
  (1, null, 1, 1),
  (2, null, 1, 2)

```