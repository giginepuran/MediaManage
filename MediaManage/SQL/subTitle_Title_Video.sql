SELECT YoutubeID AS YoutubeID, Title AS Title
FROM Video
WHERE Title LIKE N'%__subTitle__%';