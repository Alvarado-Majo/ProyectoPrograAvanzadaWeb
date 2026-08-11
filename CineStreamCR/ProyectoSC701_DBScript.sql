USE master;
GO

IF DB_ID(N'ProyectoSC701') IS NOT NULL
BEGIN
    ALTER DATABASE ProyectoSC701
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;

    DROP DATABASE ProyectoSC701;
END
GO

CREATE DATABASE ProyectoSC701;
GO

USE ProyectoSC701;
GO

SET NOCOUNT ON;
GO


/*USERS*/

CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PasswordHash VARBINARY(256) NOT NULL,
    PasswordSalt VARBINARY(128) NOT NULL,
    SignUpDate DATETIME NOT NULL
        CONSTRAINT DF_Users_SignUpDate DEFAULT GETDATE(),
    IsActive TINYINT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT 1,

    CONSTRAINT PK_Users PRIMARY KEY (UserId),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO


/*DIRECTORS*/

CREATE TABLE dbo.Directors (
    DirectorId INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(100) NULL,
    Biography NVARCHAR(MAX) NULL,
    BirthDate DATE NULL,
    PictureImg NVARCHAR(255) NULL,
    IsActive TINYINT NOT NULL
        CONSTRAINT DF_Directors_IsActive DEFAULT 1,

    CONSTRAINT PK_Directors PRIMARY KEY (DirectorId)
);
GO


/*ACTORS*/

CREATE TABLE dbo.Actors (
    ActorId INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(100) NULL,
    Biography NVARCHAR(MAX) NULL,
    BirthDate DATE NULL,
    PictureImg NVARCHAR(255) NULL,
    IsActive TINYINT NOT NULL
        CONSTRAINT DF_Actors_IsActive DEFAULT 1,

    CONSTRAINT PK_Actors PRIMARY KEY (ActorId)
);
GO


/*MOVIES*/

CREATE TABLE dbo.Movies (
    MovieId INT IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    MovieRating DECIMAL(3,1) NULL,
    Synopsis NVARCHAR(MAX) NULL,
    ReleaseYear INT NOT NULL,
    DurationMinutes INT NOT NULL,
    PosterImg NVARCHAR(255) NULL,
    VideoURL NVARCHAR(500) NULL,
    Nationality NVARCHAR(70) NULL,
    CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_Movies_CreatedAt DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    IsActive TINYINT NOT NULL
        CONSTRAINT DF_Movies_IsActive DEFAULT 1,

    CONSTRAINT PK_Movies PRIMARY KEY (MovieId),
    CONSTRAINT CK_Movies_DurationMinutes CHECK (DurationMinutes > 0),
    CONSTRAINT CK_Movies_Rating CHECK (
        MovieRating IS NULL OR MovieRating BETWEEN 0 AND 10
    )
);
GO


/*CATEGORIES*/

CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_Categories PRIMARY KEY (CategoryId),
    CONSTRAINT UQ_Categories_Name UNIQUE (Name)
);
GO


/*MOVIE ACTORS*/

CREATE TABLE dbo.MovieActors (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    CharacterName NVARCHAR(100) NULL,

    CONSTRAINT PK_MovieActors
        PRIMARY KEY (MovieId, ActorId),

    CONSTRAINT FK_MovieActors_Movies
        FOREIGN KEY (MovieId)
        REFERENCES dbo.Movies(MovieId)
        ON DELETE CASCADE,

    CONSTRAINT FK_MovieActors_Actors
        FOREIGN KEY (ActorId)
        REFERENCES dbo.Actors(ActorId)
        ON DELETE CASCADE
);
GO


/*MOVIE DIRECTORS*/

CREATE TABLE dbo.MovieDirectors (
    MovieId INT NOT NULL,
    DirectorId INT NOT NULL,

    CONSTRAINT PK_MovieDirectors
        PRIMARY KEY (MovieId, DirectorId),

    CONSTRAINT FK_MovieDirectors_Movies
        FOREIGN KEY (MovieId)
        REFERENCES dbo.Movies(MovieId)
        ON DELETE CASCADE,

    CONSTRAINT FK_MovieDirectors_Directors
        FOREIGN KEY (DirectorId)
        REFERENCES dbo.Directors(DirectorId)
        ON DELETE CASCADE
);
GO


/*MOVIE CATEGORIES*/

