# 🤝 Contributing to Connectiq

Thanks for your interest in contributing to **Connectiq**! We welcome bug reports, feature requests, documentation updates, and code contributions. This document will guide you through the process.

---

## 📦 Project Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/AlexAlvarezGallardo/connectiq.git
   cd connectiq
   ```

2. **Set up the development environment**

   - .NET SDK 8.0+
   - Visual Studio 2022+ or VS Code
   - Aspire .NET is needed for run all the applications or use DockerFiles instead.

3. **Restore dependencies & build**

   ```bash
   dotnet restore
   dotnet build
   ```

4. **Run tests**

   ```bash
   dotnet test
   ```

---

## 🚧 Branching Strategy

- Work from feature branches:
  - `feature/<name>`
  - `bugfix/<name>`
- Target `develop` for all new work.  
- PRs to `main` are reserved for releases only.

---

## 📋 Opening a Pull Request

1. Open your PR against the `develop` branch.
2. Use the provided Pull Request Template.
3. Make sure your PR:
   - Has a clear and descriptive title
   - Includes context and reasoning for changes
   - Links to any related issues or tasks (use `Closes #123`)
   - Includes tests for new functionality or edge cases

---

## ✅ Code Guidelines

- Follow the project’s architectural conventions (Clean Architecture, CQRS, etc.)
- Favor readability and maintainability over clever tricks
- Prefer composition over inheritance
- Use meaningful commit messages (present tense):
  - ✅ `Add JWT authentication middleware`
  - ✅ `Fix crash when no events are received`
  - ❌ `Fixed something`

---

## 🧪 Testing

- All code must be covered by unit or integration tests
- Use existing fixtures when possible
- Add architecture tests if touching boundaries or layering

---

## 🔐 Security

- Never commit secrets, credentials, or tokens.
- If you find a vulnerability, report it privately via [SECURITY.md](./SECURITY.md)

---

## 💬 Communication

- Use GitHub Issues for tracking bugs and features.
- Tag reviewers who own or understand the part of the code you're touching.
- If in doubt, open a **draft PR** to start a conversation early.

---

Thanks again for contributing! 💙  
Let’s build something awesome together.