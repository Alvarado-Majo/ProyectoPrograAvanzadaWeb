USE master;
GO

ALTER DATABASE ProyectoSC701
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE ProyectoSC701;
GO

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



INSERT INTO Users
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    PasswordSalt,
    SignUpDate,
    IsActive
)
VALUES
(
    'Maria',
    'Alvarado',
    'mariajosealvarado235@gmail.com',

    0x076A02649A6D71830CF9BE7D584572E20C12455735C954C27478BCE841C6F79BEAFA11D51A4EA6D9B2BA3F7E97C9D252F3A95279D5876FC54A1801193B2432ED,

    0x92AE91A6B0E3C740367B6D68D5AA443488CE26EA726AC57D65A70F9D980977653560CD9026923D1371FF16CA8D0A2C2D9751ACBE4B67C51482B15B55B54056C9A6CA70BA11A6534A889D0BB436B9F38244F7C93C2F09CC9F66EFF7DCCD026EAF93A020AF135BDA5DF1F3B24A2EF956F39767E37480D9600BCB4DC852AF9BC9FE,

    GETDATE(),
    1
);


UPDATE Users
SET
    PasswordHash = 0x3B1DBF06D6A1C1D25E1007DFCE81ED8242D8207470399A07CF685F65EC0151F2ABCE7C9AAACAE2C46D51206B6825B8FDC078E70DE0860D965AA569837AE4AAB9,

    PasswordSalt = 0x06C5C9AD47218F9BBA3894E3CDDB28CBD665E431DC2BFE0D7DF11F76F5CB52D100D2E22E77D17ACEE67AE2C38036E223064393B2E71207D5F265A8D61280F7274579CCE6172F7534F7147E85BE5F1F38BACEE35063D3B7FFF46CB9285B64F5C0607D4ECB41AB0B5D2277298DB196C2892BAC3376FDC9734EFD4E954D3ACA9702,

    IsActive = 1
WHERE Email = 'mariajosealvarado235@gmail.com';

INSERT INTO Movies
(
    Title,
    MovieRating,
    Synopsis,
    ReleaseYear,
    DurationMinutes,
    PosterImg,
    VideoURL,
    Nationality,
    IsActive
)
VALUES
(
    'Interstellar',
    8.7,
    'A team of explorers travels through a wormhole in space in an attempt to ensure humanity''s survival.',
    2014,
    169,
    'https://picsum.photos/seed/interstellar/300/450',
    NULL,
    'USA',
    1
),
(
    'Inception',
    8.8,
    'A skilled thief who steals secrets through dream-sharing technology is given a chance to erase his past.',
    2010,
    148,
    'https://picsum.photos/seed/inception/300/450',
    NULL,
    'USA',
    1
),
(
    'The Dark Knight',
    9.0,
    'Batman faces a criminal mastermind who throws Gotham City into chaos.',
    2008,
    152,
    'https://picsum.photos/seed/darkknight/300/450',
    NULL,
    'USA',
    1
),
(
    'Parasite',
    8.5,
    'A struggling family slowly becomes involved in the lives of a wealthy household.',
    2019,
    132,
    'https://picsum.photos/seed/parasite/300/450',
    NULL,
    'South Korea',
    1
),
(
    'The Conjuring',
    7.5,
    'Paranormal investigators help a family terrorized by a dark presence in their farmhouse.',
    2013,
    112,
    'https://picsum.photos/seed/conjuring/300/450',
    NULL,
    'USA',
    1
),
(
    'Gladiator',
    8.5,
    'A former Roman general seeks revenge against the emperor who destroyed his family.',
    2000,
    155,
    'https://picsum.photos/seed/gladiator/300/450',
    NULL,
    'USA',
    1
);


UPDATE Movies
SET VideoURL = ''
WHERE VideoURL IS NULL;

SELECT MovieId, Title, VideoURL
FROM Movies;

SELECT * FROM Categories;

INSERT INTO Categories (Name)
VALUES
('Science Fiction'),
('Action'),
('Drama'),
('Horror'),
('Thriller');

SELECT CategoryId, Name FROM Categories;
SELECT MovieId, Title FROM Movies;

INSERT INTO MovieCategories (MovieId, CategoryId)
VALUES
-- Interstellar
(1, 1), -- Science Fiction
(1, 3), -- Drama

-- Inception
(2, 1), -- Science Fiction
(2, 5), -- Thriller

-- The Dark Knight
(3, 2), -- Action
(3, 3), -- Drama

-- Parasite
(4, 3), -- Drama
(4, 5), -- Thriller

-- The Conjuring
(5, 4), -- Horror

-- Gladiator
(6, 2), -- Action
(6, 3); -- Drama