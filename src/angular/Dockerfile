FROM node:lts-alpine3.16 as build
WORKDIR /app
COPY package.json /app/package.json
COPY package-lock.json /app/package-lock.json

RUN npm install -g npm@8.17.0
#RUN npm install @angular/cli
RUN npm install
COPY . /app

RUN node_modules/.bin/ng build --configuration production

FROM nginx:1.17-alpine
RUN apk update && apk add bash
COPY ./nginx/nginx.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist/itaccept-app /usr/share/nginx/html
