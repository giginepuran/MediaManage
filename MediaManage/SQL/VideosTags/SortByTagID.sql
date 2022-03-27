SELECT YoutubeID AS YoutubeID, TagID as TagID
  INTO SORTING_BACKUP
  FROM VideosTags;

TRUNCATE TABLE VideosTags;

DECLARE @tag_id INT = (SELECT MIN(TagID) FROM SORTING_BACKUP)
DECLARE @tail INT = (SELECT MAX(TagID) FROM SORTING_BACKUP)

WHILE @tag_id <= @tail
BEGIN
  INSERT INTO VideosTags
  SELECT *
  FROM SORTING_BACKUP
  WHERE TagID = @tag_id
  SELECT @tag_id = @tag_id + 1;
END;

--DROP TABLE SORTING_BACKUP