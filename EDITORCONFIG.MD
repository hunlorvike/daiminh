## Configure Your IDEs/Editors to Use .editorconfig

### Visual Studio (2017 and Later)

- **Built-In Support:**  
  Visual Studio natively supports **.editorconfig**. When you open a project that contains an **.editorconfig** file, it
  automatically applies the rules.

- **Verify Settings:**  
  To check or override settings manually:
    1. Go to **Tools > Options > Text Editor > C# > Code Style** (or your target language).
    2. Scroll down; you’ll see a note indicating that settings from **.editorconfig** are in effect.

- **Tip:**  
  If you ever want to ignore the **.editorconfig** file temporarily, you can disable it by changing your settings—but
  it’s best to keep it enabled for consistency.

---

### Visual Studio Code

- **Native or Extension:**  
  VS Code has native support for **.editorconfig**. However, installing
  the [EditorConfig for VS Code extension](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig)
  can provide extra features and ensure compatibility.

- **Steps:**
    1. **Install the Extension (if not already installed):**
        - Open the Extensions view (`Ctrl+Shift+X` or `Cmd+Shift+X` on macOS) and search for “EditorConfig”.
    2. **Place .editorconfig in the Project Root:**
        - VS Code will automatically detect and apply the formatting rules defined in the file.
    3. **Confirm Operation:**
        - Open a file and try reformatting (or simply save, if you’ve set your editor to format on save). You should see
          the formatting reflect your **.editorconfig** rules.

- **Extra Configuration:**  
  You can combine this with VS Code’s settings (via the JSON or UI settings) like enabling `"editor.formatOnSave": true`
  to auto-format code on save.

---

### JetBrains Rider

- **Built-In Support:**  
  Rider natively supports **.editorconfig** files. When you open a project that contains one, Rider reads and applies
  the defined rules.

- **Steps:**
    1. **Open Your Project:**
        - Rider automatically detects the **.editorconfig** file in your project root.
    2. **Verify in Settings:**
        - Go to **File > Settings (or Preferences on macOS) > Editor > Code Style**.
        - Rider shows a banner if an **.editorconfig** file is active. You can click the “Enable EditorConfig support”
          option if it isn’t already enabled.
    3. **Test It Out:**
        - Edit a file and use Rider’s auto-format (usually via `Ctrl+Alt+L` or `Cmd+Alt+L` on macOS) to see that your
          formatting rules are applied.

- **Tip:**  
  If you previously imported custom settings from Visual Studio or VS Code, you can remove or reset them to let the *
  *.editorconfig** file be the single source of truth.
