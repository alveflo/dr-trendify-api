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
