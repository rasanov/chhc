﻿CREATE TABLE [dbo].[Trainings] (
    [Id] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](max),
    [Text] [nvarchar](max),
    CONSTRAINT [PK_dbo.Trainings] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[TrainingMedias] (
    [Id] [int] NOT NULL IDENTITY,
    [Url] [nvarchar](max),
    [Training_Id] [int] NOT NULL,
    CONSTRAINT [PK_dbo.TrainingMedias] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Training_Id] ON [dbo].[TrainingMedias]([Training_Id])
CREATE TABLE [dbo].[MultichoiceQuestions] (
    [Id] [int] NOT NULL IDENTITY,
    [Question] [nvarchar](max),
    [Training_Id] [int],
    CONSTRAINT [PK_dbo.MultichoiceQuestions] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Training_Id] ON [dbo].[MultichoiceQuestions]([Training_Id])
CREATE TABLE [dbo].[Answers] (
    [Id] [int] NOT NULL IDENTITY,
    [Text] [nvarchar](max),
    [IsCorrect] [bit] NOT NULL,
    [MultichoiceQuestion_Id] [int],
    CONSTRAINT [PK_dbo.Answers] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_MultichoiceQuestion_Id] ON [dbo].[Answers]([MultichoiceQuestion_Id])
CREATE TABLE [dbo].[UserSettingsTrainings] (
    [Id] [int] NOT NULL IDENTITY,
    [IsCompleted] [bit] NOT NULL,
    [Training_Id] [int] NOT NULL,
    [UserSettings_Id] [int] NOT NULL,
    CONSTRAINT [PK_dbo.UserSettingsTrainings] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Training_Id] ON [dbo].[UserSettingsTrainings]([Training_Id])
