SELECT YoutubeID AS YoutubeID, Title AS Title
  INTO #id_title_match
  FROM dbo.Video
  --WHERE YoutubeID like '%__subID__%' AND Title LIKE N'%__subTitle__%';
  WHERE YoutubeID like '%%' AND Title LIKE N'%%';

SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName IN('__tags__');

DECLARE @num_of_tag INT = (SELECT COUNT(*) FROM #TagIDs);
IF @num_of_tag = 0
  BEGIN
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
  END;
ELSE
  BEGIN
  SELECT a.YoutubeID AS YoutubeID, Count(a.YoutubeID) AS #
    INTO #Candidate
    FROM VideosTags AS a
    INNER JOIN #id_title_match AS b
    ON a.YoutubeID = b.YoutubeID
    WHERE a.TagID IN (SELECT TagID FROM #TagIDs)
    GROUP BY a.YoutubeID;

  SELECT b.YoutubeID AS YoutubeID, b.Title AS Title
    INTO #Result
    FROM #Candidate AS a
    INNER JOIN #id_title_match AS b
    ON a.YoutubeID = b.YoutubeID
    WHERE a.# = @num_of_tag;

  SELECT a.YoutubeID AS YoutubeID, a.Title AS Title, c.TagName AS TagName
    FROM #Result AS a
    INNER JOIN VideosTags AS b
    ON a.YoutubeID = b.YoutubeID
    INNER JOIN VideoTag AS c
    ON b.TagID = c.TagID;
  END;
