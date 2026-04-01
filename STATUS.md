# POS — Status

Track completed **stages** and **steps** against [ROADMAP.md](ROADMAP.md). Update this file when something is done.

**Legend:** `[ ]` not started · `[~]` in progress · `[x]` done

---

## Stage 0 — Foundation

| Step | Status |
|------|--------|
| 0.1 Git repo + `.gitignore` | [x] |
| 0.2 Solution + Core / Application / Infrastructure / WPF projects | [x] |
| 0.3 EF Core + SQLite + DbContext + migrations (incl. Stores) | [x] |
| 0.4 DI + logging in WPF host | [x] |
| 0.5 Coding standards / folder layout documented | [x] |
| 0.6 Test project for unit tests | [x] |

**Stage 0 complete:** [x]

---

## Stage 1 — MVP

| Step | Status |
|------|--------|
| 1.1 Identity (minimal or full roles — document choice) | [x] |
| 1.2 Store seed + StoreId on invoices/inventory | [x] |
| 1.3 Products & categories CRUD | [x] |
| 1.4 Inventory per store; stock on sale | [x] |
| 1.5 Invoice + line items + Open → Paid | [x] |
| 1.6 Cash payment + totals validation | [x] |
| 1.7 Main POS UI (search, lines, complete sale) | [x] |
| 1.8 ESC/POS receipt printing | [x] |
| 1.9 SQLite deployment / backup notes | [x] |

**Identity choice (1.1):** Minimal login — `Users` + `Roles` in SQLite, BCrypt password hashes, seeded users **`admin` / `admin`** and **`cashier` / `cashier`** (change after install). Session is in-memory after sign-in (`ICurrentSession`).

**Stage 1 complete:** [x]

---

## Stage 2 — Operations & UX

| Step | Status | Notes |
|------|--------|-------|
| 2.1 Multi-invoice tabs + hold/resume | [x] | Tabs, concurrent invoices, Hold/Resume (F3), orange dot on held tabs, guard on pay |
| 2.2 Users & roles + RBAC | [x] | Catalog button hidden for Cashier; role shown in user badge; IsAdmin gate in ViewModel |
| 2.3 Refund / cancel + stock movements | [x] | RefundInvoiceAsync restores stock; RefundWindow lists recent paid invoices; Admin-only |
| 2.4 Quick price check + low-stock alerts | [x] | Price check window (F2), low-stock badge on cards + detail view |
| 2.5 Local reports (daily sales, basic metrics) | [x] | ReportsWindow: revenue, invoices, items sold, avg, top products, per-invoice list; date picker |
| 2.6 Customer display window | [x] | CustomerDisplayWindow: live cart + total, clock, idle screen; toggle via header button |
| 2.7 Barcode + printer hardening (real hardware) | [x] | Scanner burst detection (timing + Enter) tested; exact-barcode auto-add enabled; raw print verifies bytes written and safely falls back to file |
| 2.8 i18n + RTL (EN/AR/HE) | [~] | Dynamic EN/AR/HE switching now localizes cashier labels/tooltips/status/dialog texts + toggles LTR/RTL; remaining: other windows (.resx-based full coverage) |
| 2.9+2.10 UI Overhaul (Bagisto + pos_cashier.html) | [x] | Sidebar nav (Home/Customers/Cashier/Orders/Products/Reports/Settings), top bar with refresh(F5)/fullscreen(F11)/dark-mode/customer-display/price-check(F2), large Bagisto-style product cards with shadow hover, collapsible numpad, DynamicResource dark mode |

**Stage 2 complete:** [~] (2.8 i18n/RTL remains)

---

## Stage 3 — Multi-currency & commercial depth

| Step | Status |
|------|--------|
| 3.1 Currencies + exchange rates + policy | [ ] |
| 3.2 Discounts & taxes on invoices | [x] |
| 3.3 Optional: advanced pricing, barcodes, units | [ ] |
| 3.4 Stock movements audit + inventory snapshot | [ ] |
| 3.5 Settings UI | [ ] |
| 3.6 Audit logs | [ ] |

**Stage 3 complete:** [ ]

---

## Stage 4 — API & sync

| Step | Status |
|------|--------|
| 4.1 ASP.NET Core API + PostgreSQL | [ ] |
| 4.2 JWT auth | [ ] |
| 4.3 Device + sync fields on entities | [ ] |
| 4.4 Sync engine + conflict strategy | [ ] |
| 4.5 Deployment + migrations (local/server) | [ ] |

**Stage 4 complete:** [ ]

---

## Stage 5 — Web dashboard & beyond

| Step | Status |
|------|--------|
| 5.1 Web dashboard (reports, users, inventory) | [ ] |
| 5.2 Multi-branch reporting / policies | [ ] |
| 5.3 Mobile / PWA / MAUI pilot | [ ] |
| 5.4 Integrations / plugins (as needed) | [ ] |

**Stage 5 complete:** [ ]

---

## Summary

| Stage | Name | Complete |
|-------|------|----------|
| 0 | Foundation | Yes |
| 1 | MVP | Yes |
| 2 | Operations & UX | No |
| 3 | Commercial depth | No |
| 4 | API & sync | No |
| 5 | Web & beyond | No |

*Last updated: Stage 2 — 2.1–2.7 + 2.9/2.10 complete; 2.8 started with EN/AR/HE runtime language switching and RTL flow direction. Stage 3.2 (discounts & taxes) completed.*



