# SOUL FORGE — GAME DESIGN DOCUMENT

**Phiên bản:** 0.1  
**Ngày:** 13/10/2026  
**Định dạng:** Markdown  
**Trạng thái:** Thiết kế nền và luật prototype; chưa phải đặc tả triển khai hoàn chỉnh.  
**Tên dự án:** Soul Forge — tên làm việc theo dự án hiện tại.

---

## 1. Phạm vi và cách đọc

Tài liệu tổng hợp cốt truyện, hệ thống quyền năng và luật chiến đấu đã trao đổi. Không mô tả tính năng đã được lập trình hoặc kiểm chứng bằng playtest.

- **Đã chốt:** lựa chọn được người dùng xác nhận trong cuộc trao đổi.
- **Định hướng ban đầu:** ý tưởng do người dùng đưa ra, chưa hoàn thiện mọi chi tiết.
- **Đề xuất:** nội dung phát triển để kết nối các ý tưởng, chưa mặc nhiên trở thành canon.
- **Chưa chốt:** cần quyết định trước khi triển khai phần liên quan.

Các tên thần, tổ chức, sách và biến cố dưới đây đều là tên tạm nếu không có ghi chú khác.

## 2. Tổng quan game

### 2.1. Ý tưởng cốt lõi

Game chiến đấu thẻ bài theo lượt trong một thế giới trung cổ giả tưởng, nơi hiệp sĩ, phù thủy, quái vật và thần linh cùng tồn tại. Những lá bài mang các mảnh quyền năng của mười hai Tổng lãnh.

Người chơi là một con người xuất thân thấp kém, lên đường tìm cách cứu mẹ khỏi quyền năng Sinh Mệnh khiến cơ thể bà phát triển bất thường mà không thể chết. Người chơi chọn một trong hai phe đối lập và thu thập các lá bài để phục hồi hai quyển sách quyền năng.

**Thu thập đủ bộ bài cốt truyện và phục hồi hai sách là đích kết thúc của hành trình**, không phải điểm mở đầu cho một cuộc săn tìm mới.

### 2.2. Sắc thái và chủ đề

**Đã chốt:** đen tối nhưng vẫn có hy vọng; nhân vật chính là người bình thường, không dựa vào gia thế đặc biệt.

Các chủ đề đề xuất:

- Sự sống không có kết thúc có thể trở thành một hình phạt.
- Sáng tạo và hủy diệt không tương đương tuyệt đối với thiện và ác.
- Một đời người bình thường vẫn có giá trị trước những quyết định của thần linh.
- Trật tự bảo vệ con người đến mức nào trước khi trở thành sự áp đặt?
- Con người có thể sửa chữa thế giới mà không xóa sạch mọi mất mát.

### 2.3. Trụ cột gameplay

1. **Chuẩn bị đội hình trước:** triệu hồi và đổi thế chủ động nằm ở Main 2, sau Battle.
2. **Sao là sức mạnh có thể tiêu hao:** giao chiến công–công làm giảm khả năng chiến đấu lâu dài.
3. **Thủ là lựa chọn hữu hạn:** mỗi quái chỉ vào thủ một lần trong mỗi lần tồn tại trên sân.
4. **Hệ ảnh hưởng xây dựng bộ bài:** phép hỗ trợ cùng hệ, hiến tế cần cùng hệ, bẫy có lợi thế khi khắc hệ.
5. **Quái nhỏ vẫn có giá trị:** mọi đòn trực diện thành công đều phá một tinh thể.

Prototype được kiểm chứng trên Android theo mục tiêu dự án. Góc nhìn, cấu trúc khám phá, phong cách đồ họa và mô hình kinh doanh chưa được chốt trong phạm vi tài liệu này.

## 3. Thế giới và lịch sử

### 3.1. Khởi nguyên — định hướng ban đầu

Từ Hư Không sinh ra một vị Thần tối cao mang cả quyền năng sáng tạo và hủy diệt. Tên của vị thần sẽ được bàn sau.

Ông tạo ra mười hai Đại Tổng lãnh:

- Sáu vị đại diện cho Sáng tạo, thường được gọi là phía Ánh sáng.
- Sáu vị đại diện cho Hủy diệt, thường được gọi là phía Bóng tối.

Thần tối cao vận hành theo chu kỳ sinh–diệt, không đánh giá thế giới bằng khái niệm tốt hoặc xấu của con người.

### 3.2. Nguyên nhân phản loạn — đề xuất

Qua nhiều chu kỳ, các vũ trụ được tạo ra rồi hoàn nguyên. Một vũ trụ sinh ra những linh hồn có ý chí riêng, có thể lựa chọn ngoài dự định của các Tổng lãnh.

Sáu vị Sáng tạo gắn bó với những sinh linh đó và tin rằng chúng có giá trị tự thân. Khi Thần tối cao chuẩn bị hoàn nguyên vũ trụ, họ cầu xin ông giữ lại các linh hồn.

