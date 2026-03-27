# POS — Roadmap

This file is the **single path from project start to the long-term vision**, stage by stage and step by step. Update [STATUS.md](STATUS.md) as items complete.

---

## Stage 0 — Foundation (before feature code)

**Goal:** Repo, solution structure, and standards so all later work stays consistent.

1. Initialize Git repository (if not already) and a sensible `.gitignore` for .NET / Visual Studio.
2. Create solution and projects: **Core**, **Application**, **Infrastructure**, **Presentation (WPF)** — names aligned with [pos_system_master_plan.md](pos_system_master_plan.md) §23.
3. Add **EF Core** + **SQLite**; implement **DbContext** and baseline **migrations** for **Stores** (and any minimal shared tables needed for FKs).
4. Wire **dependency injection** in the WPF host; add **logging** (e.g. Serilog per [stack.md](stack.md)).
5. Define **coding standards**: namespaces, folder layout per module (Sales, Inventory, …), and MVVM conventions.
6. Add a **test project** (xUnit or NUnit) for domain/application unit tests.

**Exit criteria:** App runs empty shell; DB migrates; one vertical slice can be added without restructuring.

---

## Stage 1 — MVP (Phase 1 from master plan)

**Goal:** One store, core POS flow: products → invoice → pay → print.

1. **Identity (minimal for MVP):** single default user or simple login; defer full roles if needed to ship faster (or implement Admin/Cashier per master plan §5 — pick one and document in STATUS).
2. **Store:** seed one **Store**; all invoices and inventory scoped to **StoreId** ([database.md](database.md) §3.1, §3.6).
3. **Products & categories:** CRUD products, categories, barcode field, basic validation.
4. **Inventory:** **Inventory** row per product/store; stock decrement on sale; configurable “no negative stock” ([database.md](database.md) §8).
5. **Sales — invoice:** open/complete invoice; line items; totals; **Status** (Open → Paid) aligned with [database.md](database.md) §3.7–3.8.
6. **Payments:** cash payment linked to invoice; totals match ([database.md](database.md) constraints).
7. **POS UI:** main cashier screen — search/add lines, totals, complete sale ([pos_system_master_plan.md](pos_system_master_plan.md) §9).
8. **Printing:** ESC/POS abstraction; receipt for completed sale ([stack.md](stack.md) §8).
9. **SQLite-only** deployment path documented (backup file location, restore).

**Exit criteria:** Demo: create products, sell, pay cash, print receipt, stock updates; works offline.

---

## Stage 2 — Operations & UX (Phase 2 from master plan)

**Goal:** Multi-invoice workflow, users/reports, and hardware polish.

1. **Multi-invoice tabs:** hold/resume; map to invoice statuses (Held, etc.) per [database.md](database.md) §12.1.
2. **Users & roles:** Admin, Manager, Cashier; **RBAC** on screens/actions ([pos_system_master_plan.md](pos_system_master_plan.md) §5, §10).
3. **Refund / cancel flows:** domain rules + DB status; stock movements for returns ([database.md](database.md) §11.1).
4. **Quick price check** and **low-stock alerts** ([pos_system_master_plan.md](pos_system_master_plan.md) §5).
5. **Reports (local):** daily sales, basic product/employee metrics; engine choice (FastReports vs RDLC) per [stack.md](stack.md) §7.
6. **Customer display:** secondary WPF window ([stack.md](stack.md) §9).
7. **Barcode scanner & printer** hardening: real hardware testing; keyboard-wedge scanner path.
8. **i18n:** `.resx` for EN/AR/HE; RTL layout for Arabic ([pos_system_master_plan.md](pos_system_master_plan.md) §16).

**Exit criteria:** Role-based POS; held invoices; refunds; reports; customer display; bilingual/RTL usable.

---

## Stage 3 — Multi-currency, pricing rules, commercial depth

**Goal:** Match “production” retail needs before sync.

1. **Currencies** and **exchange rates** ([database.md](database.md) §4.1); store policy (base currency, display).
2. **Discounts & taxes** (tables in [database.md](database.md) §10.1–10.2); invoice totals: items − discounts + taxes ([database.md](database.md) §16).
3. **Optional:** **ProductPrices** windows, **ProductBarcodes** variants, **Units** / weighted products ([database.md](database.md) §10.3–10.5).
4. **Stock movements** as audit trail; reconcile with **Inventory** snapshot ([database.md](database.md) §11).
5. **Settings** table UI ([database.md](database.md) §4.2).
6. **AuditLogs** (minimal): who changed what ([database.md](database.md) §4.3).

**Exit criteria:** Tax/discount receipts; movement history; configurable stock rules; audit trail for critical actions.

---

## Stage 4 — Backend API & sync (Phase 3 from master plan)

**Goal:** PostgreSQL server, REST API, device/store identity, offline sync.

1. **ASP.NET Core Web API** project; Clean Architecture ports; **PostgreSQL** provider alongside SQLite ([stack.md](stack.md) §4–5).
2. **Auth:** JWT for API ([stack.md](stack.md) §4); align with desktop identity story.
3. **Sync model:** **Device** entity; invoice **DeviceId**, **SyncVersion**, **IsSynced** ([database.md](database.md) §12.2, §13–14).
4. **Sync engine:** background worker; REST payloads; conflict strategy (version / last-write) decided and documented.
5. **Operational concerns:** API deployment, HTTPS, connection strings, migration strategy local ↔ server.

**Exit criteria:** Two clients can sync sales/inventory to a shared server without corrupting data (within chosen conflict rules).

---

## Stage 5 — Web dashboard & beyond (Phase 4 from master plan)

**Goal:** Browser-based admin and future channels.

1. **Web dashboard:** reports, user/store management, inventory ([pos_system_master_plan.md](pos_system_master_plan.md) §6).
2. **Multi-branch** reporting and policies (central vs store-level).
3. **Mobile / PWA / MAUI** — pick one pilot ([stack.md](stack.md) §13).
4. **Plugin / integration** hooks (payment gateways, accounting) — as needed ([pos_system_master_plan.md](pos_system_master_plan.md) §11).

**Exit criteria:** Stakeholders can run the business from web + POS; roadmap for integrations is clear.

---

## Ongoing (all stages)

- Performance: POS UI responsiveness; DB indexes ([database.md](database.md) §6–7, §15).
- Security: bcrypt passwords; least privilege; secrets not in source control ([stack.md](stack.md) §11).
- Documentation: keep **STATUS.md** and this file aligned with reality.

---

*Last updated: project kickoff — no stages completed yet.*
