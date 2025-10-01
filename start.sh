#!/bin/bash
# Quick start script for Weather Dashboard

set -e

echo "🌤️  Weather Dashboard - Quick Start"
echo "===================================="
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    echo "   Visit: https://docs.docker.com/get-docker/"
    exit 1
fi

echo "✓ Docker is installed"

# Check if Docker Compose is available
if ! docker compose version &> /dev/null; then
    echo "❌ Docker Compose is not available."
    echo "   Please update Docker to the latest version."
    exit 1
fi

echo "✓ Docker Compose is available"
echo ""

# Check for .env file
if [ ! -f .env ]; then
    echo "ℹ️  No .env file found. Using default configuration."
    echo "   To customize, copy .env.example to .env and edit it."
    echo ""
fi

echo "🚀 Starting Weather Dashboard..."
echo ""

# Build and start services
docker compose up --build -d

echo ""
echo "✅ Weather Dashboard is starting!"
echo ""
echo "📱 Access the application at:"
echo "   Frontend: http://localhost:3001"
echo "   Backend API: http://localhost:5101"
echo ""
echo "📊 View logs with: docker compose logs -f"
echo "🛑 Stop services with: docker compose down"
echo ""
