# Raziel — Kế hoạch hoàn thiện prototype

Ngày rà soát: **26/10/2026**. Cơ sở: working tree Soulforge tại HEAD **3cc09c7**, gồm thay đổi chưa commit. Tài liệu ghi nhận hiện trạng code và dữ liệu; không khẳng định mọi chức năng đã được kiểm chứng trong Unity hoặc Android.

## 1. Kết luận và mục tiêu

Project đã có phần lớn luồng của một trận thử nghiệm: bộ bài, rút bài, phase, kéo thả triệu hồi, công–thủ, phép có thời hạn, bẫy chặn đòn, AI cơ bản và kết quả trận. Cần ưu tiên bảo vệ dữ liệu bài và hoàn tất các đường thất bại trước khi thêm hiệu ứng.

Mục tiêu gần nhất: **chơi một trận Player–Enemy từ đầu tới kết quả, không mất/nhân đôi bài hoặc kẹt phase; chơi lại sạch dữ liệu; kiểm chứng trên Android.**

Nguồn đối chiếu: [GDD mục 9–12](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Soul_Forge_GDD_v0.1.md:235), quyết định mới trong trao đổi, source, scene, prefab, ScriptableObject và cấu hình project. Quyết định mới của người dùng được ưu tiên khi GDD đã cũ.

Phân biệt trong tài liệu:

- **Đã có:** tìm thấy đường xử lý/cấu hình tương ứng; không đồng nghĩa đã test mọi trường hợp.
- **Lỗi hiện hành:** có điều kiện cụ thể dẫn tới sai dữ liệu, exception hoặc kẹt luồng theo code.
- **Nguy cơ:** cần kiểm chứng hoặc chỉ phát sinh khi mở rộng.
- **Thiếu chức năng:** luật GDD chưa triển khai; phần chủ động hoãn không bị coi là regression.

Lần rà soát chỉ tạo tài liệu này; không sửa gameplay, scene, asset, GDD hoặc commit.

## 2. Đồng bộ luật trước khi triển khai tiếp

| Nội dung | GDD v0.1 | Luật mới/hiện trạng | Kế hoạch |
|---|---|---|---|
| Lượt mở đầu | Người đi trước bỏ Battle ở lượt đầu | Hai lượt đầu toàn trận bỏ Main I và Battle | Giữ luật mới; sửa GDD và bảng test |
| Buff | Vĩnh viễn, buff mới thay buff cũ | Cho phép cộng dồn; ByTurn giữ nguồn trên sân | Giữ luật mới; cập nhật mục 9.10 |
| Hết hạn/phá nguồn | Phép tức thời vào mộ không gỡ sao | ByTurn hết hạn hoặc nguồn bị phá thì gỡ đúng phần effect | Giữ sát thương cũ: gốc 4 +2, mất 3, hết buff còn 1 |
| Duration | Chưa làm phép duy trì | ByTurn và OneTime | Đếm theo lượt chủ nguồn; OneTime dùng xong vào mộ |
| Bẫy mẫu | Gây sát thương sau giao chiến | Chặn đòn trước giao chiến, không ai mất sao | Giữ bẫy chặn đòn; bẫy sau giao chiến là mở rộng |
| Đánh trực tiếp | Chưa mô tả nhánh phản hồi mới | Tạm không dùng bẫy | Giữ phạm vi đã chốt |
| Hết bài | Bỏ rút, tiếp tục trận | Đã tạm hoãn xử lý | Đưa vào mốc nghiệm thu trận dài, không ép trong bước sửa hiện tại |
| 40 lượt | Hòa sau lượt 40 | Đã tạm hoãn | Backlog riêng; chưa phải điều kiện của bản thử rút gọn |
| Hiến tế | Có trong prototype | Đã hoãn khi làm luồng chọn/dùng | Xếp sau khi luồng ổn định |
| Trần sao | Sao gốc tối đa 12; sao hiện tại chưa có trần chung | Runtime có MaxStar = 20 và đang dùng để test | Cần ghi rõ 20 là luật prototype hay thông số thử |
| Tinh thể | 6 mỗi bên | SampleScene dùng 2 mỗi bên | Tách preset test nhanh 2 và preset nghiệm thu GDD 6 |
| Hệ | 12 hệ; nội dung thử một cặp đối nghịch | Có dữ liệu 12 hệ; effect chưa kiểm tra cùng/khắc hệ | Chốt bộ bài mẫu và luật hệ trước khi triển khai |

