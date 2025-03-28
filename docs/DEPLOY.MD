# Deployment Guide for DaiMinh MVC Project

## 1. Prerequisite Installations
- Docker
- Docker Compose
- Git
- SSH access to deployment server

## 2. Local Development and Docker Build

### 2.1 Build Docker Image
```bash
# Navigate to project root directory
cd /path/to/daiminh

# Build Docker image
docker build -t hungcutedethuongg/daiminh-mvc:latest .
```

### 2.2 Test Local Docker Deployment
```bash
# Run docker compose locally
docker compose up -d

# Verify running containers
docker ps
```

## 3. Docker Hub Deployment

### 3.1 Login to Docker Hub
```bash
docker login
```

### 3.2 Push Image to Docker Hub
```bash
docker push hungcutedethuongg/daiminh-mvc:latest
```

## 4. Server Deployment

### 4.1 Server Preparation
- Ensure Docker and Docker Compose are installed
- Open necessary firewall ports (80, 443)

### 4.2 Server Docker Compose Configuration
Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  mvc:
    image: hungcutedethuongg/daiminh-mvc:latest
    container_name: daiminh-mvc
    restart: unless-stopped
    ports:
      - "80:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=daiminh2;Username=user;Password=your_secure_password
    depends_on:
      postgres:
        condition: service_healthy
    networks:
      - app-network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost/health"]
      interval: 30s
      timeout: 3s
      retries: 3

  postgres:
    image: postgres:16-alpine
    container_name: postgres
    restart: unless-stopped
    environment:
      - POSTGRES_USER=user
      - POSTGRES_PASSWORD=your_secure_password
      - POSTGRES_DB=daiminh2
      - POSTGRES_INITDB_ARGS=--encoding=UTF8
    ports:
      - "5432:5432"
    volumes:
      - postgres-daiminh2:/var/lib/postgresql/data
    networks:
      - app-network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user"]
      interval: 10s
      timeout: 5s
      retries: 5 

volumes:
  postgres-daiminh2:
    name: daiminh2-postgres-data

networks:
  app-network:
    name: daiminh2-network
    driver: bridge
```

### 4.3 Deployment Script
Create `deploy.sh`:

```bash
#!/bin/bash

# Deployment Script
DOCKER_USERNAME="hungcutedethuongg"
PROJECT_NAME="daiminh-mvc"
TAG="latest"

# Login to Docker Hub
docker login

# Pull latest image
docker pull ${DOCKER_USERNAME}/${PROJECT_NAME}:${TAG}

# Stop existing containers
docker compose down

# Start new containers
docker compose up -d

# Clean up old images
docker image prune -f


docker-compose -f docker-compose.web.daiminh.yml down 
docker-compose -f docker-compose.web.daiminh.yml up -d
```

### 4.4 GitHub Actions CI/CD Workflow
Create `.github/workflows/ci-cd.yml`:

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Login to Docker Hub
      uses: docker/login-action@v2
      with:
        username: ${{ secrets.DOCKER_USERNAME }}
        password: ${{ secrets.DOCKER_PASSWORD }}
    
    - name: Build and Push Docker Image
      run: |
        docker build -t hungcutedethuongg/daiminh-mvc:latest .
        docker push hungcutedethuongg/daiminh-mvc:latest
    
    - name: Deploy to Server
      uses: appleboy/ssh-action@master
      with:
        host: ${{ secrets.SERVER_IP }}
        username: ${{ secrets.SERVER_USERNAME }}
        key: ${{ secrets.SERVER_SSH_KEY }}
        script: |
          docker pull hungcutedethuongg/daiminh-mvc:latest
          docker compose down
          docker compose up -d
```

## 5. Security Recommendations
- Use strong, unique passwords
- Configure SSL/TLS
- Regularly update dependencies
- Implement network security rules
- Use Docker secrets for sensitive information

## 6. Troubleshooting
- Check container logs: `docker logs daiminh-mvc`
- Verify network connections
- Ensure all environment variables are correctly set

## 7. Maintenance
- Perform regular backups of PostgreSQL data
- Monitor application and database performance
- Keep Docker images and dependencies updated

## Appendix: Required GitHub Secrets
- `DOCKER_USERNAME`: Docker Hub username
- `DOCKER_PASSWORD`: Docker Hub password
- `SERVER_IP`: Deployment server IP
- `SERVER_USERNAME`: SSH username
- `SERVER_SSH_KEY`: SSH private key for server access