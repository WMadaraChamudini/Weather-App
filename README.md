# 🌤️ Weather Dashboard

## Description

The Weather Dashboard is a comprehensive weather application that provides real-time weather information, forecasts, and air quality data for cities worldwide. Built with a modern tech stack featuring an ASP.NET Core Web API backend and a responsive vanilla JavaScript frontend, this application offers an intuitive user experience with advanced features like GPS location detection, search history, favorite cities management, and customizable themes.

**Perfect for users who want detailed weather information with a clean, modern interface and advanced functionality beyond basic weather apps.**

## 🌟 Features

### Core Weather Features
- **Current Weather Data** - Real-time temperature, conditions, and weather icons
- **5-Day Forecast** - Extended weather predictions with daily breakdowns
- **Global City Search** - Search weather for any city worldwide
- **GPS Auto-Location** - Automatic location detection and weather fetch

### User Experience Features
- **Dark/Light Theme Toggle** - Seamless theme switching with persistence
- **Favorite Cities** - Save and quickly access frequently searched cities
- **Search History** - Track recent searches with timestamps (up to 10 entries)
- **Air Quality Index** - Real-time AQI data with health impact descriptions

### Advanced Features
- **Detailed Air Quality** - CO, NO2, O3, PM2.5, PM10 measurements
- **Historical Weather** - Past weather data and trends
- **Real-time Updates** - Live weather data from OpenWeatherMap API
- **Responsive Design** - Works perfectly on desktop, tablet, and mobile

## 🏗️ Architecture

```
Weather Dashboard/
├── WeatherDashboardAPI/          # ASP.NET Core Backend
│   ├── Controllers/
│   │   └── WeatherController.cs  # Weather API endpoints
│   ├── Program.cs               # Application configuration
│   └── Properties/
│       └── launchSettings.json  # Development settings
├── weather-dashboard-frontend/   # Frontend Application
│   ├── index.html              # Main application file
│   └── server.js              # Node.js static file server
└── README.md                   # This file
```

## 🚀 Quick Start

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (for frontend server)
- OpenWeatherMap API Key (included in demo)

### 1. Clone the Repository
```bash
git clone <repository-url>
cd "Weather app"
```

### 2. Start the Backend (ASP.NET Core API)
```bash
cd WeatherDashboardAPI
dotnet run
```
Backend will run on: `http://localhost:5101`

### 3. Start the Frontend (Node.js Server)
```bash
cd weather-dashboard-frontend
node server.js
```
Frontend will run on: `http://localhost:3001`

### 4. Open in Browser
Navigate to: `http://localhost:3001`

## 🛠️ API Endpoints

### Weather Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/weather?city={city}` | Get current weather for a city |
| `GET` | `/api/weather/forecast?city={city}` | Get 5-day forecast |
| `GET` | `/api/weather/air-quality?city={city}` | Get air quality data |
| `GET` | `/api/weather/historical?city={city}&days={days}` | Get historical weather |
| `GET` | `/api/weather/test` | API health check |

### Example API Calls
```bash
# Current weather
curl "http://localhost:5101/api/weather?city=Kottawa"

# 5-day forecast
curl "http://localhost:5101/api/weather/forecast?city=Pannipitiya"

# Air quality
curl "http://localhost:5101/api/weather/air-quality?city=NewYork"
```

## 🎨 User Interface

### Theme Options
- **Light Theme** - Clean, bright interface
- **Dark Theme** - Easy on the eyes for low-light usage

### Navigation Sections
1. **Search Bar** - Enter city names for weather lookup
2. **Quick Actions** - GPS location and theme toggle buttons
3. **Search History** - Recent searches with timestamps
4. **Favorite Cities** - Saved cities for quick access
5. **Weather Display** - Current conditions and detailed information
6. **Forecast Section** - Extended weather predictions

## 🔧 Configuration

### Backend Configuration
The backend uses the following configuration:
- **Port**: 5101
- **CORS**: Enabled for localhost:3000 and localhost:3001
- **API Key**: OpenWeatherMap (demo key included)

### Frontend Configuration
- **Port**: 3001
- **Backend URL**: http://localhost:5101
- **Storage**: localStorage for favorites, history, and theme

## 📦 Dependencies

### Backend (.NET)
- Microsoft.AspNetCore.App (9.0)
- Microsoft.Extensions.Http (9.0.9)
- System.Text.Json (built-in)

### Frontend
- Vanilla JavaScript (ES6+)
- Native Fetch API
- CSS3 with modern features
- Node.js (for static file serving)

## 🌐 External APIs

### OpenWeatherMap API
- **Current Weather**: `api.openweathermap.org/data/2.5/weather`
- **5-Day Forecast**: `api.openweathermap.org/data/2.5/forecast`
- **Air Quality**: `api.openweathermap.org/data/2.5/air_pollution`
- **Geocoding**: `api.openweathermap.org/geo/1.0/`

## 💡 Features in Detail

### 🔍 Search History
- Automatically tracks successful weather searches
- Stores up to 10 recent searches with timestamps
- Prevents duplicates by updating existing entries
- One-click re-search functionality
- Clear all history option

### ⭐ Favorite Cities
- Save frequently searched cities
- Persistent storage across sessions
- Quick access buttons with active state highlighting
- Remove individual favorites option

### 📍 GPS Integration
- Browser geolocation API integration
- Reverse geocoding to city names
- Automatic weather fetch for current location
- Error handling for location access

### 🌬️ Air Quality Monitoring
- Real-time Air Quality Index (AQI)
- Color-coded health impact levels
- Detailed pollutant breakdown
- Health recommendations based on AQI levels

## 🎯 Performance Features

- **Efficient API Calls** - Smart caching and error handling
- **Responsive Design** - Optimized for all screen sizes
- **Fast Loading** - Minimal dependencies and optimized assets
- **Error Recovery** - Graceful handling of network issues

## 🛡️ Security Features

- **CORS Protection** - Configured for specific origins
- **Input Validation** - Server-side validation for all inputs
- **Error Handling** - Secure error messages without sensitive data
- **Rate Limiting** - Built-in protection via external API limits

## 🚀 Deployment Options

### Local Development
- Backend: `dotnet run`
- Frontend: `node server.js`

### Production Deployment
- **Backend**: Deploy to Azure App Service, IIS, or Docker
- **Frontend**: Deploy to any static hosting (Netlify, Vercel, GitHub Pages)
- **Environment Variables**: Configure API keys and URLs

## 🧪 Testing

### Manual Testing Checklist
- Search for different cities
- Test GPS location detection
- Toggle between light/dark themes
- Add/remove favorite cities
- View search history
- Check 5-day forecast
- Verify air quality data
- Test responsive design

### API Testing
```bash
# Test backend health
curl http://localhost:5101/api/weather/test

# Test weather endpoint
curl "http://localhost:5101/api/weather?city=London"
```

## 🔄 Development Workflow

1. **Make Changes** to backend or frontend
2. **Restart Services** if needed
3. **Test in Browser** at http://localhost:3001
4. **Check Console** for any errors
5. **Verify API Responses** in Network tab

## 📱 Browser Compatibility

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari, Chrome Mobile)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- **OpenWeatherMap** for providing weather data API
- **Microsoft** for ASP.NET Core framework
- **MDN Web Docs** for web development standards

## 📞 Support

For support and questions:
- Check the console for error messages
- Ensure both servers are running
- Verify API endpoints are accessible
- Check network connectivity

---

**🌟 Enjoy using the Weather Dashboard!** 
