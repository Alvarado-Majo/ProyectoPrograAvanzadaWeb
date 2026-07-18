CREATE DATABASE ProyectoSC701;
GO

USE ProyectoSC701;
GO


CREATE TABLE Users (
    UserId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    PasswordHash VARBINARY(256) NOT NULL, -- Manejo de hash para password
    PasswordSalt VARBINARY(128) NOT NULL, -- Manejo de hash para password
    SignUpDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive tinyint NOT NULL DEFAULT 1
);


CREATE TABLE Directors (
    DirectorId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(100),
    Biography NVARCHAR(MAX),
    BirthDate DATE,
    PictureImg NVARCHAR(255),
    IsActive tinyint NOT NULL DEFAULT 1
);


CREATE TABLE Actors (
    ActorId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(100),
    Biography NVARCHAR(MAX),
    BirthDate DATE,
    PictureImg NVARCHAR(255),
    IsActive tinyint NOT NULL DEFAULT 1
);


CREATE TABLE Movies (
    MovieId INT IDENTITY PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    MovieRating DECIMAL(3,1) NULL, -- *** AGREGADO después de reunión con profesor.
    Synopsis NVARCHAR(MAX),
    ReleaseYear INT NOT NULL,
    DurationMinutes INT NOT NULL CHECK (DurationMinutes > 0),
    PosterImg NVARCHAR(255),
    VideoURL NVARCHAR(255),
    Nationality varchar(70),

    -- DirectorId INT NOT NULL,   -- *** (Eliminado después de reunión con el profe, se utilizarán tablas intermedias para mostrar más de un director) ***
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,                       
    IsActive tinyint NOT NULL DEFAULT 1,

    -- *** (Eliminado después de reunión con el profe, se utilizarán tablas intermedias para mostrar más de un director) ***
    -- CONSTRAINT FK_Movies_Directors 
        -- FOREIGN KEY (DirectorId) REFERENCES Directors(DirectorId)
);


CREATE TABLE Categories (
    CategoryId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) UNIQUE NOT NULL
);



-- Manejar a los directores y actores en una tabla intermedia, relacion a Actors y Movies many to many.

CREATE TABLE MovieActors (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    CharacterName NVARCHAR(100),
    PRIMARY KEY (MovieId, ActorId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId) ON DELETE CASCADE
);


-- *** AGREGADO después de reunión con profesor.
CREATE TABLE MovieDirectors (
    MovieId INT NOT NULL,
    DirectorId INT NOT NULL,

    PRIMARY KEY (MovieId, DirectorId),

    FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE,

    FOREIGN KEY (DirectorId)
        REFERENCES Directors(DirectorId)
        ON DELETE CASCADE
);


-- Tabla intermedia, many to many.
CREATE TABLE MovieCategories (
    MovieId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (MovieId, CategoryId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE
);


-- Otras tablas necesarias, según el enunciado del proyecto:

CREATE TABLE WatchLists (
    WatchListId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);


CREATE TABLE WatchListMovies (
    WatchListId INT NOT NULL,
    MovieId INT NOT NULL,
    PRIMARY KEY (WatchListId, MovieId),
    FOREIGN KEY (WatchListId) REFERENCES WatchLists(WatchListId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);


CREATE TABLE Reviews (
    ReviewId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 10), -- Sólo valores del 1 al 10
    Comment NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);


-- Índices (sólo en caso de que se necesiten)

-- Movies
CREATE INDEX IX_Movies_Title ON Movies(Title);
CREATE INDEX IX_Movies_ReleaseYear ON Movies(ReleaseYear);
CREATE INDEX IX_MovieDirectors_MovieId ON MovieDirectors(MovieId);
CREATE INDEX IX_MovieDirectors_DirectorId ON MovieDirectors(DirectorId);

-- Reviews
CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);
CREATE INDEX IX_Reviews_MovieId ON Reviews(MovieId);

-- MovieCategories
CREATE INDEX IX_MovieCategories_CategoryId ON MovieCategories(CategoryId);
CREATE INDEX IX_MovieCategories_MovieId ON MovieCategories(MovieId);

-- MovieActors
CREATE INDEX IX_MovieActors_ActorId ON MovieActors(ActorId);

-- WatchLists
CREATE INDEX IX_WatchLists_UserId ON WatchLists(UserId);



