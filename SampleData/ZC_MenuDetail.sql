IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ZC_MenuDetail]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[ZC_MenuDetail] (
    [MenuheaderCode] [varchar] (10) NOT NULL,
    [Day] [smallint] NOT NULL,
    [ItemCode] [varchar] (21) NULL CONSTRAINT DF_ZC_MenuDetail_ItemCode_00 DEFAULT (''),
    [Description] [varchar] (250) NULL CONSTRAINT DF_ZC_MenuDetail_Description_00 DEFAULT (''),
    [SalesPrice] [float] NULL CONSTRAINT DF_ZC_MenuDetail_SalesPrice_00 DEFAULT (0),
    [MenuID] [smallint] NOT NULL,
    [Background] [varchar] (128) NULL CONSTRAINT DF_ZC_MenuDetail_Background_00 DEFAULT (''),
    [Picture] [varchar] (128) NULL CONSTRAINT DF_ZC_MenuDetail_Picture_00 DEFAULT (''),
    [estimatedquantity] [int] NULL CONSTRAINT DF_ZC_MenuDetail_estimatedquantity_00 DEFAULT (0),
    CONSTRAINT [PK_ZC_MenuDetail] PRIMARY KEY NONCLUSTERED
    (
        [MenuheaderCode],
        [Day],
        [MenuID]
    ) ON [PRIMARY]
) ON [PRIMARY]
END
GO
