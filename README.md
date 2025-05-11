# Gestor de Inventarios Asocebú

¡Bienvenido al sistema de gestión de inventario de equipos de cómputo para Asocebú!

Este proyecto es una aplicación de escritorio desarrollada en **VB.NET (Windows Forms)** con acceso a base de datos **SQL Server**. Permite gestionar equipos y departamentos, realizar operaciones CRUD, aplicar filtros avanzados, exportar datos y mucho más.

---

## 🚀 ¿Cómo empezar?

### 1. Clona el repositorio

```bash
git clone https://github.com/SMPages/GestorInventarios.git
```

---

### 2. Restaura la base de datos

- En la carpeta `/Database` encontrarás el script `CreateDatabase.sql`.
- Ábrelo con SQL Server Management Studio y ejecútalo para crear la base de datos, tablas y registros de ejemplo.

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

- Abre la solución `GestorInventarios.sln` en Visual Studio (recomendado VS 2019 o superior).
- Selecciona la configuración **Release**.
- Compila la solución (`Ctrl+Shift+B`).
- El ejecutable estará en `/bin/Release/GestorInventarios.UI.exe`.
- Haz doble clic en el `.exe` para iniciar la aplicación.

---

### 5. ¿Qué incluye la aplicación?

- **CRUD de Equipos y Departamentos**
- **Filtros avanzados** por tipo, usuario, estado y departamento
- **Exportación a Excel y PDF**
- **Dashboard visual** con resumen de datos
- **Validaciones robustas** (serial único, máximo 2 equipos por usuario, etc.)
- **Interfaz alineada con la identidad visual de Asocebú**

---

### 6. ¿Tienes problemas o dudas?

- Revisa que tu SQL Server esté activo y la cadena de conexión sea correcta.
- Si tienes algún error, revisa los mensajes en pantalla o contacta al desarrollador.

---

## 📧 Contacto

Si tienes preguntas, sugerencias o necesitas soporte, puedes escribir a:

- **Correo:** asocebu@asocebu.com
- **Sitio web:** [https://www.asocebu.com](https://www.asocebu.com)

¡Gracias por probar el Gestor de Inventarios Asocebú! 