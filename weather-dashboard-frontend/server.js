const http = require('http');
const fs = require('fs');
const path = require('path');
const net = require('net');

// Function to find an available port
function findAvailablePort(startPort = 3000) {
  return new Promise((resolve, reject) => {
    const server = net.createServer();
    
    server.listen(startPort, (err) => {
      if (err) {
        reject(err);
        return;
      }
      
      const port = server.address().port;
      server.close(() => {
        resolve(port);
      });
    });
    
    server.on('error', (err) => {
      if (err.code === 'EADDRINUSE') {
        // Port is busy, try next one
        findAvailablePort(startPort + 1).then(resolve).catch(reject);
      } else {
        reject(err);
      }
    });
  });
}

const server = http.createServer((req, res) => {
  // Simple static file server
  let filePath = path.join(__dirname, req.url === '/' ? 'index.html' : req.url);
  
  // Get file extension
  const extname = path.extname(filePath);
  
  // Set content type
  let contentType = 'text/html';
  switch (extname) {
    case '.css':
      contentType = 'text/css';
      break;
    case '.js':
      contentType = 'text/javascript';
      break;
  }
  
  // Read and serve file
  fs.readFile(filePath, (err, content) => {
    if (err) {
      res.writeHead(404);
      res.end('File not found');
      return;
    }
    
    res.writeHead(200, { 
      'Content-Type': contentType,
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE',
      'Access-Control-Allow-Headers': 'Content-Type'
    });
    res.end(content);
  });
});

// Start server on available port
findAvailablePort(3000).then(port => {
  server.listen(port, () => {
    console.log(`Frontend server running at http://localhost:${port}`);
    console.log(`If port ${port} is not available, the server will automatically find the next available port.`);
  });
}).catch(err => {
  console.error('Failed to find available port:', err);
  process.exit(1);
});