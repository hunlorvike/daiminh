# JavaScript Modules Documentation

## Overview

This JavaScript codebase has been refactored to follow SOLID, DRY, KISS, and self-explanatory code principles. The
architecture uses a modular pattern with clear separation of concerns.

## Module Structure

The application is structured into the following modules:

1. **Daiminh.Core** - Foundation module containing shared configurations and utilities
2. **Daiminh.DataTable** - Handles DataTable initialization and styling
3. **Daiminh.Form** - Manages form submission and validation
4. **Daiminh.Modal** - Controls modal dialog functionality
5. **Daiminh.Init** - Application initialization and bootstrapping

### Dependencies

- jQuery
- Bootstrap (for modals)
- DataTables

## Usage Guide

### File Inclusion Order

For proper functioning, include these files in the correct order:

```html
<!-- Dependencies -->
<script src="jquery.min.js"></script>
<script src="bootstrap.bundle.min.js"></script>
<script src="jquery.dataTables.min.js"></script>

<!-- Daiminh modules -->
<script src="Daiminh.Core.js"></script>
<script src="Daiminh.DataTable.js"></script>
<script src="Daiminh.Form.js"></script>
<script src="Daiminh.Modal.js"></script>
<script src="Daiminh.Main.js"></script>
```

### DataTable Usage

Initialize a standard DataTable:

```javascript
// Main table is automatically initialized
const mainTable = Daiminh.Init.getInstances().dataTables.main;

// Custom DataTable initialization
const customTable = Daiminh.DataTable.initialize('#custom-table', {
    pageLength: 25,
    ordering: false
    // Other custom options
});
```

### Form Handling

Form handling is automatic for modals. For manual form submission:

```javascript
// Submit a form manually
Daiminh.Form.submit('#my-form');

// Check if a form is valid
if (Daiminh.Form.validate('#my-form')) {
    // Form is valid
}
```

### Modal Operations

Modals are automatically initialized. For manual modal operations:

```javascript
// Show a modal programmatically
Daiminh.Modal.show('#exampleModal');

// Hide a modal programmatically
Daiminh.Modal.hide('#exampleModal');
```

## Design Principles Applied

### SOLID Principles

- **Single Responsibility**: Each module has a clear, focused purpose
- **Open/Closed**: Modules can be extended without modification through customization options
- **Interface Segregation**: Public APIs provide only necessary methods
- **Dependency Inversion**: Higher-level modules don't depend on lower-level modules directly

### DRY (Don't Repeat Yourself)

- Common utilities centralized in `Daiminh.Core.Utils`
- Shared configuration in `Daiminh.Config`
- Reusable helper functions like `_setupModalAction` and `disableButton`

### KISS (Keep It Simple, Stupid)

- Straightforward API methods with clear naming
- Consistent patterns across modules
- Self-documenting code with meaningful variable and function names

### Self-Explanatory Code

- Clear naming conventions
- JSDoc comments for all functions and modules
- Logical organization of related functionality

## Configuration

All configurations are centralized in `Daiminh.Config` and can be modified as needed:

```javascript
// Example: Change default page length for DataTables
Daiminh.Config.defaults.dataTable.pageLength = 25;

// Example: Change selector for modal close button
Daiminh.Config.selectors.modalClose = '.my-modal-close';
```

## Error Handling

Consistent error handling throughout the application:

- AJAX errors are logged with context
- Modal errors include the modal type
- Console messages help with debugging