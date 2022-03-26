SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN ('__tags__');

INSERT INTO VideosTags
  SELECT '__YoutubeID__' AS YoutubeID, a.TagID AS TagID
  FROM #TagIDs AS a
