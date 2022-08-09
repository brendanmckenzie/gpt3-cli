build: *.cs *.csproj
	dotnet publish --configuration Release --no-restore --nologo

install: build
	mv bin/Release/net6.0/osx.12-arm64/publish/gpt3 ~/.bin

clean:
	rm -rf {bin,obj}
