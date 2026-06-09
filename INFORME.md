# Informe de Práctica - Gestión de Estudiantes

## 1. Introducción y Planteamiento del Problema
Este proyecto implementa una solución de software orientada a objetos para que los docentes universitarios gestionen sus materias, grupos e inscripciones. Al inicio de cada periodo académico, el sistema inicializa una lista de estudiantes inscritos. El sistema permite:
- Registrar nuevos estudiantes interactivos a asignaturas específicas.
- Calificar exámenes y prácticas según la modalidad del estudiante.
- Generar listados tabulares con las notas de cada estudiante.
- Calcular el porcentaje de estudiantes aprobados (calificación final $\ge 70$) por grupo.

## 2. Estructura y Diseño Orientado a Objetos (OO)

### Herencia y Abstracción
- **`Person` (Clase Base Abstracta)**: Encapsula el identificador único `Id` (`Guid`) y el nombre completo del individuo `FullName`.
- **`Student` (Clase Abstracta)**: Hereda de `Person` y agrega la lista de asignaturas del estudiante (`Subjects`) y el tipo de estudiante (`StudentType`). Declara el método abstracto de cálculo de notas.
- **`Teacher` (Clase Concreta)**: Hereda de `Person` y administra las colecciones de asignaturas y grupos asignados.

### Polimorfismo
El cálculo de la calificación final y la visualización de los datos se resuelven dinámicamente mediante la sobreescritura de métodos:
- **`PresentialStudent`**: Sobreescribe `CalculateFinalGrade` sumando Examen (máx. 70), Puntos Teóricos/Participación (máx. 10) y Puntos Prácticos (máx. 20). Su representación gráfica es `[Presencial] Nombre`.
- **`VirtualStudent`**: Sobreescribe `CalculateFinalGrade` sumando Examen Virtual (máx. 60) y Trabajo Práctico (máx. 40). Los puntos teóricos no aplican a esta modalidad. Su representación gráfica es `[A distancia] Nombre`.

### Validación de Reglas de Negocio y Errores
Se implementa una clase estandarizada `OperationResult` que encapsula la respuesta del sistema:
- `success` (indica si la operación fue exitosa o falló).
- `message` (el detalle informativo o descripción del error).
- `data` (el payload de información asociada).

Las validaciones garantizan que no se asignen calificaciones fuera de los rangos válidos establecidos para cada tipo de alumno, previniendo estados inconsistentes en memoria.

## 3. Arquitectura del Proyecto
La solución se organiza en las siguientes carpetas dentro de la solución:
- **`Entities/Base/`**: Contiene clases abstractas base (`Person.cs` y `Student.cs`).
- **`Entities/`**: Define entidades concretas de negocio (`PresentialStudent.cs`, `VirtualStudent.cs`, `Teacher.cs`, `Subject.cs`, `Group.cs`, `Grade.cs`).
- **`Enums/`**: Define el enumerador `StudentType.cs`.
- **`Services/`**: Contiene el control de errores `OperationResult.cs`, la lógica en memoria `UniversityService.cs` y el menú en consola `ConsoleMenu.cs`.
- **`Data/`**: Contiene `AddingEntities.cs` encargado de inicializar el mock de datos predeterminados en memoria.

## 4. Métricas de Aprobación
El cálculo de aprobación se realiza en tiempo real en la capa de servicios:
$$\text{Porcentaje de Aprobados} = \left( \frac{\text{Estudiantes con Nota Final} \ge 70}{\text{Total de Estudiantes en el Grupo}} \right) \times 100$$
Esto permite al docente evaluar el desempeño académico global de forma ágil y visual a través del panel de estadísticas en consola.
