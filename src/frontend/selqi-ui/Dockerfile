FROM node:22-alpine AS build
WORKDIR /app

# Install dependencies — separate layer for cache efficiency
COPY src/frontend/selqi-ui/package*.json ./
RUN npm ci

# Build Angular with production config (uses environment.production.ts → apiBase: '/api/v1')
COPY src/frontend/selqi-ui/ ./
RUN npm run build

FROM nginx:alpine AS runtime
COPY src/frontend/selqi-ui/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/selqi-ui/browser /usr/share/nginx/html
EXPOSE 80
