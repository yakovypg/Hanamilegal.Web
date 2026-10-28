# Stage 1: build
FROM node:22-alpine AS build

WORKDIR /app

# Copy build configuration
COPY package.json yarn.lock ./

# Install build dependencies
RUN yarn install --frozen-lockfile

# Copy all files
COPY . ./

# Configure environment
ARG VITE_COOKIE_POLICY_NAME
ENV VITE_COOKIE_POLICY_NAME=$VITE_COOKIE_POLICY_NAME
ARG VITE_PRIVACY_POLICY_NAME
ENV VITE_PRIVACY_POLICY_NAME=$VITE_PRIVACY_POLICY_NAME
ARG VITE_APPLICATIONS_API_BASE_URL
ENV VITE_APPLICATIONS_API_BASE_URL=$VITE_APPLICATIONS_API_BASE_URL

# Build the application
RUN yarn build:prod;

# Stage 2: serve static files with a small webserver
FROM nginx:stable-alpine AS production

# Remove default nginx static content
RUN rm -rf /usr/share/nginx/html/*

# Copy built assets from the build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Configure nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

CMD ["nginx", "-g", "daemon off;"]