CREATE TABLE dbo.MovieCategories (
    MovieId INT NOT NULL,
    CategoryId INT NOT NULL,

    CONSTRAINT PK_MovieCategories
        PRIMARY KEY (MovieId, CategoryId),

    CONSTRAINT FK_MovieCategories_Movies
        FOREIGN KEY (MovieId)
        REFERENCES dbo.Movies(MovieId)
        ON DELETE CASCADE,

    CONSTRAINT FK_MovieCategories_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES dbo.Categories(CategoryId)
        ON DELETE CASCADE
);
GO


/*WATCHLISTS*/

CREATE TABLE dbo.WatchLists (
    WatchListId INT IDENTITY(1,1) NOT NULL,
    UserId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_WatchLists_CreatedAt DEFAULT GETDATE(),

    CONSTRAINT PK_WatchLists PRIMARY KEY (WatchListId),

    CONSTRAINT FK_WatchLists_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users(UserId)
        ON DELETE CASCADE
);
GO


/*WATCHLIST MOVIES*/

CREATE TABLE dbo.WatchListMovies (
    WatchListId INT NOT NULL,
    MovieId INT NOT NULL,

    CONSTRAINT PK_WatchListMovies
        PRIMARY KEY (WatchListId, MovieId),

    CONSTRAINT FK_WatchListMovies_WatchLists
        FOREIGN KEY (WatchListId)
        REFERENCES dbo.WatchLists(WatchListId)
        ON DELETE CASCADE,

    CONSTRAINT FK_WatchListMovies_Movies
        FOREIGN KEY (MovieId)
        REFERENCES dbo.Movies(MovieId)
        ON DELETE CASCADE
);
GO


/*REVIEWS*/

CREATE TABLE dbo.Reviews (
    ReviewId INT IDENTITY(1,1) NOT NULL,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    IsLike bit NOT NULL,
    Comment NVARCHAR(MAX) NULL,
    ReviewDate DATETIME NOT NULL
        CONSTRAINT DF_Reviews_ReviewDate DEFAULT GETDATE(),

    CONSTRAINT PK_Reviews PRIMARY KEY (ReviewId),



    CONSTRAINT FK_Reviews_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Reviews_Movies
        FOREIGN KEY (MovieId)
        REFERENCES dbo.Movies(MovieId)
        ON DELETE CASCADE
);
GO


/*ÍNDICES*/

CREATE INDEX IX_Movies_Title
    ON dbo.Movies(Title);

CREATE INDEX IX_Movies_ReleaseYear
    ON dbo.Movies(ReleaseYear);

CREATE INDEX IX_MovieDirectors_MovieId
    ON dbo.MovieDirectors(MovieId);

CREATE INDEX IX_MovieDirectors_DirectorId
    ON dbo.MovieDirectors(DirectorId);

CREATE INDEX IX_Reviews_UserId
    ON dbo.Reviews(UserId);

CREATE INDEX IX_Reviews_MovieId
    ON dbo.Reviews(MovieId);

CREATE INDEX IX_MovieCategories_CategoryId
    ON dbo.MovieCategories(CategoryId);

CREATE INDEX IX_MovieCategories_MovieId
    ON dbo.MovieCategories(MovieId);

CREATE INDEX IX_MovieActors_ActorId
    ON dbo.MovieActors(ActorId);

CREATE INDEX IX_WatchLists_UserId
    ON dbo.WatchLists(UserId);
GO


/* =========================================================
   DATOS INICIALES
   ========================================================= */

/* =========================================================
   USUARIO INICIAL
   ========================================================= */

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Users
    WHERE Email = 'mariajosealvarado235@gmail.com'
)
BEGIN

    INSERT INTO dbo.Users
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
        0x3B1DBF06D6A1C1D25E1007DFCE81ED8242D8207470399A07CF685F65EC0151F2ABCE7C9AAACAE2C46D51206B6825B8FDC078E70DE0860D965AA569837AE4AAB9,
        0x06C5C9AD47218F9BBA3894E3CDDB28CBD665E431DC2BFE0D7DF11F76F5CB52D100D2E22E77D17ACEE67AE2C38036E223064393B2E71207D5F265A8D61280F7274579CCE6172F7534F7147E85BE5F1F38BACEE35063D3B7FFF46CB9285B64F5C0607D4ECB41AB0B5D2277298DB196C2892BAC3376FDC9734EFD4E954D3ACA9702,
        GETDATE(),
        1
    );

