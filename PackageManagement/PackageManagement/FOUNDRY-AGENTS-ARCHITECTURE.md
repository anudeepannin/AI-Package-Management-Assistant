# Azure AI Foundry Multi-Agent Architecture

## Overview

The AI Package Management Assistant uses Azure AI Foundry Agents with Agent-to-Agent (A2A) communication to automate package lifecycle management.

The solution is built using Azure AI Foundry, Azure OpenAI, ASP.NET Core APIs, Azure SQL Database, and OpenAPI tools.

---

# Solution Architecture

```text
PackageManagementAgent
        │
        ├── RenewalAgent
        │       ├── Create Renewal Request
        │       ├── Approval Workflow
        │       └── Activate Package
        │
        ├── SupportAgent
        │       ├── Create Ticket
        │       ├── Check Ticket Status
        │       └── Close Ticket
        │
        └── ComplianceAgent
                ├── Compliance Report
                ├── Open Tickets Check
                ├── Pending Renewal Check
                └── Package Health Validation
```

---

# PackageManagementAgent

## Purpose

Acts as the central coordinator for all package management operations.

## Responsibilities

- Understand user requests
- Route requests to specialist agents
- Coordinate package lifecycle operations
- Provide a unified conversational experience

## Example Requests

- Renew package 1002
- Create support ticket for package 1002
- Check compliance for package 1002

---

# RenewalAgent

## Purpose

Handles package renewal workflows.

## Capabilities

- Create renewal requests
- Collect renewal duration
- Confirm renewal actions
- Submit approval requests
- Activate packages after approval

## Example Workflow

```text
User
 ↓
Renew package 1002
 ↓
Select duration
 ↓
Confirm request
 ↓
Create renewal request
 ↓
Approve request
 ↓
Activate package
```

---

# SupportAgent

## Purpose

Handles package support and incident management.

## Capabilities

- Create support tickets
- Check ticket status
- Close tickets
- Track support history

## Example Requests

- Package 1002 login issue
- Create support ticket
- Check ticket status
- Close ticket

---

# ComplianceAgent

## Purpose

Validates package compliance and operational health.

## Capabilities

- Generate compliance reports
- Check package status
- Check pending renewals
- Check open support tickets
- Validate package ownership

## Example Requests

- Check compliance for package 1002
- Generate compliance report
- Audit package 1002

---

# Agent-to-Agent (A2A) Communication

Azure AI Foundry Agent-to-Agent communication is used to allow PackageManagementAgent to collaborate with specialist agents.

Benefits:

- Separation of responsibilities
- Reusable specialist agents
- Simplified orchestration
- Better scalability

---

# OpenAPI Tool Integration

Each specialist agent uses OpenAPI tools connected to ASP.NET Core APIs.

```text
Azure AI Foundry Agent
        │
        ▼
OpenAPI Tool
        │
        ▼
ASP.NET Core API
        │
        ▼
Azure SQL Database
```

---

# Database Entities

## Packages

Stores package information.

Fields:

- PackageId
- PackageName
- OwnerName
- Status
- ExpiryDate

## RenewalRequests

Stores renewal workflow information.

Fields:

- RequestId
- PackageId
- Duration
- Status
- CreatedDate

## SupportTickets

Stores support ticket information.

Fields:

- TicketId
- PackageId
- IssueDescription
- Severity
- Status
- CreatedDate

---

# Azure Services Used

- Azure AI Foundry
- Azure OpenAI
- Azure AI Search
- Azure App Service
- Azure SQL Database
- Azure Identity
- Azure AI Projects SDK

---

# Technology Stack

- ASP.NET Core 10
- Entity Framework Core
- Semantic Kernel
- Azure AI Foundry Agents
- OpenAPI Tools
- Azure SQL Database
- React (Frontend)
- Azure AI Search

---

# Key Features

- Multi-Agent Architecture
- Agent-to-Agent Communication
- Renewal Workflow Automation
- Support Ticket Management
- Compliance Validation
- OpenAPI Tool Integration
- Azure Cloud Deployment
- SQL-Based Persistence

---

# Future Enhancements

- Teams Notifications
- Email Approval Workflows
- Dashboard Analytics
- AI Search Powered Knowledge Base
- Agent Memory
- Monitoring and Telemetry