Không quay lại công thức ngũ hành/hiến tế trong các tin nhắn đầu: GDD hiện dùng 12 hệ và số lượng quái hiến tế, với ít nhất một quái cùng hệ.

**Cần chốt thêm trước khi sửa thủ:** nếu buff hết hạn đúng Standby, cập nhật sao/giới hạn thủ trước hay sau kiểm tra hết thủ? Đề xuất xử lý hết hạn effect, cập nhật giới hạn, rồi tăng/kiểm tra bộ đếm thủ trong cùng Standby. Đây là đề xuất thứ tự, chưa phải luật đã được xác nhận.

## 3. Đã làm được gì?

| Nhóm | Đã có trong code/dữ liệu | Còn thiếu hoặc chưa kiểm chứng |
|---|---|---|
| Card asset | 42 lá: 32 Soul, 5 Mighty, 5 Rupture; có catalog/editor gán dữ liệu, ảnh | 7 lá chức năng chưa có effect |
| Bộ bài | Hai deck 20 lá; asset tách khỏi CardInstance; BattleDeck riêng | Validation dữ liệu và seed tái hiện trận |
| Rút bài | Tay đầu 5, Draw 1; UI/pool tay bài | Hết bài, rút thiếu và tay nhiều lá trên Android |
| Bàn | 5 Soul + 5 functional mỗi bên; scene và UI đủ 20 ô | Kiểm chứng mọi thao tác trên thiết bị |
| Phase | Setup → Standby → Draw → Main I → Battle → Main II → End; ngoại lệ hai lượt đầu | Bộ đếm tổng lượt độc lập nếu đưa lại giới hạn 40 |
| Triệu hồi | Player kéo thả/chọn thế; Enemy lọc Soul/Rupture Main II; chặn ô đầy | Định danh bài còn lỗi; chưa quota/hiến tế |
| Kéo thả/pool | Khôi phục transform khi hủy; về parent trước thao tác đổi vùng; pool tay/sân có prefab | Test chọn/hủy liên tục và replay; chưa có số liệu performance |
| Combat | Công–công trừ đồng thời; công–thủ bên mạnh giữ sao; hòa công–thủ giữ cả hai | 6 ca C# độc lập đạt, chưa thay thế Play Mode |
| Quyền đánh | CanAttack kiểm tra phe, hàng, sống, thế, quyền; đánh dấu trước hỏi bẫy | Cần test runtime đòn bị chặn và đánh trực tiếp |
| Thủ | Có elapsed/limit, tự về công ở Standby chủ quái | Sai điều kiện hết hạn; limit chưa theo sao; thiếu xác nhận End |
| Phép | Player/Enemy dùng FirstMainPhase; AI lọc target theo từng effect; chặn dùng lại | AI chọn nguồn còn đơn giản; chưa điều kiện cùng hệ |
| Vòng đời effect | ByTurn/OneTime; modifier theo source; gỡ nguồn giữ sát thương, cập nhật sao/chết | Test nhiều nguồn, nhiều target, chết do hết buff |
| Bẫy | Flow chung hai phía; chặn đòn, OneTime về mộ | Chọn bẫy AI có đường kẹt; chưa nối Bỏ qua |
| Kết quả | Đánh trực tiếp phá 1 tinh thể; EndGame → Result và win/lose UI | Chưa chơi lại; preset đang 2 tinh thể |
| Android | Có SampleScene trong Build Settings; cấu hình IL2CPP/ARM64, min SDK 25 | Chưa chứng minh build/cài/chơi trên thiết bị |

**Dữ liệu cụ thể:**