Đối với Thần, mọi thứ chỉ trở về nguồn rồi tiếp tục một chu kỳ khác. Đối với sáu vị Sáng tạo, tái sử dụng vật chất không thể thay thế những cá nhân từng sống.

Họ phản loạn để giữ lại thế giới, dù biết hành động đó có thể phá hủy trật tự tồn tại. Động cơ có thể đáng cảm thông, nhưng quyết định vẫn là việc sáu cá thể tự chọn thay cho toàn bộ sinh linh.

### 3.3. Cái chết của Thần tối cao — đề xuất cơ chế

Khi trao quyền vận hành thế giới cho các Tổng lãnh, Thần tối cao đã thực sự phân chia một phần quyền năng. Trong nghi thức hoàn nguyên, ông phải mở bản thể để thu hồi những quyền năng đó.

Sáu Tổng lãnh Sáng tạo lợi dụng thời điểm này để phá hủy ý thức thống nhất của ông. Họ không thể xóa bỏ quyền năng nguyên thủy, nên phải hợp sức giữ nó khỏi tràn ra.

Sáu Tổng lãnh Hủy diệt, trung thành với Thần tối cao và chu kỳ hủy diệt, tấn công nhằm phá phong tỏa và hồi sinh chủ nhân.

### 3.4. Đại Khai Sinh — khung lịch sử ban đầu, tên tạm

Cuộc chiến kéo dài hàng tỷ năm trong vũ trụ cũ. Sáu vị Sáng tạo vừa chiến đấu vừa khống chế quyền năng từ cái chết của Thần.

Cuối cùng, cuộc xung đột dẫn tới vụ nổ hủy diệt vũ trụ cũ và khai sinh vũ trụ hiện tại: sự kiện tương ứng với Big Bang trong thần thoại của game.

Các Tổng lãnh mất phần lớn khả năng hành động và bị phong ấn trong chính vũ trụ mới.

**Chi tiết bi kịch đề xuất:** thế giới mà sáu vị Sáng tạo muốn cứu cuối cùng vẫn biến mất; vũ trụ hiện tại là hệ quả ngoài dự tính của cả hai phía.

### 3.5. Hai quyển sách

**Định hướng ban đầu:** quyền năng rò rỉ kết tinh thành hai sách, rơi xuống thế giới loài người và được hai tổ chức đối lập phát hiện, khai thác.

| Tên tạm | Nguồn quyền năng |
|---|---|
| Sáng Thế Thư | Sáu Tổng lãnh Sáng tạo |
| Chung Mạt Thư | Sáu Tổng lãnh Hủy diệt |

**Đề xuất giới hạn:** sách là phương tiện truy cập và phối hợp quyền năng, không chứa toàn bộ sức mạnh vũ trụ. Khôi phục sách không mặc nhiên khiến người sở hữu toàn năng.

### 3.6. Đêm Vỡ Trang — 400 năm trước, tên và nguyên nhân chi tiết là đề xuất

Hai tổ chức cùng tiến hành nghi lễ hợp nhất sách để giải quyết những tai họa của thế giới. Họ bất đồng về quyền quyết định trật tự sau nghi lễ và giao chiến ngay khi quyền năng đang được kết nối.

Cơ chế tự bảo vệ của sách phân tách quyền năng thành các Ấn Bài, phát tán khắp thế giới để ngăn một ý chí chiếm toàn bộ quyền kiểm soát.

**Mốc đã xác định:** câu chuyện hiện tại diễn ra 400 năm sau biến cố phân tách hai sách thành bài.

## 4. Mười hai quyền năng

### 4.1. Sáu cặp đối nghịch

Ba cặp đầu phát triển từ ý tưởng ban đầu của người dùng. Ba cặp sau là đề xuất bổ sung, chưa phải danh sách tên canon cuối cùng.

| Sáng tạo / Ánh sáng | Hủy diệt / Bóng tối | Trục đối lập | Trạng thái |
|---|---|---|---|
| Tinh Tú | Vực Thẳm | Phát sáng, phát năng lượng ↔ hấp thụ, hội tụ; lấy cảm hứng từ ngôi sao và hố đen | Ý tưởng gốc; tên tạm |
| Sinh Mệnh | Tử Vong | Khai sinh, sinh trưởng ↔ kết thúc, phân rã | Ý tưởng gốc; tên tạm |
| Định Luật | Cảm Xúc | Quy tắc, lý trí ↔ xung động, ham muốn và ngoại lệ | Ý tưởng gốc; tên tạm |
| Hình Thể | Tiêu Giải | Hình dạng và ranh giới ↔ tan rã hình dạng và ranh giới | Đề xuất |
| Ký Ức | Lãng Quên | Bản sắc, lịch sử, lời thề ↔ xóa dấu vết, tháo bỏ quá khứ | Đề xuất; đã dùng làm ví dụ bẫy |
| Định Mệnh | Biến Động | Con đường nhân quả ổn định ↔ bất định, thay đổi kết quả | Đề xuất |

