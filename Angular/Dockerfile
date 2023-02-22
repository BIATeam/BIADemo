#Url Red Hat container:
#https://catalog.redhat.com/software/containers/search

#stage 1
FROM registry.access.redhat.com/ubi8/nodejs-16@sha256:3f3a85040eb4d09f137083538eecfdbba604f63b709aa3680d134320dea65f98 as build
USER root
WORKDIR /src
# COPY /ca-certificates/ca-bundle.crt /etc/pki/ca-trust/source/anchors/ca-bundle.crt
# RUN update-ca-trust
# COPY . .
# RUN npm config set registry "https://artifacts.cloud.xxxxxx/repository/npm-group/"
## RUN npm config set strict-ssl false
# RUN npm config set noproxy "artifacts.cloud.xxxxxx"
RUN npm install
RUN npm run build:docker

#stage 2
FROM registry.access.redhat.com/ubi8/nginx-120@sha256:87f658744af695273c434fac8246b716728b691a17d5d0c5b89247362e61e45c
COPY --from=build /src/dist/BIADemo /usr/share/nginx/html
# COPY /dist/BIADemo  /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 8081
CMD ["nginx", "-g", "daemon off;"]

# docker build -t biademofront-image .
# docker run --name biademofront-container -d -p 4200:8081 biademofront-image