- Player: **9 Soul + 6 Mighty + 5 Rupture = 20**.
- Enemy: **7 Soul + 7 Mighty + 6 Rupture = 20**.
- SingleStarBlessing: +1, ByTurn, 2 lượt chủ nguồn, OwnSoul.
- TwinStarBlessing: +2, ByTurn, 3 lượt chủ nguồn, OwnSoul.
- BarrierOfInterception: chặn đòn, OneTime, EnemySoul.
- Enemy deck có **WarriorsLastWish chưa gắn effect**, bên cạnh 5 BarrierOfInterception.

**Đã sửa đúng gần đây:** index gốc của AI; random danh sách ô/target rỗng; lọc target nhận được buff; hòa công–thủ; tiêu quyền đánh trước khi bị bẫy chặn. Không đưa những lỗi cũ này vào backlog lần nữa.

## 4. Lỗi cần sửa trước

### B01 — Lấy một lá khỏi tay nhưng đặt lá khác lên sân — ưu tiên cao

**Vị trí:** [DuelInformation.Summon](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Battle/DuelInformation.cs:86), [nút Summon debug](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/StateMachine/Base/DuelStateMachine.cs:133), SecondMainPhase và nút còn nối trong SampleScene.

**Nguyên nhân:** lấy bài khỏi tay bằng handIndex, nhưng kiểm tra/đặt sân bằng SelectedCard; chưa kiểm tra hai thứ trỏ cùng một lá. Nút debug gửi (0,0). Khi chờ chọn thế, việc khóa drag chưa khóa nút ChooseCard.

**Ca tái hiện:** chọn/kéo lá index 1 rồi hủy; bấm Summon debug và xác nhận thế. Lá index 0 rời tay, SelectedCard index 1 được đặt nhưng vẫn còn trên tay. Hoặc mở chọn thế cho index 1 rồi bấm ChooseCard để đổi SelectedCard sang index 0 trước xác nhận.

**Sửa nhỏ:** xác thực index và đúng tham chiếu Hand[index] với lá định triệu hồi trước mọi thay đổi; khóa chọn lại khi chờ thế; vô hiệu hóa các nút debug không còn phù hợp.

**Hướng lâu dài:** một yêu cầu triệu hồi dùng cùng CardInstance cho kiểm tra/lấy/đặt. Thất bại không đổi vùng bài. Khi thêm hiến tế, kiểm tra toàn bộ rồi mới thực hiện cả thao tác.

**Đạt khi:** một instance chỉ nằm ở một vùng; mọi ca chọn/hủy/đổi thế/nút debug không làm mất hoặc nhân đôi bài.

### B02 — AI chọn bẫy không dùng được, kẹt AwaitingReaction — ưu tiên cao

**Vị trí:** [ChooseRupture](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Battle/AI/EnemyAIControllerInput.cs:282), [BattlePhase.HandleRequested](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/StateMachine/Phase/BattlePhase.cs:159), [TryUseRuptureCard](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Battle/BattleFlowController.cs:69).

**Nguyên nhân:** mở phản hồi vì có ít nhất một bẫy có effect, nhưng AI chọn từ mọi Rupture. Dùng thất bại không có nhánh chọn lại hoặc Pass.

**Ca hiện hành:** BarrierOfInterception và WarriorsLastWish cùng sân; AI random trúng WarriorsLastWish không có effect → không resolve → flow đứng chờ. Đây là lá đã có trong enemy deck, không chỉ tình huống giả định.

**Sửa:** dùng chung một kiểm tra bẫy hợp lệ cho mở phản hồi, chọn của AI và thực hiện. Kiểm tra phe sở hữu, slot còn chứa đúng lá, loại Rupture, Effect, chưa dùng, target và CanActivate theo AttackContext. Không có lựa chọn thì Pass; thất bại phải có đường xử lý rõ ràng.

**Đạt khi:** bẫy hợp lệ đứng cạnh lá chưa có effect không làm kẹt; đòn kết thúc đúng một lần. Loại lá chưa hoàn thiện khỏi deck nghiệm thu chỉ là bổ sung, không thay validation.

### B03 — Thời gian thủ và giới hạn thủ chưa đúng GDD — ưu tiên vừa

