import { createServer } from 'node:http';
import { extname, join, normalize, resolve } from 'node:path';
import { readFile } from 'node:fs/promises';

const root = resolve('src/frontend/selqi-ui/dist/selqi-ui/browser');
const port = Number.parseInt(process.env.PORT ?? '4200', 10);

const contentTypes = new Map([
  ['.css', 'text/css'],
  ['.html', 'text/html'],
  ['.ico', 'image/x-icon'],
  ['.js', 'text/javascript']
]);

createServer(async (request, response) => {
  const requestPath = decodeURIComponent(request.url?.split('?')[0] ?? '/');
  const relativePath = requestPath === '/' ? 'index.html' : requestPath.slice(1);
  const filePath = normalize(join(root, relativePath));

  if (!filePath.startsWith(root)) {
    response.writeHead(403);
    response.end();
    return;
  }

  try {
    const content = await readFile(filePath);
    response.writeHead(200, {
      'Content-Type': contentTypes.get(extname(filePath)) ?? 'application/octet-stream'
    });
    response.end(content);
  } catch {
    const index = await readFile(join(root, 'index.html'));
    response.writeHead(200, { 'Content-Type': 'text/html' });
    response.end(index);
  }
}).listen(port, 'localhost', () => {
  console.log(`Selqi frontend preview listening on http://localhost:${port}`);
});
