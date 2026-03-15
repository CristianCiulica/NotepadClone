# Notepad++ Clone (C# / WPF)

A clone of the popular text editor Notepad++, built from scratch using **C#** and **WPF** (Windows Presentation Foundation). 

This project was developed strictly adhering to the **MVVM (Model-View-ViewModel)** design pattern, ensuring a complete separation of concerns between the user interface (XAML) and the business logic (C#), keeping the *code-behind* entirely clean.

## Implemented Features

### 1. File Management (Tabs)
* **Multiple Tabs:** Open, edit, and navigate through multiple files simultaneously.
* **Smart Saving:** Full support for *New*, *Open*, *Save*, *Save As*, *Close*, and *Close All* operations.
* **Unsaved Changes Tracking:** Modified files automatically display a `*` marker in the tab title.
* **Warning System:** Safety prompts prevent accidental data loss when closing unsaved files or tabs ("Do you want to save changes?").

### 2. Folder Explorer (Directory Tree)
* Tree-based navigation through all system drives and folders.
* **Lazy Loading:** Folder contents are only read from the disk when the user expands them, ensuring instant performance without UI freezes.
* **Double-Click Integration:** Quickly open any text file from the explorer tree directly into a new tab.
* **Context Menu (Right-Click on folders):**
  * *New file:* Instantly creates a new text file within the selected directory.
  * *Copy path:* Copies the absolute path of the folder to the clipboard.
  * *Copy / Paste folder:* Recursively copies an entire directory (including all nested files and subfolders) to another location.

### 3. Find and Replace (Search)
* Dedicated, floating search window for *Find*, *Replace*, and *Replace All* actions.
* Scope selection: Apply searches and replacements to the **Current Tab** only, or simultaneously across **All Tabs**.

### 4. User Interface & Experience (UI/UX)
* Modern, "flat" design aesthetic inspired by contemporary code editors.
* **View Menu:** Toggle the visibility of the side directory tree (*Standard Mode* vs. *Folder Explorer Mode*).
* Convenient top toolbar providing quick shortcuts for frequent file operations.

##  Architecture & Technologies
* **Framework:** .NET / WPF
* **Design Pattern:** MVVM (Model-View-ViewModel)
* **Data Binding:** Extensive use of data bindings (`INotifyPropertyChanged`, `ObservableCollection`) to automatically synchronize the UI with the underlying data state.
* **Commands:** Button interactions are routed through `ICommand` (`RelayCommand`), completely bypassing traditional event handlers in the UI code-behind.


##  How to Run the Project
1. Clone this repository: `git clone https://github.com/your-username/NotepadPlus.git`
2. Open the `.sln` file in **Visual Studio**.
3. Press `F5` or click the **Start** button to build and run the application.