CREATE INDEX [IX_UserSettings_Id] ON [dbo].[UserSettingsTrainings]([UserSettings_Id])
CREATE TABLE [dbo].[UserSettings] (
    [Id] [int] NOT NULL IDENTITY,
    [MembershipUserId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.UserSettings] PRIMARY KEY ([Id])
)
ALTER TABLE [dbo].[TrainingMedias] ADD CONSTRAINT [FK_dbo.TrainingMedias_dbo.Trainings_Training_Id] FOREIGN KEY ([Training_Id]) REFERENCES [dbo].[Trainings] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[MultichoiceQuestions] ADD CONSTRAINT [FK_dbo.MultichoiceQuestions_dbo.Trainings_Training_Id] FOREIGN KEY ([Training_Id]) REFERENCES [dbo].[Trainings] ([Id])
ALTER TABLE [dbo].[Answers] ADD CONSTRAINT [FK_dbo.Answers_dbo.MultichoiceQuestions_MultichoiceQuestion_Id] FOREIGN KEY ([MultichoiceQuestion_Id]) REFERENCES [dbo].[MultichoiceQuestions] ([Id])
ALTER TABLE [dbo].[UserSettingsTrainings] ADD CONSTRAINT [FK_dbo.UserSettingsTrainings_dbo.Trainings_Training_Id] FOREIGN KEY ([Training_Id]) REFERENCES [dbo].[Trainings] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserSettingsTrainings] ADD CONSTRAINT [FK_dbo.UserSettingsTrainings_dbo.UserSettings_UserSettings_Id] FOREIGN KEY ([UserSettings_Id]) REFERENCES [dbo].[UserSettings] ([Id]) ON DELETE CASCADE
/*
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](255) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId])
)
BEGIN TRY
    EXEC sp_MS_marksystemobject 'dbo.__MigrationHistory'
END TRY
BEGIN CATCH
END CATCH
*/
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201402061833026_InitMigration', 0x1F8B0800000000000400DD5CDD729C3614BEEF4CDF81E1AAED4C16DB4967DACC6E32CEDA6E3C8DEDC4EBE4D6238376AD29080AC25DF7D57AD147EA2B5462810549C0818535F69D17A4A3F3F3217D3AD2F17FFFFC3B7DBFF65CE3018711F1E9CC3C9C1C9806A6B6EF10BA9A99315BBEFAC57CFFEEFBEFA6A78EB736BE65ED5E8B76BC278D66E63D63C15BCB8AEC7BECA168E2113BF4237FC926B6EF59C8F1ADA383835FADC3430B731126976518D3EB9832E2E1E407FF39F7A98D031623F7C277B01BA5CFF99B4522D5B8441E8E0264E39939FFF8713E3939FE3439A7CB10452C8C6D1687D8348E5D82B83A0BEC2E4D2378F3F66B84172CF4E96A112046907BF31860FE7E89DC08A7CABF0DDE40F53F3812FA5B88529F71713EED64BF995BC66D3BE53E608F42ADC4BE9979132242B9E38BAD78BBDFF163E9017FF439F4031CB2C76BBC4CFB9E3BA66195FB5972C7BC5BA18F189EFF45D9EB23D3B88C5D17DDB938F71277E382F921FE0D531C22869DCF88311C52D11727EA2BA34A63DC10E6E26C181E0F619E7146D6D8F984E98ADDE7435DA075F684FF691A5F29E130E49D78847151B5CDEF8651F19A0D3FE8257A20AB040CD2F017D821C834AEB19BBC8DEE49B001E6240B71D2E2360FB87116FADEB5EF1640506E717B83C2151636F9B5CD167E1CDA2D34FD1293BFB58A5EC42E23F6BD4F6CFC25C69178AB55B7AE9DA2746DE3B6AAF3EF3B5C60C678DF281312696DD1B5D41A53DB50B1A6BEB5CE9CA9B5FDE64133C106482F683AF81ABA4FF85D6E63DEE7A79945BAE1D3CCE0D309109A2FE725C122B769A4D8E865369471029A3AA1EA1FD3E82FCECEB4DA6FDEDDEA3054D0BDBA9532F3D534DD69DADBC87D49C0DE0F11910D8BE67E18623B1FF983CFC386A8C64230A3D1616708A8C91F0900959DA0A65BBC5F12F00406BCC0C5BC6F7F28A89D237B62593200609CAC0B69845B51EED56049F121CC9A528FDE50FD92D07C81BD3BBEBAF130090B9B47DCD31EA23B2E407B092D9220B8388E22DF268909BA0DC56DC584774A1D03C67437EB99C29AF93A26E6E9C02536D76A66FEA444A27188DC31EA10B2F4435306F4153DC162CA338E6D96E4A9E628B291A30282FBCD29F9B1E0B17A47D632C12A5B61B4706BB176F984BB16B6276F76F0C1647228030EECA81A2650A53784166CB54EC9620BBF40186EAB20ECE4A0FAF5B2CA06E0E2B935434B745A380D981319E9E7DA3C6DB7B25BBFB60FEC6CFDC2A11FF4099CBE5982E63E655C5F1CA68E3FF13DFE533C153B20D95ED183AB2C2D4E5CFBED7AA6A049719A5E48BA10550A4ADF3748D37CFC3AEDB4734483E87CA7AE48CB66B406017A9029D2F4CD5AC86E90A9C82AE0A3222C85ED40A1717DFE4DC62F98A4E4A629D8B05ACBCCBE3A8D4CD50F56D9110027D5A7975457C149487B1A523051FF11D4780F443C06F2615DE641F520949DB4E52705E3F2EFBCC6610046D2321E1D3CD7B071579DD782B974E02E058321535807B2321002015B44A02FAB094A478AD2BB4FB59CA4629416AECD36B4399BD8DE04B0365701B22B0356C59D81E9050A0231EAB667FAC458A417085E2DDA9FDD7B1B19961D698EF0736DF391981FA21596DEF2A1B9A667248CD80962E80E894CC5DCF1946620EE948D2553283590D91A9EF5107F6F7A55DCA598547DCB5B679E71FB3C4C59622AD67C506AD7E436077251A8C91FCD7D37F668550EAAAE777ABFA028207DD4424692A42F89489EA812A69664BFEC6D4B71B742AACBF16B155DCD71F08E11D64A6C11E68AFEC3C43A393C2E764F1E8C264ADAE5B98F6001580A20642029C3046ECBC48A32AA764C4F18C28CB3F51135DD61262850551D079A40C1935FE5F8DBE3C6921ADBC7A3896FF3A95FC760430403420F13330C104A27867220F317A30C65EF21DC3174FB0A997A2C5694A5BEDD7FF0CA4CBF86D514369C2DD94BA1672B9622B63815E7DBFAEC8FEA3C50487331BA082459D64C406705D36D5E4705DBE925EFDE5A87BD3ED9D5890DD5400098E169BEEE32463840127A7B018591DCE1774872F2721E89F3F8FC2C1E68FCCEB8AA4B0042D99ABE378CA069E2D39CDEEB181D9DC41EF0D49CE01C159A605ED819580DF9D16EE4B066CA0292543978A0DCE708262D5016F8792C65808C6F477494850C89105D26B7A3F74BA286428A2EBF3D12B428D96BB949CEB6F32CB694AD9EA6996340199C9C4ADE34310DAEFB0371441A79F11831EC4D4483C9E24F77EE12CEEFB70D2E10254B3E6BDEF87F603A338F0E0E7E968AE83A14B85951E4B863AD7223C2FCC69B884D9709EB0ADCE8030AED7B14FEE0A1F58FBB14ADB510F45C8A8A06F17EA19E6837DF1717B882BEB5EA3CAFEA9D41FC2F17EE0C1C841DD0FF54B525C34C3ADDA60A8D9E72A9C81D6942BD2AA48A87F71DC271D46C0C12504DB9469748749AC634B3AA4CE47A9F119FB65061901056D52874F75A6FD7F8F77C3F57AB437AF7AE7BE9000F3E0E456C90CB0936DF3FF1DE4ABAFA7348A84D02E4CA1E50B708105009DB7291F29B131C602AD0A2B5143260C3B63A1F40F2769327862E6768052BF58E7C495665BA6E975288D1430576A23F46C07428EBE821C40D20CA2E4E77A80EE90495AEE1DB113595370AD491E009E23D60A65BA5CB18162FFD65C99D0B3E463F3F01AF338C7182EAA7E2E7090A6C460BBC868B11E3011F20FFDC3B00F5D54FEA455F39D0FAE2A5DA02A84D8677663A773E87C26637517D7F7DC7FAA8BAC19226CD236AD7755809956E7450F583BED8AAA6D64A375245E546BF8558BA812BCAD1FBACD96A1A7634555DDA320D78ED16BCFE6BBCF55ADD5D51F3E955DE8A791625583D18A6DD39A8B73B9E515D55779CD4CD60E06AA1E7562A052E54DAABAB5A943EA987C49CC014FEA32A675411596D4588DB3414DB25EA92B739A74B3FA3529246591325D1C990C379CD71C8C812D98CBFB6711425E67E436ECC9B9C7A77D839A757310B62C64DC6DE9D5BFA272F8289D58D9FD47795759E5E05C99ADF87095C4DC24DC057F4434C5C27D7FB4C939CAD1021285E9A3D16B164228BBC7ACC255DFA142828755FCE4C6FB017B85C58744517E80157EBD6ECC3B2C7A62704AD42E445A98C6D7FFE93C3CFF1D6EFFE07621ECF6E13580000, '5.0.0.net45')
