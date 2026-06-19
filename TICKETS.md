# Project Tickets

This file contains actionable tasks to improve the Classroom Library application.

## Status

- `[ ]` Open
- `[~]` In progress
- `[x]` Done

## 🟢 Priority: Low (Good for Beginners)

### [x] L1. Move `CleanInputString` to Shared Utility
- **Description**: The `CleanInputString` method in `AddStudentDialog.axaml.cs` is useful for all dialogs. Move it to a static utility class (e.g., `ClassroomLibrary.Utils.StringUtils`).
- **Files**: `Views/Dialogs/AddStudentDialog.axaml.cs`
- **Goal**: Reduce code duplication and follow the TODO in the file.

### [x] L2. Improve `AddBookDialog` Validation
- **Description**: Ensure that when adding a book, the Title and Author are not empty (currently, they might allow whitespace or empty strings).
- **Files**: `Views/Dialogs/AddBookDialog.axaml.cs`
- **Goal**: Prevent invalid data from entering the library.

### [ ] 3. Add "Clear Search" Button
- **Description**: Add a small button to clear the search bar in the Books or Students view.
- **Files**: `Views/BooksView.axaml`, `Views/StudentsView.axaml`
- **Goal**: Improve UX for clearing filters.

---

## 🟡 Priority: Medium (Functional Improvements)

### [ ] M1. Search and Filter Books
- **Description**: Implement a search bar in the Books view to filter by title, author, or genre.
- **Files**: `ViewModels/BooksViewModel.cs`, `Views/BooksView.axaml`
- **Goal**: Make it easier to find books as the library grows.

### [ ] M2. Prevent Deleting Student with Active Checkouts
- **Description**: If a student has books checked out, show a warning or prevent deletion until books are returned.
- **Files**: `Services/LibraryService.cs`, `ViewModels/StudentsViewModel.cs`
- **Goal**: Maintain data integrity.

### [ ] M3. Mark Overdue Checkouts in UI
- **Description**: Display overdue checkouts in a different color (e.g., Red) in the Checkouts view.
- **Files**: `Views/CheckoutsView.axaml`, `Models/Checkout.cs`
- **Goal**: Help the teacher quickly identify late books.

### [x] M4. Separate Student First and Last Names
- **Description**: Separate the first and last names of students in the Students view for better readability.
- **Files**: `Views/StudentsView.axaml`, `Models/Student.cs`
- **Goal**: Improve the user experience by making it easier to read and understand student names.

### [x] M5. Separate Author First and Last Names
- **Description**: Separate authors' first and last names in the Books view for consistency with student names.
- **Files**: `Views/BooksView.axaml`, `Views/Dialogs/AddBookDialog.axaml`, `Views/Dialogs/AddBookDialog.axaml.cs`, `Models/Book.cs`
- **Goal**: Keep name entry and display consistent across students and authors.

### [X] M6. Uniquify all instances of books, regardless of same title and author
- **Description**: Every book is a physical object, and each of those physical objects can be checked out by a student.
We need to be able to tell when a student is checking out a particular instances of a book. So no need for copy number, jsut separate instances.
- **Files**: idk 
- **Goal**: Uniquifiy all instances of books. Perhaps provide a "rolled up" view when books happen to have the same fields otherwise.

### [] M7. Remove All traces of legacy support
- **Description**: Till present, when making changes to "db", we have added support for legacy data. But The app is nowhere near close to being production ready. Just delete the legacy support and move to the new formats.
- **Files**: idk
- **Goal**: Remove legacy workaround support.

---

## 🔴 Priority: High (Core Features)

### [ ] H1. Export Library to CSV
- **Description**: Add a feature to export the list of books and their status to a CSV file for backup or printing.
- **Files**: `Services/LibraryService.cs`, `ViewModels/BooksViewModel.cs`
- **Goal**: Allow the user to use their data outside the app.

### [ ] H2. Book History View
- **Description**: When selecting a book, show a list of students who have checked it out in the past.
- **Files**: `ViewModels/BooksViewModel.cs`, `Views/BooksView.axaml`
- **Goal**: Track the "life" of a book.

### [ ] H3. Bulk Import Students
- **Description**: Allow importing a list of students from a text file or CSV (Name, Grade, Teacher).
- **Files**: `Services/LibraryService.cs`, `Views/Dialogs/AddStudentDialog.axaml`
- **Goal**: Save time at the beginning of the school year.

---

## 🟣 Priority: Indefinite (Nice to Haves)

### [x] I1. Give the Menu Bar App a Name
- **Description**: Rename Menu Bar App menu from "Avalonia Application" to "Classroom Library"
- **Files**: `App.axaml`
- **Goal**: Reflect the purpose of the application and make it seem less generic.

### [x] I2. Factor out color schemes to a file and implement "light and dark mode"
- **Description**: App currently follows system color scheme. Can we implement something to allow it to define its own color scheme(s)?"
- **Files**: `Themes/ClassroomTheme.axaml`, `Services/ThemeService.cs`, `Views/SettingsView.axaml`
- **Goal**: Beautify the app and setup framework to allow for future theming. 

## ⭕️ Priority: Bug Fixes

### [ ] B1. Book or student deletion works on multiple selections
- **Description**: When 2 or more books or students are selected, only a single one is deleted with delete button
- **Files**: IDK
- **Goal**: allow multiple selections to be deleted

### [ ] B2. Sometimes selectd Book or Students are not deleted
- **Description**: May have to do with legacy books having multiple copies, but deleting a selection does not seem to delete that instance
- **Files**: IDK
- **Goal**: Fix a bug

