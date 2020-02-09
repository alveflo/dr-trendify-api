all:
	make clean -i
	make build
	make dev

clean:
	cd src/DrTrendify.Api && docker container rm drtrendify-dev
build:
	cd src/DrTrendify.Api && docker build -t drtrendify . 
dev:
	cd src/DrTrendify.Api && docker run -p 8080:80 --name drtrendify-dev drtrendify

deploy:
	make build
	cd src/DrTrendify.Api && heroku container:push web --app dr-trendify-api
	cd src/DrTrendify.Api && heroku container:release web --app dr-trendify-api