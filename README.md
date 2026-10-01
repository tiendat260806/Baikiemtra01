# Nguyễn Tự Tiến Đạt - 24810320260
### Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types (Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap).
### 1. Value Types (Kiểu giá trị)
* *Kiểu dữ liệu:* int, float, bool, char, struct, enum...
* *Vùng nhớ lưu trữ:* Dữ liệu lưu trực tiếp trên *Stack* (khi là biến cục bộ) hoặc nội tuyến bên trong đối tượng chứa nó.
* *Cơ chế sao chép:*
*Copy-by-value* (sao chép toàn bộ giá trị). Gán b = a tạo bản sao độc lập, thay đổi một bên không làm ảnh hưởng bên kia.
* *Thu hồi bộ nhớ:* Tự động giải phóng ngay lập tức khi luồng thực thi đi ra ngoài phạm vi (scope) của hàm/block.

### 2. Reference Types (Kiểu tham chiếu)
* *Kiểu dữ liệu:* class, string, interface, mảng (array), delegate...
* *Vùng nhớ lưu trữ:* Đối tượng thực tế (Object data) luôn nằm trên *Heap*; biến đại diện chỉ là một con trỏ tham chiếu nằm trên *Stack* để trỏ tới vùng nhớ đó.
* *Cơ chế sao chép:*
*Copy-by-reference* (sao chép địa chỉ vùng nhớ). Gán b = a khiến cả hai cùng trỏ tới một khối dữ liệu trên Heap.
* *Thu hồi bộ nhớ:* Được quản lý tự động bởi trình gom rác *Garbage Collector (GC)* khi không còn biến tham chiếu nào trỏ tới.
### Câu 2: Tính năng Init-only Properties (init) trong C# 9/10
### 1. So sánh cốt lõi với thuộc tính có set thông thường
Thuộc tính có set (Mutable): Cho phép gán hoặc thay đổi dữ liệu tại bất kỳ thời điểm nào trong suốt vòng đời của đối tượng.

Thuộc tính có init (Immutable): Chỉ cho phép gán dữ liệu duy nhất một lần tại thời điểm khởi tạo đối tượng (thông qua hàm tạo Constructor hoặc cú pháp Object Initializer).

Tính bất biến (Read-only sau khởi tạo): Khi quá trình tạo đối tượng hoàn tất, thuộc tính sẽ khóa lại và trở thành chỉ đọc; mọi thao tác cố tình gán lại giá trị sau đó đều bị trình biên dịch chặn lại ngay lập tức.

### 2. Trường hợp ứng dụng thực tế
Mô hình DTOs (Data Transfer Objects): Dùng để đóng gói dữ liệu truyền tải giữa các tầng ứng dụng (như API Request/Response Models) nhằm đảm bảo dữ liệu không bị thay đổi ngoài ý muốn trong quá trình luân chuyển.

An toàn trong môi trường đa luồng (Thread-safety): Việc tạo ra các đối tượng bất biến (Immutable Objects) giúp nhiều luồng cùng đọc dữ liệu mà không sợ xung đột tranh chấp (Race Condition), loại bỏ nhu cầu sử dụng khóa lock phức tạp.

### Câu 3: Phân biệt virtual ở lớp cha và override ở lớp con (Tính Đa hình)
### 1. Phương thức virtual (Ở lớp Base / Lớp cha)
Mục đích: Cung cấp sẵn một phần cài đặt mặc định (implementation) cho phương thức.

Cấp quyền kế thừa: Đóng vai trò như một giấy phép cho phép các lớp con dẫn xuất có quyền định nghĩa lại hành vi nếu cần.

Hành vi mặc định: Nếu lớp con không viết lại phương thức này, chương trình sẽ tự động gọi logic mặc định của lớp cha khi thực thi.

### 2. Phương thức override (Ở lớp Derived / Lớp con)
Mục đích: Triển khai lại hoặc thay thế hoàn toàn logic thực thi của một phương thức virtual (hoặc abstract) được kế thừa từ lớp cha.

Cơ chế liên kết động (Dynamic Dispatch / Late Binding): Khi gọi phương thức qua một biến tham chiếu kiểu lớp cha nhưng trỏ đến thể hiện thực tế của lớp con (ví dụ: Animal a = new Dog(); a.Speak();), hệ thống CLR sẽ tra bảng phương thức ảo (v-table) tại thời điểm chạy (runtime) để thực thi đúng mã lệnh trong phương thức override của lớp con thay vì gọi mã của lớp cha.

### Câu 4: Vì sao thành phần static không thể truy xuất qua một thể hiện (Instance) tạo bằng new?
### 1. Khác biệt về cấp độ sở hữu bộ nhớ
Cấp độ Kiểu dữ liệu (Type / Class level): Thành phần static thuộc sở hữu chung của toàn bộ lớp, được nạp và khởi tạo một lần duy nhất vào bộ nhớ khi kiểu dữ liệu đó được tải, dùng chung cho toàn bộ chương trình.

Cấp độ Đối tượng (Object level): Một thể hiện tạo bằng toán tử new chỉ quản lý vùng nhớ và trạng thái dữ liệu độc lập của riêng cá thể đối tượng đó trên vùng nhớ Heap.

### 2. Định hướng thiết kế ngôn ngữ của C#
Tránh nhầm lẫn ngữ nghĩa: Trình biên dịch cấm truy xuất qua thể hiện (như instance.StaticMember) để lập trình viên không ngộ nhận rằng giá trị của thành phần đó phụ thuộc hoặc thay đổi riêng biệt theo từng đối tượng.

Đảm bảo tính tường minh của mã nguồn (Code Clarity): Phân định rạch ròi ngay tại cú pháp gọi hàm giữa hành vi toàn cục thuộc về hệ thống (như Math.Sqrt(), DateTime.Now) và hành vi xử lý dữ liệu nội bộ của một đối tượng cụ thể.
