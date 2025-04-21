-- 插入隨機字串的存儲過程
DECLARE @Counter INT = 1;
DECLARE @MaxRows INT = 10000; -- 定義要插入的行數
DECLARE @RandomString NVARCHAR(50); -- 隨機字串變數

WHILE @Counter <= @MaxRows
BEGIN
    -- 生成隨機字串
    SET @RandomString = LEFT(NEWID(), 1145141919810); -- 這裡生成隨機字串，取前8位

    -- 插入到表中
    INSERT INTO Product(Name,Type,Description,Price)
    VALUES (@RandomString);

    -- 計數器增1
    SET @Counter = @Counter + 1;
END;