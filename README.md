# Gestor de Inventarios Asocebú

¡Hola! Este es un sistema de gestión de inventario de equipos de cómputo desarrollado especialmente para la prueba técnica de Asocebú.

La aplicación está hecha en **VB.NET (Windows Forms)** y utiliza **SQL Server** como base de datos. Permite administrar equipos y departamentos, realizar búsquedas, aplicar filtros, exportar datos y mucho más, todo con una interfaz amigable y moderna.

---

## 🚀 ¿Cómo empezar?

### 1. Clona el repositorio

```bash
git clone https://github.com/SMPages/GestorInventarios.git
```

---

### 2. Restaura la base de datos

- Dentro de la carpeta `/Database` encontrarás el script `CreateDatabase.sql`.
- Ábrelo con SQL Server Management Studio y ejecútalo para crear la base de datos, las tablas y algunos registros de ejemplo.

---

### 3. Configura la cadena de conexión

- Abre el archivo `App.config` en el proyecto principal.
- Busca la sección `<connectionStrings>` y edita el valor de `Data Source` para que apunte a tu servidor SQL.

Ejemplo:
```xml
<connectionStrings>
  <add name="DefaultConnection" connectionString="Data Source=TU_SERVIDOR;Initial Catalog=GestorInventarios;Integrated Security=True"/>
</connectionStrings>
```

- Si usas autenticación SQL, reemplaza `Integrated Security=True` por `User ID=usuario;Password=contraseña`.

---

### 4. Compila y ejecuta la aplicación

- Abre la solución `GestorInventarios.sln` en Visual Studio (VS 2019 o superior recomendado).
- Selecciona la configuración **Release**.
- Compila la solución (`Ctrl+Shift+B`).
- El ejecutable estará en `/bin/Release/GestorInventarios.UI.exe`.
- Haz doble clic en el `.exe` para iniciar la aplicación.

---

### 5. ¿Qué puedes hacer con la app?

- Crear, editar, consultar y desactivar equipos y departamentos
- Filtrar equipos por tipo, usuario, estado y departamento
- Exportar los datos a Excel y PDF
- Ver un dashboard visual con los totales principales
- Validaciones: serial único, máximo 2 equipos por usuario, campos obligatorios, etc.
- Interfaz inspirada en la identidad visual de Asocebú

---

### 6. ¿Dudas o problemas?

- Asegúrate de que tu SQL Server esté activo y la cadena de conexión sea correcta.
- Si tienes algún error, revisa los mensajes en pantalla o escríbeme.

---

### 7. Ejecutable listo para probar

- El archivo ejecutable compilado (`GestorInventarios.UI.exe`) está disponible en la carpeta `/dist` del repositorio.
- Solo necesitas configurar la cadena de conexión en `App.config` y hacer doble clic en el `.exe` para probar la aplicación sin necesidad de compilar.

---

## 📧 Contacto del desarrollador

- **Correo:** sebastianmarciales40@gmail.com

### Nota importante sobre la cadena de conexión

Si solo vas a ejecutar el programa desde el archivo `.exe` (sin abrir Visual Studio), **debes editar el archivo `GestorInventarios.UI.exe.config`** que está en la misma carpeta que el ejecutable. 

Abre ese archivo con un editor de texto, busca la sección `<connectionStrings>` y ajusta la cadena de conexión según tu servidor SQL. Guarda los cambios y ejecuta el programa normalmente.