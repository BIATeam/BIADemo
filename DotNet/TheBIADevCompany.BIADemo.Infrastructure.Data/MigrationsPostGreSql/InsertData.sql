-- Clean existing data (order respects FK dependencies like original)
DELETE FROM "PlanePlaneType";
DELETE FROM "PlaneAirport";
DELETE FROM "EnginePart";
DELETE FROM "Engines";
DELETE FROM "Planes";
DELETE FROM "PlanesTypes";
DELETE FROM "MaintenanceTeamCountry";
DELETE FROM "MaintenanceTeamAirport";
DELETE FROM "Sites";
DELETE FROM "MaintenanceTeams";
DELETE FROM "Airports";
DELETE FROM "AircraftMaintenanceCompanies";
DELETE FROM "Teams";

-- Insert Teams
INSERT INTO "Teams" ("Id","TeamTypeId","Title") VALUES
 (1,2,'Air France'),
 (2,2,'Lufthansa'),
 (3,2,'British Airways'),
 (9,3,'Maintenance Company 1'),
 (10,3,'Maintenance Company 2'),
 (11,3,'Maintenance Company 3'),
 (12,4,'Teams Maintenance 1'),
 (13,4,'Teams Maintenance 2'),
 (14,4,'Teams Maintenance 3');

-- AircraftMaintenanceCompanies
INSERT INTO "AircraftMaintenanceCompanies" ("Id") VALUES (9),(10),(11);

-- Airports
INSERT INTO "Airports" ("Id","Name","City") VALUES
 (1,'CDG','Paris-CDG'),
 (2,'LHR','London'),
 (3,'AMS','Amsterdam'),
 (4,'MAD','Madrid'),
 (5,'FRA','Frankfurt'),
 (6,'BCN','Barcelona'),
 (7,'FCO','Rome'),
 (8,'ORY','Paris-Orly'),
 (9,'MUC','Munich'),
 (10,'ZRH','Zurich'),
 (11,'BRU','Brussels'),
 (12,'DUB','Dublin'),
 (13,'LIS','Lisbon'),
 (14,'VIE','Vienna'),
 (15,'OSL','Oslo');

-- MaintenanceTeams
INSERT INTO "MaintenanceTeams"
("Id","AircraftMaintenanceCompanyId","IsActive","Code","ApprovedDate","AverageOperationCost",
 "AverageOperationDuration","AverageTravelDuration","CurrentAirportId","CurrentCountryId",
 "FirstOperation","IncidentCount","IsApproved","LastOperation","MaxOperationDuration",
 "MaxTravelDuration","NextOperation","OperationCount","TotalOperationCost",
 "TotalOperationDuration","TotalTravelDuration","IsFixed","IsArchived")
VALUES
 (12,9,FALSE,'TM2','2024-12-18 00:00:00'::timestamp,685.0000,58,15,1,1,'2024-12-01 10:56:47'::timestamp,4,TRUE,'2024-12-10 10:59:50'::timestamp,'10:00:52'::time,'11:59:55'::time,'2024-12-10 00:00:00'::timestamp,1,6487.0000,2,3,FALSE,FALSE),
 (13,9,FALSE,'TM2',NULL,7854.0000,954,65,2,1,'2024-12-16 10:58:12'::timestamp,NULL,TRUE,NULL,'10:59:17'::time,NULL,'2024-12-09 00:00:00'::timestamp,6,9654.0000,58,63,FALSE,FALSE),
 (14,9,FALSE,'TM3','2024-12-09 00:00:00'::timestamp,85.0000,956,85,4,4,'2024-12-09 10:58:59'::timestamp,55,TRUE,'2024-12-10 10:59:05'::timestamp,'10:01:12'::time,'11:59:09'::time,'2024-12-27 00:00:00'::timestamp,54,585.0000,45,547,FALSE,FALSE);

-- Sites
INSERT INTO "Sites" ("Id") VALUES (1),(2),(3);

-- MaintenanceTeamAirport
INSERT INTO "MaintenanceTeamAirport" ("MaintenanceTeamId","AirportId") VALUES
 (12,3),(13,4),(14,5);