### 4.2. Nguyên tắc biểu đạt

- Ánh sáng không tự động chính nghĩa; Bóng tối không tự động tà ác.
- Cảm Xúc không bị xem là xấu: tình yêu cũng có thể thúc đẩy phá bỏ trật tự.
- Sinh Mệnh có thể tạo dị biến; Tử Vong có thể đem lại giải thoát.
- Ký Ức có thể giữ con người trong thù hận; Lãng Quên có thể giúp bắt đầu lại.

**Luật chiến đấu đã chốt:** quan hệ đối nghịch tạo lợi thế bổ sung cho bẫy. Không có luật mặc định cho quái tăng sát thương hoặc tăng sao chỉ vì khắc hệ.

## 5. Hai phe đối lập — đề xuất tên và tư tưởng

**Định hướng đã có:** người chơi chọn một trong hai phe; cả hai cùng muốn thu thập bài để phục hồi hai sách. Không phân định hoàn toàn phe tốt và phe xấu.

| Nội dung | Hội Thủ Ước | Liên Minh Phá Mệnh |
|---|---|---|
| Niềm tin | Quyền năng cần được kiểm soát để bảo vệ số đông | Không tổ chức nào được độc quyền quyết định số phận con người |
| Mục tiêu công khai | Khôi phục sách để ổn định thế giới | Khôi phục sách để phá quyền độc chiếm thần lực |
| Mặt tích cực | Cứu trợ, bảo vệ thành trì, gìn giữ tri thức | Chống áp bức, bảo vệ người bị trật tự ruồng bỏ |
| Mặt nguy hiểm | Kiểm duyệt, đặc quyền, hy sinh thiểu số | Bất ổn, giải phóng sức mạnh khó kiểm soát |

**Đề xuất:** cả hai phe có thể dùng bài từ cả hai nhóm quyền năng. Chọn phe không mặc nhiên khóa người chơi vào sáu hệ. Mức ảnh hưởng của phe lên nhiệm vụ, bộ bài và kết thúc chưa chốt.

## 6. Nhân vật chính và động lực

### 6.1. Xuất thân — đã chấp nhận

Nhân vật là con một gia đình làm nghề đào huyệt tại thị trấn biên giới. Họ kiếm sống bằng việc ghi chép, viết thuê và ghi tên người chết, không có huyết thống thần linh hay gia thế danh giá.

Nhân vật có khả năng đọc viết nhưng không phải học giả; chưa phải chiến binh được đào tạo khi câu chuyện bắt đầu. Năng lực chiến đấu sẽ được hình thành qua hành trình.

Tên, tuổi, giới tính và các chi tiết gia đình khác chưa chốt.

### 6.2. Người mẹ — đã chốt

Một sự cố quyền năng Sinh Mệnh tác động trực tiếp lên cơ thể mẹ của nhân vật.

- Cơ thể bà tiếp tục phát triển, phát sinh mô và cấu trúc bất thường, gây dị biến.
- Bà chịu đau đớn nhưng vẫn sống.
- Bà không thể chết, kể cả khi tự tìm cách kết thúc sự sống.
- Quyền năng đính vào chính bà; không phụ thuộc việc bà ở trong vùng đất bị ảnh hưởng.
- Di chuyển khỏi thị trấn không giải trừ tình trạng.
- Vùng đất chỉ bị ảnh hưởng trong cùng sự cố, không phải nguồn duy trì tình trạng của bà.

**Mục tiêu cá nhân:** gỡ bỏ tác động quyền năng và cứu mẹ khỏi tình trạng dị biến, không chỉ kéo dài sự sống hoặc đưa bà đi nơi khác.

### 6.3. Biến cố mở đầu — đề xuất

Thị trấn nằm trên một di tích chứa quyền năng Sinh Mệnh. Một cuộc tranh giành hoặc tai nạn phá vỡ phong ấn, phát tán sức mạnh lên con người và môi trường.

Nhân vật lấy được một lá bài từ người bảo vệ di tích đã chết và dùng nó cứu những người mắc kẹt. Lá đầu tiên có thể là Ấn Cầm Giữ thuộc Hình Thể, dùng để giữ một kết cấu khỏi sụp trong thời gian ngắn.

Lá bài không chọn họ vì định mệnh hoặc huyết thống; họ là người có mặt và quyết định sử dụng nó.

Hai phe can thiệp bằng những cách khác nhau. Nhân vật gia nhập một phe ở vị trí thấp để tiếp cận kiến thức, huấn luyện và hành trình thu hồi bài.

### 6.4. Kết nối việc cứu mẹ với thu thập bài — đề xuất

Nhân vật ban đầu tìm quyền năng Tử Vong, nhưng phát hiện nó không tự động phục hồi cơ thể mẹ. Kết thúc sự sống và chữa lành sự sống là hai việc khác nhau.

Muốn gỡ tác động Sinh Mệnh mà giữ lại con người bà, cần phối hợp nhiều quyền năng. Hai sách hoàn chỉnh có thể điều phối quá trình ấy.

