# Gestión de Estudiantes - Universidad

Este proyecto es una aplicación de consola en C# (.NET 10.0) desarrollada para que los docentes universitarios gestionen sus asignaturas, grupos, estudiantes y calificaciones en memoria.

## Características

- **POO y Polimorfismo**: Clasificación de estudiantes en *Presenciales* y *A Distancia* (Virtuales) con cálculo de notas adaptado polimórficamente según su modalidad:
  - **Presenciales**: Examen (70%), Teoría/Participación (10%), Práctica (20%).
  - **A Distancia**: Examen Virtual (60%), Trabajo Práctico (40%).
- **Estandarización de Operaciones**: Respuestas encapsuladas en un modelo `OperationResult` (`message`, `success`, `data`).
- **Datos Semilla**: Inicialización automática con datos de un docente, asignaturas, grupos y estudiantes precargados.
- **Interfaz de Consola Minimalista**: Visualización tabular y cálculo instantáneo de métricas grupales (% de alumnos aprobados).

## Cómo Ejecutar el Proyecto

Ejecuta el siguiente comando en la terminal desde el directorio raíz:

```powershell
dotnet run --project GestionEstudiantes
```
