# golem-client-mock
Mockup of Golem Client with all the standardized APIs.

## Building the project

### Prerequisites
- .NET Core SDK 2.2
- NPM (Node.js package manager) - required for React frontend application

### Windows

Install the prerequisites, then:

Get project sources:
```
git clone https://github.com/stranger80/golem-market-api.git
```

Get NPM dependencies for client app:
```
cd golem-client-mock/GolemClientMockAPI/ClientApp
npm install
cd ../..
```

Build the project:
```
dotnet publish
```

### Ubuntu

**Setup prerequisites:**

(As per [https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current], but note .NET Core 2.2. is required rather than the latest version) :

```
wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

Get .NET Core 2.2. SDK

```
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.2
```

Get NPM:

```
sudo apt install npm
```

Get project sources:
```
git clone https://github.com/stranger80/golem-client-mock.git
```

Get NPM dependencies for client app:
```
cd golem-client-mock/GolemClientMockAPI/ClientApp
npm install
```


## Running the project

In command prompt, in golem-client-mock folder, run:
```
dotnet run -p GolemCLientMockAPI
```

...and observe the messages from the API startup.

Open the browser to view the Control Dashboard:
```
http://localhost:5001/
```

...or view the Swagger UI:
```
http://localhost:5001/swagger
```