Trong hành trình, nhân vật gặp những người chịu hậu quả khác của thần lực phân tán. Động lực mở rộng từ cứu mẹ sang sửa chữa thế giới, nhưng không loại bỏ mục tiêu ban đầu.

## 7. Khung kịch bản chiến dịch — đề xuất

| Chặng | Nội dung | Vai trò |
|---|---|---|
| Mở đầu | Tai họa tại thị trấn, mẹ bị dị biến, tiếp xúc lá đầu tiên | Thiết lập động lực cá nhân |
| Gia nhập | Chọn phe, được huấn luyện, nhận nhiệm vụ thu hồi | Giới thiệu thế giới và luật chiến đấu |
| Hy vọng đầu tiên | Tìm một giải pháp đơn lẻ nhưng không thể chữa lành mẹ | Chuyển sang mục tiêu phục hồi sách |
| Hành trình chính | Thu hồi các mảnh sách, giải quyết xung đột địa phương | Phát triển bộ bài, nhân vật và tư tưởng |
| Sự thật | Khám phá lịch sử phản loạn và ý định của các thế lực | Đặt người chơi trước lựa chọn sử dụng sách |
| Cao trào | Tranh giành hoặc giành quyền tiếp cận lá cuối | Hoàn tất mục tiêu thu thập |
| Kết thúc | Phục hồi hai sách, thực hiện lựa chọn, thể hiện hệ quả | Khép lại chiến dịch |

**Nguyên tắc kết thúc đã chốt:** hoàn thành thu thập là đích đến của cốt truyện.

**Định hướng kết thúc đề xuất:** quyền năng có thể sửa tổn thương do thần lực và cứu mẹ, nhưng không đồng loạt hồi sinh người chết hoặc xóa mọi mất mát. Số lượng kết thúc và điều kiện cứu mẹ chưa được chốt.

### 7.1. Phân biệt bài cốt truyện và bài chiến đấu — đề xuất

- **Ấn Bài Nguyên Bản:** tập hữu hạn tạo thành hai sách; cần thu thập đủ để hoàn thành cốt truyện.
- **Ấn Bài Phái Sinh:** các biểu hiện quyền năng phục vụ chiến đấu, có thể tồn tại nhiều bản.

Cách phân loại này chưa được xác nhận. Giới hạn bộ bài 20 lá của prototype không có nghĩa toàn bộ thế giới hoặc bộ sưu tập chỉ có 20 lá.

## 8. Đấu Giới và lý do phải tuân luật bài

**Nhu cầu thiết kế ban đầu:** việc chiến đấu theo luật bài phải được thế giới cưỡng chế, không chỉ dựa vào thỏa thuận của người tham chiến.

### 8.1. Luật Phân Quyền — đề xuất giải thích

Sau biến cố vỡ sách, mỗi lá bài là một mệnh lệnh thần lực bị giới hạn. Khi những người mang Ấn trực tiếp dùng quyền năng đối kháng, Đấu Giới cô lập xung đột và phân xử các mệnh lệnh theo luật.

- Bộ bài là những quyền năng được liên kết để sử dụng trong trận.
- Lượt và giai đoạn là thứ tự phân xử hành động.
- Sao biểu thị sức mạnh hiện thời của hiện thân quái.
- Hiến tế cung cấp liên kết hệ và năng lượng cho quái mạnh hiện thân.
- Hành động vật lý bên trong Đấu Giới cũng chịu cơ chế phân xử, tránh việc bỏ qua luật bằng vũ khí thông thường.

### 8.2. Linh hồn — đã chốt cho prototype

Khi vào Đấu Giới, một phần linh hồn biểu hiện thành **6 tinh thể**. Mất hết tinh thể đồng nghĩa thua trận.

Hậu quả trong cốt truyện của việc thua — chết, mất ý thức, mất quyền năng hoặc hệ quả khác — chưa chốt. Không mặc định thua trận đồng nghĩa chết vĩnh viễn.

## 9. Luật chiến đấu prototype v0.1 — đã chốt

### 9.1. Bộ bài và rút bài

- Mỗi bộ bài có **20 lá**.
- Mỗi người rút **5 lá** khi bắt đầu trận.
- Mỗi Draw rút **1 lá**, bao gồm Draw đầu tiên của người đi trước.
- Khi phải rút nhưng bộ bài hết, bỏ qua lần rút và tiếp tục trận.
- Hết bài không gây sát thương, không phá tinh thể và không xử thua.
- Prototype dùng hai deck cố định, cho phép lặp bài; không giới hạn tay và không có mulligan. UI phải xem được toàn bộ bài trên tay. Người đi trước cố định; xáo deck bằng seed có thể ghi lại.

### 9.2. Bàn đấu

Tổng cộng **20 ô**, chia đều cho hai bên:

| Khu vực mỗi bên | Số ô | Công dụng |
|---|---:|---|
| Hàng trước | 5 | Quái |
| Hàng sau | 5 | Phép và bẫy |

