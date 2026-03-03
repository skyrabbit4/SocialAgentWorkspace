# SocialAgentWorkspace

An AI-powered social media agent that drafts and publishes content to X (Twitter). The application uses OpenAI (via Microsoft Semantic Kernel) to generate engaging tweets and the Twitter API (via Tweetinvi) to post them, with a full history of published posts stored in a local SQLite database.

## Tech Stack

| Layer    | Technology                                                                 |
| -------- | -------------------------------------------------------------------------- |
| Backend  | ASP.NET Core 8 Minimal API, Microsoft Semantic Kernel, Tweetinvi, EF Core  |
| Frontend | Angular 20, TypeScript, RxJS                                               |
| Database | SQLite (via Entity Framework Core)                                         |
| AI Model | OpenAI `gpt-4o-mini`                                                       |

## Project Structure

```
SocialAgentWorkspace/
├── Backend/
│   └── SocialAgent.Api/          # ASP.NET Core Minimal API
│       ├── Data/                  # EF Core DbContext
│       ├── Migrations/            # EF Core migrations
│       ├── Models/                # Domain models (SocialPost)
│       ├── Program.cs             # Application entry point & API endpoints
│       ├── appsettings.json       # App configuration
│       └── SocialAgent.Api.csproj
├── Frontend/
│   └── social-agent-ui/           # Angular 20 application
│       └── src/
│           └── app/
│               ├── components/    # UI components (history)
│               ├── models/        # TypeScript interfaces
│               ├── services/      # HTTP services (PostService)
│               ├── app.ts         # Root AppComponent
│               └── app.html       # Dashboard template
└── SocialAgentWorkspace.sln       # Visual Studio solution file
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or later)
- [Angular CLI](https://angular.dev/tools/cli) (`npm install -g @angular/cli`)
- An **OpenAI API key**
- **X (Twitter) API** credentials (API key, API secret, access token, access secret)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/skyrabbit4/SocialAgentWorkspace.git
cd SocialAgentWorkspace
```

### 2. Configure the Backend

Store your secrets using [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) so they stay out of source control:

```bash
cd Backend/SocialAgent.Api
dotnet user-secrets set "AI:ApiKey"       "<your-openai-api-key>"
dotnet user-secrets set "X:ApiKey"        "<your-twitter-api-key>"
dotnet user-secrets set "X:ApiSecret"     "<your-twitter-api-secret>"
dotnet user-secrets set "X:AccessToken"   "<your-twitter-access-token>"
dotnet user-secrets set "X:AccessSecret"  "<your-twitter-access-secret>"
```

### 3. Apply Database Migrations

```bash
cd Backend/SocialAgent.Api
dotnet ef database update
```

### 4. Run the Backend

```bash
cd Backend/SocialAgent.Api
dotnet run
```

The API will start at `http://localhost:5221` by default. Swagger UI is available at `/swagger` in development mode.

### 5. Run the Frontend

```bash
cd Frontend/social-agent-ui
npm install
ng serve
```

Open your browser at `http://localhost:4200`.

## API Endpoints

| Method | Route                        | Description                              |
| ------ | ---------------------------- | ---------------------------------------- |
| GET    | `/api/status`                | Health-check / connectivity test         |
| GET    | `/api/chat?userPrompt=...`   | Generate AI-drafted content              |
| POST   | `/api/tweet?tweetText=...`   | Publish a tweet to X and save to history |
| GET    | `/api/history`               | Retrieve all published posts             |

## Configuration

Configuration keys are read from `appsettings.json` and user secrets:

| Key               | Description                  |
| ----------------- | ---------------------------- |
| `AI:ApiKey`       | OpenAI API key               |
| `X:ApiKey`        | Twitter API key              |
| `X:ApiSecret`     | Twitter API secret           |
| `X:AccessToken`   | Twitter access token         |
| `X:AccessSecret`  | Twitter access secret        |

## License

This project does not currently specify a license. Please contact the repository owner for usage terms.
