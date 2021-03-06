--INSERT INTO securitas.Roles (DisplayName) VALUES
--('Miembro'),('Admin')



-- Para Test
GO
SET IDENTITY_INSERT [securitas].[Roles] ON 

GO
INSERT [securitas].[Roles] ([Id], [DisplayName]) VALUES (1, N'Miembro')
GO
INSERT [securitas].[Roles] ([Id], [DisplayName]) VALUES (2, N'Admin')
GO
SET IDENTITY_INSERT [securitas].[Roles] OFF
GO
SET IDENTITY_INSERT [securitas].[Users] ON 

GO
INSERT [securitas].[Users] ([Id], [UserName], [DisplayName], [PasswordHash], [PasswordSalt], [IsActive], [CreationDate], [LastModificationDate], [RoleId]) VALUES (3, N'ocarranza@grupolagsa.com', N'Mimoso exCalbon', 0x505D406736F41D5584FC2D0F4B6E84C6D8E3A2D129FB611D1807E28A23807853124118E7FBC41C1A52363A625368D900D4996C3A0C3EA6129C012F5371ECFACF, 0x2196C3D43DD5C73E408FB063FDFC6B9D3DE35BD4D69B09CAE9C7448A8DC46670D61294934F3DBA1C6495823B23DB320F2A7A277BC414C7368B9164E09A04F1A631F04A34995342C23A807ABB43108F8D39A93692111845714053AB9E5989B8A86B20E242D74C6557054B58B97509E2C2E44EF87D9C4D3DD6CF47D5829E357A6C, 1, CAST(N'2020-09-26 17:47:03.5264395' AS DateTime2), CAST(N'2020-09-26 17:47:03.5265206' AS DateTime2), 1)
GO
INSERT [securitas].[Users] ([Id], [UserName], [DisplayName], [PasswordHash], [PasswordSalt], [IsActive], [CreationDate], [LastModificationDate], [RoleId]) VALUES (4, N'carlossotoocio@gmail.com', N'Charly ah� les voy', 0x8BA385F6FF5085A3C2CD97C91C1A08C8A057C8A488B6F5EC2F4476AC26A7D4CD93BA15C04326DCD072E4495834EF9001C6A9B9AB0B7BEB3FE4B645F3D8486B6F, 0x36324E6E3A015838FD217742CFE722682BA059B334CCA112516AC370BD17BA23408E8AA653438DADDC7BE78C2DC9B7CE45FCD3618B13EC4749FDF3A8EB4979ABB0ED6A546B91845C82A01003E039073CC9B692A5BAF9F32A88DC052C9CB1B7D17D661354D071A69AA08F8D5A04ADE2DB24AEE1FA11CDF243673714383E8A4D10, 1, CAST(N'2020-09-26 19:04:44.4421271' AS DateTime2), CAST(N'2020-09-26 19:04:44.4422306' AS DateTime2), 2)
GO
INSERT [securitas].[Users] ([Id], [UserName], [DisplayName], [PasswordHash], [PasswordSalt], [IsActive], [CreationDate], [LastModificationDate], [RoleId]) VALUES (5, N'gcabrera@grupolagsa.com', N'Gera Cabrera', 0x6923FC629A27ECDCEF8323F736C78FE779EB6DEDFD3F6C278C0697378ABBFBEC59A608351E146C76F5BA516C689F79D1BAE493E2BFFE66BC8F92800D8C07B081, 0xCF3853E2A6819B4D29FBFF9FDC09B1863496EE3BFFE953F24166CDC17AA105D3BB84AF35525D18E2781AB90B1F930154E080C530CEBA1B8823C2A4111AE81FC2240C141F0B2B45390C8C5D76B509747C27DD9C7F048FA69CD2CE2FDA96E1FC48C295E0178CA7D051C9994AC675C6885636044228EC22E2FEC40E11AAFE564C2F, 1, CAST(N'2020-09-26 19:06:39.4523851' AS DateTime2), CAST(N'2020-09-26 19:06:39.4524124' AS DateTime2), 1)
GO
SET IDENTITY_INSERT [securitas].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Members] ON 