Bộ bài, tay bài và mộ là các khu vực quản lý bài ngoài 10 ô sân của mỗi bên. Được chọn bất kỳ quái địch nào làm mục tiêu; không có luật cột hoặc chắn đòn mặc định trong prototype.

### 9.3. Điều kiện thắng thua

- Mỗi bên có **6 tinh thể linh hồn**.
- Chỉ được tấn công trực diện khi đối phương không còn quái.
- Phép/bẫy còn trên sân không tự ngăn đánh trực diện.
- Mỗi lần tấn công trực diện thành công phá đúng **1 tinh thể**, bất kể sao của quái.
- Mất toàn bộ 6 tinh thể là điều kiện thua duy nhất hiện tại.
- Nếu chưa có kết quả do tinh thể, hòa sau khi giải quyết xong lượt thứ **40 toàn trận**. Một lượt của một bên tăng bộ đếm một lần. Kết quả do tinh thể được ưu tiên trước hòa do giới hạn lượt.
- Bộ bài mẫu không có hiệu ứng làm cả hai bên mất tinh thể cuối cùng đồng thời; trường hợp đó ngoài phạm vi hiện tại.

### 9.4. Trình tự turn

**Bắt đầu turn → Draw → Main 1 → Battle → Main 2 → Kết thúc turn.**

| Giai đoạn | Hành động / xử lý |
|---|---|
| Bắt đầu turn | Tăng bộ đếm thủ của quái đang thủ do người đang vào turn triệu hồi; kiểm tra hết hạn |
| Draw | Rút 1 lá nếu còn bài |
| Main 1 | Dùng phép |
| Battle | Quái thế công tấn công; mỗi quái tối đa 1 lần |
| Main 2 | Triệu hồi thường tối đa 1 lần, chọn thế dự kiến cho quái, đặt bẫy |
| Kết thúc turn | Khi nhấn End, xác nhận thế đã chọn; hoàn tất xử lý cuối lượt, kiểm tra kết quả rồi chuyển lượt |

- Người đi trước tự động bỏ Battle nếu đó là lượt đầu tiên của toàn trận.
- Phép vừa rút có thể dùng ở Main 1 của cùng lượt.
- Quái triệu hồi thường ở Main 2 không có Battle tiếp theo trong cùng lượt.
- Việc đặt hoặc dùng phép duy trì ở Main 2 chưa được cho phép trong luật hiện tại; phép được dùng ở Main 1.

### 9.5. Dữ liệu và sao của quái

Quái có ba nhóm thông tin gameplay chính:

1. **Hệ:** một trong mười hai quyền năng.
2. **Sao:** sao gốc in trên bài, tối đa 12.
3. **Biểu tượng đặc biệt:** dành cho quái đẫm quyền năng, biểu thị kỹ năng liên quan đến hệ.

Phân biệt:

- **Sao gốc:** dùng xác định chi phí triệu hồi.
- **Sao hiện tại:** dùng giải quyết giao chiến và giới hạn thủ.
- Buff có thể đưa sao hiện tại vượt 12; chưa đặt trần tổng quát.
- Sao đã mất không tự hồi sau giao chiến hoặc khi đổi lượt.
- Sao về 0 hoặc thấp hơn do hiệu ứng ngoài giao chiến cũng đưa quái vào mộ, sau khi hiệu ứng hoàn tất.

### 9.6. Công–công

Hai bên đồng thời chịu mất sao bằng số sao hiện tại của đối phương **trước khi giải quyết giao chiến**. Quái về 0 sao hoặc thấp hơn vào mộ.

| Trước giao chiến | Sau giao chiến |
|---|---|
| 8 công đánh 4 công | Quái 4 vào mộ; quái 8 còn 4 sao |
| 4 công đánh 8 công | Quái 4 vào mộ; quái 8 còn 4 sao |
| 4 công đánh 4 công | Cả hai vào mộ |

Không tính kết quả tuần tự theo kiểu giảm sao một bên rồi mới dùng số sao đã giảm để tính bên còn lại.

### 9.7. Công–thủ

- Khác sao: bên ít sao hơn vào mộ; bên nhiều sao hơn không mất sao.
- Bằng sao: cả hai giữ nguyên, không quái nào vào mộ.
- Một lần tấn công hòa vẫn tiêu quyền tấn công của quái đó trong lượt.
- Quái thủ không chủ động tấn công.
- Tiêu quyền tấn công khi yêu cầu hợp lệ được chấp nhận; đòn bị vô hiệu vẫn tiêu quyền. Yêu cầu sai không tiêu quyền.
- Nếu mục tiêu biến mất trước giao chiến, hủy giao chiến; không tự đổi thành đánh trực diện.

| Trước giao chiến | Sau giao chiến |
|---|---|
| 8 công đánh 4 thủ | Quái 4 vào mộ; quái 8 giữ 8 sao |
| 4 công đánh 8 thủ | Quái 4 vào mộ; quái 8 giữ 8 sao |
| 4 công đánh 4 thủ | Cả hai giữ 4 sao; bên công đã dùng lần tấn công |

