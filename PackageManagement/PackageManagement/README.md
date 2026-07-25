# AI Package Management Assistant

## Overview

AI Package Management Assistant is an intelligent package management system built using .NET and Azure AI services.

The application enables users to interact with package information using natural language. Users can check package status, retrieve package ownership details, search package documentation, and generate package-related communications through an AI-powered interface.

The project is designed to demonstrate modern AI application development using Semantic Kernel, Azure OpenAI, Azure AI Search, Azure SQL Database, and Agent-based workflows.

## Objectives

- Build an enterprise-style AI application
- Learn Semantic Kernel and Function Calling
- Implement Retrieval-Augmented Generation (RAG)
- Integrate Azure AI services
- Demonstrate AI Agent capabilities
- Showcase real-world AI architecture using .NET

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Semantic Kernel
- Azure OpenAI
- Azure AI Search
- Azure SQL Database
- Azure Blob Storage
- Azure AI Foundry

## Planned Features

### Package Management

- Retrieve package status
- Retrieve package ownership information
- View package expiration details
- Search package information

### AI Features

- Natural language interaction
- Function Calling
- AI-generated summaries
- AI-generated email drafts
- Document-based question answering

### Knowledge Search

- Document indexing
- Vector Search
- Retrieval-Augmented Generation (RAG)

### Agent Capabilities

- Package Management Agent
- Documentation Search Agent
- Notification Agent

## Architecture

User
→ Web API
→ Semantic Kernel
→ Azure OpenAI
→ Plugins
→ Azure Services

## Repository Structure

```text
PackageManagement/
├── Controllers/
├── Plugins/
├── Services/
├── Models/
├── Program.cs
└── README.md
```