# POS System – Master Reference Document

## 1. Project Overview

This project aims to build a professional Point of Sale (POS) system targeting small to medium retail businesses, with a strong architectural foundation that allows future scalability into enterprise-level systems.

The system will support:
- Fast cashier operations
- Multi-invoice handling
- Hardware integration (printers, barcode scanners, scales)
- Multi-language support (Arabic, English, Hebrew)
- Multi-currency support
- Offline-first operation with synchronization

The long-term vision is to evolve into a scalable, multi-branch, cloud-connected retail management platform.

---

## 2. Architecture Strategy

### Approach: Modular Monolith with Clean Architecture

The system will be built as a single application divided into independent modules:

Modules:
- Sales
- Inventory
- Customers
- Payments
- Reports
- Users & Roles

Each module will:
- Contain its own business logic
- Communicate via interfaces
- Be easily extractable into microservices in the future

Layers:

1. Core Layer
   - Business logic
   - Entities
   - Rules
   - No dependency on UI or database

2. Application Layer
   - Use cases (e.g., CreateInvoice, RefundInvoice)
   - Services

3. Infrastructure Layer
   - Database (SQLite / PostgreSQL)
   - Hardware integrations
   - External APIs

4. Presentation Layer
   - WPF (POS system)
   - Web Dashboard (future)

---

## 3. Technology Stack

### Frontend (POS)
- C#
- .NET
- WPF

### Backend (Future API)
- .NET Web API (preferred)

### Database
- Local: SQLite
- Server: PostgreSQL

### Mobile / Tablet (Future)
- .NET MAUI OR Web App (PWA / Blazor)

### Other Technologies
- REST APIs
- JSON for data exchange
- Dependency Injection

---

## 4. Hardware Support

Supported devices:

### Printers
- ESC/POS protocol
- USB / Network

### Barcode Scanner
- Keyboard emulation (primary)
- USB/HID

### Scale
- Serial (COM port)
- Vendor-specific protocols

### Customer Display
- Secondary screen
- Fullscreen display

---

## 5. Core Features

### Sales
- Multi-invoice tabs
- Hold / Resume invoice
- Refund handling
- Quick price check

### Inventory
- Stock tracking
- Alerts for low stock

### Payments
- Cash
- Future: digital payments

### Users & Roles
- Admin
- Manager
- Cashier

### Reports
- Daily sales
- Profit reports
- Product performance
- Employee performance

### Localization
- Arabic (RTL)
- English
- Hebrew
- Language per invoice printing

### Currency
- Multi-currency support
- Exchange rate API integration

---

## 6. Admin Dashboard (Future)

Accessible via web browser.

Features:
- Reports & analytics
- User management
- Inventory management
- Multi-branch control
- Currency configuration

---

## 7. Database Design (High-Level)

### Main Tables

#### Products
- Id
- Name
- Barcode
- Price
- Cost
- StockQuantity

#### Invoices
- Id
- Date
- Status
- TotalAmount
- Currency

#### InvoiceItems
- Id
- InvoiceId
- ProductId
- Quantity
- Price

#### Customers
- Id
- Name
- Phone

#### Payments
- Id
- InvoiceId
- Amount
- Method

#### Users
- Id
- Username
- PasswordHash
- Role

#### Roles
- Id
- Name

---

### Relationships
- Invoice → InvoiceItems (1:N)
- Product → InvoiceItems (1:N)
- Invoice → Payments (1:N)
- User → Role (N:1)

---

## 8. Offline & Sync Strategy

- Local database (SQLite)
- All operations stored locally
- Each record has:
  - Unique ID (GUID)
  - Timestamp

Future:
- Sync service with server
- Conflict resolution strategy

---

## 9. UI / Screens (POS)

### Main Screens

1. POS Screen (Cashier)
   - Product search
   - Invoice panel
   - Quick actions

2. Invoice Tabs
   - Multiple active invoices

3. Payment Screen

4. Product Management

