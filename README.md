# Readme del Proyecto

Proyecto: CineStreamCR

## Integrantes finales del grupo

- Méndez González Javier
- Guatemala Camacho Angelik
- Pasos Solano Keisly
- Alvarado Fernández Maria José

## Enlace del repositorio

https://github.com/Alvarado-Majo/ProyectoPrograAvanzadaWeb

## Especificación básica del proyecto

1) Arquitectura del proyecto

- Solución con arquitectura en capas:
  - CineStreamCR (Web): Aplicación ASP.NET Core (TargetFramework: net10.0). Contiene controladores, vistas y recursos estáticos (Controllers, Views, wwwroot).
  - CineStreamCR.BLL (Business Logic Layer): Biblioteca de clases con DTOs y servicios (servicio por entidad) que encapsula la lógica de negocio.
  - CineStreamCR.DAL (Data Access Layer): Biblioteca de clases con Entity Framework Core, DbContext, entidades y repositorios para el acceso a datos.

2) Librerías / paquetes NuGet utilizados

- CineStreamCR.BLL:
  - AutoMapper (v16.1.1)
- CineStreamCR.DAL:
  - Microsoft.EntityFrameworkCore (v10.0.9)
  - Microsoft.EntityFrameworkCore.Abstractions (v10.0.9)
  - Microsoft.EntityFrameworkCore.Design (v10.0.9)
  - Microsoft.EntityFrameworkCore.SqlServer (v10.0.9)
  - Microsoft.EntityFrameworkCore.Tools (v10.0.9)
  - Microsoft.Extensions.Configuration.UserSecrets (v6.0.1)

3) Principios SOLID aplicados (y justificación)

- Single Responsibility (SRP): Cada repositorio (DAL) tiene la responsabilidad única de acceso a datos para una entidad; cada servicio (BLL) encapsula la lógica de negocio de una entidad concreta. Ej.: IActorRepository/ActorRepository vs IActorService/ActorService.
- Open/Closed (OCP): Las abstracciones (interfaces) permiten extender el comportamiento creando nuevas implementaciones sin modificar las existentes (por ejemplo, nuevas implementaciones de repositorios o servicios).
- Liskov Substitution (LSP): Las implementaciones de interfaces respetan los contratos definidos, permitiendo sustituir implementaciones concretas por sus interfaces en DI.
- Interface Segregation (ISP): Se exponen interfaces específicas por área (IUserRepository, IMovieRepository, IReviewRepository) en lugar de una interfaz monolítica.
- Dependency Inversion (DIP): El proyecto Web depende de abstracciones (interfaces) y usa inyección de dependencias en Program.cs para registrar repositorios y servicios.

4) Patrones de diseño utilizados y en qué parte del código

- Repository Pattern: Implementado en CineStreamCR.DAL/Repositories (por ejemplo: ActorRepository, MovieRepository). Centraliza y abstrae el acceso a la base de datos.
- DTO (Data Transfer Object): En CineStreamCR.BLL/DTO — separa las entidades de dominio de los modelos usados en la UI o en la capa de servicios.
- Service Layer / Facade: CineStreamCR.BLL/Services agrupa operaciones de negocio y ofrece una API clara al proyecto Web.
- Dependency Injection: Uso del contenedor DI de ASP.NET Core (Program.cs) para registrar repositorios y servicios (AddScoped).
- AutoMapper: Mapeo entre entidades y DTOs (ClassMapping y configuración en Program.cs).
- (Implícito) Unit of Work: EF Core DbContext (ProyectoDBContext) actúa como unidad de trabajo para las transacciones y el seguimiento de cambios.

5) Decisiones de diseño de base de datos

- Motor y acceso: SQL Server (Microsoft.EntityFrameworkCore.SqlServer). La cadena de conexión se configura en appsettings.json y se inyecta en Program.cs.
- Modelo relacional: Entidades principales (Movies, Actors, Directors, Users, Categories, Reviews, WatchLists).
- Relaciones muchos-a-muchos: Implementadas mediante tablas intermedias explícitas:
  - MovieActors (MovieId, ActorId) — relación Movie <-> Actor
  - MovieCategories (MovieId, CategoryId) — relación Movie <-> Category
  - MovieDirectors (MovieId, DirectorId) — relación Movie <-> Director
  - WatchListMovies (WatchListId, MovieId) — relación WatchList <-> Movie
- Restricciones e índices:
  - Usuarios: Email con índice único.
  - Categories: Name con índice único.
  - Llaves primarias compuestas para tablas de relación (p.ej. MovieActors tiene clave {MovieId, ActorId}).
- Valores por defecto y tipos:
  - Campos IsActive con valor por defecto (activo=1).
  - Timestamps: CreatedAt con GETDATE() por defecto en Movies; SignUpDate con GETDATE() en Users.