**Vị trí:** [ProcessDefenseElapsed](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Battle/BattleBoard.cs:350), [CardInstance](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Pure/BattleCardInformation.cs:37), lựa chọn thế ở SecondMainPhase/AI.

**Nguyên nhân:** tăng elapsed rồi kiểm tra > thay vì >=; limit chỉ lấy sao gốc/2 ở constructor, không cập nhật khi buff/giao chiến/gỡ buff, không cap 5. Chưa chặn quái 1 sao vào thủ.

**Sửa:** dùng công thức chung min(floor(BattleStar/2),5); cập nhật limit sau thay đổi sao nhưng giữ elapsed; kiểm tra >= tại Standby chủ quái; dùng chung điều kiện vào thủ cho Player/AI.

**Đạt khi:** 2–3 sao về công ở đầu lượt chủ kế tiếp; 6 sao đã thủ 1 lượt giảm còn 2 thì limit thành 1 nhưng chờ Standby mới chuyển; trên 10 sao limit vẫn 5; 1 sao không thủ.

### B04 — Hết bài không hoàn tất Draw — đã hoãn, cần trước test trận dài

**Vị trí:** [DuelInformation.DrawCard](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Battle/DuelInformation.cs:60), [Deck.DrawCard](/C:/Users/Official_41/Documents/Unity_Project/Soulforge/Assets/Script/Pure/Deck.cs:42), DrawPhase.

**Nguyên nhân:** rút thất bại không phát ChangesInHand, trong khi đây là đường chuyển phase. Deck.DrawCard cũng chỉ kiểm tra còn bài trước vòng lặp; rút 2 khi còn 1 có thể đổi dữ liệu một phần rồi lỗi.

**Sửa:** tách hoàn thành yêu cầu rút khỏi tay thay đổi. Rút tối đa số còn lại; hoàn tất Draw dù lấy 0; không mất tinh thể. Chốt hành vi rút thiếu trước khi thêm phép rút nhiều lá.

**Đạt khi:** Draw với 0 vẫn chuyển đúng phase; rút 2 khi còn 1 không exception hoặc sai vùng bài.

## 5. Nguy cơ và cải tiến có điều kiện

| Mục | Nhận định | Hướng xử lý |
|---|---|---|
| Nhiều phản hồi trong một Battle | SetSelectedRupture đăng ký mỗi cửa sổ, chỉ gỡ ở Exit | Gỡ khi đóng cửa sổ hoặc đăng ký một lần; làm trước AI nhiều đòn |
| AttackContext chưa độc lập | Resolve còn đọc selection sống của BattleBoard | Trước bẫy phá quái/đổi slot: lưu instance và resolve theo context; target mất thì hủy, không đổi thành đánh trực tiếp |
| Reset cùng instance | SetupDeck cộng thêm cấu hình; InitialDeck chỉ xóa BattleDeck | Prototype chơi lại bằng tải scene; reset tại chỗ cần xóa mọi runtime state |
| AI phép random lần nữa | Đã giữ boarIndex/target nhưng lại random lúc dùng | Dùng lại lựa chọn đã kiểm tra; chưa chứng minh gây kẹt ở AddStars đồng bộ |
| AI bỏ qua phép khác dùng được | Random nguồn chỉ lọc Mighty, gặp nguồn không dùng được thì bỏ lượt phép | Cải tiến sau: chọn từ các phương án nguồn + ô + target hợp lệ |
| Tay nhiều lá trên Android | Layout spacing -500, chưa thấy cuộn/phân trang riêng | Test 5/10/20 lá trên máy; chưa kết luận mọi lá đều không chọn được |
| Public API/direct transition | Có đường gọi bỏ qua điều kiện riêng của phase | Gom input Player/AI qua kiểm tra chung khi hoàn thiện; không cần viết lại toàn bộ |
| Nội dung chưa có effect | 7 lá chức năng mới là dữ liệu; 1 lá nằm trong deck enemy | Validator và deck nghiệm thu chỉ chứa bài đã làm |

Hành vi thả sang hàng khác nhưng tự tìm hàng theo type và cùng index đã được người dùng đồng ý tạm giữ; không ghi lại thành lỗi.

