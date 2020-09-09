# VidlyBackend

This is the backend of a small movie application. It connects to an MongoDB
database using CRUD and has a REST API to handle incoming requests. The main
purpose of this project is to test a front-end movie service I'm building in
React.

As I progress with my React application, I will be increasing the features of
this application as well to compliment it. There are tons of things that still
need implementing!

## Things to do for next update

 - Create a relationship between Genre and Movie
 - Streamline controllers as they are getting bulky
	- Create a more robust DataManager library and move controller actions there
 - Github actions to upload to AWS or a free service

## Recent Additions as of 9/9/20

 - Custom JWT Authentication with token builder mappings
 - Roles Authorization
 - Login handling is a go
 - Reorganized everything to make a bit more sense

## Recent Additions as of 9/4/20

- Localhost HTTPS certificate
- User Registration
- Added the infrastructure to handle JWT

## Recent Additions as of 8/31/20

- Added Docker and now can deploy to a container using docker-compose
- Fixed some issues with CORS for now, but they are temporary and not good for
  production

## ToDo

- Learning envoy for microservices
- Docker environment variables
- Business logic layer
- Try to decouple as much as I can
- Remake the way claims are built as it's a little silly (Reflection with foreach?)
- Move password validation and creation to a different library
