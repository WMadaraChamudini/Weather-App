const http = require('http');
const fs = require('fs');
const path = require('path');

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

const port = 3001;
server.listen(port, () => {
  console.log(`Frontend server running at http://localhost:${port}`);
});