5. Reports Screen

6. Settings

---

### Customer Display Screen
- Product list
- Total price
- Clean UI

---

## 10. Security

- Role-based access control
- Secure password hashing
- Audit logs (future)

---

## 11. Future Expansion

- Multi-branch support
- Cloud synchronization
- Mobile application
- Integration with payment gateways
- Plugin system

---

## 12. Development Phases

### Phase 1 (MVP)
- POS screen
- Products
- Invoices
- Printing
- SQLite

### Phase 2
- Users & roles
- Reports
- Customer display

### Phase 3
- Backend API
- Sync system

### Phase 4
- Web dashboard
- Mobile support

---

## 13. Key Principles

- Offline-first design
- High performance (critical for POS)
- Separation of concerns
- Scalable architecture
- Hardware compatibility

---

## 14. Notes

- Avoid tight coupling between layers
- Use interfaces for extensibility
- Prepare for future migration to microservices if needed

---

---

## 15. Detailed Technology Decisions (Stack Deep Dive)

### Desktop POS
- Framework: WPF (.NET)
- Pattern: MVVM
- UI: XAML (Touch-optimized)

### Backend API
- Framework: ASP.NET Core Web API
- Architecture: Clean Architecture
- Auth: JWT (future)

### Databases Compatibility

#### SQLite vs PostgreSQL
Yes, compatibility is achievable with proper abstraction:

Strategy:
- Use ORM: Entity Framework Core
- Avoid DB-specific SQL
- Use common data types only

Notes:
- SQLite is file-based and lenient with types
- PostgreSQL is strict and more powerful

Best Practice:
- Design schema compatible with PostgreSQL first
- Then adapt SQLite locally

---

## 16. Internationalization (i18n)

- Use resource files (.resx)
- Support RTL (Arabic)
- Dynamic language switching
- Invoice language override feature

---

## 17. Reporting System

- Engine: Local reporting (e.g., FastReports or RDLC)
- Export formats: PDF / Print
- Future: Server-side reporting

---

## 18. Printing System

- ESC/POS commands
- Printer abstraction layer
- Queue-based printing

---

## 19. Stores / Multi-Branch Design

- Store entity from day 1
- Each invoice linked to StoreId
- Future: Centralized reporting

---

## 20. Security

- Password hashing (bcrypt)
- Role-based access control
- Audit logs (future)

---

## 21. DDD - Bounded Contexts

### Sales Context
- Invoices
- InvoiceItems
- Discounts

### Inventory Context
- Products
- Stock
- Adjustments

### Identity Context
- Users
- Roles

### Payment Context
- Payments
- Methods

### Reporting Context
- Aggregations
- Analytics

---

## 22. Detailed Database Schema (Advanced)

### Products
- Id (GUID)
- Name
- Barcode
- Price
- Cost
- StockQuantity
- CreatedAt

### Stores
- Id
- Name

### Invoices
- Id
- StoreId
- UserId
- Status
- TotalAmount
- Currency
- CreatedAt

### InvoiceItems
- Id
- InvoiceId
- ProductId
- Quantity
- Price

### Payments
- Id
- InvoiceId
- Amount
- Method

### Users
- Id
- Username
- PasswordHash
- RoleId

### Roles
- Id
- Name

---

### ERD (Textual Representation)

- Store 1 --- N Invoices
- Invoice 1 --- N InvoiceItems
- Product 1 --- N InvoiceItems
- Invoice 1 --- N Payments
- Role 1 --- N Users

---

## 23. Folder Structure (Suggested)

- Core/
- Application/
- Infrastructure/
- Presentation/

---

## 24. API Design (Preview)

- POST /api/invoices
- GET /api/products
- POST /api/payments

---

## 25. UI Wireframes (Planned)

- POS Screen layout
- Invoice tabs
- Payment dialog

---

## 26. Coding Standards

- Clean code principles
- SOLID
- Naming conventions (PascalCase for C#)

---

END OF DOCUMENT

