CREATE DATABASE ProyectoSC701;
GO

USE ProyectoSC701;
GO


CREATE TABLE Users (
    IdUser INT IDENTITY PRIMARY KEY,
    FName VARCHAR(50),
    LName VARCHAR(50),
    Email VARCHAR(150) UNIQUE,
    PasswordHash VARCHAR(255),
    SignUpDate DATETIME DEFAULT GETDATE()
);


CREATE TABLE Movies (
    IdMovie INT IDENTITY PRIMARY KEY,
    Title VARCHAR(200),
    Synopsis TEXT,
    Yeear INT,
    Duration INT,
    Rating DECIMAL(3,1),
    PosterImg VARCHAR(255),
    VideoURL VARCHAR(255),
    IdDirector INT
);


CREATE TABLE Directors (
    IdDirector INT IDENTITY PRIMARY KEY,
    FName VARCHAR(50),
    LName VARCHAR(50),
    Nationality VARCHAR(100),
    Biography TEXT,
    BirthDate DATE,
    PictureImg VARCHAR(255)
);


CREATE TABLE Actors (
    IdActor INT IDENTITY PRIMARY KEY,
    FName VARCHAR(50),}
    LName VARCHAR(50),
    Nationality VARCHAR(100),
    Biography TEXT,
    BirthDate DATE,
    PictureImg VARCHAR(255)
);