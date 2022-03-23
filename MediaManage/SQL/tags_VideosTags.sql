SELECT * 
INTO #ContainTag
FROM VideoTag
WHERE TagName IN('__tags__');

SELECT CT.TagID AS TagID, VTs.YoutubeID AS YoutubeID
INTO #MatchVideo
FROM #ContainTag AS CT
INNER JOIN VideosTags AS VTs
ON CT.TagID = VTs.TagID;

SELECT COUNT(YoutubeID) AS Frequency, YoutubeID AS YoutubeID
INTO #TagCount
FROM #MatchVideo
GROUP BY YoutubeID;

DECLARE @num_of_tag INT = (SELECT COUNT(*) FROM #ContainTag);
SELECT YoutubeID AS YoutubeID
INTO #ResultID
FROM #TagCount
WHERE Frequency = @num_of_tag;

SELECT VsTs.YoutubeID AS YoutubeID, VsTs.TagID AS TagID
FROM VideosTags AS VsTs
INNER JOIN #ResultID AS R
ON VsTs.YoutubeID = R.YoutubeID;