### 9.8. Thời gian và quyền vào thủ

**Lượt thủ tối đa = min(phần nguyên(sao hiện tại / 2), 5).**

| Sao hiện tại | Lượt thủ tối đa |
|---:|---:|
| 1 | 0 — không được vào thủ |
| 2–3 | 1 |
| 4–5 | 2 |
| 6–7 | 3 |
| 8–9 | 4 |
| 10 trở lên | 5 |

- Theo dõi lượt đã thủ và lượt thủ tối đa.
- Khi vào thủ, lượt đã thủ bắt đầu bằng 0.
- Đầu turn của **người triệu hồi**, tăng lượt đã thủ lên 1 đối với quái đang thủ.
- Khi lượt đã thủ đạt hoặc vượt lượt thủ tối đa, quái tự chuyển về công.
- Chuyển về công do hết hạn là chuyển thế bắt buộc, không cần chờ Main 2.
- Tăng sao có thể tăng giới hạn thủ nhưng không đặt lại lượt đã thủ.
- Mỗi quái chỉ được vào thủ một lần trong mỗi lần tồn tại liên tục trên sân.
- Đổi thế trong Main 2 là lựa chọn dự kiến; **chỉ xác nhận khi nhấn End**. Chỉnh/hủy lựa chọn trước End không tiêu quyền vào thủ.
- End xác nhận công → thủ thì tiêu quyền vào thủ và bắt đầu lượt đã thủ bằng 0. Quái mới triệu hồi cũng được chọn thủ theo cách này.
- Main 2 lượt sau có thể chọn thủ → công, xác nhận bằng End; không được vào thủ lại khi vẫn trên sân. Chọn thủ trở lại trước End chỉ hủy lựa chọn về công.
- Sao thay đổi giữa lượt chỉ cập nhật **lượt thủ tối đa**, giữ nguyên lượt đã thủ; đến đầu lượt kế tiếp của người triệu hồi mới kiểm tra hết hạn để đổi thế.
- Việc trì hoãn đổi thế không ngăn quái có sao ≤ 0 vào mộ sau khi hiệu ứng hoàn tất.

**Ví dụ:** quái 2 sao vào thủ tại Main 2 của Player, bảo vệ trong lượt Enemy, rồi đầu turn Player kế tiếp tăng bộ đếm lên 1 và tự về công.

**Ví dụ:** quái 6 sao đã thủ 1 lượt giảm còn 2 sao: giới hạn đổi từ 3 xuống 1, lượt đã thủ vẫn là 1 và vẫn ở thủ; đầu lượt kế tiếp của người triệu hồi mới xử lý hết hạn. Đổi quyền điều khiển và rời sân rồi trở lại không thuộc prototype.

### 9.9. Triệu hồi thường và hiến tế

- Mỗi người được triệu hồi thường tối đa **1 lần mỗi lượt**, tại Main 2.
- Chi phí dựa trên sao gốc của quái được triệu hồi.
- Khi cần hiến tế, ít nhất **1 quái cùng hệ** với quái được triệu hồi; những quái còn lại có thể thuộc bất kỳ hệ nào.
- Không dùng máu hoặc tinh thể để thay hiến tế trong prototype.

| Sao gốc | Số quái hiến tế | Điều kiện hệ |
|---:|---:|---|
| 1–4 | 0 | Không yêu cầu |
| 5–6 | 1 | 1 cùng hệ |
| 7–8 | 2 | Ít nhất 1 cùng hệ |
| 9–10 | 3 | Ít nhất 1 cùng hệ |
| 11–12 | 4 | Ít nhất 1 cùng hệ |

- Chỉ hiến tế quái mình điều khiển trên sân; bài hiến tế vào mộ.
- Kiểm tra toàn bộ yêu cầu trước khi chuyển bài. Sân đầy vẫn được triệu hồi nếu hiến tế tạo ô trống hợp lệ.
- Quái mới vào công; được chọn thủ trong Main 2 và xác nhận khi nhấn End.

### 9.10. Phép — Mảnh vỡ quyền năng

- Có hệ và hiệu ứng riêng; không có chỉ số sao như quái.
- Thiên về buff có lợi cho quái cùng hệ; không tự mở rộng buff sang quái khác hệ.
- Dùng từ tay ở Main 1 và cần một ô hàng sau trống.
- Phép tức thời chiếm ô để giải quyết, sau đó vào mộ.
- Prototype chỉ làm phép tức thời tăng sao **vĩnh viễn**, không tự hết theo lượt; lá phép vào mộ không gỡ sao đã cộng. Phép duy trì hoãn sau prototype.
- Buff không cộng dồn: **buff mới thay buff cũ**, giữ nguyên phần sao đã mất do giao chiến/hiệu ứng.
- Sao mới = sao hiện tại − giá trị buff cũ + giá trị buff mới. Gỡ/cộng trong cùng một lần xử lý rồi mới kiểm tra chết; không xét chết ở bước trung gian.
- Ví dụ đã chốt: gốc 4, buff +2 thành 6, mất 3 còn 3; thay buff +1 thì còn **2 sao**.
- Giá trị buff phép mẫu còn cần chọn. Không hồi sao đã mất khi thay buff.

