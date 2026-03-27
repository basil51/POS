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
| 1.1 Identity (minimal or full roles — document choice) | [ ] |
| 1.2 Store seed + StoreId on invoices/inventory | [ ] |
| 1.3 Products & categories CRUD | [ ] |
| 1.4 Inventory per store; stock on sale | [ ] |
| 1.5 Invoice + line items + Open → Paid | [ ] |
| 1.6 Cash payment + totals validation | [ ] |
| 1.7 Main POS UI (search, lines, complete sale) | [ ] |
| 1.8 ESC/POS receipt printing | [ ] |
| 1.9 SQLite deployment / backup notes | [ ] |

**Stage 1 complete:** [ ]

---

## Stage 2 — Operations & UX

| Step | Status |
|------|--------|
| 2.1 Multi-invoice tabs + hold/resume | [ ] |
| 2.2 Users & roles + RBAC | [ ] |
| 2.3 Refund / cancel + stock movements | [ ] |
| 2.4 Quick price check + low-stock alerts | [ ] |
| 2.5 Local reports (daily sales, basic metrics) | [ ] |
| 2.6 Customer display window | [ ] |
| 2.7 Barcode + printer hardening (real hardware) | [ ] |
| 2.8 i18n + RTL (EN/AR/HE) | [ ] |

**Stage 2 complete:** [ ]

---

## Stage 3 — Multi-currency & commercial depth

| Step | Status |
|------|--------|
| 3.1 Currencies + exchange rates + policy | [ ] |
| 3.2 Discounts & taxes on invoices | [ ] |
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
| 1 | MVP | No |
| 2 | Operations & UX | No |
| 3 | Commercial depth | No |
| 4 | API & sync | No |
| 5 | Web & beyond | No |

*Last updated: Stage 0 completed — solution, EF Stores migration, WPF host, Serilog, tests.*
