SELECT YoutubeID AS YoutubeID, Title AS Title
  INTO #id_title_match
  FROM dbo.Video
  WHERE YoutubeID like '%__subID__%' AND Title LIKE N'%__subTitle__%';
  --WHERE YoutubeID like '%%' AND Title LIKE N'%%';

SELECT a.YoutubeID AS YoutubeID, b.TagName AS TagName
  INTO #FullVideosTags
  FROM VideosTags AS a
  INNER JOIN VideoTag AS b
  ON a.TagID = b.TagID
  ORDER BY a.YoutubeID

SELECT a.YoutubeID AS YoutubeID, a.Title AS Title, b.TagName AS TagName
  FROM #id_title_match AS a
  INNER JOIN #FullVideosTags AS b
  ON a.YoutubeID = b.YoutubeID
  
