# UI Automated Test with Selenium C#

## Project Overview

This solution is organized as a multi-project structure, serving the purpose of automating UI and API testing for a software system. The project uses Selenium, RestSharp, NUnit, FluentAssertions, and other supporting libraries.

---

## Solution Structure

```
JIRA-013/
│
├── Core/
│   ├── API/
│   ├── Drivers/
│   ├── Elements/
│   ├── Extensions/
│   ├── Reports/
│   ├── ShareData/
│   └── Utilities/
│
├── Service/
│   ├── APIContant.cs
│   ├── ObjectDto/
│   └── Services/
│
├── ProjectTest/
│   ├── Constants/
│   ├── DataProviders/
│   ├── Models/
│   ├── Resources/
│   └── Tests/
│
├── JIRA-013.sln
└── README.md
```

---

## 1. Core

**Purpose:**  
The `Core` project contains shared components, utilities, helper classes, and extension methods used for both UI and API testing.

**Main components:**
- **API/**: Helper classes for API calls and a common API client.
- **Drivers/**: Browser (WebDriver) initialization and configuration for Selenium.
- **Elements/**: Helpers for interacting with UI elements.
- **Extensions/**: Extension methods for RestSharp, String, etc.
- **Utilities/**: Utilities for reading files, handling test data, configuration, etc.
- **Reports/**: Test report generation support.
- **ShareData/**: Shared data between test cases or layers.

---

## 2. Service

**Purpose:**  
The `Service` project acts as the service layer, defining services for API operations, DTOs (Data Transfer Objects), and logic for handling API-related data.

**Main components:**
- **APIContant.cs**: API-related constants (endpoints, headers, etc.).
- **ObjectDto/**: Request/response models for APIs.
- **Services/**: Service classes for API operations.

---

## 3. ProjectTest

**Purpose:**  
The `ProjectTest` project contains all test code, including both UI and API tests.

**Main components:**
- **Constants/**: Constants used in tests.
- **Models/**: Data models for testing.
- **Resources/**: Test data files (JSON, schema, etc.).
- **Tests/**: Test cases (UI, API), test classes, data providers, etc.
- **DataProviders/**: Data for test cases (data-driven testing).

---

## Environment Setup

### Prerequisites

- **.NET SDK 6.0 or later**  
  Download from: https://dotnet.microsoft.com/download

- **Visual Studio 2022** (or later) or Visual Studio Code

- **Chrome browser** (for Selenium WebDriver)

---

### Required NuGet Packages (with Version Recommendations)

Below are the main NuGet packages required for each project, along with recommended versions.  
You can install them using the NuGet Package Manager or with the `dotnet add package` command.

#### Core

- `Selenium.WebDriver` **(v4.20.0)**
- `Selenium.WebDriver.ChromeDriver` **(v124.0.6367.0)**
- `Selenium.Support` **(v4.20.0)**
- `Newtonsoft.Json` **(v13.0.3)**
- `ExtentReports` **(v5.0.9)** *(if reporting is used)*
- `FluentAssertions` **(v6.13.0)**

#### Service

- `RestSharp` **(v110.2.0)**
- `Newtonsoft.Json` **(v13.0.3)**

#### ProjectTest

- `NUnit` **(v3.14.0)**
- `NUnit3TestAdapter` **(v4.5.0)**
- `Microsoft.NET.Test.Sdk` **(v17.10.0)**
- `FluentAssertions` **(v6.13.0)**
- `Selenium.WebDriver` **(v4.20.0)**
- `Selenium.WebDriver.ChromeDriver` **(v124.0.6367.0)**
- `RestSharp` **(v110.2.0)**
- `ExtentReports` **(v5.0.9)** *(if reporting is needed)*

---

#### Example installation commands

```cmd
dotnet add package Selenium.WebDriver --version 4.20.0
dotnet add package Selenium.WebDriver.ChromeDriver --version 124.0.6367.0
dotnet add package Selenium.Support --version 4.20.0
dotnet add package Newtonsoft.Json --version 13.0.3
dotnet add package ExtentReports --version 5.0.9
dotnet add package FluentAssertions --version 6.13.0
dotnet add package RestSharp --version 110.2.0
dotnet add package NUnit --version 3.14.0
dotnet add package NUnit3TestAdapter --version 4.5.0
dotnet add package Microsoft.NET.Test.Sdk --version 17.10.0
```

> Adjust the versions if you need to match your team's compatibility or project requirements.
---

## Build & Run Tests

1. **Build Solution:**  
   Open the solution in Visual Studio and build the entire solution (`Build Solution`), or use the command line:
   ```cmd
   dotnet build
   ```

2. **Run Tests:**  
   - Run tests directly from Visual Studio Test Explorer.
   - Or use the command line to run all tests:
     ```cmd
     dotnet test ProjectTest/ProjectTest.csproj
     ```
   - **Run a specific test class:**
     ```cmd
     dotnet test ProjectTest/ProjectTest.csproj --filter FullyQualifiedName~Namespace.ClassName
     ```
   - **Run a specific test method:**
     ```cmd
     dotnet test ProjectTest/ProjectTest.csproj --filter FullyQualifiedName~Namespace.ClassName.MethodName
     ```
   - **Run tests by category (if using [Category] attribute):**
     ```cmd
     dotnet test ProjectTest/ProjectTest.csproj --filter Category=Smoke
     ```
   - **Combine filters (e.g., category and class):**
     ```cmd
     dotnet test ProjectTest/ProjectTest.csproj --filter "Category=Smoke&FullyQualifiedName~LoginTest"
     ```

> **Tip:**  
> Replace `Namespace.ClassName` and `MethodName` with the actual namespace, class, and method names in your project.

---

## Contact

- Developer: [Tin Nguyen]
