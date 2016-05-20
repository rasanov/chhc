CREATE TABLE [dbo].UserProfile
	(UserId int IDENTITY(1,1) NOT NULL,
	UserName nvarchar(56) NOT NULL,
	-- Password CHAR(40) NOT NULL,
	PRIMARY KEY CLUSTERED
	(UserId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	UNIQUE NONCLUSTERED
	(UserName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

CREATE TABLE [dbo].webpages_Membership
	(UserId int NOT NULL,
	CreateDate datetime NULL,
	ConfirmationToken nvarchar(128) NULL,
	IsConfirmed bit NULL,
	LastPasswordFailureDate datetime NULL,
	PasswordFailuresSinceLastSuccess int NOT NULL,
	Password nvarchar(128) NOT NULL,
	PasswordChangedDate datetime NULL,
	PasswordSalt nvarchar(128) NOT NULL,
	PasswordVerificationToken nvarchar(128) NULL,
	PasswordVerificationTokenExpirationDate datetime NULL,
	PRIMARY KEY CLUSTERED
	(UserId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
ALTER TABLE [dbo].webpages_Membership ADD DEFAULT ((0)) FOR IsConfirmed
GO
ALTER TABLE [dbo].webpages_Membership ADD DEFAULT ((0)) FOR PasswordFailuresSinceLastSuccess
GO

CREATE TABLE [dbo].webpages_OAuthMembership
	(Provider nvarchar(30) NOT NULL,
	ProviderUserId nvarchar(100) NOT NULL,
	UserId int NOT NULL,
	PRIMARY KEY CLUSTERED
		(Provider ASC,
		ProviderUserId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

CREATE TABLE [dbo].webpages_Roles
	(RoleId int IDENTITY(1,1) NOT NULL,
	RoleName nvarchar(256) NOT NULL,
	PRIMARY KEY CLUSTERED
		(RoleId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	UNIQUE NONCLUSTERED
	(RoleName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

CREATE TABLE [dbo].webpages_UsersInRoles
	(UserId int NOT NULL,
	RoleId int NOT NULL,
	PRIMARY KEY CLUSTERED
		(UserId ASC,
		RoleId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
ALTER TABLE [dbo].webpages_UsersInRoles WITH CHECK ADD CONSTRAINT fk_RoleId FOREIGN KEY (RoleId) REFERENCES [dbo].webpages_Roles (RoleId)
GO
ALTER TABLE [dbo].webpages_UsersInRoles CHECK CONSTRAINT fk_RoleId
GO
ALTER TABLE [dbo].webpages_UsersInRoles WITH CHECK ADD CONSTRAINT fk_UserId FOREIGN KEY (UserId) REFERENCES [dbo].UserProfile (UserId)
GO
ALTER TABLE [dbo].webpages_UsersInRoles CHECK CONSTRAINT fk_UserId
GO


IF (NOT EXISTS(SELECT 1 FROM dbo.webpages_Roles WHERE RoleName = 'ControlPanelAdmin'))
	INSERT INTO dbo.webpages_Roles (RoleName) VALUES ('ControlPanelAdmin')

IF (NOT EXISTS(SELECT 1 FROM dbo.webpages_Roles WHERE RoleName = 'CHHCAdmin'))
	INSERT INTO dbo.webpages_Roles (RoleName) VALUES ('CHHCAdmin')

IF (NOT EXISTS(SELECT 1 FROM dbo.webpages_Roles WHERE RoleName = 'Training_Edit'))
	INSERT INTO dbo.webpages_Roles (RoleName) VALUES ('Training_Edit')
	
IF (NOT EXISTS(SELECT 1 FROM dbo.webpages_Roles WHERE RoleName = 'Training_View'))
	INSERT INTO dbo.webpages_Roles (RoleName) VALUES ('Training_View')

IF (NOT EXISTS(SELECT [UserId] FROM [UserProfile] WHERE (UPPER([UserName]) = N'ADMINISTRATOR')))
	INSERT INTO [UserProfile] ([UserName]) VALUES (N'administrator')
	
DECLARE @UserId INT
SELECT @UserId = [UserId] FROM [UserProfile] WHERE (UPPER([UserName]) = N'ADMINISTRATOR')
IF (NOT EXISTS(SELECT 1 FROM [webpages_Membership] WHERE UserId = @UserId))
	INSERT INTO [webpages_Membership] (UserId, [Password], PasswordSalt, IsConfirmed, ConfirmationToken, CreateDate, PasswordChangedDate, PasswordFailuresSinceLastSuccess)
	VALUES (@UserId, N'ABP9S0P7q0LsaRKtgxUv8hSfe4QhiLBfSg9HjfbgMYEujVuhRyutG96j1WN3hVJ9CQ==', N'', 1, NULL, '2014-03-25', '2014-03-25', 0)
	
DECLARE @RoleId INT
SELECT @RoleId = RoleId FROM dbo.webpages_Roles WHERE RoleName = 'ControlPanelAdmin'
IF (NOT EXISTS(SELECT 1 FROM [UserProfile] u, webpages_UsersInRoles ur, webpages_Roles r Where (u.[UserName] = N'administrator' and r.RoleName = N'ControlPanelAdmin' and ur.RoleId = r.RoleId and ur.UserId = u.[UserId])))
	INSERT INTO webpages_UsersInRoles (UserId, RoleId) VALUES (@UserId, @RoleId); 


IF (NOT EXISTS(SELECT [UserId] FROM [UserProfile] WHERE (UPPER([UserName]) = N'YGRUPIN')))
	INSERT INTO [UserProfile] ([UserName]) VALUES (N'ygrupin')
	
SELECT @UserId = [UserId] FROM [UserProfile] WHERE (UPPER([UserName]) = N'YGRUPIN')
IF (NOT EXISTS(SELECT 1 FROM [webpages_Membership] WHERE UserId = @UserId))
	INSERT INTO [webpages_Membership] (UserId, [Password], PasswordSalt, IsConfirmed, ConfirmationToken, CreateDate, PasswordChangedDate, PasswordFailuresSinceLastSuccess)
	VALUES (@UserId, N'AKwwvcMYDCCgJNC8PLlhADRLutInRL1eyXS0zmzzxJyk2tvMyFSjozt3E9QlYMRYIA==', N'', 1, NULL, '2014-03-25', '2014-03-25', 0)
	
SELECT @RoleId = RoleId FROM dbo.webpages_Roles WHERE RoleName = 'CHHCAdmin'
IF (NOT EXISTS(SELECT 1 FROM [UserProfile] u, webpages_UsersInRoles ur, webpages_Roles r Where (u.[UserName] = N'ygrupin' and r.RoleName = N'CHHCAdmin' and ur.RoleId = r.RoleId and ur.UserId = u.[UserId])))
	INSERT INTO webpages_UsersInRoles (UserId, RoleId) VALUES (@UserId, @RoleId); 


