# PAW – Programación Avanzada en Web (SC-701)

Proyecto en **ASP.NET Core 8** con arquitectura en capas: una **API REST** que accede a SQL Server con Entity Framework Core (database-first) y una **aplicación web MVC** que consume la API mediante servicios.

## Integrantes

- Nancy Sofía Gutiérrez Jarquín

## Tareas implementadas

| Tarea | Descripción |
|---|---|
| **Tarea 1 y 2** | Mismo flujo de `Product` replicado para todas las entidades: Repository, API Controller, Entities y DTO, Service, MVC Controller y Views |
| **Tarea 3** | Edit, Details, Add y Delete funcionando en todas las pantallas. Details en *Partial Views* reutilizables (modal, sin cargar una pantalla nueva). Paginación máxima de 25 por página |

Además, todas las entidades tienen acceso desde el menú **Entities** del Layout.

### Entidades

| Catálogo | Seguridad | General |
|---|---|---|
| Product | User | PawTask (Task) |
| Category | Role | Notification |
| Inventory | UserRole | |
| Supplier | UserAction | |
| Component | | |

## Arquitectura

```
PAW.sln
├── PAW.Models          Entidades (scaffold de la BD) y DTOs
├── PAW.DataAccess      DbContext (EF Core) y repositorios
├── PAW.Architecture    RestProvider, JsonProvider y excepciones comunes
├── PAW.API             API REST (controllers que usan los repositorios)
└── PAW.Web             MVC (controllers, servicios, vistas y partial views)
```

Flujo de una petición:

```
Vista (Razor)  →  MVC Controller  →  Service  →  RestProvider (HTTP)
                                                      │
                                                      ▼
                              API Controller  →  Repository  →  SQL Server
```

- La **Web no se conecta a la base de datos**: solo la API lo hace.
- Los **DTO** (`ConvertFrom`, `ConvertTo`, `ApplyTo`) separan el modelo de la BD del que viaja por HTTP.

## Funcionalidades

- **CRUD completo** de las 11 entidades (Create, Edit, Details, Delete).
- **Details en Partial Views** reutilizables, mostrados en un modal:
  - Product: muestra el **inventario**, la **categoría** y el **supplier** asociados.
  - Category, Supplier e Inventory: muestran sus datos y los **productos asociados**.
- **Paginación** (máximo 25 por página) con el formato:

  ```
  «  ‹  …  5  [6]  7  …  ›  »
  ```

  Primera, anterior, `…`, página anterior, **actual**, siguiente, `…`, siguiente y última.
- **Delete** con confirmación. Si el registro tiene datos relacionados, muestra un mensaje en lugar de fallar.
- Mensajes de éxito y error en el Layout.
- Menú **Entities** con todas las pantallas, agrupadas en Catalog, Security y General.


## Ejecución

| Proyecto | URL |
|---|---|
| API | https://localhost:7038 |
| Swagger | https://localhost:7038/swagger |
| Web | https://localhost:7076 |

## Endpoints del API

Todas las entidades siguen el mismo patrón (ejemplo con `Category`):

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/Category` | Lista todos los registros |
| GET | `/Category/{id}` | Obtiene un registro |
| POST | `/Category` | Crea un registro |
| PUT | `/Category/{id}` | Actualiza un registro |
| DELETE | `/Category/{id}` | Elimina un registro |

Controllers disponibles: `Product`, `Category`, `Inventory`, `Supplier`, `Component`, `User`, `Role`, `UserRole`, `UserAction`, `PawTask`, `Notification`.

## Notas técnicas

- `UserAction` y `UserRole` no tienen llave primaria en la base de datos. Se usa `Id` como llave lógica en EF para poder editarlas y eliminarlas, y su `Id` se digita manualmente.
- `UserDTO` recibe una contraseña en texto y la guarda como hash SHA-256. Nunca se devuelve en las respuestas.
- La Web fija la cultura `en-US` para interpretar bien los decimales de los formularios.
- `RestProviderHelpers` agrega una `/` final al `BaseAddress` para que las rutas con id (`/Product/5`) se resuelvan correctamente.

## Estructura de las vistas

```
PAW.Web/Views
├── <Entidad>/
│   ├── Index.cshtml                 Listado paginado
│   ├── Create.cshtml / Edit.cshtml  Formularios (usan _Form)
│   ├── _Form.cshtml                 Campos del formulario
│   └── _<Entidad>Details.cshtml     Partial view de detalle
└── Shared/
    ├── _Layout.cshtml               Menú Entities y alertas
    ├── _Pagination.cshtml           Paginación reutilizable
    ├── _DetailsModal.cshtml         Modal donde se cargan los detalles
    └── _DeleteForm.cshtml           Formulario oculto para eliminar
```

El comportamiento de Details y Delete está en `PAW.Web/wwwroot/js/site.js`.

## Autor

- Nancy Sofía Gutiérrez Jarquín
Curso **SC-701 – Programación Avanzada en Web**
