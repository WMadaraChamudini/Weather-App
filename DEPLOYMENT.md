# 🚀 Weather Dashboard - Deployment Guide

This guide provides comprehensive instructions for deploying the Weather Dashboard application to various platforms.

## 🎯 Choose Your Deployment Method

| Method | Difficulty | Time | Best For |
|--------|-----------|------|----------|
| **Docker Compose** | ⭐ Easy | 5 min | Quick local or single-server deployment |
| **Azure App Service** | ⭐⭐ Medium | 15 min | Production with Microsoft ecosystem |
| **Heroku** | ⭐⭐ Medium | 10 min | Quick cloud deployment with free tier |
| **Netlify/Vercel** | ⭐ Easy | 5 min | Frontend only (need separate backend) |
| **DigitalOcean** | ⭐⭐ Medium | 20 min | Full control with managed services |
| **Manual (IIS/Nginx)** | ⭐⭐⭐ Advanced | 30+ min | Custom server setups |

## Table of Contents

1. [Quick Deploy with Docker](#quick-deploy-with-docker) ⭐ **Recommended**
2. [Deploy to Cloud Platforms](#deploy-to-cloud-platforms)
3. [Manual Deployment](#manual-deployment)
4. [Environment Configuration](#environment-configuration)
5. [Production Considerations](#production-considerations)

---

## Quick Deploy with Docker

The fastest way to deploy the Weather Dashboard is using Docker.

### Prerequisites

- [Docker](https://www.docker.com/get-started) installed
- [Docker Compose](https://docs.docker.com/compose/install/) installed

### Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Weather-App
   ```

2. **Configure environment variables** (optional)
   ```bash
   # Create .env file in the root directory
   echo "OPENWEATHER_API_KEY=your_api_key_here" > .env
   ```

3. **Build and run with Docker Compose**
   ```bash
   docker compose up --build
   ```

4. **Access the application**
   - Frontend: http://localhost:3001
   - Backend API: http://localhost:5101

### Docker Commands

```bash
# Start services
docker compose up -d

# Stop services
docker compose down

# View logs
docker compose logs -f

# Rebuild after changes
docker compose up --build
```

---

## Deploy to Cloud Platforms

### 1. Deploy to Azure

#### Backend (Azure App Service)

1. **Install Azure CLI**
   ```bash
   # For Linux/macOS
   curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
   ```

2. **Login to Azure**
   ```bash
   az login
   ```

3. **Create Resource Group**
   ```bash
   az group create --name weather-app-rg --location eastus
   ```

4. **Create App Service Plan**
   ```bash
   az appservice plan create --name weather-app-plan \
     --resource-group weather-app-rg \
     --sku B1 --is-linux
   ```

5. **Create Web App for Backend**
   ```bash
   az webapp create --name weather-api-app \
     --resource-group weather-app-rg \
     --plan weather-app-plan \
     --runtime "DOTNET|9.0"
   ```

6. **Deploy Backend**
   ```bash
   cd WeatherDashboardAPI
   dotnet publish -c Release -o ./publish
   cd publish
   zip -r ../deploy.zip .
   cd ..
   az webapp deployment source config-zip \
     --resource-group weather-app-rg \
     --name weather-api-app \
     --src deploy.zip
   ```

7. **Configure CORS**
   ```bash
   az webapp cors add --resource-group weather-app-rg \
     --name weather-api-app \
     --allowed-origins https://your-frontend-url.com
   ```

#### Frontend (Azure Static Web Apps)

1. **Build Frontend**
   ```bash
   cd weather-dashboard-frontend
   # If using React version
   npm install
   npm run build
   ```

2. **Deploy to Static Web App**
   ```bash
   az staticwebapp create \
     --name weather-dashboard-frontend \
     --resource-group weather-app-rg \
     --location eastus
   ```

3. **Update API URL**
   Update the API URL in frontend code to point to your Azure backend

### 2. Deploy to Heroku

#### Backend Deployment

1. **Install Heroku CLI**
   ```bash
   curl https://cli-assets.heroku.com/install.sh | sh
   ```

2. **Login to Heroku**
   ```bash
   heroku login
   ```

3. **Create Heroku App**
   ```bash
   heroku create weather-dashboard-api
   ```

4. **Add .NET Buildpack**
   ```bash
   heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
   ```

5. **Deploy**
   ```bash
   git subtree push --prefix WeatherDashboardAPI heroku main
   ```

#### Frontend Deployment

Deploy to Heroku, Netlify, or Vercel (see below)

### 3. Deploy Frontend to Netlify

1. **Install Netlify CLI**
   ```bash
   npm install -g netlify-cli
   ```

2. **Login to Netlify**
   ```bash
   netlify login
   ```

3. **Deploy (from frontend directory)**
   ```bash
   cd weather-dashboard-frontend
   
   # For vanilla JS version (serves index.html directly)
   netlify deploy --dir=. --prod
   
   # For React version
   npm run build
   netlify deploy --dir=build --prod
   ```

4. **Update API URL**
   Update the frontend code to use your deployed backend URL

### 4. Deploy Frontend to Vercel

1. **Install Vercel CLI**
   ```bash
   npm install -g vercel
   ```

2. **Login to Vercel**
   ```bash
   vercel login
   ```

3. **Deploy (from frontend directory)**
   ```bash
   cd weather-dashboard-frontend
   
   # For React version
   npm install
   vercel --prod
   ```

4. **Configure Environment Variables**
   Add environment variables in Vercel dashboard:
   - `REACT_APP_API_URL`: Your backend API URL

### 5. Deploy to DigitalOcean App Platform

1. **Connect Repository**
   - Go to DigitalOcean App Platform
   - Click "Create App"
   - Connect your GitHub repository

2. **Configure Backend**
   - Type: Web Service
   - Source Directory: `WeatherDashboardAPI`
   - Build Command: `dotnet publish -c Release -o ./publish`
   - Run Command: `cd publish && dotnet WeatherDashboardAPI.dll`

3. **Configure Frontend**
   - Type: Static Site
   - Source Directory: `weather-dashboard-frontend`
   - Build Command: `npm install && npm run build`
   - Output Directory: `build`

---

## Manual Deployment

### Backend Deployment (IIS on Windows)

1. **Publish the application**
   ```bash
   cd WeatherDashboardAPI
   dotnet publish -c Release -o ./publish
   ```

2. **Install IIS and .NET Hosting Bundle**
   - Enable IIS in Windows Features
   - Download and install [.NET Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/9.0)

3. **Create IIS Site**
   - Open IIS Manager
   - Add New Website
   - Set Physical Path to publish folder
   - Configure Application Pool for .NET

4. **Configure CORS**
   Update `Program.cs` to include production frontend URL

### Backend Deployment (Linux with Nginx)

1. **Publish the application**
   ```bash
   dotnet publish -c Release -o /var/www/weather-api
   ```

2. **Create systemd service** (`/etc/systemd/system/weather-api.service`)
   ```ini
   [Unit]
   Description=Weather Dashboard API
   
   [Service]
   WorkingDirectory=/var/www/weather-api
   ExecStart=/usr/bin/dotnet /var/www/weather-api/WeatherDashboardAPI.dll
   Restart=always
   RestartSec=10
   KillSignal=SIGINT
   SyslogIdentifier=weather-api
   User=www-data
   Environment=ASPNETCORE_ENVIRONMENT=Production
   Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
   
   [Install]
   WantedBy=multi-user.target
   ```

3. **Enable and start service**
   ```bash
   sudo systemctl enable weather-api
   sudo systemctl start weather-api
   ```

4. **Configure Nginx** (`/etc/nginx/sites-available/weather-api`)
   ```nginx
   server {
       listen 80;
       server_name api.yourdomain.com;
       
       location / {
           proxy_pass http://localhost:5101;
           proxy_http_version 1.1;
           proxy_set_header Upgrade $http_upgrade;
           proxy_set_header Connection keep-alive;
           proxy_set_header Host $host;
           proxy_cache_bypass $http_upgrade;
           proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header X-Forwarded-Proto $scheme;
       }
   }
   ```

5. **Enable site and restart Nginx**
   ```bash
   sudo ln -s /etc/nginx/sites-available/weather-api /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl restart nginx
   ```

### Frontend Deployment (Static Hosting)

The frontend can be deployed to any static hosting service since it's a client-side application.

#### Using Nginx

1. **Copy files to web root**
   ```bash
   sudo cp -r weather-dashboard-frontend/* /var/www/html/
   ```

2. **Configure Nginx** (`/etc/nginx/sites-available/weather-dashboard`)
   ```nginx
   server {
       listen 80;
       server_name yourdomain.com;
       root /var/www/html;
       index index.html;
       
       location / {
           try_files $uri $uri/ /index.html;
       }
   }
   ```

3. **Enable site**
   ```bash
   sudo ln -s /etc/nginx/sites-available/weather-dashboard /etc/nginx/sites-enabled/
   sudo systemctl restart nginx
   ```

---

## Environment Configuration

### Backend Environment Variables

Create `appsettings.Production.json` in `WeatherDashboardAPI/`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "OpenWeatherMap": {
    "ApiKey": "your_production_api_key",
    "BaseUrl": "https://api.openweathermap.org/data/2.5"
  },
  "Cors": {
    "AllowedOrigins": [
      "https://yourdomain.com",
      "https://www.yourdomain.com"
    ]
  }
}
```

### Frontend Configuration

Update API endpoints in frontend code:

**For vanilla JS version** (`index.html`):
```javascript
const API_BASE_URL = 'https://your-api-domain.com';
```

**For React version** (`.env.production`):
```
REACT_APP_API_URL=https://your-api-domain.com
```

---

## Production Considerations

### Security

1. **API Key Management**
   - Never commit API keys to source control
   - Use environment variables or secret management services
   - Rotate API keys regularly

2. **CORS Configuration**
   - Restrict CORS to specific production domains
   - Don't use wildcard (`*`) in production

3. **HTTPS**
   - Always use HTTPS in production
   - Configure SSL/TLS certificates (Let's Encrypt recommended)

4. **Rate Limiting**
   - Implement rate limiting to prevent abuse
   - Monitor API usage

### Performance

1. **Caching**
   - Implement response caching for weather data
   - Use CDN for frontend static assets

2. **Compression**
   - Enable gzip/brotli compression
   - Minify frontend assets

3. **Monitoring**
   - Set up application monitoring (Application Insights, New Relic, etc.)
   - Configure logging and alerting

### Scaling

1. **Horizontal Scaling**
   - Use load balancers for multiple instances
   - Consider container orchestration (Kubernetes)

2. **Database**
   - Add caching layer (Redis) for frequently accessed data
   - Consider implementing a database for user data

### Backup

1. **Configuration Backup**
   - Keep backups of configuration files
   - Document all environment variables

2. **Disaster Recovery**
   - Have a rollback plan
   - Test disaster recovery procedures

---

## CI/CD with GitHub Actions

The repository includes a GitHub Actions workflow (`.github/workflows/deploy.yml`) that automatically:
- Builds and tests the application
- Creates Docker images
- Deploys to your hosting platform

See `.github/workflows/deploy.yml` for configuration details.

---

## Troubleshooting

### Common Issues

1. **CORS Errors**
   - Verify CORS configuration in backend
   - Check that frontend URL is whitelisted

2. **API Connection Failed**
   - Verify backend is running and accessible
   - Check firewall rules and network configuration

3. **Build Failures**
   - Ensure correct .NET SDK version (9.0)
   - Check Node.js version for frontend

4. **Environment Variables Not Loading**
   - Verify .env file location
   - Check environment variable names

### Getting Help

- Check application logs for error details
- Review the main README.md for setup instructions
- Open an issue on GitHub for support

---

## Additional Resources

- [ASP.NET Core Deployment](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/)
- [Docker Documentation](https://docs.docker.com/)
- [Azure App Service](https://azure.microsoft.com/en-us/services/app-service/)
- [Netlify Documentation](https://docs.netlify.com/)
- [Vercel Documentation](https://vercel.com/docs)

---

**Happy Deploying! 🚀**