END
ELSE
BEGIN

    UPDATE dbo.Users
    SET
        FirstName = 'Maria',
        LastName = 'Alvarado',
        PasswordHash = 0x3B1DBF06D6A1C1D25E1007DFCE81ED8242D8207470399A07CF685F65EC0151F2ABCE7C9AAACAE2C46D51206B6825B8FDC078E70DE0860D965AA569837AE4AAB9,
        PasswordSalt = 0x06C5C9AD47218F9BBA3894E3CDDB28CBD665E431DC2BFE0D7DF11F76F5CB52D100D2E22E77D17ACEE67AE2C38036E223064393B2E71207D5F265A8D61280F7274579CCE6172F7534F7147E85BE5F1F38BACEE35063D3B7FFF46CB9285B64F5C0607D4ECB41AB0B5D2277298DB196C2892BAC3376FDC9734EFD4E954D3ACA9702,
        IsActive = 1
    WHERE Email = 'mariajosealvarado235@gmail.com';

END
GO


/* =========================================================
   PELÍCULAS
   ========================================================= */

INSERT INTO dbo.Movies
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
);
GO


INSERT INTO dbo.Movies
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
    'Inception',
    8.8,
    'A skilled thief who steals secrets through dream-sharing technology is given a chance to erase his past.',
    2010,
    148,
    'https://picsum.photos/seed/inception/300/450',
    NULL,
    'USA',
    1
);
GO


INSERT INTO dbo.Movies
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
    'The Dark Knight',
    9.0,
    'Batman faces a criminal mastermind who throws Gotham City into chaos.',
    2008,
    152,
    'https://picsum.photos/seed/darkknight/300/450',
    NULL,
    'USA',
    1
);
GO


INSERT INTO dbo.Movies
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
    'Parasite',
    8.5,
    'A struggling family slowly becomes involved in the lives of a wealthy household.',
    2019,
    132,
    'https://picsum.photos/seed/parasite/300/450',
    NULL,
    'South Korea',
    1
);
GO


INSERT INTO dbo.Movies
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
    'The Conjuring',
    7.5,
    'Paranormal investigators help a family terrorized by a dark presence in their farmhouse.',
    2013,
    112,
    'https://picsum.photos/seed/conjuring/300/450',
    NULL,
    'USA',
    1
);
GO


INSERT INTO dbo.Movies
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
GO


-- Dejar VideoURL vacío mientras agregamos los videos reales

UPDATE dbo.Movies
SET VideoURL = ''
WHERE VideoURL IS NULL;
GO


/* =========================================================
   CATEGORÍAS
   ========================================================= */

INSERT INTO dbo.Categories (Name)
VALUES ('Science Fiction');
GO

INSERT INTO dbo.Categories (Name)
VALUES ('Action');
GO

INSERT INTO dbo.Categories (Name)
VALUES ('Drama');
GO

INSERT INTO dbo.Categories (Name)
VALUES ('Horror');
GO

INSERT INTO dbo.Categories (Name)
VALUES ('Thriller');
GO


/* =========================================================
   CATEGORÍAS DE PELÍCULAS
   ========================================================= */

-- Interstellar
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (1, 1);
GO

INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (1, 3);
GO


-- Inception
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (2, 1);
GO

INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (2, 5);
GO


-- The Dark Knight
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (3, 2);
GO

INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (3, 3);
GO


-- Parasite
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (4, 3);
GO

INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (4, 5);
GO


-- The Conjuring
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (5, 4);
GO


-- Gladiator
INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (6, 2);
GO

INSERT INTO dbo.MovieCategories (MovieId, CategoryId)
VALUES (6, 3);
GO


/* =========================================================
   VERIFICACIÓN
   ========================================================= */

SELECT *
FROM dbo.Users;

SELECT MovieId, Title, VideoURL
FROM dbo.Movies;

SELECT CategoryId, Name
FROM dbo.Categories;

SELECT *
FROM dbo.MovieCategories;
GO

UPDATE dbo.Movies
SET VideoURL = ''
WHERE VideoURL IS NULL;

GO

