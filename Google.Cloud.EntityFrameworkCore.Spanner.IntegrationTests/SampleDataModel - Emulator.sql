﻿CREATE TABLE Singers (
  SingerId  INT64 NOT NULL,
  FirstName STRING(200),
  LastName  STRING(200) NOT NULL,
  FullName  STRING(400),
  BirthDate DATE,
  Picture   BYTES(MAX),
) PRIMARY KEY (SingerId);

CREATE INDEX Idx_Singers_FullName ON Singers (FullName);

CREATE TABLE Albums (
  AlbumId     INT64 NOT NULL,
  Title       STRING(100) NOT NULL,
  ReleaseDate DATE,
  SingerId    INT64 NOT NULL,
  CONSTRAINT FK_Albums_Singers FOREIGN KEY (SingerId) REFERENCES Singers (SingerId),
) PRIMARY KEY (AlbumId);

CREATE TABLE Tracks (
  AlbumId         INT64 NOT NULL,
  TrackId         INT64 NOT NULL,
  Title           STRING(200) NOT NULL,
  Duration        FLOAT64,
  LyricsLanguages ARRAY<STRING(2)>,
  Lyrics          ARRAY<STRING(MAX)>,
) PRIMARY KEY (AlbumId, TrackId), INTERLEAVE IN PARENT Albums;

CREATE UNIQUE INDEX Idx_Tracks_AlbumId_Title ON Tracks (TrackId, Title);

CREATE TABLE Venues (
  Code      STRING(10) NOT NULL,
  Name      STRING(100),
  Active    BOOL NOT NULL,
  Capacity  INT64,
  Ratings   ARRAY<FLOAT64>,
) PRIMARY KEY (Code);

CREATE TABLE Concerts (
  VenueCode STRING(10) NOT NULL,
  StartTime TIMESTAMP NOT NULL,
  SingerId  INT64 NOT NULL,
  Title     STRING(200),
  CONSTRAINT FK_Concerts_Venues FOREIGN KEY (VenueCode) REFERENCES Venues (Code),
  CONSTRAINT FK_Concerts_Singers FOREIGN KEY (SingerId) REFERENCES Singers (SingerId),
) PRIMARY KEY (VenueCode, StartTime, SingerId);

CREATE TABLE Performances (
  VenueCode        STRING(10) NOT NULL,
  ConcertStartTime TIMESTAMP NOT NULL,
  SingerId         INT64 NOT NULL,
  AlbumId          INT64 NOT NULL,
  TrackId          INT64 NOT NULL,
  StartTime        TIMESTAMP,
  Rating           FLOAT64,
  CONSTRAINT FK_Performances_Concerts FOREIGN KEY (VenueCode, ConcertStartTime, SingerId) REFERENCES Concerts (VenueCode, StartTime, SingerId),
  CONSTRAINT FK_Performances_Singers FOREIGN KEY (SingerId) REFERENCES Singers (SingerId),
  CONSTRAINT FK_Performances_Tracks FOREIGN KEY (AlbumId, TrackId) REFERENCES Tracks (AlbumId, TrackId),
) PRIMARY KEY (VenueCode, SingerId, StartTime);

CREATE TABLE TableWithAllColumnTypes (
	ColInt64 INT64 NOT NULL,
	ColFloat64 FLOAT64,
	ColNumeric FLOAT64,
	ColBool BOOL,
	ColString STRING(100),
	ColStringMax STRING(MAX),
	ColBytes BYTES(100),
	ColBytesMax BYTES(MAX),
	ColDate DATE,
	ColTimestamp TIMESTAMP,
	ColCommitTS TIMESTAMP OPTIONS (allow_commit_timestamp=true),
	ColInt64Array ARRAY<INT64>,
	ColFloat64Array ARRAY<FLOAT64>,
	ColNumericArray ARRAY<FLOAT64>,
	ColBoolArray ARRAY<BOOL>,
	ColStringArray ARRAY<STRING(100)>,
	ColStringMaxArray ARRAY<STRING(MAX)>,
	ColBytesArray ARRAY<BYTES(100)>,
	ColBytesMaxArray ARRAY<BYTES(MAX)>,
	ColDateArray ARRAY<DATE>,
	ColTimestampArray ARRAY<TIMESTAMP>,
	ColComputed STRING(MAX),
) PRIMARY KEY (ColInt64);

CREATE NULL_FILTERED INDEX IDX_TableWithAllColumnTypes_ColDate_ColCommitTS ON TableWithAllColumnTypes (ColDate, ColCommitTS);