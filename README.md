# Movies Semantic Search

A full-stack application for **semantic movie search**, built with modern technologies and designed for scalability and developer convenience. The system uses a powerful AI model for vector embeddings, stores and queries data with Elasticsearch, and brings everything together using .NET Aspire.

## 🔍 Features

- **Semantic Search** using vector embeddings for natural language movie queries
- **AI-powered vectorization** of both movie data and user search queries via an Ollama-hosted model
- **Multilanguage support** for semantic search, enabling queries and data in multiple languages
- **Fast and scalable search** using Elasticsearch with vector support
- **C# API backend** for handling search logic and model/database communication
- **React + TypeScript frontend** for an intuitive and responsive UI
- **.NET Aspire integration** for easy orchestration of all services in one unified environment

## 🚀 Tech Stack

- **Frontend**: React, TypeScript, Vite
- **Backend**: .NET 9, ASP.NET Core, REST API
- **AI Model**: Ollama and a Hugging face model for vector embeddings
- **Database/Search**: Elasticsearch with vector search enabled
- **Orchestration**: .NET Aspire

![Screenshot of net aspire orchestrating all the components](/images/net-aspire.png)

## 🧠 Semantic Search Details

The system uses an AI model hosted with Ollama to generate vector embeddings for both:
- Movie description
- User queries (e.g., "a sci-fi movie about time travel")

These vectors are indexed and searched in Elasticsearch using k-NN (approximate nearest neighbor) vector similarity.

![Screenshot of a semantic search using the UI](/images/semantic-search1.png)

![Screenshot of a semantic search using the UI](/images/semantic-search2.png)


### Multilanguage Support

The AI model used for generating embeddings is multilingual, allowing the system to handle movie descriptions and user queries in multiple languages seamlessly.