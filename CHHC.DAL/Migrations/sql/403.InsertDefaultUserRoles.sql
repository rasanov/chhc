-- training
INSERT INTO [dbo].[webpages_UsersInRoles] ([UserId],[RoleId])
SELECT UserId, 4 FROM UserProfile up WHERE UserName NOT IN ('administrator','ygrupin') AND NOT EXISTS (SELECT 1 FROM [dbo].[webpages_UsersInRoles] ur WHERE ur.UserId = up.UserId and ur.RoleId = 4)

-- case conference
INSERT INTO [dbo].[webpages_UsersInRoles] ([UserId],[RoleId])
SELECT UserId, 7 FROM UserProfile up WHERE UserName NOT IN ('administrator','ygrupin') AND NOT EXISTS (SELECT 1 FROM [dbo].[webpages_UsersInRoles] ur WHERE ur.UserId = up.UserId and ur.RoleId = 7)

-- document
INSERT INTO [dbo].[webpages_UsersInRoles] ([UserId],[RoleId])
SELECT UserId, 9 FROM UserProfile up WHERE UserName NOT IN ('administrator','ygrupin') AND NOT EXISTS (SELECT 1 FROM [dbo].[webpages_UsersInRoles] ur WHERE ur.UserId = up.UserId and ur.RoleId = 9)