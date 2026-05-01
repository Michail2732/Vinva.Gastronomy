FROM node:22-alpine AS build

WORKDIR /app

ARG VITE_API_URL=/api
ENV VITE_API_URL=$VITE_API_URL

COPY src/Web/Vinva.Gastronomy.WebClient/package*.json ./
RUN npm install

COPY src/Web/Vinva.Gastronomy.WebClient ./
RUN npm run build


FROM nginx:latest

COPY --from=build /app/dist /usr/share/nginx/html