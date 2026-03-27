# Coding standards — POS

## Solution layout

| Project | Responsibility |
|---------|------------------|
| `POS.Core` | Domain entities, value objects, domain-only types (no UI, no EF attributes if we keep persistence pure — EF config stays in Infrastructure). |
| `POS.Application` | Use cases, orchestration, interfaces for infrastructure. Folders under `Modules/` mirror bounded contexts (Sales, Inventory, Identity, Payments, Reporting). |
| `POS.Infrastructure` | EF Core, SQLite/PostgreSQL, hardware and external APIs later. |
| `POS.Wpf` | MVVM views, view models, WPF-specific code. |

## Naming

- **Assemblies:** `POS.*`
- **C#:** PascalCase for types/methods/properties; `camelCase` for locals; `_camelCase` for private fields if used.
- **Async methods:** suffix with `Async`.

## MVVM (WPF)

- ViewModels in `POS.Wpf` (or `Views`/`ViewModels` folders).
- Prefer **CommunityToolkit.Mvvm** (`ObservableObject`, `[RelayCommand]`) when we add the package in Stage 1+.
- Views: minimal code-behind; behavior in ViewModels.

## Data access

- **PostgreSQL-first** schema mindset; SQLite for local dev ([pos_system_master_plan.md](../pos_system_master_plan.md)).
- Migrations live in `POS.Infrastructure/Data/Migrations/`.
- Design-time factory: `PosDbContextFactory` for `dotnet ef` without running the desktop app.

## Tests

- `POS.Tests`: unit tests for Core/Application; name files `*Tests.cs`.
- Run: `dotnet test` from the repository root.

## Tooling

- **EF CLI:** `dotnet ef` (install: `dotnet tool install dotnet-ef --global --version 8.0.x`). If the tool fails to start, set `DOTNET_ROOT` to your SDK folder (see [README](../README.md)).
