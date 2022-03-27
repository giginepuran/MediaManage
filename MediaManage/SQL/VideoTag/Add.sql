DECLARE @tail INT = (SELECT MAX(TagID) FROM VideoTag);
INSERT INTO VideoTag(TagID, TagName)
  VALUES (@tail+1, N'__TagName__');