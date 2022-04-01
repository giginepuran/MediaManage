SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN ('__tags__');

IF (SELECT COUNT(TagID) FROM #TagIDs) != 0
  INSERT INTO VideosTags (YoutubeID, TagID)
    SELECT '__YoutubeID__' AS YoutubeID, a.TagID AS TagID
    FROM #TagIDs AS a;