GO
INSERT [dbo].[Members] ([Id], [UserId], [MemberId], [PhotoUrl], [MembershipExpiration], [MembershipTypeActiveId], [BlockingReason], [IsVerified]) VALUES (1, 3, N'', NULL, CAST(N'2020-09-25 17:47:24.8955976' AS DateTime2), NULL, NULL, 0)
GO
INSERT [dbo].[Members] ([Id], [UserId], [MemberId], [PhotoUrl], [MembershipExpiration], [MembershipTypeActiveId], [BlockingReason], [IsVerified]) VALUES (2, 4, N'', NULL, CAST(N'2020-09-25 19:04:45.3983185' AS DateTime2), NULL, NULL, 1)
GO
INSERT [dbo].[Members] ([Id], [UserId], [MemberId], [PhotoUrl], [MembershipExpiration], [MembershipTypeActiveId], [BlockingReason], [IsVerified]) VALUES (3, 5, N'', NULL, CAST(N'2020-10-25 19:06:39.4939421' AS DateTime2), NULL, NULL, 0)
GO

UPDATE GeneralSettings SET ScheduledWeek = '[{day:''monday'',rangeDates:[{starHour:''06:00'',endHour:''07:50''},{starHour:''08:00'',endHour:''09:50''},{starHour:''10:00'',endHour:''11:50''},{starHour:''12:00'',endHour:''13:50''},{starHour:''14:00'',endHour:''15:50''},{starHour:''16:00'',endHour:''17:50''},{starHour:''18:00'',endHour:''19:50''},{starHour:''20:00'',endHour:''21:50''},{starHour:''22:00'',endHour:''23:50''}]},{day:''tuesday'',rangeDates:[{starHour:''06:00'',endHour:''07:50''},{starHour:''08:00'',endHour:''09:50''},{starHour:''10:00'',endHour:''11:50''},{starHour:''12:00'',endHour:''13:50''},{starHour:''14:00'',endHour:''15:50''},{starHour:''16:00'',endHour:''17:50''},{starHour:''18:00'',endHour:''19:50''},{starHour:''20:00'',endHour:''21:50''}]},{day:''wednesday'',rangeDates:[{starHour:''08:00'',endHour:''09:50''},{starHour:''10:00'',endHour:''11:50''},{starHour:''12:00'',endHour:''13:50''},{starHour:''14:00'',endHour:''15:50''},{starHour:''16:00'',endHour:''17:50''},{starHour:''18:00'',endHour:''19:50''}]},{day:''thursday'',rangeDates:[{starHour:''06:00'',endHour:''07:50''},{starHour:''08:00'',endHour:''09:50''},{starHour:''10:00'',endHour:''11:50''},{starHour:''12:00'',endHour:''13:50''},{starHour:''14:00'',endHour:''15:50''},{starHour:''16:00'',endHour:''17:50''},{starHour:''18:00'',endHour:''19:50''},{starHour:''20:00'',endHour:''21:50''}]},{day:''friday'',rangeDates:[{starHour:''07:00'',endHour:''08:50''},{starHour:''09:00'',endHour:''10:50''},{starHour:''11:00'',endHour:''12:50''},{starHour:''13:00'',endHour:''14:50''},{starHour:''15:00'',endHour:''16:50''},{starHour:''17:00'',endHour:''18:50''},{starHour:''19:00'',endHour:''20:50''},{starHour:''21:00'',endHour:''22:50''}]},{day:''saturday'',rangeDates:[{starHour:''04:00'',endHour:''05:50''},{starHour:''06:00'',endHour:''07:50''},{starHour:''08:00'',endHour:''09:50''},{starHour:''10:00'',endHour:''11:50''},{starHour:''12:00'',endHour:''13:50''},{starHour:''14:00'',endHour:''15:50''}, {starHour:''16:00'',endHour:''17:50''},{starHour:''18:00'',endHour:''19:50''},{starHour:''20:00'',endHour:''21:50''},{starHour:''22:00'',endHour:''23:50''},{starHour:''23:55'',endHour:''02:00''}]}]'
UPDATE GeneralSettings SET ScheduleChangeHours = 6
GO
SET IDENTITY_INSERT [dbo].[Members] OFF
GO
SET IDENTITY_INSERT [dbo].[AuthorizedCapacities] ON 

GO
INSERT [dbo].[AuthorizedCapacities] ([Id], [StartDate], [EndDate], [Capacity], [PercentageCapacity]) VALUES (1, CAST(N'2020-09-01 00:00:00.0000000' AS DateTime2), CAST(N'2020-09-25 00:00:00.0000000' AS DateTime2), 25, CAST(0.25 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[AuthorizedCapacities] OFF
GO
SET IDENTITY_INSERT [dbo].[GeneralSettings] ON 

GO
INSERT [dbo].[GeneralSettings] ([Id], [TotalCapacity], [ScheduleChangeHours], [LoginAttempts]) VALUES (1, 100, 12, 3)
GO
SET IDENTITY_INSERT [dbo].[GeneralSettings] OFF
GO




