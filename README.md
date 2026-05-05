# Inge2_Centro_Deportivo

Trabajo en grupo de la materia ingeniería de software 2

## Integrantes

- Bautista Aguilar
- Gabriel Aguirre
- Hugo Contrera
- Nelson Joaquin Piñanelli
- Adrián Alejandro Sambido

---

## 🚀 Cómo ejecutar el proyecto

Este proyecto está desarrollado en **.NET 8** y utiliza una arquitectura en capas.

### 📋 Requisitos previos

* Tener instalado el **.NET SDK 8 o superior**
* Verificar instalación:

```bash
dotnet --version
```

---

### 📥 Clonar el repositorio

```bash
git clone https://github.com/AdrianASambido/Inge2_Centro_Deportivo.git
cd Inge2_Centro_Deportivo
```

---

### 🔧 Restaurar dependencias

Antes de ejecutar el proyecto, es necesario descargar las dependencias:

```bash
dotnet restore
```

---

### ▶️ Ejecutar la aplicación

El proyecto ejecutable es:

```
CentroDeportivo.UI
```

Podés ejecutarlo de dos formas:

#### Opción 1: Desde la carpeta del proyecto

```bash
cd CentroDeportivo.UI
dotnet run
```

#### Opción 2: Desde la raíz del repositorio

```bash
dotnet run --project CentroDeportivo.UI
```

---

### 🏗️ Compilar el proyecto (opcional)

```bash
dotnet build
```

---

### 📁 Estructura del proyecto

* `CentroDeportivo.Aplicacion` → lógica de negocio
* `CentroDeportivo.Infraestructura` → acceso a datos
* `CentroDeportivo.UI` → interfaz / punto de entrada (ejecutable)

---

### ⚠️ Notas importantes

* Las carpetas `bin/` y `obj/` no están versionadas (se generan automáticamente)
* Archivos temporales de base de datos (`*.db-wal`, `*.db-shm`) están ignorados
* Si hay problemas al ejecutar, asegurarse de haber corrido `dotnet restore`

---