## 6. Kế hoạch thực hiện theo thứ tự

### Mốc A — Ổn định chức năng đang có

- [ ] **A1. Sửa B01:** đúng CardInstance giữa tay/sân; khóa lựa chọn chờ thế và dọn nút debug.
- [ ] **A2. Sửa B02:** lọc bẫy hợp lệ, xử lý thất bại và fallback cho AI.
- [ ] **A3. Nối PassReaction:** Player có nút Bỏ qua khi chờ; giữ bẫy úp và tiếp tục đòn; khóa đánh/chuyển phase trong cửa sổ.
- [ ] **A4. Sửa B03:** thời hạn thủ, sao hiện tại, cap 5, chặn 1 sao.
- [ ] **A5. AI nhiều đòn:** sau AttackFinished tìm đòn tiếp theo; lọc IsAttacked/thế/sao/CanAttack; hết phương án mới Main II.
- [ ] **A6. Dọn tương tác:** dùng lại boarIndex/target; không cộng listener; báo lý do từ chối hành động.

A1 làm trước mở rộng triệu hồi. A2–A3 làm trước A5. Quyền đánh đã được sửa, không triển khai lại. AI hiện đánh một đòn rồi kết thúc là chiến thuật yếu, không tự vi phạm luật tối đa một lần/quái.

### Mốc B — Khép kín trận và kiểm chứng Android

- [ ] **B1. Hết bài:** quay lại B04 trước test trận dài.
- [ ] **B2. Kết quả/chơi lại:** tải lại scene từ Result; một kết quả duy nhất; input trận cũ dừng.
- [ ] **B3. Deck nghiệm thu:** 20 lá mỗi bên, chỉ dùng nội dung có chức năng; preset đầy đủ 6 tinh thể, preset test nhanh 2.
- [ ] **B4. Tái hiện lỗi:** seed được ghi lại hoặc deck test định trước; log phase, người hành động và instance bài.
- [ ] **B5. Test Unity:** EditMode cho combat thuần; Play Mode cho phase/vùng bài/pool/effect/bẫy/replay.
- [ ] **B6. Android:** build/cài máy thật; test trọn trận, cảm ứng, tay nhiều lá, pause/resume, hướng màn hình theo cấu hình.

A+B là bản prototype rút gọn theo quyết định mới. Chưa gọi là hoàn thành toàn bộ GDD khi mục C vẫn thiếu.

### Mốc C — Các luật GDD còn thiếu, theo phạm vi giữ lại

- [ ] **C1. Tối đa một triệu hồi Soul thường mỗi lượt:** chỉ tiêu khi thành công; không tiêu cho đặt bẫy; Player/AI dùng chung.
- [ ] **C2. Thế dự kiến:** Main II chọn/hủy, End xác nhận; một quyền vào thủ mỗi lần tồn tại; tự hết hạn là chuyển bắt buộc.
- [ ] **C3. Hiến tế — từng hoãn:** sao gốc 1–4 không hiến; 5–6 hiến 1; 7–8 hiến 2; 9–10 hiến 3; 11–12 hiến 4; ít nhất 1 cùng hệ. Kiểm tra toàn bộ trước chuyển vùng; sân đầy vẫn được gọi nếu hiến tạo ô.
- [ ] **C4. Điều kiện hệ:** phép cùng hệ nếu giữ luật đó; khắc hệ do từng effect định nghĩa. Không tự thêm +1 sao cho mọi combat hoặc quay lại 5 hệ.
- [ ] **C5. Giới hạn 40 lượt — từng hoãn:** bộ đếm toàn trận riêng; kiểm tra cuối lượt, ưu tiên kết quả tinh thể. Không dùng biến Turn hiện chỉ đếm hai lượt đầu.
- [ ] **C6. Cập nhật GDD:** sửa luật phép/bẫy/ngoại lệ, trần sao, preset, phạm vi và bảng test. Làm phần quyết định trước; hoàn tất tài liệu khi chốt phạm vi nghiệm thu.

Bẫy gây sát thương sau giao chiến chỉ triển khai nếu được đưa lại vào phạm vi; không tự làm chỉ vì ví dụ GDD cũ.

