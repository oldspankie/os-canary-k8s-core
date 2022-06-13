# os-canary-k8s-core

A netcore application for testing and messing around in k8s.

### Scope
As of this writing, I was just using this as a way to test building a small HTTP listener which would respond with a JSON object, build in `.NET 6` and in a `Docker` container.  The Docker container would be the target of a `helm` chart and deployed via `ArgoCD`.  The other purpose of this app was to report on certain `environment variables` from the pods they run on in a k8s cluster.

The main helm chart repo is in [os-charts](https://github.com/oldspankie/os-charts).  The node.js version of this application is [os-canary-k8s-js](https://github.com/oldspankie/os-canary-k8s-js).

### Publish
Included in this repo is a script to automatically build and publish the docker container to a private repo.  This is meant to be called by a CI/CD pipeline - specifically, this was used with TeamCity in mind using a Linux build agent with PowerShell and Docker installed.

Called in a `Command Line` build step like this:
>`pwsh -file publish.ps1 -name:"os-canary-k8s-core" -uploadurl:"local.repo:9001"`

### Related
- `os-charts` - https://github.com/oldspankie/os-charts
- `os-canary-k8s-js` - https://github.com/oldspankie/os-canary-k8s-js