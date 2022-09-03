# YARP Sample

Example how to work with YARP using appsettings.json and code based configuration.

-----------------------------------

This example shows how to work with YARP, a reverse proxy library for building fast proxy servers in .NET. The implementation shows the basic setup using appsettings.json and code based configuration approach. 

Further information: https://www.fzankl.de/en/blog/building-a-fast-and-reliable-reverse-proxy-with-yarp

## How to run this sample

To run this sample you have to run all projects in the solution.
Then just open your browser and make a request to https://localhost:500/api/service/. 

Via 'UseCodeBasedConfig' application setting you can switch between appsettings.json and code based configuration.
The result is the same ;-)