--VIDEOS DE PRUEBA

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4'
WHERE Title = 'Interstellar';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4'
WHERE Title = 'Inception';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4'
WHERE Title = 'The Dark Knight';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4'
WHERE Title = 'Parasite';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/ForBiggerMeltdowns.mp4'
WHERE Title = 'The Conjuring';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4'
WHERE Title = 'Gladiator';

GO

SELECT MovieId, Title, VideoURL
FROM dbo.Movies;

--eliminar de ser necesario es una PRUEBA
UPDATE dbo.Movies
SET VideoURL = 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4'
WHERE Title = 'Interstellar';

UPDATE dbo.Movies
SET VideoURL = 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4'
WHERE Title = 'Inception';

UPDATE dbo.Movies
SET VideoURL = 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4'
WHERE Title = 'The Dark Knight';

UPDATE dbo.Movies
SET VideoURL = 'https://storage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4'
WHERE Title = 'Parasite';

UPDATE dbo.Movies
SET VideoURL = 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4'
WHERE Title = 'The Conjuring';

UPDATE dbo.Movies
SET VideoURL = 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4'
WHERE Title = 'Gladiator';


-- actualización de imágenes
UPDATE dbo.Movies
SET PosterImg = '/images/movies/interstellar.jpg'
WHERE Title = 'Interstellar';

UPDATE dbo.Movies
SET PosterImg = '/images/movies/inception.jpg'
WHERE Title = 'Inception';

UPDATE dbo.Movies
SET PosterImg = '/images/movies/darkknight.jpg'
WHERE Title = 'The Dark Knight';

UPDATE dbo.Movies
SET PosterImg = '/images/movies/parasite.jpg'
WHERE Title = 'Parasite';

UPDATE dbo.Movies
SET PosterImg = '/images/movies/conjuring.jpg'
WHERE Title = 'The Conjuring';

UPDATE dbo.Movies
SET PosterImg = '/images/movies/gladiator.jpg'
WHERE Title = 'Gladiator';

/*DIRECTORES*/

INSERT INTO dbo.Directors
(
    FirstName,
    LastName,
    Nationality,
    Biography,
    BirthDate,
    PictureImg,
    IsActive
)
VALUES
(
    'Christopher',
    'Nolan',
    'British-American',
    'Film director known for complex narratives and visually ambitious productions.',
    '1970-07-30',
    '/images/directors/christopher-nolan.jpg',
    1
);

INSERT INTO dbo.Directors
(
    FirstName,
    LastName,
    Nationality,
    Biography,
    BirthDate,
    PictureImg,
    IsActive
)
VALUES
(
    'Bong',
    'Joon-ho',
    'South Korean',
    'South Korean filmmaker known for combining drama, suspense and social commentary.',
    '1969-09-14',
    '/images/directors/bong-joon-ho.jpg',
    1
);

INSERT INTO dbo.Directors
(
    FirstName,
    LastName,
    Nationality,
    Biography,
    BirthDate,
    PictureImg,
    IsActive
)
VALUES
(
    'James',
    'Wan',
    'Australian',
    'Film director and producer known especially for horror and suspense films.',
    '1977-02-26',
    '/images/directors/james-wan.jpg',
    1
);

INSERT INTO dbo.Directors
(
    FirstName,
    LastName,
    Nationality,
    Biography,
    BirthDate,
    PictureImg,
    IsActive
)
VALUES
(
    'Ridley',
    'Scott',
    'British',
    'British filmmaker known for directing historical dramas and science fiction films.',
    '1937-11-30',
    '/images/directors/ridley-scott.jpg',
    1
);

GO

/*DIRECTORES DE CADA PELÍCULA*/

-- Interstellar - Christopher Nolan
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (1, 1);

-- Inception - Christopher Nolan
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (2, 1);

-- The Dark Knight - Christopher Nolan
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (3, 1);

-- Parasite - Bong Joon-ho
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (4, 2);

-- The Conjuring - James Wan
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (5, 3);

-- Gladiator - Ridley Scott
INSERT INTO dbo.MovieDirectors (MovieId, DirectorId)
VALUES (6, 4);

GO

/*ACTORES*/

-- INTERSTELLAR

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Matthew', 'McConaughey', 'American',
'American actor known for dramatic and adventure films.',
'1969-11-04',
'/images/actors/matthew-mcconaughey.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Anne', 'Hathaway', 'American',
'American actress known for drama, comedy and science fiction films.',
'1982-11-12',
'/images/actors/anne-hathaway.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Jessica', 'Chastain', 'American',
'American actress known for dramatic and science fiction roles.',
'1977-03-24',
'/images/actors/jessica-chastain.jpg',
1);