### 9.11. Bẫy

- Có hệ và hiệu ứng riêng, thiên về phản công.
- Đặt úp ở hàng sau tại Main 2.
- Chỉ kích hoạt trong lượt đối phương khi thỏa điều kiện của lá.
- Có hiệu ứng thông thường với các hệ; gặp hệ đối nghịch có hiệu ứng bổ sung hoặc mạnh hơn.
- Khắc hệ không tự mặc định nhân đôi hiệu ứng: từng lá phải nêu rõ lợi thế.

**Bẫy mẫu đã chốt, chưa cân bằng:** sau giao chiến công–công trong lượt đối phương, nếu quái địch sống sót và vừa mất sao, bẫy Ký Ức cho phép khiến nó mất thêm 1 sao; nếu thuộc Lãng Quên thì mất 2 thay vì 1.

- Có bẫy hợp lệ thì **chờ người chơi chọn kích hoạt hoặc bỏ qua**, không tự động kích hoạt. AI quyết định cho Enemy.
- Tối đa một bẫy mỗi sự kiện; sát thương bẫy mẫu không tạo chuỗi phản ứng khác.
- Dùng xong vào mộ. Bỏ qua giữ bẫy úp và đóng cửa sổ phản ứng của sự kiện hiện tại.
- Trong lúc chờ, không nhận đòn đánh mới hoặc chuyển phase. Sau quyết định/hiệu ứng, kiểm tra kết quả rồi tiếp tục.
- Bẫy mẫu kích hoạt **sau** giao chiến; không áp dụng giả định cũ rằng mọi bẫy chạy trước đòn đánh.
- Nhiều bẫy hợp lệ: chưa chốt người chơi chọn bất kỳ lá nào hay quyết định với lá ưu tiên trái sang phải.

## 10. Phạm vi prototype và những gì chưa được chứng minh

### 10.1. Nền tảng đã thống nhất

- Một trận đấu theo lượt với hai phía Player và Enemy.
- Bộ bài 20 lá, tay đầu 5 lá, Draw 1 lá.
- Bàn 5 ô quái và 5 ô phép/bẫy mỗi bên.
- Công–thủ, sao tiêu hao, thủ hữu hạn.
- Triệu hồi thường có hiến tế.
- Phép cùng hệ và bẫy có hiệu ứng khắc hệ.
- Sáu tinh thể và điều kiện thua khi mất hết.
- Hết bộ bài thì bỏ qua rút.
- Hòa sau lượt 40 toàn trận nếu chưa có kết quả do tinh thể.
- Android là nền tảng kiểm chứng; UI tạm cảm ứng, AI đơn giản, kết quả trận và chơi lại sạch dữ liệu.
- Dữ liệu hỗ trợ 12 hệ, nội dung thử chỉ dùng một cặp đối nghịch; Ký Ức/Lãng Quên là cặp đề xuất phù hợp bẫy mẫu.
- Hai deck cố định có bài lặp; quái không có kỹ năng riêng; một phép tức thời, một bẫy mẫu. Chưa cần 20 loại bài khác nhau.

Đây là phạm vi thiết kế trận prototype, không phải báo cáo các tính năng đã hoàn thành.

### 10.2. Chưa thuộc luật prototype đã chốt

- Trả linh hồn thay hiến tế: đã tạm bỏ.
- Triệu hồi đặc biệt, hồi sinh, đổi quyền điều khiển: chưa định nghĩa.
- Hệ thống chuỗi phản ứng: chưa định nghĩa.
- Bản đồ chiến dịch, nhiệm vụ, kinh tế, phần thưởng và lưu tiến trình: chưa đặc tả.
- Danh sách chính xác của hai deck 20 lá và giá trị buff phép mẫu cần chọn; không mở rộng thư viện bài trước khi khép kín trận.
- Phép duy trì, buff có thời hạn và kỹ năng riêng của quái hoãn sau prototype.

## 11. Quyết định còn mở

Các quyết định đã xác nhận ngày 13/10/2026 đã được đưa vào mục 9–10. Chỉ các mục còn mở dưới đây cần giải quyết tiếp.

