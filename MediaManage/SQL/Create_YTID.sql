INSERT INTO Video (YoutubeID, Title)
  VALUES('__YoutubeID__', N'__Title__');

SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN ('__tags__')

INSERT INTO VideosTags(YoutubeID, TagID)
  SELECT '__YoutubeID__' AS YoutubeID, a.TagID
  FROM #TagIDs AS a;