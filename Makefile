build: *.cs *.csproj
	dotnet publish

install: build
	cp ./bin/Debug/net6.0/osx.12-arm64/publish/gpt3 ~/.bin

clean:
	rm -rf {bin,obj}