-- INCEPTION

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Leonardo', 'DiCaprio', 'American',
'American actor known for numerous internationally successful films.',
'1974-11-11',
'/images/actors/leonardo-dicaprio.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Joseph', 'Gordon-Levitt', 'American',
'American actor known for drama, action and science fiction films.',
'1981-02-17',
'/images/actors/joseph-gordon-levitt.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Tom', 'Hardy', 'British',
'British actor known for action and dramatic performances.',
'1977-09-15',
'/images/actors/tom-hardy.jpg',
1);


-- THE DARK KNIGHT

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Christian', 'Bale', 'British',
'British actor known for dramatic roles and his portrayal of Batman.',
'1974-01-30',
'/images/actors/christian-bale.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Heath', 'Ledger', 'Australian',
'Australian actor known for a wide range of acclaimed performances.',
'1979-04-04',
'/images/actors/heath-ledger.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Gary', 'Oldman', 'British',
'British actor known for highly versatile film performances.',
'1958-03-21',
'/images/actors/gary-oldman.jpg',
1);


-- PARASITE

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Song', 'Kang-ho', 'South Korean',
'South Korean actor known for acclaimed Korean films.',
'1967-01-17',
'/images/actors/song-kang-ho.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Lee', 'Sun-kyun', 'South Korean',
'South Korean actor known for film and television performances.',
'1975-03-02',
'/images/actors/lee-sun-kyun.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Cho', 'Yeo-jeong', 'South Korean',
'South Korean actress known for film and television roles.',
'1981-02-10',
'/images/actors/cho-yeo-jeong.jpg',
1);


-- THE CONJURING

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Vera', 'Farmiga', 'American',
'American actress known for drama, suspense and horror films.',
'1973-08-06',
'/images/actors/vera-farmiga.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Patrick', 'Wilson', 'American',
'American actor known for drama, action and horror films.',
'1973-07-03',
'/images/actors/patrick-wilson.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Lili', 'Taylor', 'American',
'American actress known for film, television and theater.',
'1967-02-20',
'/images/actors/lili-taylor.jpg',
1);


-- GLADIATOR

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Russell', 'Crowe', 'New Zealand',
'Actor known for dramatic and historical films.',
'1964-04-07',
'/images/actors/russell-crowe.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Joaquin', 'Phoenix', 'American',
'American actor known for intense dramatic performances.',
'1974-10-28',
'/images/actors/joaquin-phoenix.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Connie', 'Nielsen', 'Danish',
'Danish actress known for international film and television roles.',
'1965-07-03',
'/images/actors/connie-nielsen.jpg',
1);

GO

SELECT ActorId, FirstName, LastName
FROM dbo.Actors
ORDER BY ActorId;

/*ELENCO DE LAS PELÍCULAS*/

-- INTERSTELLAR
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (1, 1, 'Cooper');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (1, 2, 'Brand');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (1, 3, 'Murph');


-- INCEPTION
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (2, 4, 'Cobb');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (2, 5, 'Arthur');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (2, 6, 'Eames');


-- THE DARK KNIGHT
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (3, 7, 'Bruce Wayne / Batman');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (3, 8, 'Joker');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (3, 9, 'James Gordon');


-- PARASITE
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (4, 10, 'Kim Ki-taek');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (4, 11, 'Park Dong-ik');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (4, 12, 'Chung-sook');


-- THE CONJURING
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (5, 13, 'Lorraine Warren');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (5, 14, 'Ed Warren');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (5, 15, 'Carolyn Perron');


-- GLADIATOR
INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (6, 16, 'Maximus');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (6, 17, 'Commodus');

INSERT INTO dbo.MovieActors (MovieId, ActorId, CharacterName)
VALUES (6, 18, 'Lucilla');

GO

--imagenes

UPDATE dbo.Directors
SET PictureImg = '/images/directors/christopher-nolan.jpg'
WHERE FirstName = 'Christopher'
  AND LastName = 'Nolan';

UPDATE dbo.Directors
SET PictureImg = '/images/directors/bong-joon-ho.jpg'
WHERE FirstName = 'Bong'
  AND LastName = 'Joon-ho';