## 7. Lịch làm đề xuất

Mỗi buổi khoảng **3 giờ**, dành khoảng 30 phút cuối cho test và ghi kết quả. Đây là ước lượng, chưa phải cam kết thời gian; tính lại sau mỗi mốc.

| Buổi | Công việc | Điều kiện chuyển tiếp |
|---|---|---|
| 1 | A1: định danh bài và khóa lựa chọn | Không sai lá/nhân đôi qua chọn, hủy, nút debug |
| 2 | A2: chọn/dùng bẫy AI | Có bẫy chưa gắn effect vẫn không kẹt |
| 3 | A3: Pass và dọn phản hồi | Dùng/Bỏ qua đóng đúng một đòn |
| 4 | A4: thủ theo sao và lượt | Ca 1/2/4/6/trên 10 sao đạt |
| 5 | A5–A6: AI nhiều đòn và tương tác | Không đánh lại, biết chờ bẫy, không cộng listener |
| 6 | B1: hết bài/rút thiếu và validation | Draw hoàn tất cả khi không rút được |
| 7 | B2: kết quả/chơi lại | Ba trận nối tiếp không giữ dữ liệu cũ |
| 8 | B3–B5: deck, effect, test hồi quy | Có kết quả checklist, không chỉ đường thuận |
| 9 | B6: build/cài/test Android | Chơi được một trận trên máy, ghi lỗi UI/input |
| 10 | Sửa lỗi thiết bị, nghiệm thu lại | Bản thử Android có giới hạn được ghi rõ |

Khung A+B: khoảng **30 giờ**, dự phòng **2–3 buổi**. Nếu giữ 2 tuần × 3 giờ/ngày = 42 giờ, nên nhắm A+B và dự phòng, chưa hứa cả A+B+C.

Phần C cộng thêm dự kiến: quota/thế End 2–3 buổi; hiến tế 2–3 buổi; điều kiện hệ/deck mẫu 1–2 buổi; giới hạn lượt và tài liệu khoảng 1 buổi. Muốn nghiệm thu “đủ GDD”, phải triển khai các luật giữ lại hoặc sửa phạm vi GDD rõ ràng.

## 8. Checklist nghiệm thu

### Dữ liệu và phase

- [ ] Deck 20/20; tay đầu 5; Draw đầu rút thêm 1.
- [ ] Hai lượt đầu Draw → Main II; lượt thứ ba có Main I/Battle.
- [ ] Một CardInstance chỉ nằm trong một vùng; triệu hồi thành công chuyển đúng lá.
- [ ] Hủy/thất bại/index sai/ô đầy/đổi lựa chọn khi chờ thế không mất bài.
- [ ] Hết bài vẫn qua Draw; rút nhiều hơn số còn lại không lỗi.

### Combat và bẫy

- [ ] Sáu ca công–công/công–thủ đạt trong Unity.
- [ ] Đánh trực tiếp chỉ khi không còn Soul địch; mỗi đòn đúng một tinh thể.
- [ ] Yêu cầu sai không tiêu quyền; đòn hợp lệ/hòa/bị chặn đều tiêu quyền.
- [ ] Quái không đánh lần hai cùng lượt; quái thủ không tấn công.
- [ ] Bẫy chặn không ai mất sao, nguồn về mộ đúng một lần.
- [ ] Bỏ qua giữ bẫy úp, tiếp tục đòn, không mở lại cùng sự kiện.
- [ ] Bẫy có effect + không effect cùng sân không làm AI kẹt.
- [ ] Chờ phản hồi thì không đánh mới/chuyển phase; listener không cộng dồn.

### Phép và thủ

