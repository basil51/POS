# STACK – Technical Decisions (POS System)

## 1. Overview

This document defines the final technical stack for the POS system.

Goals:
- High performance (critical for POS)
- Strong hardware support
- Scalability for future SaaS
- Maintainability and developer productivity

---

## 2. Architecture Style

- Modular Monolith
- Clean Architecture
- Offline-first design

---

## 3. Desktop Application (POS)

### Framework
- .NET (Latest LTS)
- WPF

### Pattern
- MVVM

### Libraries
- CommunityToolkit.Mvvm
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging

---

## 4. Backend API (Future)

### Framework
- ASP.NET Core Web API

### Features
- REST API
- JWT Authentication
- Multi-tenant ready

---

## 5. Database

### Local
- SQLite

### Server
- PostgreSQL

### ORM
- Entity Framework Core

---

## 6. Synchronization

- Custom Sync Engine
- REST-based sync
- Background service

---

## 7. Reporting

Options:
- FastReports (recommended)
- RDLC (alternative)

---

## 8. Printing

- ESC/POS protocol
- Raw printing via Windows APIs

---

## 9. Hardware Integration

### Barcode Scanner
- Keyboard input (default)

### Scale
- Serial (COM port)
- Custom protocol handler

### Customer Display
- Secondary window (WPF)

---

## 10. Internationalization

- .resx resource files
- RTL support

---

## 11. Security

- Password hashing (BCrypt)
- Role-based access

---

## 12. Logging

- Serilog

---

## 13. Future Mobile

- .NET MAUI OR Web App

---

## 14. Development Tools

- Visual Studio 2022+
- Git

---

## 15. Coding Practices

- SOLID principles
- Clean code
- Dependency Injection

---

END OF DOCUMENT

