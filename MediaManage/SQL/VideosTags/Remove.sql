SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN ('__tags__');


DELETE FROM VideosTags
  WHERE YoutubeID = '__YoutubeID__' AND TagID IN (SELECT TagID FROM #TagIDs);
