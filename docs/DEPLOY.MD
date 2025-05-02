## 🔄 CI/CD Flow chuẩn

1. **Build & Push image từ máy dev hoặc GitHub Actions**:

```bash
docker build -t hungcutedethuongg/daiminh-web:latest .
docker push hungcutedethuongg/daiminh-web:latest
```

2. **Trên server**:

```bash
docker compose -f docker-compose.db.yml up -d
docker compose -f docker-compose.minio.yml up -d
docker compose -f docker-compose.web.yml pull
docker compose -f docker-compose.web.yml up -d
```

