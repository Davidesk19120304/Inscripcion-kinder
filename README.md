# Sistema de Inscripción para Kinder

Aplicación de escritorio en C# (Windows Forms) para gestionar la inscripción de alumnos y el registro de docentes de un jardín de infancia. Usa **Guna UI2** para la interfaz, **MySQL** como base de datos y **AForge.NET** para capturar la foto del alumno con la cámara web.

> **Aviso de privacidad:** este repositorio no incluye datos reales de alumnos, docentes ni fotografías de menores. Si usas el sistema con datos reales, cumple con la normativa de protección de datos de tu país.

## Funcionalidades

- **Inicio de sesión** con usuario y contraseña. Las contraseñas se guardan con hash SHA-256.
- **Registro de usuarios** con nombre de usuario, fecha de registro y género.
- **Registro de alumnos:** captura la foto del alumno desde la cámara web y la guarda en la base de datos.
- **Registro de docentes:** guarda cédula, nombre y apellido, y evita duplicados.
- **Búsqueda, actualización y eliminación** de docentes.

## Tecnologías

| Componente | Tecnología |
|---|---|
| Lenguaje | C# |
| Plataforma | .NET Framework 4.6.1 |
| Interfaz | Windows Forms + Guna UI2 2.0.4.6 |
| Base de datos | MySQL (MySql.Data 8.0.13) |
| Cámara | AForge.NET 2.2.5 |
| Entorno | Visual Studio con NuGet |

## Requisitos

- Windows 10 u 11.
- Visual Studio con soporte para .NET Framework 4.6.1 o superior.
- MySQL o MariaDB, por ejemplo a través de XAMPP.
- Una cámara web (solo para el registro de alumnos).

## Instalación

1. **Clona el repositorio:**

   ```bash
   git clone https://github.com/Davidesk19120304/Inscripcion-kinder.git
   ```

2. **Abre la solución** `inscripcion.sln` en Visual Studio.

3. **Restaura los paquetes NuGet:** haz clic derecho en la solución y elige **Restaurar paquetes NuGet**. Esto descarga AForge y Guna UI2.

4. **Crea la base de datos** en MySQL, con el nombre `kinder`:

   ```sql
   CREATE DATABASE kinder CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```

5. **Crea las tablas** que usa la aplicación:

   ```sql
   USE kinder;

   CREATE TABLE login (
       id INT AUTO_INCREMENT PRIMARY KEY,
       fecha VARCHAR(50),
       user VARCHAR(50) NOT NULL UNIQUE,
       password VARCHAR(255) NOT NULL,
       genero VARCHAR(20)
   );

   CREATE TABLE maestros (
       CI_maestro VARCHAR(20) PRIMARY KEY,
       Nombre VARCHAR(100),
       Apellido VARCHAR(100)
   );

   CREATE TABLE ninos (
       id INT AUTO_INCREMENT PRIMARY KEY,
       doc_foto LONGBLOB
   );
   ```

6. **Configura la conexión.** Los datos de conexión están escritos directamente en el código (archivos `Form1.cs`, `Form2.cs`, `registrardocente.cs`, `buscardocente.cs` y `registroalumno.cs`). Cambia `Uid` y `Pwd` por tus credenciales locales antes de ejecutar:

   ```
   Server=localhost;Database=kinder;Uid=TU_USUARIO;Pwd=TU_CONTRASEÑA;
   ```

7. **Ejecuta** el proyecto con **F5**.

## Uso

1. Crea un usuario desde la pantalla de **Registro**.
2. Inicia sesión con ese usuario.
3. Desde el menú principal, registra alumnos, docentes o busca y actualiza docentes.

## Estructura del proyecto

```
Inscripcion-kinder/
├── inscripcion.sln          # Solución de Visual Studio
└── inscripcion/
    ├── Form1.cs             # Registro de usuarios
    ├── Form2.cs             # Inicio de sesión
    ├── Form4.cs             # Menú principal
    ├── registroalumno.cs    # Registro de alumnos con foto desde la cámara
    ├── registrardocente.cs  # Registro de docentes
    ├── buscardocente.cs     # Búsqueda y eliminación de docentes
    ├── actualizar docente.cs# Edición de datos de docentes
    └── Resources/           # Imágenes de la interfaz
```

## Limitaciones conocidas

- Las credenciales de la base de datos están en el código. Lo recomendable es moverlas a `App.config`.
- El hash SHA-256 no usa sal. Lo recomendable es usar BCrypt o PBKDF2.
- Algunos formularios usan la base de datos `kinder` y uno usa `inscripcion`. Conviene unificarlo.
- La aplicación no tiene control de roles: cualquier usuario registrado puede acceder a todas las funciones.

## Próximos pasos

- [ ] Mover la cadena de conexión a `App.config` y usar un usuario de MySQL sin permisos de administrador.
- [ ] Reemplazar el hash SHA-256 por BCrypt o PBKDF2.
- [ ] Unificar el nombre de la base de datos.
- [ ] Agregar control de roles (administrador y docente).
- [ ] Agregar capturas de pantalla del sistema.

## Equipo

Proyecto desarrollado durante el Servicio Comunitario en el [Instituto Universitario Politécnico ¨Santiago Mariño¨]
[GitHub](https://github.com/Davidesk19120304) · [LinkedIn](https://www.linkedin.com/in/guillermo-adrianza-308222426)
