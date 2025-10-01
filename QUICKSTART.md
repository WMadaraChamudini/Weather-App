# ⚡ Quick Start Guide

Get the Weather Dashboard running in under 5 minutes!

## 🐳 Fastest Method: Docker (Recommended)

### Requirements
- Docker installed ([Get Docker](https://docs.docker.com/get-docker/))

### Steps
```bash
# 1. Clone the repository
git clone https://github.com/WMadaraChamudini/Weather-App.git
cd Weather-App

# 2. Run the quick start script
./start.sh
```

**That's it!** 🎉

Access the application:
- 🌐 **Frontend**: http://localhost:3001
- 🔌 **Backend API**: http://localhost:5101

### Common Docker Commands
```bash
# View logs
docker compose logs -f

# Stop the application
docker compose down

# Restart after code changes
docker compose up --build
```

---

## 🛠️ Alternative: Manual Setup

### Requirements
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

### Backend Setup (Terminal 1)
```bash
cd WeatherDashboardAPI
dotnet run
```
✅ Backend running at http://localhost:5101

### Frontend Setup (Terminal 2)
```bash
cd weather-dashboard-frontend
node server.js
```
✅ Frontend running at http://localhost:3001

---

## 🎯 Next Steps

### Try These Features
1. 🔍 Search for any city (e.g., "London", "Tokyo", "New York")
2. 📍 Click the GPS button to get weather for your location
3. ⭐ Add cities to favorites for quick access
4. 🌓 Toggle between light and dark themes
5. 📊 View 5-day weather forecast
6. 🌬️ Check air quality data

### Customize Your Setup
- **API Key**: Use your own OpenWeatherMap API key
  - Create `.env` from `.env.example`
  - Add your key: `OPENWEATHER_API_KEY=your_key_here`
- **Port Configuration**: Edit `docker-compose.yml` or `appsettings.json`

### Deploy to Production
See **[DEPLOYMENT.md](DEPLOYMENT.md)** for:
- Azure App Service
- Heroku
- Netlify/Vercel
- DigitalOcean
- Custom servers (IIS, Nginx)

### Contribute
See **[CONTRIBUTING.md](CONTRIBUTING.md)** for development guidelines

---

## 🆘 Troubleshooting

### Docker Issues
```bash
# Check if Docker is running
docker --version

# View running containers
docker ps

# Check logs for errors
docker compose logs backend
docker compose logs frontend
```

### Port Already in Use
```bash
# Change ports in docker-compose.yml
ports:
  - "3002:3001"  # Use port 3002 instead of 3001
```

### CORS Errors
- Make sure both backend and frontend are running
- Check `Program.cs` for CORS configuration
- Verify frontend is accessing the correct backend URL

### Can't Access from Another Device
```bash
# In docker-compose.yml, bind to all interfaces
ports:
  - "0.0.0.0:3001:3001"
  - "0.0.0.0:5101:5101"
```

---

## 📚 More Information

- **Main Documentation**: [README.md](README.md)
- **Deployment Guide**: [DEPLOYMENT.md](DEPLOYMENT.md)
- **Contributing**: [CONTRIBUTING.md](CONTRIBUTING.md)
- **API Documentation**: See README.md → API Endpoints

---

**Happy Weather Tracking! 🌤️**
