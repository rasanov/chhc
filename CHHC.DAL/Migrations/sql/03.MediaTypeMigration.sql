﻿CREATE TABLE [dbo].[MediaTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Key] [nvarchar](max),
    [DisplayName] [nvarchar](max),
    CONSTRAINT [PK_dbo.MediaTypes] PRIMARY KEY ([Id])
)
ALTER TABLE [dbo].[TrainingMedias] ADD [MediaType_Id] [int]
ALTER TABLE [dbo].[TrainingMedias] ADD CONSTRAINT [FK_dbo.TrainingMedias_dbo.MediaTypes_MediaType_Id] FOREIGN KEY ([MediaType_Id]) REFERENCES [dbo].[MediaTypes] ([Id])
CREATE INDEX [IX_MediaType_Id] ON [dbo].[TrainingMedias]([MediaType_Id])
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201402241642529_MediaTypeMigration', 0x1F8B0800000000000400ED1DDB6EE434F41D897F88F204484CDA0524584D1795E9162AB6DB65A7BBAF2B37F14C231227249ED2F26B3CF049FC02762617C797E4E4364D2BDE3A897DEE3E3EC73E27FDF7EF7F963FDE8781758793D48FC8897DBC38B22D4CDCC8F3C9F6C4DED1CDD7DFDB3FBEFAFCB3E56B2FBCB73E16E3BEE1E3D84C929ED8B794C62F1D27756F7188D245E8BB4994461BBA70A3D0415EE4BC383AFAC1393E76300361335896B57CBF23D40F71F683FD5C45C4C531DDA1E032F27090E6CFD99B7506D57A8B429CC6C8C527F6EA975F568BB3D3378B0BB249504A939D4B7709B6ADD3C0478C9C350E36B6157FFBF2438AD73489C8761D23EAA3E0FA21C6ECFD060529CE897F197F0BA5FFE805A7DF41844494818B482FFEED9233C6DB6B2603FAC0C9CAF83BB1AF13E413267871141BF72B7EA83D608FDE25518C13FAF01E6FF2B9179E6D39F5798E3CB19C26CCE1E8D95F847EF3C2B6DEEE8200DD04B8941213E39A4609FE19139C208ABD7788529C103E1767E42B58251CD73E0D708186E983B3679DFBF7D87B83C996DE96A82ED17DF184FD695B1F88CFCC904D621AC62269FBDF2D58F13D9D1EE95B74E76F336390D05F62CF47B6F51E07D9DBF4D68FF786B928549C8DF8542ADC3A4FA2F07D140846501FF1E91A255BCC798A1A87ADA35DE276A0F4B79DFF9796D0CB5D407DF736F25DFCDB0EA7FCAD96DCA6710AD18D83BB92CED677B2C694B2B9690124D5F2A21BA965A671A0C24DF3681D3B4BA75AF3204FB037A4FFDDC14077F021091ED11B549636A64328ECABC5211446DBC975715101A815C69AC82D87B4D05B8DD3110C5E371545CF68CD301A0EBE62CEFC340ED003FF31316EB86ED5CDE33969B9E469A68E6A9480407602A0E8014AFE2949FF64098A96FAFDBB4F3A1B1268378F5236FF86A18376FE3DDCE764D88789C565C6D2559424D82D31FF1431B521A2E110BC33EA6C670A53931709C02A7B999A2E7E7D4E86C76D208C03CCE68E67058D3E72A444433600585AD2276F8273519FD5C289F810C64D6DC66856FD9CACF91287376C77636AE21CB6633C501ADDDF2E40E9B4D6922076719AA691EB672CE872EA4F0687F79A78162CEDDAEF674A52C4F631EEA7E3C077195527F6578A265A5194825151C8D08F6DD9A0AFC819E62ECF3A75697654BB42A98B3CD52098DCBC9A1C05897511A429E532B1A9C9084717A59A4D0A3884ACB50EFF68B138968D0B2C94C6F0D844352C561648D7C5147021C1CEEADAAD6E90A01AC22313DD9058A9A23A8FA03BC80512F67752C22001350711261E801145C58636FAEB2034E059E94C7D58FB5ED6896F7DC033B1B0F5BBA91EE923087DBF2FAF224219BD38290E96A290FDE44F795A28F3CB673092A58D86515F6DF28A352942D303C9B71423A0FC7D0BB472F3D0D124EC2C6D60541FA205A873352DA0CB5310055AE1185B00E86D5581A61FD601760B4C0596606606ED0AA99630B8F9A05D5E06E000B0644D3131A733CC62F16A60AA7270EA82E82C24C14ADBA46488ED3A457723C8490DE704A0E28A1C2CA9E6434E555CF0A8AF7BDC27F2A875170DF203457A13595BD3F9972A416838D8352014982B3D6283C0002160477DF4905CCBF1912ABC0EA1628F60516018E2EC7B4487135920E0A002284B7344D833261C5DA6DA20D080A583688B6395327CAB4AB29C7D4D56FE60E9188AB79697288E39D66A66FEC45AE7955C5FAFBB1751857B188E9B6A6AA94A6A4B4C344AD0164B6F196A46E9B99FA4F40C517483F879D9CA0B9561A060B5C025C7ACAA228B68A798C1FFDECF3214B52D4C6BB912E639E32FC48466AC62CD8252A75ABCAC0E0528D19C62AEA2601712D34968D3ECBCB24304903FEA0023BB2AAA81C89EA810968EC4BF2C6D4711B792C5D4F5D749BB9ABA9C811AD642ECA066C3FCF9EA3AABC91121640F66A36921AC1C43CDC6481AA0E286B9D3A8372B3811A7670FE0F36BC523229CDA8BF9A85A17CD8DA2F4F6A016A27E0894690CA10ADC4518A6A38847546111E28FA1355D05064851A68913F960F05E69C45FD548D4C8A81ECF46BFEDA50A3D950D010C503D0CCC3486502B73901559BE98A52A4757E140D51D4A65EA5DBE084B7D7B78E5D513C3862058389FE818EC0A333B05B53C233614E5E88F5555E181545A82D16920BB052900F426303F15E8496037BAE4647FA0DA8553E4AE7A17A68EAB78E59CB8A7602B38A3AB5E391F3F88EEADACC5CEF3B30BD08B94D70A95754250C607DB4FF3D97AAF68BAC185000F94DB6B3CE7E84E20F707B332AC06E607DB55D37D0334DAD7CF8605F81AFDB4DF26F4F54C1A8823D853FB7DCAACAC092685C186D5721DD32FB9687059C02447561EE8AA65064E0B74E9F4344221C005534FEBA80399D2427417473DA55F033595A5E8AED366622DCA65993CA4CCD6CA4B33E9726C995F54013E7F20DF5CED87D816A3FDCEF7F8ADD5FA21A5385CF0018BF51FC12AF0597E580DB844C4DF30AF791DFD8EC989FDE2E8E83BE9E3093D3E6CE0A4A917CCF5EB063E67BFB5FCBEAD82BEA99399DCA1C4BD45C91721BAFF72C8C70A3A007A2ACDE43397BED01B3E4C8BE2562970DE91B17A0AA68533A487F6F1FAA3273103A1357A90F234EDCE53ACC459B4304FA208B97B79E2A53440078FD5603B8D1BECB77568E894FB656FFC1EBECB90978DADC27934AE4EA2504DCF6A1F4D8CB41929813D14D413E9D69C4485A646CDFE521BAD97F1C0FD385A1AF2E2EFFE4D7F4CF938E1BA41014BB8583ECD662BD75FEF129FB87E8C0259026ACA08312ACE5B09527E7386634CB8B5683985206C3966291148D26E93C4843D9DD2F54FF7D6CA19D946536DD60C8CA3ED36EA00D6D1ABB9B593D3693110E3E1FE90C6D8D93B1258FDD81CDD498F26DF1154DC624445FF5B8F5EE17E7EA5A7FA065A8DB17E4DE35AC0D74907B0997E7DCF73086DF49D1C83DB7F67EF9F80C573737450E3F47F3F42BBF56C0DAFA50C6F3EC607B8AD1ADD00A5FBA19C12B50B4956B4BE95BDB11D7E7F1F74627B371133857DAE696EAE33A10076CB3721CB86B463147206059B5829A6C1D4D0FAAB60D1450F2A3E6D15880E33A40154A6A1083614B4C50B1D2643F3EAB85DFB3AC486EF428DD9E0DF8676369F00D076AAC21BD8E14DF0736BEE5716A7D490340F111CA86BBFBF3534781F63B1E29368C41F81316D8AA616DD3DA1EEFAFE76D2E4C4C13DE34FAD611EDCAE7E5051756880576B7758A428FC831316BAA6FEB602C18B1C09766B316239E6826CA2226695282A8628F70D14792C803C4DA8BF412E65AF5D9CA619BB1F51B063435E8737D8BB20573B1AEF286319873741ADA39387BC4DF8B32EFF3ACDCBAB380B7BC6608191E93316F015F969E7075E49F7B9E68EC40082C7D2F9250ED725E59739DB8712D2DB880001E5E22B53806B1CC60103965E9135BAC366DADA655897D8F2CC47DB0485690EA39ACF7E32F3F3C2FB57FF0187497F5AA2670000, '5.0.0.net45')


INSERT INTO [dbo].[MediaTypes] ([Key], [DisplayName]) VALUES ('youtube', 'YouTube')
INSERT INTO [dbo].[MediaTypes] ([Key], [DisplayName]) VALUES ('googledrive', 'Google Drive')