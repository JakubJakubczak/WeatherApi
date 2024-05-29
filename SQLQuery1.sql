CREATE TABLE [dbo].[Weathers] (
    [WeatherId]   UNIQUEIDENTIFIER           NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Temperatura] FLOAT (53)    NOT NULL,
    [Cisnienie]   FLOAT (53)    NOT NULL,
    [Wilgotnosc]  FLOAT (53)    NOT NULL,
    [Czas]        DATETIME2 (7) NOT NULL,
    [StationId]   UNIQUEIDENTIFIER            NOT NULL,
    CONSTRAINT FK_Weather_Station FOREIGN KEY (StationId) REFERENCES Station(StationId)
);