UPDATE dbo.Directors
SET PictureImg = '/images/directors/james-wan.jpg'
WHERE FirstName = 'James'
  AND LastName = 'Wan';

UPDATE dbo.Directors
SET PictureImg = '/images/directors/ridley-scott.jpg'
WHERE FirstName = 'Ridley'
  AND LastName = 'Scott';

  SELECT
    DirectorId,
    FirstName,
    LastName,
    PictureImg
FROM dbo.Directors;

/*ACTORES*/

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Matthew', 'McConaughey', 'American',
'American actor known for dramatic and adventure films.',
'1969-11-04',
'/images/actors/matthew-mcconaughey.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Anne', 'Hathaway', 'American',
'American actress known for drama and science fiction films.',
'1982-11-12',
'/images/actors/anne-hathaway.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Jessica', 'Chastain', 'American',
'American actress known for dramatic and science fiction roles.',
'1977-03-24',
'/images/actors/jessica-chastain.jpg',
1);


INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Leonardo', 'DiCaprio', 'American',
'American actor known for internationally successful films.',
'1974-11-11',
'/images/actors/leonardo-dicaprio.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Joseph', 'Gordon-Levitt', 'American',
'American actor known for drama, action and science fiction films.',
'1981-02-17',
'/images/actors/joseph-gordon-levitt.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Tom', 'Hardy', 'British',
'British actor known for action and dramatic performances.',
'1977-09-15',
'/images/actors/tom-hardy.jpg',
1);


INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Christian', 'Bale', 'British',
'British actor known for dramatic roles and his portrayal of Batman.',
'1974-01-30',
'/images/actors/christian-bale.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Heath', 'Ledger', 'Australian',
'Australian actor known for acclaimed film performances.',
'1979-04-04',
'/images/actors/heath-ledger.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Gary', 'Oldman', 'British',
'British actor known for highly versatile film performances.',
'1958-03-21',
'/images/actors/gary-oldman.jpg',
1);


INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Song', 'Kang-ho', 'South Korean',
'South Korean actor known for acclaimed Korean films.',
'1967-01-17',
'/images/actors/song-kang-ho.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Lee', 'Sun-kyun', 'South Korean',
'South Korean actor known for film and television performances.',
'1975-03-02',
'/images/actors/lee-sun-kyun.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Cho', 'Yeo-jeong', 'South Korean',
'South Korean actress known for film and television roles.',
'1981-02-10',
'/images/actors/cho-yeo-jeong.jpg',
1);


INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Vera', 'Farmiga', 'American',
'American actress known for drama, suspense and horror films.',
'1973-08-06',
'/images/actors/vera-farmiga.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Patrick', 'Wilson', 'American',
'American actor known for drama, action and horror films.',
'1973-07-03',
'/images/actors/patrick-wilson.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Lili', 'Taylor', 'American',
'American actress known for film, television and theater.',
'1967-02-20',
'/images/actors/lili-taylor.jpg',
1);


INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Russell', 'Crowe', 'New Zealand',
'Actor known for dramatic and historical films.',
'1964-04-07',
'/images/actors/russell-crowe.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Joaquin', 'Phoenix', 'American',
'American actor known for intense dramatic performances.',
'1974-10-28',
'/images/actors/joaquin-phoenix.jpg',
1);

INSERT INTO dbo.Actors
(FirstName, LastName, Nationality, Biography, BirthDate, PictureImg, IsActive)
VALUES
('Connie', 'Nielsen', 'Danish',
'Danish actress known for international film and television roles.',
'1965-07-03',
'/images/actors/connie-nielsen.jpg',
1);

GO

SELECT ActorId, FirstName, LastName, PictureImg
FROM dbo.Actors
ORDER BY ActorId;

SELECT
    ma.MovieId,
    m.Title,
    ma.ActorId,
    a.FirstName,
    a.LastName,
    ma.CharacterName
FROM dbo.MovieActors ma
INNER JOIN dbo.Movies m
    ON ma.MovieId = m.MovieId
INNER JOIN dbo.Actors a
    ON ma.ActorId = a.ActorId
ORDER BY ma.MovieId, ma.ActorId;

UPDATE dbo.MovieActors
SET CharacterName = 'Cooper'
WHERE MovieId = 1 AND ActorId = 1;

UPDATE dbo.MovieActors
SET CharacterName = 'Brand'
WHERE MovieId = 1 AND ActorId = 2;

