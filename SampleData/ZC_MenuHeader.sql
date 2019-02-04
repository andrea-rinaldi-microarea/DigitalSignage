IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ZC_MenuHeader]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[ZC_MenuHeader] (
    [TBGuid] [uniqueidentifier] NULL CONSTRAINT DF_ZC_MenuHeader_TBGuid_00 DEFAULT ('00000000-0000-0000-0000-000000000000'),
    [MenuheaderCode] [varchar] (10) NOT NULL,
    [Week] [smallint] NULL CONSTRAINT DF_ZC_MenuHeader_Week_00 DEFAULT (0),
    [FromDate] [datetime] NULL CONSTRAINT DF_ZC_MenuHeader_FromDate_00 DEFAULT ('17991231'),
    [ToDate] [datetime] NULL CONSTRAINT DF_ZC_MenuHeader_ToDate_00 DEFAULT ('17991231'),
    [Description] [varchar] (100) NULL CONSTRAINT DF_ZC_MenuHeader_Description_00 DEFAULT (''),
    [Storage] [varchar] (8) NULL CONSTRAINT DF_ZC_MenuHeader_Storage_00 DEFAULT (''),
    [Background] [varchar] (128) NULL CONSTRAINT DF_ZC_MenuHeader_Background_00 DEFAULT (''),
    CONSTRAINT [PK_ZC_MenuHeader] PRIMARY KEY NONCLUSTERED
    (
        [MenuheaderCode]
    ) ON [PRIMARY]
) ON [PRIMARY]
END
GO