-- MaintenanceTeamCountry
INSERT INTO "MaintenanceTeamCountry" ("MaintenanceTeamId","CountryId") VALUES
 (12,1),(13,2),(14,4);

-- PlanesTypes
INSERT INTO "PlanesTypes" ("Id","Title","CertificationDate") VALUES
 (1,'Boeing 737','2024-12-04 00:00:00'::timestamp),
 (2,'Airbus A320','2024-12-19 00:00:00'::timestamp),
 (3,'Airbus A350','2024-01-08 00:00:00'::timestamp),
 (4,'Boeing 777','2024-04-08 00:00:00'::timestamp),
 (5,'Airbus A330','2024-08-13 00:00:00'::timestamp),
 (6,'Boeing 767','2024-12-25 00:00:00'::timestamp);

-- Main data generation (Planes, relations, Engines, EnginePart)
DO $$
DECLARE
  max_id            int := 3000;
  id                int := 1;
  nb_airport        int;
  nb_planetype      int;
  nb_plane          int;
  engine_id         int;
  enginepart_id     int;
  v_engine2_id      int;
  v_part_id         int;
BEGIN
  SELECT count(*) INTO nb_airport   FROM "Airports";
  SELECT count(*) INTO nb_planetype FROM "PlanesTypes";

  -- Generate Planes
  WHILE id <= max_id LOOP
    INSERT INTO "Planes"
    ("Msn","IsActive","LastFlightDate","Capacity","PlaneTypeId","SiteId","DeliveryDate",
     "SyncTime","EstimatedPrice","FuelLevel","Probability","CurrentAirportId","FirstFlightDate",
     "FuelCapacity","IsMaintenance","Manufacturer","MotorsCount","NextMaintenanceDate","OriginalPrice",
     "SyncFlightDataTime","TotalFlightHours","IsFixed","IsArchived")
    VALUES (
      'MSN' || (id + max_id * 10),
      CASE WHEN floor(random()*2)::int = 0 THEN FALSE ELSE TRUE END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE ((DATE '2023-01-01' + (floor(random()*3650)) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz END,
      floor(random()*200)+1,
      (SELECT "Id" FROM "PlanesTypes" ORDER BY random() LIMIT 1),
      (SELECT "Id" FROM "Sites" ORDER BY random() LIMIT 1),
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE ((DATE '2023-01-01' + (floor(random()*3650)) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (TIME '00:00:00' + (floor(random()*86400) * INTERVAL '1 second'))::time END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*1000000)::numeric / 100.0) END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*10000)::numeric / 100.0) END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*100)::numeric / 100.0) END,
      (SELECT "Id" FROM "Airports" ORDER BY random() LIMIT 1),
      ((DATE '2023-01-01' + (floor(random()*3650)) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz,
      floor(random()*10),
      CASE WHEN floor(random()*2)::int = 0 THEN FALSE ELSE TRUE END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE 'Manufacturer ' || floor(random()*1000000000)::text END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE floor(random()*10) END,
      ((DATE '2023-01-01' + (floor(random()*3650)) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz,
      (floor(random()*1000000)::numeric / 100.0),
      (TIME '00:00:00' + (floor(random()*86400) * INTERVAL '1 second'))::time,
      floor(random()*10000),
      FALSE,
      FALSE
    );
    id := id + 1;
  END LOOP;

  SELECT count(*) INTO nb_plane FROM "Planes";

  -- PlaneAirport relations
  INSERT INTO "PlaneAirport" ("PlaneId","AirportId")
  SELECT p."Id", (p."Id" % nb_airport) + 1 FROM "Planes" p;

  INSERT INTO "PlaneAirport" ("PlaneId","AirportId")
  SELECT p."Id",
         ((pa_first."AirportId" % nb_airport) + 1)
  FROM "Planes" p
  JOIN LATERAL (
     SELECT pa."AirportId"
     FROM "PlaneAirport" pa
     WHERE pa."PlaneId" = p."Id"
     ORDER BY random()
     LIMIT 1
  ) pa_first ON TRUE;

  -- PlanePlaneType relations
  INSERT INTO "PlanePlaneType" ("PlaneId","PlaneTypeId")
  SELECT p."Id", (p."Id" % nb_planetype) + 1 FROM "Planes" p;

  INSERT INTO "PlanePlaneType" ("PlaneId","PlaneTypeId")
  SELECT p."Id",
         ((ppt_first."PlaneTypeId" % nb_planetype) + 1)
  FROM "Planes" p
  JOIN LATERAL (
     SELECT ppt."PlaneTypeId"
     FROM "PlanePlaneType" ppt
     WHERE ppt."PlaneId" = p."Id"
     ORDER BY random()
     LIMIT 1
  ) ppt_first ON TRUE;

  -- Engines
  engine_id := 1;
  WHILE engine_id <= (nb_plane * 5) LOOP
    INSERT INTO "Engines"
    ("Reference","LastMaintenanceDate","SyncTime","Power","PlaneId","AverageFlightHours",
     "AverageFuelConsumption","DeliveryDate","EstimatedPrice","ExchangeDate","FlightHours",
     "FuelConsumption","IgnitionTime","IsHybrid","Manufacturer","NextMaintenanceDate",
     "NoiseLevel","OriginalPrice","PrincipalPartId","IsToBeMaintained","IsFixed")
    VALUES (
      'REF' || engine_id,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE ((DATE '2023-01-01' + floor(random()*3650) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz END,
      (TIME '00:00:00' + (floor(random()*86400) * INTERVAL '1 second'))::time,
      floor(random()*10000),
      (SELECT "Id" FROM "Planes" ORDER BY random() LIMIT 1),
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*10000)::numeric / 100.0) END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*10000)::numeric / 100.0) END,
      ((DATE '2023-01-01' + floor(random()*3650) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (floor(random()*1000000)::numeric / 100.0) END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE ((DATE '2023-01-01' + floor(random()*3650) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz END,
      (floor(random()*100000)::numeric / 100.0),
      (floor(random()*100000)::numeric / 100.0),
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (TIME '00:00:00' + (floor(random()*86400) * INTERVAL '1 second'))::time END,
      --CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE floor(random()*2)::int END,
      CASE WHEN floor(random()*2)::int = 0 THEN FALSE ELSE TRUE END,
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE 'Manufacturer ' || floor(random()*1000000000)::text END,
      ((DATE '2023-01-01' + floor(random()*3650) * INTERVAL '1 day') AT TIME ZONE 'UTC')::timestamptz,
      floor(random()*100),
      (floor(random()*1000000)::numeric / 100.0),
      CASE WHEN floor(random()*2)::int = 0 THEN NULL ELSE (SELECT "Id" FROM "Parts" ORDER BY random() LIMIT 1) END,
      CASE WHEN floor(random()*2)::int = 0 THEN FALSE ELSE TRUE END,
      FALSE
    );
    engine_id := engine_id + 1;
  END LOOP;

  -- EnginePart (unique-like pairs)
  enginepart_id := 1;
  WHILE enginepart_id <= (nb_plane * 5) LOOP
    SELECT "Id" INTO v_engine2_id FROM "Engines" ORDER BY random() LIMIT 1;
    SELECT "Id" INTO v_part_id FROM "Parts" ORDER BY random() LIMIT 1;
    IF NOT EXISTS (
      SELECT 1 FROM "EnginePart" ep
      WHERE ep."EngineId" = v_engine2_id AND ep."PartId" = v_part_id
    ) THEN
      INSERT INTO "EnginePart" ("EngineId","PartId") VALUES (v_engine2_id, v_part_id);
    END IF;
    enginepart_id := enginepart_id + 1;
  END LOOP;

  -- Reset sequences (optional if identity/serial was used)
  PERFORM setval(pg_get_serial_sequence('"Teams"','Id'),        (SELECT max("Id") FROM "Teams"));
  PERFORM setval(pg_get_serial_sequence('"Airports"','Id'),     (SELECT max("Id") FROM "Airports"));
  PERFORM setval(pg_get_serial_sequence('"PlanesTypes"','Id'),  (SELECT max("Id") FROM "PlanesTypes"));
  PERFORM setval(pg_get_serial_sequence('"Planes"','Id'),       (SELECT max("Id") FROM "Planes"));
  PERFORM setval(pg_get_serial_sequence('"Engines"','Id'),      (SELECT max("Id") FROM "Engines"));
END
$$ LANGUAGE plpgsql;