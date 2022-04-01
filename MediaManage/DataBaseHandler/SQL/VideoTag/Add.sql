DECLARE @tail INT = (SELECT MAX(TagID) FROM VideoTag);
IF @tail IS NULL
  SELECT @tail = 0;
INSERT INTO VideoTag(TagID, TagName)
  VALUES (@tail+1, N'__TagName__');