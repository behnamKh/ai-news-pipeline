# AI News Pipeline

A microservices-based pipeline built with .NET and RabbitMQ to simulate AI-driven processing of news articles.  
This project demonstrates a modular, scalable architecture using message queues for decoupled service communication.

## ✨ Project Overview

The AI News Pipeline consists of several independent services that process news articles through various stages:

1. **News Fetcher** – Simulates fetching raw news articles.
2. **Summarizer** – Summarizes raw articles into concise text.
3. **Sentiment Analyzer** – Analyzes the emotional tone of the summaries.
4. **Tagging Service** – Tags articles based on content and sentiment.
5. **Storage Service** – Saves the final processed output.

Each service communicates via RabbitMQ queues, forming a lightweight, event-driven architecture.

---

## 🛠 Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [RabbitMQ](https://www.rabbitmq.com/) (Message Broker)
- [Docker](https://www.docker.com/) (for running RabbitMQ locally)
- [Serilog](https://serilog.net/) (Logging)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for RabbitMQ)
- Git

### Clone the Repository

```bash
git clone https://github.com/behnamKh/ai-news-pipeline.git
cd ai-news-pipeline
