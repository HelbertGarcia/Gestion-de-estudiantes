# Informe de Práctica - Gestión de Estudiantes

## 1. Introducción
Este proyecto implementa un sistema en memoria para que los docentes gestionen sus estudiantes (Presenciales y Virtuales) y calificaciones, cumpliendo con los requerimientos académicos planteados.

## 2. Estructura y Diseño OO
- **Herencia**: Las clases `PresentialStudent` y `VirtualStudent` heredan de la clase abstracta `Student`, la cual extiende de `Person`.
- **Polimorfismo**: El cálculo de la nota final se sobreescribe de forma polimórfica según la modalidad:
  - **Presencial**: Examen (70 pts) + Teoría (10 pts) + Práctica (20 pts).
  - **A Distancia**: Examen Virtual (60 pts) + Trabajo Práctico (40 pts).
- **Manejo de Errores**: Se utiliza la clase contenedora `OperationResult` (`message`, `success`, `data`) para transferir respuestas validadas entre los servicios y la consola.

## 3. Estructura del Proyecto
- `Entities/`: Clases de dominio (`Student`, `Teacher`, `Group`, `Grade`, etc.).
- `Enums/`: Tipos definidos (`StudentType`).
- `Services/`: Lógica de control de datos (`UniversityService`) y menús de consola (`ConsoleMenu`).
- `Data/`: Inicialización de datos semilla en memoria (`AddingEntities`).

## 4. Conclusión
La separación de responsabilidades y el uso de polimorfismo permiten una estructura limpia, extensible y libre de acoplamientos rígidos con bases de datos.
