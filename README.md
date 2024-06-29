<div align="center">
  <h1>ApiWithDapper Sample Project
</h1>
  <br />
  <a href="#getting-started"><strong>Getting Started »</strong></a>
  <br />
  <br />
  <a href="https://github.com/HamidMolareza/ApiWithDapper-Sample/issues/new?assignees=&labels=bug&template=BUG_REPORT.md&title=bug%3A+">Report a Bug</a>
  ·
  <a href="https://github.com/HamidMolareza/ApiWithDapper-Sample/issues/new?assignees=&labels=enhancement&template=FEATURE_REQUEST.md&title=feat%3A+">Request a Feature</a>
  .
  <a href="https://github.com/HamidMolareza/ApiWithDapper-Sample/issues/new?assignees=&labels=question&template=SUPPORT_QUESTION.md&title=support%3A+">Ask a Question</a>
</div>

<div align="center">
<br />


![Build Status](https://github.com/HamidMolareza/ApiWithDapper-Sample/actions/workflows/build.yml/badge.svg?branch=main)

![GitHub](https://img.shields.io/github/license/HamidMolareza/ApiWithDapper-Sample)
[![Pull Requests welcome](https://img.shields.io/badge/PRs-welcome-ff69b4.svg?style=flat-square)](https://github.com/HamidMolareza/ApiWithDapper-Sample/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22)

[![code with love by HamidMolareza](https://img.shields.io/badge/%3C%2F%3E%20with%20%E2%99%A5%20by-HamidMolareza-ff1414.svg?style=flat-square)](https://github.com/HamidMolareza)

</div>

## About

### Project Overview

This project is a sample ASP.NET Core Web API for a Todo application that uses **Dapper** as the ORM (Object-Relational Mapper). The Todo API provides a simple yet effective way to manage tasks, illustrating the integration of Dapper for efficient database operations in an ASP.NET Core environment.

### Purpose

The primary purpose of this project is to demonstrate the implementation of a RESTful API for managing Todo items using ASP.NET Core and **Dapper**. It serves as a practical example for developers looking to understand how to build lightweight, high-performance APIs with a focus on simplicity and efficiency.

### Project Goals

- **Simplify Task Management**: Provide a straightforward and efficient way to handle task creation, retrieval, updating, and deletion.
- **Demonstrate Dapper Integration**: Showcase how to integrate **Dapper** with ASP.NET Core to perform database operations efficiently.
- **Educate and Guide Developers**: Serve as an educational resource for developers learning to build APIs using modern technologies.

### Built With

- .NET 8

## Getting Started

### Prerequisites

- Clone project
- Install dotnet 8 or docker

### How to run?

#### Approach 1: Use dotnet

```bash
dotnet restore src
dotnet build src
dotnet run --project src/ApiWithDapper.csproj
```

#### Approach 2: Use docker

```bash
docker build -t api-with-dapper:latest .
docker run --rm -p 8080:8080 api-with-dapper:latest
```

Finally you can remove the image with the below command:
```bash
docker rmi -f api-with-dapper
```

### How to use?

After the project is executed, you can use [swagger](http://localhost:8080/swagger/) or [Todo.http](src/http/Todo.http) file.


## Project Structure

```txt
src
├── API
│   ├── ApiWithDapper.csproj
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── Helpers
│   │   ├── PageData.cs
│   │   └── PaginationHelpers.cs
│   ├── http
│   │   └── Todo.http
│   ├── Migrations
│   │   └── Init_20240628005459.cs
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   └── Todo
│       ├── ITodoRepository.cs
│       ├── TodoController.cs
│       ├── Todo.cs
│       └── TodoRepository.cs
├── src.sln
└── TestProject
    ├── GlobalUsings.cs
    ├── PaginationHelpersTests.cs
    ├── TestProject.csproj
    ├── TodoControllerTests.cs
    └── TodoRepositoryTests.cs
```

## Contributing

First off, thanks for taking the time to contribute! Contributions are what make the free/open-source community such an
amazing place to learn, inspire, and create. Any contributions you make will benefit everybody else and are **greatly
appreciated**.

Please read [our contribution guidelines](docs/CONTRIBUTING.md), and thank you for being involved!

## Authors & contributors

The original setup of this repository is by [HamidMolareza](https://github.com/HamidMolareza).

For a full list of all authors and contributors,
see [the contributors page](https://github.com/HamidMolareza/ApiWithDapper-Sample/contributors).

## Security

This project follows good practices of security, but 100% security cannot be assured. This project is provided **"as
is"** without any **warranty**.

_For more information and to report security issues, please refer to our [security documentation](docs/SECURITY.md)._

## License

This project is licensed under the **GPLv3**.

See [LICENSE](LICENSE) for more information.