- [ ] AI không có phép/ô/target: bỏ qua, tay không đổi, phase tiếp tục.
- [ ] Quái 4 và 20 sao cùng sân: buff chọn target nhận được phép.
- [ ] Hai nguồn buff: hết một nguồn chỉ gỡ phần của nguồn đó.
- [ ] Gốc 4 +2, mất 3 còn 3; hết buff còn 1, không hồi về sao gốc.
- [ ] 19 nhận +2 dưới trần 20: thực nhận 1, gỡ chỉ trừ 1.
- [ ] Phá nguồn sớm gỡ effect giống hết hạn; sao <=0 vào mộ/release một lần.
- [ ] Một source nhiều target: target A chết không cản gỡ ở target B còn sống.
- [ ] Quái 2 sao hết thủ ở Standby chủ kế tiếp; sao đổi không reset elapsed.
- [ ] Cap thủ 5; quái 1 sao không thủ; thời điểm expiry/kiểm tra thủ có quy tắc thống nhất.

### Kết thúc và thiết bị

- [ ] Preset 6 tinh thể: sống sau 5 đòn trực tiếp, kết thúc ở đòn thứ 6.
- [ ] Một kết quả duy nhất, không nhận hành động trận cũ sau Result.
- [ ] Chơi lại 3 trận: không cộng deck/giữ modifier/lựa chọn/listener.
- [ ] Tay 5/10/20 lá: mọi lá xem/chọn/kéo được trên Android.
- [ ] Cảm ứng kéo/hủy/thả/chọn mục tiêu hoạt động đúng.
- [ ] Pause/resume và hướng màn hình đạt theo cấu hình đã chọn.
- [ ] Có APK chạy được; ghi thiết bị, phiên bản build và lỗi còn lại.

## 9. Kiểm chứng đã thực hiện và giới hạn

- Đọc source/diff, GDD prototype, scene/prefab, cấu hình và dữ liệu; xác nhận số card/deck/slot/effect nêu trên.
- Unity **6000.3.8f1**; URP 17.3.0, uGUI 2.0.0, Input System 1.18.0, Test Framework 1.6.0.
- Chạy source thật BattleResolver.cs qua PowerShell Add-Type với CardInstance tối thiểu: **6/6 ca số học đạt**.
- Công–công: 8/4 → 4/-4; 4/8 → -4/4; 4/4 → 0/0. Công–thủ: 8/4 → 8/-4; 4/8 → -4/8; 4/4 → 4/4.
- Sao âm của quái chết đang được chấp nhận trong thử nghiệm; không tự xếp thành lỗi cần sửa.
- git diff --check không báo lỗi whitespace; có cảnh báo chuẩn hóa LF/CRLF.
- Editor.log có lần compile Assembly-CSharp thành công trước audit; đây là log có sẵn, không phải lần Unity compile do audit khởi chạy.
- Di chuyển AddStars giữ GUID, hai effect asset còn trỏ đúng; không thấy lỗi GUID từ riêng việc di chuyển.

**Chưa thực hiện trong audit:** Unity Play Mode/Test Runner, build APK/AAB, cài thiết bị, profiling hoặc test toàn trận có UI. Không thấy bộ test riêng của game trong Assets; cài Test Framework không đồng nghĩa có test coverage. Sáu ca C# độc lập không chứng minh lifecycle, UI, scene hoặc build Android.

## 10. Giữ phạm vi và đóng gói

- Hai phép cộng sao và một bẫy chặn đủ kiểm chứng vòng đời effect hiện tại; chưa cần làm toàn bộ 42 lá thành hiệu ứng riêng.
- Chưa đưa chiến dịch, kinh tế, phần thưởng, deckbuilder ngoài trận, multiplayer, chain hoặc hồi sinh vào mốc hoàn thiện trận.
- Phép rút nhiều lá cần B1 trước; bẫy phá quái cần resolve bằng context trước.
- CardOnBoard 1.prefab chưa được các scene/prefab/asset đã quét tham chiếu; xác nhận mục đích trước khi dọn. Không tự xóa.
- Khi commit sau này, di chuyển AddStars phải đi cùng .meta giữ GUID; chia commit theo chức năng/lỗi, không trộn tất cả.
- Sau nghiệm thu: README đúng luật/phạm vi, video một trận, APK, sơ đồ luồng và kết quả test thực tế. Chưa ghi “Android hoàn chỉnh/đã tối ưu” chỉ từ settings hoặc việc dùng pool.

**Việc bắt đầu đề xuất: A1 — thống nhất định danh lá bài trong triệu hồi.**
