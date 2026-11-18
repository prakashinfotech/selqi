FROM node:22-alpine AS build

WORKDIR /app

# Install dependencies first for better Docker cache
COPY src/frontend/selqi-ui/package*.json ./

RUN npm ci

# Copy Angular application
COPY src/frontend/selqi-ui/ ./

# Build Angular application
RUN npm run build


# Runtime image
FROM nginx:alpine AS runtime

COPY src/frontend/selqi-ui/nginx.conf \
     /etc/nginx/conf.d/default.conf

COPY --from=build \
     /app/dist/selqi-ui/browser \
     /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
