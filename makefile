all: news

news: $(srcs)
	gmcs -out:news -pkg:dotnet $(srcs)  
	
clean:
	rm -f news
    
srcs = *.cs sdk/*.cs