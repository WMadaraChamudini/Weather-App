# Use Node.js as base image
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies (if any)
RUN npm install --production || true

# Copy all frontend files
COPY . .

# Expose port
EXPOSE 3001

# Start the server
CMD ["node", "server.js"]
