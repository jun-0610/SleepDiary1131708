-- ==========================================
-- 睡眠品質日記 (Sleep Quality Diary) 資料庫設定腳本
-- 學號：1131708  姓名：梁文心
-- 適用資料庫：SQL Server
-- ==========================================

-- 1. 建立使用者資料表 (TableUsers1131708)
CREATE TABLE TableUsers1131708 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL, -- 登入密碼 (為求方便測試與演示，可用明碼或簡單雜湊)
    FullName NVARCHAR(100) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. 建立睡眠紀錄資料表 (TableSleepRecords1131708)
CREATE TABLE TableSleepRecords1131708 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    LogDate DATE NOT NULL,
    SleepHours DECIMAL(4,2) NOT NULL, -- 睡眠時數 (例如 7.5 小時)
    QualityScore INT NOT NULL CHECK (QualityScore BETWEEN 1 AND 10), -- 睡眠評分 (1-10分)
    Factors NVARCHAR(500) NULL, -- 影響因素 (例如：咖啡因、睡前運動、滑手機)
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_SleepLogs_Users_1131708 FOREIGN KEY (UserId) REFERENCES TableUsers1131708(Id) ON DELETE CASCADE
);

-- 3. 建立索引以優化查詢
CREATE INDEX IX_TableSleepRecords_UserId_Date_1131708 ON TableSleepRecords1131708 (UserId, LogDate DESC);

-- 4. 插入預設測試資料 (測試帳密：admin / password123)
INSERT INTO TableUsers1131708 (Username, Password, FullName)
VALUES ('admin', 'password123', N'梁文心');

-- 5. 插入測試睡眠數據 (為 admin，UserId = 1)
-- 這些數據會跨越過去兩週，方便測試「週平均」與「分頁顯示」功能
INSERT INTO TableSleepRecords1131708 (UserId, LogDate, SleepHours, QualityScore, Factors)
VALUES 
(1, CAST(GETDATE()-1 AS DATE), 7.50, 8, N'喝溫牛奶, 睡前冥想'),
(1, CAST(GETDATE()-2 AS DATE), 6.20, 5, N'睡前滑手機, 喝咖啡'),
(1, CAST(GETDATE()-3 AS DATE), 8.00, 9, N'無咖啡因, 運動'),
(1, CAST(GETDATE()-4 AS DATE), 5.50, 4, N'工作壓力大, 喝茶'),
(1, CAST(GETDATE()-5 AS DATE), 7.00, 7, N'睡前閱讀'),
(1, CAST(GETDATE()-6 AS DATE), 7.80, 8, N'無特殊影響'),
(1, CAST(GETDATE()-7 AS DATE), 8.20, 9, N'週末放鬆'),
(1, CAST(GETDATE()-8 AS DATE), 6.00, 6, N'看電視到太晚'),
(1, CAST(GETDATE()-9 AS DATE), 7.10, 7, N'正常睡眠'),
(1, CAST(GETDATE()-10 AS DATE), 5.80, 4, N'蚊子吵, 睡眠中斷'),
(1, CAST(GETDATE()-11 AS DATE), 8.50, 9, N'下雨天好睡');
