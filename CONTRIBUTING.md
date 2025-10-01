# Contributing to Weather Dashboard

Thank you for your interest in contributing to the Weather Dashboard! This document provides guidelines and instructions for contributing.

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [Docker](https://www.docker.com/get-started) (optional, for containerized development)
- Git

### Setting Up Development Environment

1. **Fork and clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/Weather-App.git
   cd Weather-App
   ```

2. **Start the backend**
   ```bash
   cd WeatherDashboardAPI
   dotnet restore
   dotnet run
   ```

3. **Start the frontend** (in a new terminal)
   ```bash
   cd weather-dashboard-frontend
   node server.js
   ```

4. **Access the application**
   - Frontend: http://localhost:3001
   - Backend API: http://localhost:5101

### Using Docker for Development

```bash
# Start all services with Docker
./start.sh

# Or manually
docker compose up --build

# View logs
docker compose logs -f

# Stop services
docker compose down
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout -b feature/your-feature-name
```

### 2. Make Your Changes

- Follow the existing code style
- Write clear, descriptive commit messages
- Keep changes focused and atomic

### 3. Test Your Changes

#### Backend Testing

```bash
cd WeatherDashboardAPI
dotnet build
dotnet run
```

Test the API endpoints:
```bash
# Health check
curl http://localhost:5101/api/weather/test

# Get weather
curl "http://localhost:5101/api/weather?city=London"
```

#### Frontend Testing

1. Start the frontend server
2. Open http://localhost:3001 in your browser
3. Test all features:
   - Search for cities
   - GPS location detection
   - Theme toggle
   - Favorite cities
   - Search history

### 4. Commit Your Changes

```bash
git add .
git commit -m "Add feature: brief description"
```

### 5. Push and Create Pull Request

```bash
git push origin feature/your-feature-name
```

Then create a pull request on GitHub.

## Code Style Guidelines

### Backend (C#)

- Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose

### Frontend (JavaScript)

- Use ES6+ syntax
- Use `const` and `let` instead of `var`
- Use arrow functions where appropriate
- Follow consistent indentation (2 or 4 spaces)
- Add comments for complex logic

### CSS

- Use meaningful class names
- Follow BEM naming convention where applicable
- Group related styles together
- Add comments for complex styles

## Project Structure

```
Weather-App/
├── WeatherDashboardAPI/          # Backend API
│   ├── Controllers/              # API controllers
│   │   └── WeatherController.cs
│   ├── Program.cs               # App configuration
│   └── Dockerfile               # Backend container
├── weather-dashboard-frontend/   # Frontend
│   ├── index.html              # Main HTML file
│   ├── server.js               # Static file server
│   ├── src/                    # React components (if using React)
│   └── Dockerfile              # Frontend container
├── docker-compose.yml           # Container orchestration
├── DEPLOYMENT.md               # Deployment guide
└── README.md                   # Main documentation
```

## Areas for Contribution

### Features
- Additional weather data visualization
- Weather alerts and notifications
- Historical weather comparison
- Multi-language support
- User authentication and profiles
- Weather widgets
- Mobile app development

### Improvements
- Performance optimization
- Enhanced error handling
- Better caching strategies
- Accessibility improvements
- UI/UX enhancements
- Test coverage

### Documentation
- API documentation
- Code examples
- Tutorial videos
- Translation to other languages

### Bug Fixes
- Report bugs via GitHub Issues
- Include steps to reproduce
- Provide system information
- Include screenshots if applicable

## Pull Request Process

1. **Update documentation** if you're changing functionality
2. **Add tests** if you're adding features (when test infrastructure is available)
3. **Ensure the application builds** without errors
4. **Test manually** to verify your changes work as expected
5. **Update CHANGELOG.md** with your changes (if file exists)
6. **Provide a clear PR description** explaining:
   - What problem you're solving
   - How you solved it
   - How to test your changes

## Reporting Issues

When reporting issues, please include:

1. **Description** - Clear description of the issue
2. **Steps to reproduce** - How to recreate the problem
3. **Expected behavior** - What should happen
4. **Actual behavior** - What actually happens
5. **Environment**:
   - OS (Windows, Linux, macOS)
   - Browser (if frontend issue)
   - .NET version
   - Node.js version
6. **Screenshots** - If applicable
7. **Logs** - Any error messages or console output

## Questions?

If you have questions about contributing:

1. Check the [README.md](README.md) for basic information
2. Check the [DEPLOYMENT.md](DEPLOYMENT.md) for deployment questions
3. Open a GitHub Discussion
4. Open an issue with the "question" label

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers
- Accept constructive criticism
- Focus on what's best for the community
- Show empathy towards others

## License

By contributing, you agree that your contributions will be licensed under the same license as the project.

---

**Thank you for contributing! 🎉**