UPDATE dbo.MovieActors
SET CharacterName = 'Murph'
WHERE MovieId = 1 AND ActorId = 3;

UPDATE dbo.MovieActors
SET CharacterName = 'Cobb'
WHERE MovieId = 2 AND ActorId = 4;

UPDATE dbo.MovieActors
SET CharacterName = 'Arthur'
WHERE MovieId = 2 AND ActorId = 5;

UPDATE dbo.MovieActors
SET CharacterName = 'Eames'
WHERE MovieId = 2 AND ActorId = 6;

UPDATE dbo.MovieActors
SET CharacterName = 'Bruce Wayne / Batman'
WHERE MovieId = 3 AND ActorId = 7;

UPDATE dbo.MovieActors
SET CharacterName = 'Joker'
WHERE MovieId = 3 AND ActorId = 8;

UPDATE dbo.MovieActors
SET CharacterName = 'James Gordon'
WHERE MovieId = 3 AND ActorId = 9;

UPDATE dbo.MovieActors
SET CharacterName = 'Kim Ki-taek'
WHERE MovieId = 4 AND ActorId = 10;

UPDATE dbo.MovieActors
SET CharacterName = 'Park Dong-ik'
WHERE MovieId = 4 AND ActorId = 11;

UPDATE dbo.MovieActors
SET CharacterName = 'Chung-sook'
WHERE MovieId = 4 AND ActorId = 12;

UPDATE dbo.MovieActors
SET CharacterName = 'Lorraine Warren'
WHERE MovieId = 5 AND ActorId = 13;

UPDATE dbo.MovieActors
SET CharacterName = 'Ed Warren'
WHERE MovieId = 5 AND ActorId = 14;

UPDATE dbo.MovieActors
SET CharacterName = 'Carolyn Perron'
WHERE MovieId = 5 AND ActorId = 15;

UPDATE dbo.MovieActors
SET CharacterName = 'Maximus'
WHERE MovieId = 6 AND ActorId = 16;

UPDATE dbo.MovieActors
SET CharacterName = 'Commodus'
WHERE MovieId = 6 AND ActorId = 17;

UPDATE dbo.MovieActors
SET CharacterName = 'Lucilla'
WHERE MovieId = 6 AND ActorId = 18;

--videos
UPDATE Movies
SET VideoUrl = 'https://youtu.be/LYS2O1nl9iM?si=AJz_DR-ic4B1Saj1'
WHERE MovieId = 1;

UPDATE Movies
SET VideoUrl = 'https://youtu.be/YoHD9XEInc0?si=6Vp8FFl_7ucznRx9'
WHERE MovieId = 2;

UPDATE Movies
SET VideoUrl = 'https://youtu.be/EXeTwQWrcwY?si=-q-mNwp4mQx7eaYv'
WHERE MovieId = 3;

UPDATE Movies
SET VideoUrl = 'https://youtu.be/5xH0HfJHsaY?si=_JzHlDcCfGJVLA6v'
WHERE MovieId = 4;

UPDATE Movies
SET VideoUrl = 'https://youtu.be/k10ETZ41q5o?si=mTadRYAmiz5cPrRh'
WHERE MovieId = 5;
UPDATE Movies
SET VideoUrl = 'https://youtu.be/P5ieIbInFpg?si=LvwrV8t05DyRyIPy'
WHERE MovieId = 6;

-- BORRAR PELICULA DUPLICADA----
DELETE FROM dbo.MovieCategories
WHERE MovieId = 2;

DELETE FROM dbo.MovieDirectors
WHERE MovieId = 2;

DELETE FROM dbo.MovieActors
WHERE MovieId = 2;

DELETE FROM dbo.WatchListMovies
WHERE MovieId = 2;


DELETE FROM dbo.Movies
WHERE MovieId = 2;

SELECT MovieId, Title
FROM dbo.Movies
WHERE Title = 'Interstellar';


SELECT
    cc.name AS ConstraintName,
    cc.definition
FROM sys.check_constraints cc
JOIN sys.tables t ON cc.parent_object_id = t.object_id
WHERE t.name = 'Reviews';


 

 SELECT
    cc.name AS ConstraintName,
    cc.definition
FROM sys.check_constraints cc
JOIN sys.tables t ON cc.parent_object_id = t.object_id
WHERE t.name = 'Reviews';