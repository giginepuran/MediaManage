-- Usage: replace the following strings
-- Replace __subID__ Like 1a2b3c
-- Replace __subTitle__ Like cover OR 歌%cover
-- Replace '__tags__' Like N'japanese',N'紫咲シオン'

SELECT YoutubeID AS YoutubeID, Title AS Title
  INTO #id_match
  FROM dbo.Video
  WHERE YoutubeID like '%__subID__%';

SELECT YoutubeID AS YoutubeID, Title AS Title
  INTO #Filter1
  FROM #id_match
  WHERE Title LIKE N'%__subTitle__%';

SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN('__tags__');

SELECT b.YoutubeID AS YoutubeID
  INTO #videos
  FROM #TagIDs AS a
  INNER JOIN VideosTags AS b
  ON a.TagID = b.TagID;

SELECT COUNT(YoutubeID) AS Frequency, YoutubeID AS YoutubeID
  INTO #TagCount
  FROM #videos
  GROUP BY YoutubeID;

DECLARE @num_of_tag INT = (SELECT COUNT(*) FROM #TagIDs);

SELECT YoutubeID AS YoutubeID
  INTO #Filter2
  FROM #TagCount
  WHERE Frequency = @num_of_tag;

IF @num_of_tag = 0
  INSERT INTO #Filter2
  SELECT YoutubeID AS YoutubeID
  FROM Video;

SELECT a.YoutubeID AS YoutubeID, a.Title AS Title
  INTO #Result
  FROM #Filter1 AS a
  INNER JOIN #Filter2 AS b
  ON a.YoutubeID = b.YoutubeID

SELECT a.YoutubeID AS YoutubeID, a.Title AS Title, c.TagName AS TagName
  FROM #Result AS a
  INNER JOIN VideosTags AS b
  ON a.YoutubeID = b.YoutubeID
  INNER JOIN VideoTag AS c
  ON b.TagID = c.TagID
  ORDER BY a.YoutubeID ASC