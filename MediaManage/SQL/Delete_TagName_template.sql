SELECT TagID AS TagID
  INTO #TagIDs
  FROM VideoTag
  WHERE TagName = N'__tag__';


DELETE FROM VideosTags
  WHERE TagID IN (SELECT TagID FROM #TagIDs);


DELETE FROM VideoTag
  WHERE TagID IN (SELECT TagID FROM #TagIDs);