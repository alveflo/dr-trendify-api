.PHONY: all

all:
	make clean -i
	make build
	make dev

clean:
	docker container rm drtrendify-dev
build:
	docker build -t drtrendify . 
dev:
	docker run -p 8080:80 --name drtrendify-dev drtrendify

deploy: build
	heroku container:push web --app dr-trendify-api
	heroku container:release web --app dr-trendify-api