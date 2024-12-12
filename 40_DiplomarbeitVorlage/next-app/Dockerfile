# Dockerfile für das Deployment der nextjs app.
# See https://authjs.dev/getting-started/deployment
#
# Build:                 docker build -t nextjs-app .
# Testen des containers: docker rm -f nextjs-app && docker run -d -p 3000:3000 --env-file .env.local --name nextjs-app nextjs-app
#
# Beides zusammen:
# docker build -t nextjs-app . && docker rm -f nextjs-app && docker run -d -p 443:443 --env-file .env.local --name nextjs-app nextjs-app


# ##################################################################################################
# build stage
# ##################################################################################################
FROM node:22-alpine AS builder
RUN apk add --no-cache openssl
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
# Generiere ein selbstsigniertes Zertifikat.
# Für offizielle Zertifikate wird mit den folgenden Anweisungen statt dessen das Zertifikat kopiert:
# COPY certs/server.crt /app/server.crt
# COPY certs/server.key /app/server.key
RUN mkdir certs && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout certs/server.key -out certs/server.crt \
    -subj "/CN=localhost"

RUN npx prisma generate
RUN npm run build

# ##################################################################################################
# production stage
# ##################################################################################################
FROM node:22-alpine
# For prisma
RUN apk add --no-cache openssl
WORKDIR /app

COPY --from=builder /app/package.json /app/package-lock.json ./
RUN npm install --only=production

COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static
COPY --from=builder /app/prisma ./prisma
COPY --from=builder /app/certs ./certs
COPY --from=builder /app/public ./public
COPY scripts/server.mjs server.mjs

ENV NODE_ENV=production
ENV PORT=443

CMD ["node", "server.mjs"]
EXPOSE 443
