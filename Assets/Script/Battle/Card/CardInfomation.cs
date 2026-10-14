
using System;
using System.Collections.Generic;

public enum CardNameId
{
    // Bài quái
    DivineSeaDragon = 0,          // Tinh Long — Thủy Thần Long
    DarkKnight = 1,               // Kỵ sĩ Bóng Đêm
    Wyvern = 2,                   // Thằn Lằn Bay
    HeadlessWhiteKnight = 3,      // Kỵ sĩ Giáp Trắng Không Đầu
    SkeletonKing = 4,             // Vua Bộ Xương (Skeleton King)
    ImmortalDemonKing = 5,        // Quỷ Vương Bất Tử
    EarthDragonKing = 6,          // Thổ Long Vương
    SpectralKnight = 7,           // Bóng Ma Hiệp Sĩ
    StarfallPaladin = 8,          // Hiệp sĩ Sao Rơi
    CelestialGriffin = 9,         // Sư Điểu Thiên Quang
    AbyssalSeraph = 10,           // Thiên Sứ Vực Sâu
    VoidHound = 11,               // Ma Khuyển Hư Không
    SaintOfLivingThorns = 12,     // Thánh Nữ Gai Sống
    AncientGroveStag = 13,        // Lộc Vương Cổ Thụ
    PlagueRider = 14,             // Kỵ sĩ Dịch Hạch
    FuneralBellWarden = 15,       // Hộ Vệ Chuông Tang
    BlindJudicator = 16,          // Phán Quan Mù
    IronCovenantAngel = 17,       // Thiên Sứ Thiết Ước
    WitchOfLament = 18,           // Phù Thủy Ai Ca
    WrathDemonLion = 19,          // Sư Ma Cuồng Nộ
    CathedralColossus = 20,       // Cự Tượng Thánh Đường
    IroncladBasilisk = 21,        // Thiết Giáp Xà Vương
    AshenProphet = 22,            // Tiên Tri Tro Tàn
    CorrosionDrake = 23,          // Long Thú Ăn Mòn
    UndeadChronicler = 24,        // Tử Thư Cổ Sử
    ReliquarySentinel = 25,       // Hộ Vệ Thánh Tích
    FacelessSister = 26,          // Nữ Tu Vô Diện
    NamelessKnight = 27,          // Hiệp sĩ Vô Danh
    FateweaverWitch = 28,         // Phù Thủy Dệt Mệnh
    FateboundExecutioner = 29,    // Đao Phủ Mệnh Tuyến
    TempestSeraph = 30,           // Thiên Sứ Bão Loạn
    ThunderChimera = 31,          // Chimera Lôi Biến

    // Bài phép
    TwinStarBlessing = 32,        // Cường Hóa Hai Sao
    SingleStarBlessing = 33,      // Cường Hóa Một Sao
    DivineMight = 34,             // Thần Lực
    TwinRevelation = 35,          // Khải Thị Đôi
    RenewedGuard = 36,            // Tái Lập Thế Thủ

    // Bài bẫy
    BarrierOfInterception = 37,   // Kết Giới Chặn Đòn
    SavingStar = 38,              // Ngôi Sao Cứu Nguy
    LethalReprisal = 39,          // Phản Sát
    WarriorsLastWish = 40,        // Di Nguyện Chiến Binh
    ChainsOfSubmission = 41,      // Xiềng Xích Thế Thủ
}

public enum CardElemental // Hệ
{
    Astral,      // Tinh Tú
    Abyss,       // Vực Thẳm 
    Life,        // Sinh Mệnh
    Death,       // Cái chết
    Law,         // Định Luật
    Emotion,     // Cảm Xúc
    Form,        // Hình Thể 
    Dissolution, // Tiêu Giải 
    Memory,      // Ký Ức  
    Oblivion,    // Lãng Quên
    Fate,        // Định Mệnh
    Upheaval,    // Biến Động 
}

public enum CardType // Loại
{
    Soul,
    Mighty,
    Rupture
}

[Serializable]
public class CardInfomationEntry
{
    public CardNameId CardNameId;
    public CardType CardType;
}

[Serializable]
public class CardElementGroup
{
    public CardElemental CardElemental;
    public List<CardInfomationEntry> CardInfomationEntries;
}
