# Blizzard UI Automation Framework

This is a robust UI Test Automation Framework built using **Selenium WebDriver + C#**, designed for testing the [Blizzard Entertainment website](https://www.blizzard.com/en-us/). The framework follows best practices including:

* ✅ Page Object Model (POM)
* ✅ Dependency Injection (via Microsoft DI)
* ✅ Config-driven cross-browser support (Chrome, Firefox, Edge)
* ✅ Headless mode support
* ✅ Extent Reports integration (with screenshots)
* ✅ Docker-ready & CI/CD integrable

---

## 📁 Project Structure

```
BlizzardFramework/
│
├── Pages/                   → Page Object Models
│   └── HomePage.cs
│
├── Drivers/                 → WebDriver factory using DI
│   └── WebDriverFactory.cs
│
├── Utilities/               → Config, Base Test, Reporting, Helpers
│   └── TestFixture.cs
│   └── ExtentReportManager.cs
│
├── Tests/                   → Test Classes
│   └── HomePageTests.cs
│
├── appsettings.json         → Configuration (browser, headless, expected UI)
├── Dockerfile               → (Optional) Docker image definition
└── README.md                → You're here
```

---

## ⚙️ Tech Stack

| Tool / Library           | Purpose                         |
| ------------------------ | ------------------------------- |
| **Selenium WebDriver**   | UI automation                   |
| **NUnit**                | Test framework                  |
| **ExtentReports**        | HTML test reporting             |
| **WebDriverManager**     | Auto downloads browser drivers  |
| **Microsoft.Extensions** | DI and configuration management |
| **Docker** *(optional)*  | Containerized execution         |

---

## ⚙️ Configuration

### `appsettings.json`:

```json
{
  "Browser": "chrome",
  "Headless": false,
  "WaitTimeout": "5",
  "ExpectedMenus": {
    "Warcraft": [
      "World of Warcraft\r\nMMORPG",
      "World of Warcraft: The War Within\r\nExpansion",
      "World of Warcraft: Mists of Pandaria Classic\r\nExpansion",
      "Hearthstone\r\nStrategy Card Game",
      "Warcraft Rumble\r\nMobile Action Strategy",
      "Warcraft I: Remastered\r\nReal-Time Strategy\r\nNEW",
      "Warcraft II: Remastered\r\nReal-Time Strategy\r\nNEW",
      "Warcraft III: Reforged\r\nReal-Time Strategy"
    ],
    "Diablo": [
      "Diablo IV\r\nAction RPG",
      "Diablo IV: Vessel of Hatred\r\nAction RPG\r\nNEW",
      "Diablo Immortal\r\nAction RPG",
      "Diablo II: Resurrected\r\nAction RPG",
      "Diablo III\r\nAction RPG"
    ],
    "Overwatch": [
      "Overwatch 2\r\nTeam-Based Action"
    ],
    "Starcraft": [
      ""
    ],
    "About": [
      "About Blizzard",
      "News",
      "BlizzCon\r\nNEW",
      "Careers",
      "Gear"
    ]
  },
  "GameCards": [
    "World of Warcraft: Mists of Pandaria Classic",
    "Warcraft I: Remastered",
    "Warcraft II: Remastered",
    "Warcraft III: Reforged",
    "Diablo IV: Vessel of Hatred",
    "World of Warcraft: The War Within",
    "Warcraft Rumble",
    "World of Warcraft",
    "Overwatch 2",
    "Hearthstone"
  ]
}
```

---

## 🚀 How to Run Tests

### ✅ Locally (with Visual Studio or CLI)

```bash
dotnet test
```

### ↻ To Use Headless Mode

Set in `appsettings.json`:

```json
"Headless": false
```

---

## 📸 HTML Report

After tests run, the **Extent Report** is generated:

📄 `TestReport.html`
It includes:

* Test Pass/Fail status
* Screenshots (on pass & fail)
* Execution time & logs

---

## ⚖️ Cross-Browser Support

The browser is configured in `appsettings.json`:

```json
"Browser": "chrome"  // or "firefox", "edge"
```

Browser drivers are auto-managed via `WebDriverManager`.

---

## 🧪 Sample Test Case

```csharp
[Test]
public void DownloadButtonNavigatesToDownloadPage()
{
    _homePage.GoTo();
    _homePage.ClickDownloadButton();

    Assert.IsTrue(_homePage.IsDownloadPage(), "Did not navigate to download page.");
}
```


You can publish `TestReport.html` as an artifact.

---

## 🔹 Features Implemented

* ✔ Navigation menu verification
* ✔ Download button navigation
* ✔ Summer Sale banner test
* ✔ Featured games content & titles check
* ✔ Custom HTML reporting
* ✔ Screenshot capture (pass & fail)
* ✔ Headless compatibility

---

## 📌 Tips

* For debugging UI layout issues, toggle `"Headless": false`
* Screenshots are auto-attached in `TestReport.html`
* Logs are generated via Extent Reports
* You can expand this for test data from JSON, Excel, or APIs

---

