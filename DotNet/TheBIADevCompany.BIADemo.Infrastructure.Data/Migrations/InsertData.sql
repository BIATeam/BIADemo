DELETE FROM [dbo].[PlanePlaneType]
GO
DELETE FROM [dbo].[PlaneAirport]
GO
DELETE FROM [dbo].[EnginePart]
GO
DELETE FROM [dbo].[Engines]
GO
DELETE FROM [dbo].[Planes]
GO

DELETE FROM [dbo].[PlanesTypes]
GO
DELETE FROM [dbo].[MaintenanceTeamCountry]
GO
DELETE FROM [dbo].[MaintenanceTeamAirport]
GO
DELETE FROM [dbo].[Sites]
GO
DELETE FROM [dbo].[MaintenanceTeams]
GO
DELETE FROM [dbo].[Airports]
GO
DELETE FROM [dbo].[AircraftMaintenanceCompanies]
GO
DELETE FROM [dbo].[Teams]
GO

SET IDENTITY_INSERT [dbo].[Teams] ON 
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (1, 2, N'Air France')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (2, 2, N'Lufthansa')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (3, 2, N'British Airways')
GO
--INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (4, 2, N'KLM')
--GO
--INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (5, 2, N'Alitalia')
--GO
--INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (6, 2, N'Ryanair')
--GO
--INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (7, 2, N'EasyJet')
--GO
--INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (8, 2, N'American Airlines')
--GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (9, 3, N'Maintenance Company 1')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (10, 3, N'Maintenance Company 2')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (11, 3, N'Maintenance Company 3')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (12, 4, N'Teams Maintenance 1')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (13, 4, N'Teams Maintenance 2')
GO
INSERT [dbo].[Teams] ([Id], [TeamTypeId], [Title]) VALUES (14, 4, N'Teams Maintenance 3')
GO
SET IDENTITY_INSERT [dbo].[Teams] OFF
GO
INSERT [dbo].[AircraftMaintenanceCompanies] ([Id]) VALUES (9)
GO
INSERT [dbo].[AircraftMaintenanceCompanies] ([Id]) VALUES (10)
GO
INSERT [dbo].[AircraftMaintenanceCompanies] ([Id]) VALUES (11)
GO
SET IDENTITY_INSERT [dbo].[Airports] ON 
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (1, N'CDG', N'Paris-CDG')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (2, N'LHR', N'London')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (3, N'AMS', N'Amsterdam')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (4, N'MAD', N'Madrid')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (5, N'FRA', N'Frankfurt')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (6, N'BCN', N'Barcelona')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (7, N'FCO', N'Rome')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (8, N'ORY', N'Paris-Orly')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (9, N'MUC', N'Munich')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (10, N'ZRH', N'Zurich')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (11, N'BRU', N'Brussels')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (12, N'DUB', N'Dublin')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (13, N'LIS', N'Lisbon')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (14, N'VIE', N'Vienna')
GO
INSERT [dbo].[Airports] ([Id], [Name], [City]) VALUES (15, N'OSL', N'Oslo')
GO
SET IDENTITY_INSERT [dbo].[Airports] OFF
GO
INSERT [dbo].[MaintenanceTeams] ([Id], [AircraftMaintenanceCompanyId], [IsActive], [Code], [ApprovedDate], [AverageOperationCost], [AverageOperationDuration], [AverageTravelDuration], [CurrentAirportId], [CurrentCountryId], [FirstOperation], [IncidentCount], [IsApproved], [LastOperation], [MaxOperationDuration], [MaxTravelDuration], [NextOperation], [OperationCount], [TotalOperationCost], [TotalOperationDuration], [TotalTravelDuration]) VALUES (12, 9, 0, N'TM2', CAST(N'2024-12-18T00:00:00.0000000' AS DateTime2), 685.0000, 58, 15, 1, 1, CAST(N'2024-12-01T10:56:47.0000000' AS DateTime2), 4, 1, CAST(N'2024-12-10T10:59:50.0000000' AS DateTime2), CAST(N'10:00:52' AS Time), CAST(N'11:59:55' AS Time), CAST(N'2024-12-10T00:00:00.0000000' AS DateTime2), 1, 6487.0000, 2, 3)
GO
INSERT [dbo].[MaintenanceTeams] ([Id], [AircraftMaintenanceCompanyId], [IsActive], [Code], [ApprovedDate], [AverageOperationCost], [AverageOperationDuration], [AverageTravelDuration], [CurrentAirportId], [CurrentCountryId], [FirstOperation], [IncidentCount], [IsApproved], [LastOperation], [MaxOperationDuration], [MaxTravelDuration], [NextOperation], [OperationCount], [TotalOperationCost], [TotalOperationDuration], [TotalTravelDuration]) VALUES (13, 9, 0, N'TM2', NULL, 7854.0000, 954, 65, 2, 1, CAST(N'2024-12-16T10:58:12.0000000' AS DateTime2), NULL, 1, NULL, CAST(N'10:59:17' AS Time), NULL, CAST(N'2024-12-09T00:00:00.0000000' AS DateTime2), 6, 9654.0000, 58, 63)
GO
INSERT [dbo].[MaintenanceTeams] ([Id], [AircraftMaintenanceCompanyId], [IsActive], [Code], [ApprovedDate], [AverageOperationCost], [AverageOperationDuration], [AverageTravelDuration], [CurrentAirportId], [CurrentCountryId], [FirstOperation], [IncidentCount], [IsApproved], [LastOperation], [MaxOperationDuration], [MaxTravelDuration], [NextOperation], [OperationCount], [TotalOperationCost], [TotalOperationDuration], [TotalTravelDuration]) VALUES (14, 9, 0, N'TM3', CAST(N'2024-12-09T00:00:00.0000000' AS DateTime2), 85.0000, 956, 85, 4, 4, CAST(N'2024-12-09T10:58:59.0000000' AS DateTime2), 55, 1, CAST(N'2024-12-10T10:59:05.0000000' AS DateTime2), CAST(N'10:01:12' AS Time), CAST(N'11:59:09' AS Time), CAST(N'2024-12-27T00:00:00.0000000' AS DateTime2), 54, 585.0000, 45, 547)
GO
INSERT [dbo].[Sites] ([Id]) VALUES (1)
GO
INSERT [dbo].[Sites] ([Id]) VALUES (2)
GO
INSERT [dbo].[Sites] ([Id]) VALUES (3)
GO
--INSERT [dbo].[Sites] ([Id]) VALUES (4)
--GO
--INSERT [dbo].[Sites] ([Id]) VALUES (5)
--GO
--INSERT [dbo].[Sites] ([Id]) VALUES (6)
--GO
--INSERT [dbo].[Sites] ([Id]) VALUES (7)
--GO
--INSERT [dbo].[Sites] ([Id]) VALUES (8)
--GO
INSERT [dbo].[MaintenanceTeamAirport] ([MaintenanceTeamId], [AirportId]) VALUES (12, 3)
GO
INSERT [dbo].[MaintenanceTeamAirport] ([MaintenanceTeamId], [AirportId]) VALUES (13, 4)
GO
INSERT [dbo].[MaintenanceTeamAirport] ([MaintenanceTeamId], [AirportId]) VALUES (14, 5)
GO
INSERT [dbo].[MaintenanceTeamCountry] ([MaintenanceTeamId], [CountryId]) VALUES (12, 1)
GO
INSERT [dbo].[MaintenanceTeamCountry] ([MaintenanceTeamId], [CountryId]) VALUES (13, 2)
GO
INSERT [dbo].[MaintenanceTeamCountry] ([MaintenanceTeamId], [CountryId]) VALUES (14, 4)
GO
SET IDENTITY_INSERT [dbo].[PlanesTypes] ON 
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (1, N'Boeing 737', CAST(N'2024-12-04T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (2, N'Airbus A320', CAST(N'2024-12-19T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (3, N'Airbus A350', CAST(N'2024-01-08T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (4, N'Boeing 777', CAST(N'2024-04-08T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (5, N'Airbus A330', CAST(N'2024-08-13T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[PlanesTypes] ([Id], [Title], [CertificationDate]) VALUES (6, N'Boeing 767', CAST(N'2024-12-25T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[PlanesTypes] OFF
GO


Declare @Id int
Set @Id = 1

Declare @MaxId int
Set @MaxId = 3000

Declare @nbAirport int
Set @nbAirport = (select count(*) from Airports)

Declare @nbPlaneType int
Set @nbPlaneType = (select count(*) from PlanesTypes)
 
While @Id <= @MaxId
Begin 
  INSERT INTO [dbo].[Planes]
  ([Msn], [IsActive], [LastFlightDate], [Capacity], [PlaneTypeId], [SiteId], [DeliveryDate],
  [SyncTime], [EstimatedPrice], [FuelLevel], [Probability], [CurrentAirportId], [FirstFlightDate], 
  [FuelCapacity], [IsMaintenance], [Manufacturer], [MotorsCount], [NextMaintenanceDate], [OriginalPrice], [SyncFlightDataTime], [TotalFlightHours])
  VALUES
  (
  'MSN' + CAST((@Id + @MaxId * 10) as nvarchar(10)),
  ABS(CHECKSUM(NEWID())) % 2,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01') END,
  ABS(CHECKSUM(NEWID()) % 200) + 1,
  (SELECT TOP 1 Id FROM [dbo].[PlanesTypes] ORDER BY NEWID()),
  (SELECT TOP 1 Id FROM [dbo].[Sites] ORDER BY NEWID()),
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01') END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(DATEADD(SECOND, ABS(CHECKSUM(NEWID()) % 86400), '00:00:00') AS TIME) END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(ABS(CHECKSUM(NEWID()) % 1000000) / 100.0 AS MONEY) END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(ABS(CHECKSUM(NEWID()) % 10000) / 100.0 AS REAL) END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE ABS(CHECKSUM(NEWID()) % 100) / 100.0 END,
  (SELECT TOP 1 Id FROM [dbo].[Airports] ORDER BY NEWID()),
  DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01'),
  ABS(CHECKSUM(NEWID()) % 10),
  ABS(CHECKSUM(NEWID())) % 2, --CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE ABS(CHECKSUM(NEWID())) % 2 END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE 'Manufacturer ' + CAST(ABS(CHECKSUM(NEWID())) AS NVARCHAR(50)) END,
  CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE ABS(CHECKSUM(NEWID()) % 10) END,
  DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01'),
  ABS(CHECKSUM(NEWID())) % 1000000 / 100.0,
  CAST(DATEADD(SECOND, ABS(CHECKSUM(NEWID()) % 86400), '00:00:00') AS TIME),
  ABS(CHECKSUM(NEWID()) % 10000))
  Set @Id = @Id + 1
End

Declare @nbPlane int
Set @nbPlane = (select count(*) from  [dbo].[Planes])

insert into [PlaneAirport] ([PlaneId], [AirportId]) select p.Id, (p.Id % @nbAirport) + 1 from Planes p
insert into [PlaneAirport] ([PlaneId], [AirportId]) select p.Id, (SELECT TOP 1 (AirportId % @nbAirport) + 1 FROM [dbo].[PlaneAirport] where PlaneId = p.Id) from Planes p

insert into [PlanePlaneType] ([PlaneId], [PlaneTypeId]) select p.Id, (p.Id % @nbPlaneType) + 1 from Planes p
insert into [PlanePlaneType] ([PlaneId], [PlaneTypeId]) select p.Id, (SELECT TOP 1 ([PlaneTypeId] % @nbPlaneType) + 1 FROM [dbo].[PlanePlaneType] where PlaneId = p.Id) from Planes p

-- Insérer des données dans la table Engines
Declare @EngineId int
Set @EngineId = 1

While @EngineId <= (@nbPlane * 5)
Begin 
  INSERT INTO [dbo].[Engines]
  ([Reference], [LastMaintenanceDate], [SyncTime], [Power], [PlaneId], [AverageFlightHours], [AverageFuelConsumption], [DeliveryDate], [EstimatedPrice], [ExchangeDate], [FlightHours], [FuelConsumption], [IgnitionTime], [IsHybrid], [Manufacturer], [NextMaintenanceDate], [NoiseLevel], [OriginalPrice], [PrincipalPartId], [IsToBeMaintained])
  VALUES
  (
    'REF' + CAST(@EngineId as nvarchar(10)),
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01') END,
    CAST(DATEADD(SECOND, ABS(CHECKSUM(NEWID()) % 86400), '00:00:00') AS TIME),
    ABS(CHECKSUM(NEWID()) % 10000),
    (SELECT TOP 1 Id FROM [dbo].[Planes] ORDER BY NEWID()),
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE ABS(CHECKSUM(NEWID())) % 10000 / 100.0 END,
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(ABS(CHECKSUM(NEWID())) % 10000 / 100.0 AS REAL) END,
    DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01'),
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(ABS(CHECKSUM(NEWID())) % 1000000 / 100.0 AS DECIMAL(18, 2)) END,
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01') END,
    ABS(CHECKSUM(NEWID())) % 100000 / 100.0,
    CAST(ABS(CHECKSUM(NEWID())) % 100000 / 100.0 AS REAL),
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE CAST(DATEADD(SECOND, ABS(CHECKSUM(NEWID()) % 86400), '00:00:00') AS TIME) END,
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE ABS(CHECKSUM(NEWID())) % 2 END,
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE 'Manufacturer ' + CAST(ABS(CHECKSUM(NEWID())) AS NVARCHAR(50)) END,
    DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2023-01-01'),
    ABS(CHECKSUM(NEWID())) % 100,
    ABS(CHECKSUM(NEWID())) % 1000000 / 100.0,
    CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN NULL ELSE (SELECT TOP 1 Id FROM [dbo].[Parts] ORDER BY NEWID()) END,
    ABS(CHECKSUM(NEWID())) % 2
  )
  Set @EngineId = @EngineId + 1
End

-- Insérer des relations dans la table EnginePart
--Declare @EnginePartId int
--Set @EnginePartId = 1

--While @EnginePartId <= 200
--Begin 
--  INSERT INTO [dbo].[EnginePart] ([EngineId], [PartId])
--  VALUES (
--    (SELECT TOP 1 Id FROM [dbo].[Engines] ORDER BY NEWID()),
--    (SELECT TOP 1 Id FROM [dbo].[Parts] ORDER BY NEWID())
--  )
--  Set @EnginePartId = @EnginePartId + 1
--End

-- Insertion de relations uniques dans la table EnginePart
Declare @EnginePartId int
Set @EnginePartId = 1

While @EnginePartId <= (@nbPlane * 5)
Begin 
  DECLARE @Engine2Id int
  DECLARE @PartId int

  SET @Engine2Id = (SELECT TOP 1 Id FROM [dbo].[Engines] ORDER BY NEWID())
  SET @PartId = (SELECT TOP 1 Id FROM [dbo].[Parts] ORDER BY NEWID())

  IF NOT EXISTS (SELECT 1 FROM dbo.EnginePart WHERE EngineId = @Engine2Id AND PartId = @PartId)
  BEGIN
    INSERT INTO [dbo].[EnginePart] ([EngineId], [PartId])
    VALUES (
      @Engine2Id,
      @PartId
    )
  END
  SET @EnginePartId = @EnginePartId + 1
End

GO