| Mục | Câu hỏi cần giải quyết | Gợi ý, chưa chốt |
|---|---|---|
| Chọn bẫy | Người chơi chọn bất kỳ bẫy hợp lệ hay chỉ lá ưu tiên? | Đề xuất chọn một lá hợp lệ hoặc bỏ qua; tối đa một bẫy mỗi sự kiện |
| Bộ bài mẫu | Cặp hệ, danh sách quái, số bản sao và giá trị buff phép? | Ký Ức/Lãng Quên, hai deck cố định có bài lặp; cần danh sách cụ thể |
| Gỡ buff độc lập | Có đưa hiệu ứng gỡ buff vào prototype không? | Chưa đưa vào bộ bài mẫu; sao không mất chỉ vì phép tức thời vào mộ. Thay buff đã được định nghĩa ở mục 9.10 |
| Canon | Tên thần, sáu cặp, hai phe và nguyên nhân vỡ sách? | Duyệt riêng phần truyện đề xuất |
| Thất bại cốt truyện | Mất hết tinh thể gây hậu quả gì ngoài trận? | Chưa chọn |

## 12. Kế hoạch kiểm chứng prototype — đề xuất, chưa thực hiện

### 12.1. Tình huống luật cần kiểm tra

| Tình huống | Kết quả mong đợi theo luật đã chốt |
|---|---|
| 8 công đánh 4 công | Quái 4 vào mộ; quái 8 còn 4 |
| 4 công đánh 8 công | Quái 4 vào mộ; quái 8 còn 4 |
| 4 công đánh 4 công | Cả hai vào mộ |
| 8 công đánh 4 thủ | Quái 4 vào mộ; quái 8 giữ nguyên |
| 4 công đánh 8 thủ | Quái 4 vào mộ; quái 8 giữ nguyên |
| 4 công đánh 4 thủ | Không mất sao; tiêu lần tấn công |
| Quái 2 sao thủ từ Main 2 | Tự về công ở đầu turn kế tiếp của người triệu hồi |
| Quái 4 sao đã thủ 1 lượt được buff lên 6 | Giới hạn thành 3; lượt đã thủ vẫn là 1 |
| Quái trên 12 sao | Được phép tồn tại; giới hạn thủ vẫn tối đa 5 |
| Chủ động kết thúc thủ | Không được vào thủ lại khi vẫn trên sân |
| Gọi quái 7 sao bằng 2 quái khác hệ hoàn toàn | Không hợp lệ |
| Gọi quái 7 sao bằng 1 cùng hệ và 1 khác hệ | Đủ điều kiện số lượng/hệ |
| Quái bất kỳ đánh trực diện thành công | Phá 1 tinh thể |
| Địch còn quái | Không được đánh trực diện |
| Người đi trước ở turn đầu toàn trận | Bỏ Battle |
| Draw khi hết bài | Không rút; trận tiếp tục; không mất tinh thể |
| Dùng phép khi hàng sau kín | Không hợp lệ theo yêu cầu ô trống |
| Gốc 4, buff +2, mất 3, thay buff +1 | Còn 2 sao; giữ sát thương cũ |
| Thay buff | Kiểm tra chết sau cả thao tác, không ở bước gỡ buff trung gian |
| Quái 6 sao đã thủ 1 lượt giảm còn 2 | Giới hạn thành 1; chờ đầu lượt người triệu hồi mới xét về công |
| Chọn công → thủ rồi hủy trước End | Chưa tiêu quyền vào thủ |
| End xác nhận vào thủ, lượt sau End xác nhận về công | Không được vào thủ lại khi vẫn trên sân |
| Có bẫy hợp lệ, người chơi chưa quyết định | Chờ; không nhận đánh tiếp hoặc chuyển phase |
| Bỏ qua bẫy | Bẫy còn úp; không mở lại phản ứng cùng sự kiện |
| Bẫy Ký Ức tác động quái Lãng Quên sống sót sau công–công | Mất thêm 2 sao; về 0 thì vào mộ; không tạo chain |
| Kết thúc lượt 40, chưa có kết quả theo tinh thể | Hòa |
| Mất tinh thể cuối trong lượt 40 | Kết quả theo tinh thể, không bị ghi đè thành hòa |

### 12.2. Câu hỏi cân bằng cần quan sát

- Sáu tinh thể có khiến nhiều quái nhỏ kết thúc trận quá nhanh không?
- Một lần triệu hồi thường mỗi lượt có làm quái cần nhiều hiến tế quá khó sử dụng không?
- Công–thủ không tiêu hao bên mạnh có tạo bế tắc kéo dài không?
- Buff sao không có trần có làm một quái vượt quá khả năng xử lý của bộ bài đối phương không?
- Bẫy có hiệu ứng thường có giảm được tình trạng bài không dùng được mà vẫn giữ ý nghĩa khắc hệ không?
- Rút và dùng phép ngay có tạo đủ biến chuyển mà không lấn át chuẩn bị từ lượt trước không?
- Khi hết bài, hai bên còn khả năng kết thúc trận không?

Không coi các rủi ro trên là lỗi đã được chứng minh. Chỉ điều chỉnh thông số sau khi có kết quả thử nghiệm hoặc lập luận cụ thể từ danh sách bài.

---

**Kết thúc GDD v0.1.** Tài liệu là mốc tổng hợp thiết kế; các mục đề xuất và chưa chốt cần được duyệt trước khi trở thành đặc tả chính thức.
