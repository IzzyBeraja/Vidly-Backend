# VidlyBackend

This is the backend of a small movie application. It connects to an MongoDB
database using CRUD and has a REST API to handle incoming requests. The main
purpose of this project is to test a front-end movie service I'm building in
React.

As I progress with my React application, I will be increasing the features of
this application as well to compliment it. There are tons of things that still
need implementing!

## Recent Additions as of 9/4/20

- Localhost HTTPS certificate
- User Registration
- Added the infrastructure to handle JWT

## Recent Additions as of 8/31/20

- Added Docker and now can deploy to a container using docker-compose
- Fixed some issues with CORS for now, but they are temporary and not good for
  production

## ToDo

- InsertMany
- Create relationship between genre and movie
  - Only allow movies to be added with already valid genreIds
- JWT authentication
- Login handling
- Create a more streamlined controller setup
  - Currently the genre and movie controllers have similar implementations but
    should be generic
- Learning envoy for microservices
