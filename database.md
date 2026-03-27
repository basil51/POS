# DATABASE DESIGN – POS SYSTEM

## 1. Overview

This document defines the full database design for the POS system.

Goals:
- Support offline-first architecture
- Ensure compatibility between SQLite (local) and PostgreSQL (server)
- Enable future scalability (multi-branch, sync, analytics)
- Maintain high performance for POS operations

---

## 2. Design Principles

- Use GUIDs for primary keys (important for sync)
- Avoid database-specific features
- Use normalized structure (up to 3NF where applicable)
- Include audit fields in all tables
- Design with multi-branch support from day one

Standard Fields (in most tables):
- Id (GUID, PK)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- IsDeleted (Soft delete)

---

## 3. Core Entities

### 3.1 Stores

Represents business locations.

Fields:
- Id (GUID, PK)
- Name (string)
- Address (string)
- Phone (string)

---

### 3.2 Users

Fields:
- Id (GUID, PK)
- Username (string, unique)
- PasswordHash (string)
- RoleId (FK)
- StoreId (FK)
- IsActive (bool)

---

### 3.3 Roles

Fields:
- Id (GUID, PK)
- Name (string)

---

### 3.4 Products

Fields:
- Id (GUID, PK)
- Name (string)
- Barcode (string, indexed)
- Price (decimal)
- Cost (decimal)
- CategoryId (FK)
- IsWeighted (bool)
- IsActive (bool)

---

### 3.5 Categories

Fields:
- Id (GUID, PK)
- Name (string)

---

### 3.6 Inventory

Tracks stock per store.

Fields:
- Id (GUID, PK)
- ProductId (FK)
- StoreId (FK)
- Quantity (decimal)

---

### 3.7 Invoices

Fields:
- Id (GUID, PK)
- StoreId (FK)
- UserId (FK)
- CustomerId (FK, nullable)
- Status (enum: Open, Paid, Refunded, Cancelled)
- TotalAmount (decimal)
- Currency (string)
- Notes (string)

---

### 3.8 InvoiceItems

Fields:
- Id (GUID, PK)
- InvoiceId (FK)
- ProductId (FK)
- Quantity (decimal)
- UnitPrice (decimal)
- TotalPrice (decimal)

---

### 3.9 Payments

Fields:
- Id (GUID, PK)
- InvoiceId (FK)
- Amount (decimal)
- Method (enum: Cash, Card, Other)
- PaidAt (DateTime)

---

### 3.10 Customers

Fields:
- Id (GUID, PK)
- Name (string)
- Phone (string)

---

## 4. Supporting Tables

### 4.1 Currencies

Fields:
- Id (GUID, PK)
- Code (string, e.g., USD, ILS)
- Name (string)
- ExchangeRate (decimal)

---

### 4.2 Settings

Fields:
- Id (GUID, PK)
- Key (string)
- Value (string)

---

### 4.3 AuditLogs (Future)

Fields:
- Id (GUID, PK)
- UserId (FK)
- Action (string)
- EntityName (string)
- EntityId (GUID)
- Timestamp (DateTime)

---

## 5. Relationships (ERD)

- Store 1 → N Users
- Store 1 → N Invoices
- Store 1 → N Inventory

- Role 1 → N Users

- Product 1 → N InvoiceItems
- Product 1 → N Inventory

- Invoice 1 → N InvoiceItems
- Invoice 1 → N Payments

- Customer 1 → N Invoices

---

## 6. Indexing Strategy

Important indexes:
- Products.Barcode
- Users.Username
- Invoices.CreatedAt
- Inventory(ProductId, StoreId)

---

## 7. Sync Considerations

Each table should include:
- Id (GUID)
- CreatedAt
- UpdatedAt

Future additions:
- SyncStatus
- Version number (for conflict resolution)

---

## 8. Constraints & Rules

- Invoice must have at least one item
- Payment amount <= Invoice total
- Product must exist before adding to invoice
- Stock cannot go negative (configurable)

---

## 9. Future Enhancements

- Discounts table
- Tax system
- Loyalty system
- Multi-warehouse support
- Advanced analytics tables

---

---

## 10. Advanced Commercial Features (Production Ready)

### 10.1 Discounts System

#### Discounts
- Id (GUID, PK)
- Name (string)
- Type (enum: Percentage, Fixed)
- Value (decimal)
- IsActive (bool)

#### InvoiceDiscounts
- Id (GUID, PK)
- InvoiceId (FK)
- DiscountId (FK)
- Amount (decimal)

---

### 10.2 Tax System

#### Taxes
- Id (GUID, PK)
- Name (string)
- Rate (decimal)
- IsIncludedInPrice (bool)

#### InvoiceTaxes
- Id (GUID, PK)
- InvoiceId (FK)
- TaxId (FK)
- Amount (decimal)

---

### 10.3 Pricing System (Advanced)

#### ProductPrices
- Id (GUID, PK)
- ProductId (FK)
- Price (decimal)
- Currency (string)
- StartDate (DateTime)
- EndDate (DateTime, nullable)

---

### 10.4 Units & Weighted Products

#### Units
- Id (GUID, PK)
- Name (string) (e.g., Piece, Kg, Liter)

#### ProductUnits
- Id (GUID, PK)
- ProductId (FK)
- UnitId (FK)
- ConversionFactor (decimal)

---

### 10.5 Barcode Variants

#### ProductBarcodes
- Id (GUID, PK)
- ProductId (FK)
- Barcode (string)

---

## 11. Inventory – Professional Design

### 11.1 Stock Movements (Critical)

Instead of only storing quantity, track all movements.

#### StockMovements
- Id (GUID, PK)
- ProductId (FK)
- StoreId (FK)
- Quantity (decimal, positive/negative)
- Type (enum: Sale, Purchase, Adjustment, Return)
- ReferenceId (GUID) (e.g., InvoiceId)
- CreatedAt (DateTime)

---

### 11.2 Stock Snapshot (Fast Access)

#### Inventory (Updated)
- Id (GUID, PK)
- ProductId (FK)
- StoreId (FK)
- Quantity (decimal)

---

## 12. Invoice Enhancements

### 12.1 Invoice Status (Advanced)

- Draft
- Open
- Held
- Paid
- Cancelled
- Refunded
- Synced

---

### 12.2 Invoice Metadata

Add fields:
- IsSynced (bool)
- SyncVersion (int)
- DeviceId (string)

---

## 13. Multi-Store & Multi-Device

### Devices
- Id (GUID, PK)
- StoreId (FK)
- Name (string)

Invoices should include:
- DeviceId (FK)

---

## 14. Sync-Ready Design (Critical)

All tables should include:
- Id (GUID)
- CreatedAt
- UpdatedAt
- SyncVersion (int)
- IsDeleted (bool)

Conflict Strategy (future):
- Last write wins OR
- Version-based resolution

---

## 15. Performance Considerations

- Use indexes on foreign keys
- Cache products locally
- Preload most-used data

---

## 16. Data Integrity Rules

- No orphan records (FK constraints)
- Invoice total = sum(items) - discounts + taxes
- Payments must match invoice total (configurable)

---

## 17. Future SaaS Enhancements

- Multi-tenant support (TenantId in all tables)
- Subscription plans
- Feature flags per tenant

---

## 18. Optional Multi-Tenancy (Future Ready)

Add to all tables:
- TenantId (GUID)

---

END OF DOCUMENT

